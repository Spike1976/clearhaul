namespace ClearHaul.Client;

public sealed class ServerHealthResult
{
    private ServerHealthResult(
        bool succeeded,
        int? statusCode,
        string? responseBody,
        string? reportedStatus,
        string message)
    {
        Succeeded = succeeded;
        StatusCode = statusCode;
        ResponseBody = responseBody;
        ReportedStatus = reportedStatus;
        Message = message;
    }

    public bool Succeeded { get; }

    public int? StatusCode { get; }

    public string? ResponseBody { get; }

    public string? ReportedStatus { get; }

    public string Message { get; }

    public static ServerHealthResult Success(
        int statusCode,
        string responseBody,
        string? reportedStatus,
        string message)
    {
        return new ServerHealthResult(true, statusCode, responseBody, reportedStatus, message);
    }

    public static ServerHealthResult Failure(
        string message,
        int? statusCode = null,
        string? responseBody = null)
    {
        return new ServerHealthResult(false, statusCode, responseBody, null, message);
    }
}
