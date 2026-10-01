using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClearHaul.Domain.Identity;
using ClearHaul.Domain.Records;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ClearHaul.Server.Identity;

public sealed record LoginRequest(string? Email, string? Password);

public sealed record QualificationRequest(DateOnly? PickupDate);

public sealed record LabelRequest(string? Label);

public sealed record ReviewRequest(string? Note);

public static class IdentityEndpoints
{
    public static void Map(WebApplication app, OrganizationRegistry registry, AuditLog audit, bool devDirectoryEnabled, string? devPassword)
    {
        app.MapPost("/v1/sessions", async (HttpContext http, LoginRequest? body) =>
        {
            if (!devDirectoryEnabled)
            {
                return Results.Problem(title: "Authentication is not configured.", statusCode: StatusCodes.Status404NotFound);
            }

            if (string.IsNullOrEmpty(devPassword))
            {
                return Results.Problem(title: "The development directory password is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            var email = body?.Email ?? "";
            var password = body?.Password ?? "";
            var user = registry.FindUser(email);
            if (user is null || !FixedEquals(password, devPassword))
            {
                return Results.Problem(title: "Sign-in was refused.", statusCode: StatusCodes.Status401Unauthorized);
            }

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.UserId),
                new("org", user.OrganizationId),
                new(ClaimTypes.Role, user.Role)
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Results.Ok(new { user.OrganizationId, user.Role });
        });

        app.MapPost("/v1/sessions/end", async (HttpContext http) =>
        {
            await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.NoContent();
        });

        app.MapGet("/health/worker", () => Results.Json(new
        {
            status = "Healthy",
            database = "NotConfigured"
        }));

        app.MapGet("/v1/organizations/{organizationId}", (HttpContext http, string organizationId) =>
        {
            var actor = Actor(http);
            if (actor is null)
            {
                return Results.Problem(title: "Sign-in is required.", statusCode: StatusCodes.Status401Unauthorized);
            }

            if (!string.Equals(actor.OrganizationId, organizationId, StringComparison.Ordinal))
            {
                Deny(audit, actor, organizationId);
                return Results.Problem(title: "That organization is outside your account.", statusCode: StatusCodes.Status403Forbidden);
            }

            var organization = registry.FindOrganization(organizationId);
            if (organization is null)
            {
                return Results.Problem(title: "Organization not found.", statusCode: StatusCodes.Status404NotFound);
            }

            return Results.Ok(new
            {
                organization.Id,
                organization.Kind,
                organization.LegalName,
                organization.Verified,
                organization.Suspended
            });
        });

        app.MapPost("/v1/organizations/{organizationId}/drivers", (HttpContext http, string organizationId, LabelRequest? body) =>
            Maintain(http, audit, registry, organizationId, body?.Label, registry.AddDriver, "driver"));

        app.MapPost("/v1/organizations/{organizationId}/equipment", (HttpContext http, string organizationId, LabelRequest? body) =>
            Maintain(http, audit, registry, organizationId, body?.Label, registry.AddEquipment, "equipment"));

        app.MapPost("/v1/organizations/{organizationId}/qualification", (HttpContext http, string organizationId, QualificationRequest? body) =>
        {
            var actor = RequireOwn(http, audit, organizationId);
            if (actor.Error is not null)
            {
                return actor.Error;
            }

            var organization = registry.FindOrganization(organizationId);
            if (organization is null || organization.Kind != "carrier")
            {
                return Results.Problem(title: "Qualification is available for a carrier organization.", statusCode: StatusCodes.Status403Forbidden);
            }

            var day = body?.PickupDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var decision = QualificationCalculator.Evaluate(organization, day, DateTimeOffset.UtcNow);
            audit.Append(actor.Context!, "CARRIER_QUALIFICATION_CHECKED", null, decision.Outcome.ToString(), DateTimeOffset.UtcNow, decision.Explanation, null, null, null, null, http.TraceIdentifier);
            return Results.Ok(new
            {
                outcome = decision.Outcome.ToString(),
                mayParticipate = decision.MayParticipate,
                explanation = decision.Explanation,
                isFreshExternalCheck = decision.IsFreshExternalCheck
            });
        });
    }

    private static IResult Maintain(
        HttpContext http,
        AuditLog audit,
        OrganizationRegistry registry,
        string organizationId,
        string? label,
        Func<string, string, OperationResult> add,
        string kind)
    {
        var actor = RequireOwn(http, audit, organizationId);
        if (actor.Error is not null)
        {
            return actor.Error;
        }

        var result = add(organizationId, label ?? "");
        audit.Append(actor.Context!, kind + ".maintained", null, result.Code, DateTimeOffset.UtcNow, result.Detail, null, null, null, null, http.TraceIdentifier);
        return result.Succeeded
            ? Results.Ok(new { result.Code, result.Detail })
            : Results.Problem(title: "The record was not saved.", statusCode: StatusCodes.Status400BadRequest);
    }

    private static ActorResult RequireOwn(HttpContext http, AuditLog audit, string organizationId)
    {
        var actor = Actor(http);
        if (actor is null)
        {
            return new ActorResult(default, Results.Problem(title: "Sign-in is required.", statusCode: StatusCodes.Status401Unauthorized));
        }

        if (!string.Equals(actor.OrganizationId, organizationId, StringComparison.Ordinal))
        {
            Deny(audit, actor, organizationId);
            return new ActorResult(default, Results.Problem(title: "That organization is outside your account.", statusCode: StatusCodes.Status403Forbidden));
        }

        return new ActorResult(actor, null);
    }

    private static void Deny(AuditLog audit, ActorContext actor, string organizationId)
    {
        audit.Append(actor, "ACCESS_DENIED", null, organizationId, DateTimeOffset.UtcNow, "Organization isolation refused the request.", null, null, null, null, "access-denied");
    }

    private static ActorContext? Actor(HttpContext http)
    {
        if (http.User.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var userId = http.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var organizationId = http.User.FindFirstValue("org");
        var role = http.User.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(organizationId) || string.IsNullOrWhiteSpace(role))
        {
            return null;
        }

        return new ActorContext(userId, organizationId, role);
    }

    private static bool FixedEquals(string provided, string expected)
    {
        var left = SHA256.HashData(Encoding.UTF8.GetBytes(provided));
        var right = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
        return CryptographicOperations.FixedTimeEquals(left, right);
    }

    private readonly record struct ActorResult(ActorContext? Context, IResult? Error);
}

public static class FictionalDirectory
{
    public static void Seed(OrganizationRegistry registry)
    {
        AddShipper(registry, "shipper-a", "Synthetic Shipper A", "shipper-a@example.invalid", "user-shipper-a");
        AddShipper(registry, "shipper-b", "Synthetic Shipper B", "shipper-b@example.invalid", "user-shipper-b");
        AddCarrier(registry, "carrier-ok", "Synthetic Carrier Ok", "carrier-ok@example.invalid", "user-carrier-ok", verified: true, suspended: false, authority: true, insurance: new DateOnly(2026, 12, 1), review: false, driver: "Synthetic Driver Ok", equipment: "Synthetic Trailer Ok");
        AddCarrier(registry, "carrier-suspended", "Synthetic Carrier Suspended", "carrier-suspended@example.invalid", "user-carrier-suspended", verified: true, suspended: true, authority: true, insurance: new DateOnly(2026, 12, 1), review: false, driver: "Synthetic Driver Suspended", equipment: "Synthetic Trailer Suspended");
        AddCarrier(registry, "carrier-expired", "Synthetic Carrier Expired", "carrier-expired@example.invalid", "user-carrier-expired", verified: true, suspended: false, authority: true, insurance: new DateOnly(2026, 10, 1), review: false, driver: "Synthetic Driver Expired", equipment: "Synthetic Trailer Expired");
        AddCarrier(registry, "carrier-unverified", "Synthetic Carrier Unverified", "carrier-unverified@example.invalid", "user-carrier-unverified", verified: false, suspended: false, authority: true, insurance: new DateOnly(2026, 12, 1), review: false, driver: "Synthetic Driver Unverified", equipment: "Synthetic Trailer Unverified");
        AddCarrier(registry, "carrier-review", "Synthetic Carrier Review", "carrier-review@example.invalid", "user-carrier-review", verified: true, suspended: false, authority: true, insurance: new DateOnly(2026, 12, 1), review: true, driver: "Synthetic Driver Review", equipment: "Synthetic Trailer Review");
        AddCarrier(registry, "carrier-empty", "Synthetic Carrier Empty", "carrier-empty@example.invalid", "user-carrier-empty", verified: true, suspended: false, authority: true, insurance: new DateOnly(2026, 12, 1), review: false, driver: null, equipment: null);
    }

