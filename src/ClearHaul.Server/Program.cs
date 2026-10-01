using System.Threading.RateLimiting;
using ClearHaul.Contracts;
using ClearHaul.Domain.Identity;
using ClearHaul.Domain.Records;
using ClearHaul.Server.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var devDirectoryEnabled = builder.Configuration.GetValue("Foundation:DevDirectory", false);
var devPassword = builder.Configuration["Foundation:DevPassword"];
var registry = new OrganizationRegistry();
var audit = new AuditLog();
if (devDirectoryEnabled)
{
    FictionalDirectory.Seed(registry);
}

builder.Services.AddSingleton(registry);
builder.Services.AddSingleton(audit);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "ch.session";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 60,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/problem+json";
        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                title = "Too many requests.",
                status = StatusCodes.Status429TooManyRequests
            },
            cancellationToken);
    };
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new
        {
            title = "The request failed.",
            status = StatusCodes.Status500InternalServerError
        });
    });
});

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;
        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "DENY";
        headers["Referrer-Policy"] = "no-referrer";
        headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";
        headers["Cache-Control"] = "no-store";
        headers.Remove("Server");
        return Task.CompletedTask;
    });

    await next();
});

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseRateLimiter();
}

app.UseAuthentication();
app.UseAuthorization();
IdentityEndpoints.Map(app, registry, audit, devDirectoryEnabled, devPassword);

app.MapGet("/health/live", () => Results.Json(new LiveHealthResponse(HealthStatus.Healthy)));
app.MapGet("/health", () =>
{
    var checks = new HealthCheckItem[]
    {
        new("self", HealthStatus.Healthy, "The process handled this request."),
        new("postgres", HealthStatus.NotConfigured, "Not implemented in the foundation build."),
        new("redis", HealthStatus.NotConfigured, "Not implemented in the foundation build."),
        new("objectStorage", HealthStatus.NotConfigured, "Not implemented in the foundation build.")
    };

    return Results.Json(new HealthResponse(
        HealthStatus.Combine(checks.Select(check => check.Status)),
        FoundationStage.Stage,
        FoundationStage.Version,
        checks));
});

app.MapFallback(() => Results.Problem(
    title: "Not found",
    statusCode: StatusCodes.Status404NotFound));

app.Run();

public partial class Program
{
}
