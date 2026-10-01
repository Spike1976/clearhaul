using System.Net;
using System.Text;
using ClearHaul.Client;

namespace ClearHaul.Client.Tests;

public sealed class ServerHealthClientTests
{
    [Fact]
    public async Task Successful_health_json_returns_the_status()
    {
        using var handler = new StubHandler(_ => Json(HttpStatusCode.OK, """{"status":"Degraded"}"""));
        var client = new ServerHealthClient(handler);

        var result = await client.CheckAsync("http://127.0.0.1:5080", CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal("Degraded", result.ReportedStatus);
    }

    [Fact]
    public async Task Non_success_status_is_a_failed_result()
    {
        using var handler = new StubHandler(_ => Json(HttpStatusCode.ServiceUnavailable, """{"status":"Unhealthy"}"""));
        var client = new ServerHealthClient(handler);

        var result = await client.CheckAsync("https://127.0.0.1:5080", CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Contains("503", result.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task File_address_is_rejected_before_a_request()
    {
        using var handler = new StubHandler(_ => throw new InvalidOperationException("The request was sent."));
        var client = new ServerHealthClient(handler);

        var result = await client.CheckAsync("file:///C:/secret.txt", CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(0, handler.Calls);
    }

    [Fact]
    public async Task Two_successful_calls_both_request_health()
    {
        using var handler = new StubHandler(_ => Json(HttpStatusCode.OK, """{"status":"Degraded"}"""));
        var client = new ServerHealthClient(handler);

        var first = await client.CheckAsync("http://127.0.0.1:5080/", CancellationToken.None);
        var second = await client.CheckAsync("http://127.0.0.1:5080/", CancellationToken.None);

        Assert.True(first.Succeeded);
        Assert.True(second.Succeeded);
        Assert.Equal(2, handler.Calls);
    }

    [Fact]
    public async Task Network_failure_returns_a_failed_result()
    {
        using var handler = new StubHandler(_ => throw new HttpRequestException("down"));
        var client = new ServerHealthClient(handler);

        var result = await client.CheckAsync("http://127.0.0.1:5080", CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.DoesNotContain("down", result.Message, StringComparison.Ordinal);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body)
    {
        return new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _send;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> send)
        {
            _send = send;
        }

        public int Calls { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(_send(request));
        }
    }
}
