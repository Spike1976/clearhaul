using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ClearHaul.Client;

public sealed class ServerHealthClient : IDisposable
{
    public const int MaximumResponseBytes = 65536;

    public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    private readonly HttpClient _httpClient;
    private bool _disposed;

    public ServerHealthClient(HttpMessageHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _httpClient = new HttpClient(handler, disposeHandler: false)
        {
            Timeout = RequestTimeout
        };
    }

    public async Task<ServerHealthResult> CheckAsync(
        string? serverAddress,
        CancellationToken cancellationToken = default)
    {
        if (!TryCreateHealthUri(serverAddress, out var healthUri, out var failureMessage))
        {
            return ServerHealthResult.Failure(failureMessage);
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, healthUri);
            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            var body = await ReadAtMostAsync(response.Content, cancellationToken).ConfigureAwait(false);
            var statusCode = (int)response.StatusCode;

            if (body.Truncated)
            {
                return ServerHealthResult.Failure(
                    "Server health check failed. The response exceeded 65536 bytes and was not accepted.",
                    statusCode,
                    body.Text);
            }

            if (!response.IsSuccessStatusCode)
            {
                return ServerHealthResult.Failure(
                    $"Server health check failed. The server returned HTTP {statusCode}.",
                    statusCode,
                    body.Text);
            }

            return InterpretSuccess(statusCode, body);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return ServerHealthResult.Failure(
                "Server health check failed. The request timed out after five seconds.");
        }
        catch (HttpRequestException)
        {
            return ServerHealthResult.Failure(
                "Server health check failed. The server could not be reached.");
        }
        catch (IOException)
        {
            return ServerHealthResult.Failure(
                "Server health check failed. The response could not be read.");
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _httpClient.Dispose();
    }

    private static bool TryCreateHealthUri(string? serverAddress, out Uri healthUri, out string failureMessage)
    {
        healthUri = null!;
        var text = serverAddress?.Trim() ?? string.Empty;
        if (!Uri.TryCreate(text, UriKind.Absolute, out var parsed))
        {
            failureMessage = "Server health check failed. Enter an absolute http or https server address. No request was sent.";
            return false;
        }

        if (!IsAllowedScheme(parsed))
        {
            failureMessage = "Server health check failed. Only http and https addresses are allowed. No request was sent.";
            return false;
        }

        if (!string.IsNullOrEmpty(parsed.UserInfo))
        {
            failureMessage = "Server health check failed. The address must not include a username or password. No request was sent.";
            return false;
        }

        if (string.IsNullOrEmpty(parsed.IdnHost))
        {
            failureMessage = "Server health check failed. The server address has no host. No request was sent.";
            return false;
        }

        var builder = new UriBuilder(parsed)
        {
            Path = "/health",
            Query = string.Empty,
            Fragment = string.Empty
        };
        healthUri = builder.Uri;
        failureMessage = string.Empty;
        return true;
    }

    private static bool IsAllowedScheme(Uri uri)
    {
        // Only http and https are allowed. Any other scheme, including file, fails without sending a request.
        return uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            || uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
    }

    private static ServerHealthResult InterpretSuccess(int statusCode, LimitedBody body)
    {
        try
        {
            using var document = JsonDocument.Parse(body.Bytes);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object
                || !root.TryGetProperty("status", out var statusElement)
                || statusElement.ValueKind != JsonValueKind.String)
            {
                return ServerHealthResult.Failure(
                    "Server health check failed. The response did not include a string status.",
                    statusCode,
                    body.Text);
            }

            var reportedStatus = statusElement.GetString();
            var stage = TryGetString(root, "stage");
            var version = TryGetString(root, "version");
            return ServerHealthResult.Success(
                statusCode,
                body.Text,
                reportedStatus,
                BuildMessage(statusCode, reportedStatus, stage, version));
        }
        catch (JsonException)
        {
            return ServerHealthResult.Failure(
                "Server health check failed. The response was not valid JSON.",
                statusCode,
                body.Text);
        }
    }

    private static string BuildMessage(int statusCode, string? reportedStatus, string? stage, string? version)
    {
        var message = $"The server answered HTTP {statusCode}. Reported status is {reportedStatus}.";
        if (!string.IsNullOrEmpty(stage))
        {
            message += $" Stage is {stage}.";
        }

        if (!string.IsNullOrEmpty(version))
        {
            message += $" Version is {version}.";
        }

        return message;
    }

    private static string? TryGetString(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var element) || element.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        return element.GetString();
    }

    private static async Task<LimitedBody> ReadAtMostAsync(HttpContent content, CancellationToken cancellationToken)
    {
        var contentLength = content.Headers.ContentLength;
        var buffer = new byte[MaximumResponseBytes];
        await using var stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(total, buffer.Length - total), cancellationToken)
                .ConfigureAwait(false);
            if (read == 0)
            {
                break;
            }

            total += read;
        }

        var truncated = contentLength > MaximumResponseBytes
            || (total == MaximumResponseBytes && contentLength != MaximumResponseBytes);
        var bytes = buffer.AsSpan(0, total).ToArray();
        return new LimitedBody(bytes, Encoding.UTF8.GetString(bytes), truncated);
    }

    private readonly record struct LimitedBody(byte[] Bytes, string Text, bool Truncated);
}
