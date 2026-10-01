namespace ClearHaul.Contracts;

public static class FoundationStage
{
    public const string Stage = "foundation";
    public const string Version = "0.1.0-foundation";
}

public static class HealthStatus
{
    public const string Healthy = "Healthy";
    public const string Degraded = "Degraded";
    public const string Unhealthy = "Unhealthy";
    public const string NotConfigured = "NotConfigured";

    public static string Combine(IEnumerable<string> checkStatuses)
    {
        var sawGap = false;
        foreach (var status in checkStatuses)
        {
            if (status == Unhealthy)
            {
                return Unhealthy;
            }

            if (status != Healthy)
            {
                sawGap = true;
            }
        }

        return sawGap ? Degraded : Healthy;
    }
}

public sealed record LiveHealthResponse(string Status);

public sealed record HealthCheckItem(string Name, string Status, string Description);

public sealed record HealthResponse(
    string Status,
    string Stage,
    string Version,
    IReadOnlyList<HealthCheckItem> Checks);