    private static void AddShipper(OrganizationRegistry registry, string id, string name, string email, string userId)
    {
        registry.RegisterOrganization(new OrganizationRecord
        {
            Id = id,
            Kind = "shipper",
            LegalName = name,
            Verified = true
        });
        registry.RegisterUser(new DirectoryUser { Email = email, UserId = userId, OrganizationId = id, Role = "ShipperOrganizationAdministrator" });
    }

    private static void AddCarrier(
        OrganizationRegistry registry,
        string id,
        string name,
        string email,
        string userId,
        bool verified,
        bool suspended,
        bool authority,
        DateOnly insurance,
        bool review,
        string? driver,
        string? equipment)
    {
        registry.RegisterOrganization(new OrganizationRecord
        {
            Id = id,
            Kind = "carrier",
            LegalName = name,
            Verified = verified,
            Suspended = suspended,
            AuthorityActive = authority,
            InsuranceExpiresOn = insurance,
            ManualReviewOpen = review
        });
        if (driver is not null)
        {
            registry.AddDriver(id, driver);
        }

        if (equipment is not null)
        {
            registry.AddEquipment(id, equipment);
        }

        registry.RegisterUser(new DirectoryUser { Email = email, UserId = userId, OrganizationId = id, Role = "CarrierOrganizationAdministrator" });
    }
}
