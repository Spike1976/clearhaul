using ClearHaul.Domain.Authorization;
using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Verification;

public sealed record VerificationSnapshot(
    string SubjectId,
    string SourceName,
    DateTimeOffset RetrievedAt,
    DateTimeOffset? SourceTimestamp,
    string Result,
    string RawHash,
    bool ManualReviewRequired,
    bool SourceAvailable)
{
    public string Freshness(DateTimeOffset now, TimeSpan maxAge)
    {
        if (!SourceAvailable)
        {
            return "unavailable-cached-result-is-not-a-live-check";
        }

        if (now - RetrievedAt > maxAge)
        {
            return "cached-not-a-live-check";
        }

        return "fresh";
    }
}

public interface IVerificationSource
{
    string SourceName { get; }

    VerificationSnapshot Retrieve(string subjectId, DateTimeOffset now);
}

public sealed class UnavailableVerificationSource : IVerificationSource
{
    public string SourceName => "unconfigured-verification-source";

    public VerificationSnapshot Retrieve(string subjectId, DateTimeOffset now) =>
        new(subjectId, SourceName, now, null, "unavailable", ContentHash.Sha256("unavailable"), true, false);
}

public sealed class BankChangeRequest
{
    public BankChangeRequest(DateTimeOffset holdUntil) => HoldUntil = holdUntil;

    public DateTimeOffset HoldUntil { get; }

    public bool ReviewerApproved { get; private set; }

    public string Status { get; private set; } = "hold";

    public void ApproveReview() => ReviewerApproved = true;

    public OperationResult Activate(DateTimeOffset now)
    {
        if (!ReviewerApproved || now < HoldUntil)
        {
            return OperationResult.Fail("bank-change-hold", "The bank reference stays on hold.");
        }

        Status = "active";
        return OperationResult.Ok("bank-change-active", "active");
    }
}

public sealed class CompensationDisclosure
{
    public CompensationDisclosure(string method, long? privateAmountCents)
    {
        Method = method;
        PrivateAmountCents = privateAmountCents;
    }

    public string Method { get; }

    public long? PrivateAmountCents { get; }

    public bool DriverDisputed { get; private set; }

    public bool TrustReviewRequired { get; private set; }

    public string PublicView() => Method;

    public OperationResult Dispute(Actor actor)
    {
        if (!actor.HasRole(PlatformRole.Driver) || actor.Removed)
        {
            return OperationResult.Fail("disclosure-dispute-refused", "The driver did not dispute the disclosure.");
        }

        DriverDisputed = true;
        TrustReviewRequired = true;
        return OperationResult.Ok("disclosure-disputed", Method);
    }
}

public enum EnforcementAction
{
    PatternReview,
    Warning,
    EducationRequired,
    EnhancedVerification,
    TransactionLimit,
    FundingReserve,
    EquipmentBlock,
    TemporarySuspension,
    PermanentRemoval,
    AuthorityReferralReview
}

public sealed class EnforcementCase
{
    public EnforcementCase(string shipmentId, string shipperOrganizationId, string? carrierOrganizationId, string category, string narrative, string evidenceId)
    {
        ShipmentId = shipmentId;
        ShipperOrganizationId = shipperOrganizationId;
        CarrierOrganizationId = carrierOrganizationId;
        Category = category;
        Narrative = narrative;
        EvidenceId = evidenceId;
        Action = EnforcementAction.PatternReview;
    }

    public string ShipmentId { get; }

    public string ShipperOrganizationId { get; }

    public string? CarrierOrganizationId { get; }

    public string Category { get; }

    public string Narrative { get; }

    public string EvidenceId { get; }

    public string? InvestigatorUserId { get; private set; }

    public EnforcementAction Action { get; private set; }

    public OperationResult AssignInvestigator(Actor investigator)
    {
        if (!PermissionMatrix.Allows(investigator, Permission.InvestigateReport))
        {
            return OperationResult.Fail("investigator-refused", "The user cannot investigate this report.");
        }

        if (investigator.OrganizationId == ShipperOrganizationId || investigator.OrganizationId == CarrierOrganizationId)
        {
            return OperationResult.Fail("conflict-of-interest", "The investigator has a conflict of interest.");
        }

        InvestigatorUserId = investigator.UserId;
        return OperationResult.Ok("investigator-assigned", investigator.UserId);
    }

    public OperationResult Apply(EnforcementAction action)
    {
        if (action == EnforcementAction.PermanentRemoval && InvestigatorUserId is null)
        {
            return OperationResult.Fail("removal-refused", "Permanent removal requires an assigned investigator and is not automatic.");
        }

        Action = action;
        return OperationResult.Ok("enforcement-recorded", action.ToString());
    }
}

public static class SecurementGuidance
{
    public static OperationResult Aggregate(IReadOnlyList<long> workingLoadLimits)
    {
        if (workingLoadLimits.Count == 0 || workingLoadLimits.Any(limit => limit <= 0))
        {
            return OperationResult.Fail("securement-incomplete", "Working-load limits are incomplete.");
        }

        return OperationResult.Ok("aggregate-only", workingLoadLimits.Sum().ToString());
    }

    public static OperationResult UniversalStrapCount() =>
        OperationResult.Fail("no-universal-strap-count", "This software does not prescribe a strap count.");
}

public sealed record PaperworkReminder(string Topic, string? CitationId, string? RuleVersion);

public static class PaperworkReminders
{
    public static IReadOnlyList<PaperworkReminder> Visible(IEnumerable<PaperworkReminder> reminders) =>
        reminders.Where(reminder => !string.IsNullOrWhiteSpace(reminder.CitationId) && !string.IsNullOrWhiteSpace(reminder.RuleVersion)).ToArray();
}
