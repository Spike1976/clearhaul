using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ClearHaul.Server.Tests;

public sealed class IdentityEndpointTests : IClassFixture<IdentityFactory>
{
    private readonly IdentityFactory _factory;

    public IdentityEndpointTests(IdentityFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Shipper_cannot_read_another_shipper_organization()
    {
        using var client = _factory.CreateClient();
        Assert.True((await SignIn(client, "shipper-a@example.invalid")).IsSuccessStatusCode);
        var own = await client.GetAsync("/v1/organizations/shipper-a");
        var other = await client.GetAsync("/v1/organizations/shipper-b");
        Assert.Equal(HttpStatusCode.OK, own.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, other.StatusCode);
    }

    [Fact]
    public async Task Suspended_carrier_cannot_participate()
    {
        using var client = _factory.CreateClient();
        await SignIn(client, "carrier-suspended@example.invalid");
        var response = await client.PostAsJsonAsync("/v1/organizations/carrier-suspended/qualification", new { pickupDate = "2026-10-10" });
        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Suspended", body, StringComparison.Ordinal);
        Assert.Contains("false", body, StringComparison.Ordinal);
        Assert.Contains("\"isFreshExternalCheck\":false", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Signed_out_session_cannot_request_qualification()
    {
        using var client = _factory.CreateClient();
        await SignIn(client, "carrier-ok@example.invalid");
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync("/v1/sessions/end", null)).StatusCode);
        var response = await client.PostAsJsonAsync("/v1/organizations/carrier-ok/qualification", new { pickupDate = "2026-10-10" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Carrier_can_maintain_its_own_driver_and_not_another_carriers()
    {
        using var client = _factory.CreateClient();
        await SignIn(client, "carrier-empty@example.invalid");
        var own = await client.PostAsJsonAsync("/v1/organizations/carrier-empty/drivers", new { label = "Synthetic Driver Added" });
        var other = await client.PostAsJsonAsync("/v1/organizations/carrier-ok/drivers", new { label = "Not allowed" });
        Assert.Equal(HttpStatusCode.OK, own.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, other.StatusCode);
    }

    [Fact]
    public async Task Worker_health_is_up_and_the_database_is_not_configured()
    {
        using var client = _factory.CreateClient();
        var body = await client.GetStringAsync("/health/worker");
        Assert.Contains("Healthy", body, StringComparison.Ordinal);
        Assert.Contains("NotConfigured", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Authentication_is_absent_when_the_development_directory_is_off()
    {
        using var factory = new FoundationFactory();
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/v1/sessions", new { email = "shipper-a@example.invalid", password = "dev-only-not-a-secret" });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task<HttpResponseMessage> SignIn(HttpClient client, string email) =>
        await client.PostAsJsonAsync("/v1/sessions", new { email, password = "dev-only-not-a-secret" });
}

public sealed class IdentityFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Foundation:DevDirectory", "true");
        builder.UseSetting("Foundation:DevPassword", "dev-only-not-a-secret");
    }
}
