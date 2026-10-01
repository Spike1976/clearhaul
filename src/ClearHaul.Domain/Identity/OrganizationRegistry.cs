using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Identity;

public enum QualificationOutcome
{
    Qualified,
    UnverifiedOrganization,
    Suspended,
    AuthorityInactive,
    InsuranceExpired,
    InsuranceMissing,
    ManualReviewOpen,
    EquipmentMissing,
    DriverMissing
}

public sealed record QualificationDecision(
    QualificationOutcome Outcome,
    bool MayParticipate,
    string Explanation,
    DateTimeOffset EvaluatedAt)
{
    public bool IsFreshExternalCheck => false;
}

public sealed record VerificationEvidence(
    string Id,
    string OrganizationId,
    string Kind,
    string Note,
    string RecordedByUserId,
    DateTimeOffset RecordedAt);

public sealed class OrganizationRecord
{
    public required string Id { get; init; }

    public required string Kind { get; init; }

    public required string LegalName { get; init; }

    public bool Verified { get; set; }

    public bool Suspended { get; set; }

    public bool AuthorityActive { get; set; }

    public DateOnly? InsuranceExpiresOn { get; set; }

    public bool ManualReviewOpen { get; set; }

    public List<string> DriverLabels { get; } = [];

    public List<string> EquipmentLabels { get; } = [];
}

public sealed class DirectoryUser
{
    public required string Email { get; init; }

    public required string UserId { get; init; }

    public required string OrganizationId { get; init; }

    public required string Role { get; init; }
}

public static class QualificationCalculator
{
    public static QualificationDecision Evaluate(OrganizationRecord organization, DateOnly day, DateTimeOffset now)
    {
        QualificationDecision Decide(QualificationOutcome outcome, bool mayParticipate, string explanation) =>
            new(outcome, mayParticipate, explanation + " Calculated from stored records. This is not a fresh external verification.", now);

        if (organization.Suspended)
        {
            return Decide(QualificationOutcome.Suspended, false, "The organization is suspended.");
        }

        if (!organization.Verified)
        {
            return Decide(QualificationOutcome.UnverifiedOrganization, false, "The organization is not verified.");
        }

        if (organization.ManualReviewOpen)
        {
            return Decide(QualificationOutcome.ManualReviewOpen, false, "Manual verification review is open.");
        }

        if (organization.Kind == "carrier" && !organization.AuthorityActive)
        {
            return Decide(QualificationOutcome.AuthorityInactive, false, "Carrier authority is inactive.");
        }

        if (organization.Kind == "carrier" && organization.InsuranceExpiresOn is null)
        {
            return Decide(QualificationOutcome.InsuranceMissing, false, "Insurance expiration is missing.");
        }

        if (organization.Kind == "carrier" && organization.InsuranceExpiresOn <= day)
        {
            return Decide(QualificationOutcome.InsuranceExpired, false, "Insurance is expired for the pickup date.");
        }

        if (organization.Kind == "carrier" && organization.DriverLabels.Count == 0)
        {
            return Decide(QualificationOutcome.DriverMissing, false, "No driver is on file.");
        }

        if (organization.Kind == "carrier" && organization.EquipmentLabels.Count == 0)
        {
            return Decide(QualificationOutcome.EquipmentMissing, false, "No equipment is on file.");
        }

        return Decide(QualificationOutcome.Qualified, true, "Stored records allow participation.");
    }
}

public sealed class OrganizationRegistry
{
    private readonly Dictionary<string, OrganizationRecord> _organizations = new(StringComparer.Ordinal);
    private readonly Dictionary<string, DirectoryUser> _users = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<VerificationEvidence> _evidence = [];

    public IReadOnlyCollection<VerificationEvidence> Evidence => _evidence;

    public OrganizationRecord? FindOrganization(string organizationId) =>
        _organizations.TryGetValue(organizationId, out var organization) ? organization : null;

    public DirectoryUser? FindUser(string email) =>
        _users.TryGetValue(email, out var user) ? user : null;

    public OperationResult RegisterOrganization(OrganizationRecord organization)
    {
        if (_organizations.ContainsKey(organization.Id))
        {
            return OperationResult.Fail("organization-exists", organization.Id);
        }

        _organizations.Add(organization.Id, organization);
        return OperationResult.Ok("organization-registered", organization.Id);
    }

    public OperationResult RegisterUser(DirectoryUser user)
    {
        if (!_organizations.ContainsKey(user.OrganizationId) || _users.ContainsKey(user.Email))
        {
            return OperationResult.Fail("user-rejected", user.Email);
        }

        _users.Add(user.Email, user);
        return OperationResult.Ok("user-registered", user.UserId);
    }

    public OperationResult AddDriver(string organizationId, string label)
    {
        if (!_organizations.TryGetValue(organizationId, out var organization) || string.IsNullOrWhiteSpace(label))
        {
            return OperationResult.Fail("driver-rejected", organizationId);
        }

        organization.DriverLabels.Add(label);
        return OperationResult.Ok("driver-added", label);
    }

    public OperationResult AddEquipment(string organizationId, string label)
    {
        if (!_organizations.TryGetValue(organizationId, out var organization) || string.IsNullOrWhiteSpace(label))
        {
            return OperationResult.Fail("equipment-rejected", organizationId);
        }

        organization.EquipmentLabels.Add(label);
        return OperationResult.Ok("equipment-added", label);
    }

    public OperationResult Suspend(string organizationId)
    {
        if (!_organizations.TryGetValue(organizationId, out var organization))
        {
            return OperationResult.Fail("organization-missing", organizationId);
        }

        organization.Suspended = true;
        return OperationResult.Ok("organization-suspended", organizationId);
    }

    public OperationResult RecordReview(string organizationId, string note, string recordedByUserId, DateTimeOffset now)
    {
        if (!_organizations.TryGetValue(organizationId, out var organization) || string.IsNullOrWhiteSpace(note))
        {
            return OperationResult.Fail("review-rejected", organizationId);
        }

        var evidence = new VerificationEvidence(Guid.NewGuid().ToString("N"), organizationId, "manual-review", note, recordedByUserId, now);
        _evidence.Add(evidence);
        organization.ManualReviewOpen = false;
        organization.Verified = true;
        return OperationResult.Ok("review-recorded", evidence.Id);
    }
}
