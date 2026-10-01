using ClearHaul.Domain.Authorization;
using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Equipment;

public enum HistoryConfidence
{
    CarrierDeclared,
    DriverDocumented,
    ShipperConfirmed,
    FacilityConfirmed,
    ThirdPartyVerified,
    Disputed,
    SupersededByDocumentedCorrection
}

public sealed record EquipmentHistoryEntry(
    string Id,
    string TrailerId,
    string UnitNumber,
    string EquipmentType,
    string CargoCategory,
    bool HazmatCategory,
    bool FoodOrFeed,
    bool Allergen,
    bool RefuseOrAnimalProduct,
    bool SpillOrLeak,
    bool Contamination,
    bool RejectedLoad,
    string? RepairNote,
    string? CleaningNote,
    HistoryConfidence Confidence,
    string Source,
    string RecordedByUserId,
    string OrganizationId,
    DateTimeOffset RecordedAt,
    string? AmendsEntryId,
    string? AmendmentReason,
    string? EvidenceId);

public sealed class EquipmentHistoryBook
{
    private readonly List<EquipmentHistoryEntry> _entries = [];

    public IReadOnlyList<EquipmentHistoryEntry> Entries => _entries;

    public EquipmentHistoryEntry Append(EquipmentHistoryEntry entry)
    {
        if (entry.Confidence == HistoryConfidence.ThirdPartyVerified && entry.Source == "carrier")
        {
            throw new InvalidOperationException("Carrier-declared information cannot be stored as third-party verified.");
        }

        _entries.Add(entry);
        return entry;
    }

    public OperationResult Correct(
        string originalId,
        EquipmentHistoryEntry amendment,
        string reason,
        string evidenceId)
    {
        var original = _entries.FirstOrDefault(entry => entry.Id == originalId);
        if (original is null || string.IsNullOrWhiteSpace(reason) || string.IsNullOrWhiteSpace(evidenceId))
        {
            return OperationResult.Fail("history-correction-refused", "A correction needs the original entry, a reason, and evidence.");
        }

        if (amendment.AmendsEntryId != original.Id)
        {
            return OperationResult.Fail("history-correction-refused", "The correction must point at the original entry.");
        }

        var snapshot = original;
        _entries.Add(amendment);
        var stillThere = _entries.First(entry => entry.Id == originalId);
        if (!stillThere.Equals(snapshot))
        {
            return OperationResult.Fail("history-overwritten", "The original entry changed.");
        }

        return OperationResult.Ok("history-corrected", amendment.Id);
    }
}

public sealed record HistoryAccessEvent(
    string ShipmentId,
    string ActorUserId,
    string OrganizationId,
    string TrailerId,
    string Action,
    DateTimeOffset Timestamp);

public sealed record HistoryProjection(
    string TrailerId,
    string UnitNumber,
    string EquipmentType,
    string CargoCategory,
    bool HazmatCategory,
    bool FoodOrFeed,
    bool Allergen,
    bool RefuseOrAnimalProduct,
    bool SpillOrLeak,
    bool Contamination,
    bool RejectedLoad,
    string? RepairNote,
    string? CleaningNote,
    HistoryConfidence Confidence,
    string Source);

public static class EquipmentHistoryAccess
{
    public static OperationResult View(
        Actor actor,
        string shipperOrganizationId,
        IReadOnlySet<string> authorizedUserIds,
        string? carrierOrganizationId,
        string? assignedTrailerId,
        string requestedTrailerId,
        bool carrierSelected,
        DateTimeOffset? accessExpiresAt,
        DateTimeOffset now)
    {
        if (actor.Removed || !PermissionMatrix.Allows(actor, Permission.ReviewAssignedEquipment))
        {
            return OperationResult.Fail("history-hidden", "This user cannot view equipment history.");
        }

        if (!string.Equals(actor.OrganizationId, shipperOrganizationId, StringComparison.Ordinal)
            || !authorizedUserIds.Contains(actor.UserId))
        {
            return OperationResult.Fail("history-hidden", "Equipment history is limited to an authorized user on this shipment.");
        }

        if (!carrierSelected || string.IsNullOrWhiteSpace(carrierOrganizationId))
        {
            return OperationResult.Fail("history-hidden", "Equipment history stays hidden until a carrier is conditionally selected.");
        }

        if (string.IsNullOrWhiteSpace(assignedTrailerId) || !string.Equals(assignedTrailerId, requestedTrailerId, StringComparison.Ordinal))
        {
            return OperationResult.Fail("trailer-not-assigned", "History is available only for the trailer assigned to this shipment.");
        }

        if (accessExpiresAt is not null && now >= accessExpiresAt.Value)
        {
            return OperationResult.Fail("history-access-expired", "Shipment-scoped history access has expired.");
        }

        return OperationResult.Ok("history-visible", requestedTrailerId);
    }

    public static OperationResult SearchFleet() =>
        OperationResult.Fail("fleet-search-forbidden", "A shipper cannot search a carrier fleet.");

    public static HistoryProjection Project(EquipmentHistoryEntry entry) =>
        new(
            entry.TrailerId,
            entry.UnitNumber,
            entry.EquipmentType,
            entry.CargoCategory,
            entry.HazmatCategory,
            entry.FoodOrFeed,
            entry.Allergen,
            entry.RefuseOrAnimalProduct,
            entry.SpillOrLeak,
            entry.Contamination,
            entry.RejectedLoad,
            entry.RepairNote,
            entry.CleaningNote,
            entry.Confidence,
            entry.Source);
}

public sealed record WashoutEvidence(
    string TrailerId,
    string Provider,
    string Location,
    DateTimeOffset PerformedAt,
    string Method,
    string? ReceiptHash,
    string? CertificateHash,
    bool AppearsAltered,
    bool DriverAcknowledged,
    bool FacilityConfirmed);

public static class WashoutReview
{
    public static OperationResult Accept(WashoutEvidence evidence, string assignedTrailerId)
    {
        if (evidence.AppearsAltered)
        {
            return OperationResult.Fail("washout-altered", "An altered washout document is not accepted.");
        }

        if (!string.Equals(evidence.TrailerId, assignedTrailerId, StringComparison.Ordinal))
        {
            return OperationResult.Fail("washout-trailer-mismatch", "The washout document is for a different trailer.");
        }

        if (string.IsNullOrWhiteSpace(evidence.Method) || string.IsNullOrWhiteSpace(evidence.Provider))
        {
            return OperationResult.Fail("washout-incomplete", "Cleaning method and provider are required.");
        }

        return OperationResult.Ok("washout-accepted", evidence.TrailerId);
    }
}
