using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClearHaul.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ClearHaul.Server.Tests;

public sealed class HealthEndpointTests : IClassFixture<FoundationFactory>
{
    private readonly FoundationFactory _factory;

    public HealthEndpointTests(FoundationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Live_route_returns_only_healthy_status()
    {
        using var client = _factory.CreateClient();
        using var document = JsonDocument.Parse(await client.GetStringAsync("/health/live"));

        Assert.Equal(HealthStatus.Healthy, document.RootElement.GetProperty("status").GetString());
        Assert.Single(document.RootElement.EnumerateObject());
    }

    [Fact]
    public async Task Health_route_reports_foundation_dependencies_as_not_configured()
    {
        using var client = _factory.CreateClient();
        var body = await client.GetFromJsonAsync<HealthResponse>("/health");

        Assert.NotNull(body);
        Assert.Equal(HealthStatus.Degraded, body.Status);
        Assert.Equal(FoundationStage.Stage, body.Stage);
        Assert.Equal(FoundationStage.Version, body.Version);
        Assert.Collection(
            body.Checks,
            check => AssertCheck(check, "self", HealthStatus.Healthy),
            check => AssertCheck(check, "postgres", HealthStatus.NotConfigured),
            check => AssertCheck(check, "redis", HealthStatus.NotConfigured),
            check => AssertCheck(check, "objectStorage", HealthStatus.NotConfigured));
    }

    [Fact]
    public async Task Unknown_route_returns_a_problem_without_internal_detail()
    {
        using var client = _factory.CreateClient();
        using var response = await client.GetAsync("/shipments");
        var text = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("\"title\":\"Not found\"", text, StringComparison.Ordinal);
        Assert.DoesNotContain("password", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("stack trace", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ClearHaul.Server", text, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Health_response_includes_the_foundation_headers()
    {
        using var client = _factory.CreateClient();
        using var response = await client.GetAsync("/health");

        Assert.Equal("nosniff", Header(response, "X-Content-Type-Options"));
        Assert.Equal("DENY", Header(response, "X-Frame-Options"));
        Assert.Equal("no-referrer", Header(response, "Referrer-Policy"));
        Assert.Equal("default-src 'none'; frame-ancestors 'none'", Header(response, "Content-Security-Policy"));
        Assert.Equal("no-store", Header(response, "Cache-Control"));
        Assert.False(response.Headers.Contains("Server"));
    }

    [Fact]
    public async Task Repeated_health_calls_both_succeed()
    {
        using var client = _factory.CreateClient();

        using var first = await client.GetAsync("/health");
        using var second = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
    }

    [Fact]
    public void Overall_status_is_unhealthy_when_any_check_is_unhealthy()
    {
        Assert.Equal(
            HealthStatus.Unhealthy,
            HealthStatus.Combine([HealthStatus.Healthy, HealthStatus.Unhealthy, HealthStatus.NotConfigured]));
    }

    private static void AssertCheck(HealthCheckItem check, string name, string status)
    {
        Assert.Equal(name, check.Name);
        Assert.Equal(status, check.Status);
        Assert.DoesNotContain("password", check.Description, StringComparison.OrdinalIgnoreCase);
    }

    private static string Header(HttpResponseMessage response, string name)
    {
        Assert.True(response.Headers.TryGetValues(name, out var values));
        return Assert.Single(values);
    }
}

public sealed class FoundationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
