using System.Security.Cryptography;
using System.Text;

namespace ClearHaul.Domain.Records;

public sealed record OperationResult(bool Succeeded, string Code, string Detail)
{
    public static OperationResult Ok(string code, string detail) => new(true, code, detail);

    public static OperationResult Fail(string code, string detail) => new(false, code, detail);
}

public sealed record AuditEvent(
    string Id,
    string ActorUserId,
    string OrganizationId,
    string Role,
    string EventName,
    string? OriginalValue,
    string? NewValue,
    DateTimeOffset Timestamp,
    string Source,
    string Reason,
    string? ShipmentId,
    string? EquipmentId,
    string? DocumentHash,
    string? RuleVersion,
    string CorrelationId);

public sealed class AuditLog
{
    private readonly List<AuditEvent> _events = [];

    public IReadOnlyList<AuditEvent> Events => _events;

    public AuditEvent Append(
        ActorContext actor,
        string eventName,
        string? originalValue,
        string? newValue,
        DateTimeOffset timestamp,
        string reason,
        string? shipmentId,
        string? equipmentId,
        string? documentHash,
        string? ruleVersion,
        string correlationId)
    {
        var auditEvent = new AuditEvent(
            Guid.NewGuid().ToString("N"),
            actor.UserId,
            actor.OrganizationId,
            actor.Role,
            eventName,
            originalValue,
            newValue,
            timestamp,
            "phase-zero",
            reason,
            shipmentId,
            equipmentId,
            documentHash,
            ruleVersion,
            correlationId);
        _events.Add(auditEvent);
        return auditEvent;
    }

    public OperationResult TryDelete(string eventId, ActorContext actor, DateTimeOffset timestamp, string correlationId)
    {
        Append(actor, "audit.delete-refused", eventId, null, timestamp, "Audit events are append-only.", null, null, null, null, correlationId);
        return OperationResult.Fail("audit-delete-refused", "An audit event cannot be deleted.");
    }
}

public sealed record ActorContext(string UserId, string OrganizationId, string Role);

public sealed record NotificationEvent(
    string Id,
    string Name,
    string ShipmentId,
    string CorrelationId,
    DateTimeOffset Timestamp,
    string DeliveryStatus);

public sealed class NotificationLog
{
    private readonly List<NotificationEvent> _events = [];

    public IReadOnlyList<NotificationEvent> Events => _events;

    public void Add(string name, string shipmentId, string correlationId, DateTimeOffset timestamp)
    {
        _events.Add(new NotificationEvent(
            Guid.NewGuid().ToString("N"),
            name,
            shipmentId,
            correlationId,
            timestamp,
            "not-sent-no-provider"));
    }
}

public static class ContentHash
{
    public static string Sha256(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash);
    }
}

public enum RetentionClass
{
    ShipmentOperational,
    EquipmentHistorySnapshot,
    Audit,
    PaymentLedger,
    DriverPacket
}

public sealed record RetentionPolicy(RetentionClass Class, int? ApprovedDurationDays, string Disposition)
{
    public static RetentionPolicy Unset(RetentionClass retentionClass) =>
        new(retentionClass, null, "tombstone-only-until-legal-review");
}

public static class RetentionActions
{
    public static OperationResult RequestPhysicalDelete(RetentionPolicy policy) =>
        OperationResult.Fail(
            "physical-delete-refused",
            policy.ApprovedDurationDays is null
                ? "No retention period is approved. Physical delete is refused."
                : "Physical delete is not available through this foundation.");
}

public sealed record EvidenceItem(
    string Id,
    string Kind,
    string ContentHash,
    string Source,
    string? SupersedesId,
    DateTimeOffset RecordedAt);

public sealed class EvidenceChain
{
    private readonly List<EvidenceItem> _items = [];

    public IReadOnlyList<EvidenceItem> Items => _items;

    public EvidenceItem Append(string kind, string content, string source, string? supersedesId, DateTimeOffset recordedAt)
    {
        var item = new EvidenceItem(Guid.NewGuid().ToString("N"), kind, ContentHash.Sha256(content), source, supersedesId, recordedAt);
        _items.Add(item);
        return item;
    }
}

public sealed class SignedDocumentLink
{
    public SignedDocumentLink(string subjectUserId, DateTimeOffset expiresAt)
    {
        Id = Guid.NewGuid().ToString("N");
        SubjectUserId = subjectUserId;
        ExpiresAt = expiresAt;
    }

    public string Id { get; }

    public string SubjectUserId { get; }

    public DateTimeOffset ExpiresAt { get; }

    public bool Revoked { get; private set; }

    public void Revoke() => Revoked = true;

    public OperationResult Open(string userId, bool userRemoved, DateTimeOffset now)
    {
        if (userRemoved || !string.Equals(userId, SubjectUserId, StringComparison.Ordinal))
        {
            return OperationResult.Fail("link-refused", "The signed link is not honored for this user.");
        }

        if (Revoked || now >= ExpiresAt)
        {
            return OperationResult.Fail("link-expired", "The signed link is expired or revoked.");
        }

        return OperationResult.Ok("link-opened", Id);
    }
}
