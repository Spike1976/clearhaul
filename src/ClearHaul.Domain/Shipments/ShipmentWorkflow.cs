using ClearHaul.Domain.Authorization;
using ClearHaul.Domain.Equipment;
using ClearHaul.Domain.Loads;
using ClearHaul.Domain.Payments;
using ClearHaul.Domain.Records;
using ClearHaul.Domain.Rules;

namespace ClearHaul.Domain.Shipments;

public enum ShipmentState
{
    Draft,
    ValidationRequired,
    FundingPending,
    Funded,
    Open,
    Bidding,
    CarrierConditionallySelected,
    EquipmentAssignmentPending,
    EquipmentAssigned,
    EquipmentReview,
    CleaningRequested,
    EquipmentSubstitutionRequested,
    EquipmentApproved,
    DriverAcknowledgmentPending,
    ReadyForPickup,
    AtPickup,
    Loaded,
    InTransit,
    AtDelivery,
    Delivered,
    DocumentReview,
    PaymentReleasePending,
    Paid,
    Disputed,
    Cancelled,
    Closed
}

public sealed record TransitionContract(
    string Name,
    ShipmentState From,
    ShipmentState To,
    string Actors,
    string RequiredData,
    string Validation,
    string AuditEvent,
    string NotificationEvent,
    string Reversal,
    string Failure);

public static class TransitionCatalog
{
    public static IReadOnlyList<TransitionContract> All { get; } =
    [
        Contract("submit", ShipmentState.Draft, ShipmentState.ValidationRequired, "Shipper administrator or employee", "Structured load tender", "Shipper organization is verified and the tender has no structural errors", "shipment.submitted", "shipment.validation-required", "Edit the draft and submit again", "Stay in draft and record the refusal"),
        Contract("accept-validation", ShipmentState.ValidationRequired, ShipmentState.FundingPending, "Shipper administrator", "Current tender", "Tender still validates", "shipment.validation-accepted", "shipment.funding-required", "Return to draft by cancelling before funding", "Stay in validation required"),
        Contract("record-funding", ShipmentState.FundingPending, ShipmentState.Funded, "Payment and dispute administrator", "Sandbox funding result", "Result is simulated, accepted, and balanced in the ledger", "payment.funding-confirmed", "shipment.funded", "Refund only before award and only through a reviewed payment state", "Stay in funding pending or mark funding failed"),
        Contract("publish", ShipmentState.Funded, ShipmentState.Open, "Shipper administrator or employee", "Funded tender", "Funds are reserved. Hazmat dispatch stays blocked when designated", "shipment.opened", "shipment.open", "Cancel before award", "Stay funded"),
        Contract("bid", ShipmentState.Open, ShipmentState.Bidding, "Carrier administrator or dispatcher", "Carrier identity and bid amount", "Authority active, not suspended, insurance covers the pickup date", "bid.submitted", "bid.received", "Bid remains recorded if later rejected", "Stay open or bidding"),
        Contract("select-carrier", ShipmentState.Bidding, ShipmentState.CarrierConditionallySelected, "Shipper administrator or employee", "Selected carrier bid", "Load is funded, carrier is active, insurance covers pickup", "carrier.conditionally-selected", "carrier.selected", "Cancel before pickup without a unilateral refund", "Stay bidding"),
        Contract("request-assignment", ShipmentState.CarrierConditionallySelected, ShipmentState.EquipmentAssignmentPending, "Carrier administrator or dispatcher", "Selected carrier", "Carrier is still the selected organization", "equipment.assignment-requested", "equipment.assignment-pending", "Assign a trailer or cancel before pickup", "Stay conditionally selected"),
        Contract("assign-trailer", ShipmentState.EquipmentAssignmentPending, ShipmentState.EquipmentAssigned, "Carrier administrator or dispatcher", "Trailer identifier owned by the carrier", "Trailer belongs to the selected carrier", "equipment.assigned", "equipment.assigned", "Request a substitution; do not cancel the carrier automatically", "Stay in the previous equipment state"),
        Contract("review-equipment", ShipmentState.EquipmentAssigned, ShipmentState.EquipmentReview, "Authorized shipper user", "Assigned trailer", "User is authorized for this shipment", "equipment.review-started", "equipment.review-started", "Reject or request substitution", "Stay assigned"),
        Contract("request-cleaning", ShipmentState.EquipmentReview, ShipmentState.CleaningRequested, "Shipper administrator or employee", "Washout requirement", "A cleaning requirement is set", "washout.requested", "washout.requested", "Submit matching evidence or substitute the trailer", "Stay in review"),
        Contract("submit-washout", ShipmentState.CleaningRequested, ShipmentState.EquipmentReview, "Carrier, driver, or washout facility", "Washout evidence", "Trailer matches and the document is not flagged altered", "washout.submitted", "washout.submitted", "Submit a new evidence record; do not overwrite the rejected one", "Stay in cleaning requested"),
        Contract("approve-equipment", ShipmentState.EquipmentReview, ShipmentState.EquipmentApproved, "Authorized shipper user", "Assigned trailer and relied-upon history snapshot", "Required washout evidence is accepted", "equipment.approved", "equipment.approved", "Request substitution without cancelling the carrier", "Stay in review"),
        Contract("request-substitution", ShipmentState.EquipmentApproved, ShipmentState.EquipmentSubstitutionRequested, "Shipper administrator or employee, or carrier administrator", "Reason and trailer to replace", "A carrier is still selected", "equipment.substitution-requested", "equipment.substitution-requested", "Assign a different trailer", "Stay approved"),
        Contract("request-acknowledgement", ShipmentState.EquipmentApproved, ShipmentState.DriverAcknowledgmentPending, "Carrier administrator or dispatcher", "Assigned driver", "Driver belongs to the selected carrier", "driver.ack-requested", "driver.ack-requested", "Assign a different driver before pickup", "Stay approved"),
        Contract("acknowledge", ShipmentState.DriverAcknowledgmentPending, ShipmentState.ReadyForPickup, "Assigned driver", "Downloaded packet and endorsements", "Packet matches the current revision, insurance covers pickup, and hazmat dispatch is not blocked", "driver.acknowledged", "driver.acknowledged", "A material change clears the acknowledgement", "Stay pending and keep dispatch blocked when required"),
        Contract("arrive-pickup", ShipmentState.ReadyForPickup, ShipmentState.AtPickup, "Assigned driver", "Arrival time", "Insurance still covers the pickup date", "shipment.at-pickup", "shipment.at-pickup", "Record an exception; do not delete the arrival", "Stay ready"),
        Contract("mark-loaded", ShipmentState.AtPickup, ShipmentState.Loaded, "Assigned driver", "Loaded confirmation", "Shipment is at pickup", "shipment.loaded", "shipment.loaded", "Record an exception record", "Stay at pickup"),
        Contract("depart", ShipmentState.Loaded, ShipmentState.InTransit, "Assigned driver", "Departure and route facts", "A declared route restriction has its permit reference", "shipment.in-transit", "shipment.in-transit", "Record an exception record", "Stay loaded"),
        Contract("arrive-delivery", ShipmentState.InTransit, ShipmentState.AtDelivery, "Assigned driver", "Arrival time", "Shipment is in transit", "shipment.at-delivery", "shipment.at-delivery", "Record an exception record", "Stay in transit"),
        Contract("mark-delivered", ShipmentState.AtDelivery, ShipmentState.Delivered, "Assigned driver", "Delivery evidence", "Evidence identifier is present", "shipment.delivered", "shipment.delivered", "Open a dispute or exception; do not delete delivery", "Stay at delivery"),
        Contract("submit-documents", ShipmentState.Delivered, ShipmentState.DocumentReview, "Driver or carrier administrator", "Document hash", "Delivery has been recorded", "documents.submitted", "documents.submitted", "Submit a superseding document", "Stay delivered"),
        Contract("accept-documents", ShipmentState.DocumentReview, ShipmentState.PaymentReleasePending, "Shipper administrator", "Accepted document set", "Documents were submitted", "documents.accepted", "payment.release-pending", "Dispute before release", "Stay in document review"),
        Contract("release", ShipmentState.PaymentReleasePending, ShipmentState.Paid, "Payment and dispute administrator", "Balanced sandbox ledger release", "No open dispute and reserved funds match the payout split", "payment.released", "payment.released", "Dispute or chargeback records a new ledger event", "Stay pending or mark failed payout"),
        Contract("dispute", ShipmentState.PaymentReleasePending, ShipmentState.Disputed, "Shipper or carrier administrator, or driver for a compensation disclosure", "Dispute category and evidence", "One named subject is disputed", "shipment.disputed", "shipment.disputed", "Record a resolution; do not delete the dispute", "Stay in the previous state when the dispute is incomplete"),
        Contract("cancel", ShipmentState.Draft, ShipmentState.Cancelled, "Shipper administrator", "Cancellation reason", "Pickup has not started", "shipment.cancelled", "shipment.cancelled", "Closed after review. Awarded funds are not unilaterally withdrawn", "Stay in the current state"),
        Contract("close", ShipmentState.Paid, ShipmentState.Closed, "Payment administrator or system administrator", "Resolution note", "Payment is released or the cancellation review is recorded", "shipment.closed", "shipment.closed", "Append a correction event. Do not delete the shipment", "Stay paid or cancelled"),
        Contract("material-change", ShipmentState.ReadyForPickup, ShipmentState.DriverAcknowledgmentPending, "Shipper administrator or employee", "Changed structured fact and expected version", "The editor holds the current version", "shipment.material-fact-changed", "driver.ack-required", "Driver acknowledges the new revision", "Keep the prior revision when the version conflicts")
    ];

    private static TransitionContract Contract(
        string name,
        ShipmentState from,
        ShipmentState to,
        string actors,
        string requiredData,
        string validation,
        string auditEvent,
        string notificationEvent,
        string reversal,
        string failure) =>
        new(name, from, to, actors, requiredData, validation, auditEvent, notificationEvent, reversal, failure);
}

public static class EquipmentApprovalStateMachine
{
    public static string Project(ShipmentState state) => state switch
    {
        ShipmentState.EquipmentAssignmentPending => "assignment-pending",
        ShipmentState.EquipmentAssigned => "assigned",
        ShipmentState.EquipmentReview => "review",
        ShipmentState.CleaningRequested => "cleaning-requested",
        ShipmentState.EquipmentSubstitutionRequested => "substitution-requested",
        ShipmentState.EquipmentApproved or ShipmentState.DriverAcknowledgmentPending or ShipmentState.ReadyForPickup => "approved",
        _ => "not-in-equipment-review"
    };
}

public sealed class CarrierAccount
{
    public required string OrganizationId { get; init; }

    public bool AuthorityActive { get; set; }

    public bool Suspended { get; set; }

    public DateOnly InsuranceExpiresOn { get; set; }

    public HashSet<string> TrailerIds { get; } = [];
}

public sealed class DriverProfile
{
    public required string UserId { get; init; }

    public required string CarrierOrganizationId { get; init; }

    public HashSet<string> Endorsements { get; } = [];
}

public sealed class DriverPacket
{
    public string ServerToken { get; private set; } = "rev-0";

    public string? LocalToken { get; private set; }

    public bool MatchesServer => LocalToken == ServerToken;

    public void Download() => LocalToken = ServerToken;

    public OperationResult ReadOffline() =>
        LocalToken is null
            ? OperationResult.Fail("packet-not-downloaded", "The driver has no local packet.")
            : OperationResult.Ok("packet-offline", LocalToken);

    public void ReplaceServerToken(string token) => ServerToken = token;

    public OperationResult Sync(bool online)
    {
        if (!online)
        {
            return OperationResult.Fail("offline", "Synchronization waits for connectivity.");
        }

        LocalToken = ServerToken;
        return OperationResult.Ok("packet-synced", LocalToken ?? "");
    }
}

public sealed record Bid(string CarrierOrganizationId, long AmountCents, DateTimeOffset SubmittedAt);

public sealed class Shipment
{
    public Shipment(
        string id,
        string shipperOrganizationId,
        bool shipperVerified,
        LoadTender tender,
        IEnumerable<string> authorizedShipperUserIds,
        bool hazmatExercise = false)
    {
        Id = id;
        ShipperOrganizationId = shipperOrganizationId;
        ShipperVerified = shipperVerified;
        Tender = tender;
        HazmatExercise = hazmatExercise;
        foreach (var userId in authorizedShipperUserIds)
        {
            AuthorizedShipperUserIds.Add(userId);
        }

        Payment = new PaymentAccount(id);
    }

    public string Id { get; }

    public ShipmentState State { get; private set; } = ShipmentState.Draft;

    public int Version { get; private set; }

    public LoadTender Tender { get; private set; }

    public bool ShipperVerified { get; }

    public string ShipperOrganizationId { get; }

    public HashSet<string> AuthorizedShipperUserIds { get; } = [];

    public string? CarrierOrganizationId { get; private set; }

    public string? AssignedTrailerId { get; private set; }

    public string? AssignedDriverUserId { get; private set; }

    public string? BoundRulePackageId { get; private set; }

    public bool WashoutAccepted { get; private set; }

    public int MaterialRevision { get; private set; }

    public int? AcknowledgedRevision { get; private set; }

    public DriverPacket Packet { get; } = new();

    public PaymentAccount Payment { get; }

    public DateTimeOffset? HistoryAccessExpiresAt { get; private set; }

    public string? ReliedUponHistoryHash { get; private set; }

    public List<Bid> Bids { get; } = [];

    public List<HistoryAccessEvent> HistoryAccesses { get; } = [];

    public bool DispatchBlocked { get; private set; }

    public bool HazmatExercise { get; }

    internal void Move(ShipmentState state) => State = state;

    internal void Bump() => Version++;

    internal void ReplaceTender(LoadTender tender) => Tender = tender;

    internal void Bind(string? packageId) => BoundRulePackageId = packageId;

    internal void Select(string carrierId) => CarrierOrganizationId = carrierId;

    internal void AssignTrailer(string trailerId)
    {
        AssignedTrailerId = trailerId;
        WashoutAccepted = false;
    }

    internal void AssignDriver(string driverId) => AssignedDriverUserId = driverId;

    internal void AcceptWashout() => WashoutAccepted = true;

    internal void Acknowledge(int revision) => AcknowledgedRevision = revision;

    internal void Revise() => MaterialRevision++;

    internal void BlockDispatch() => DispatchBlocked = true;

    internal void RememberHistory(string hash) => ReliedUponHistoryHash = hash;

    internal void ExpireHistory(DateTimeOffset at) => HistoryAccessExpiresAt = at;
}

public sealed class ShipmentWorkflow
{
    private readonly AuditLog _audit;
    private readonly NotificationLog _notifications;
    private readonly Ledger _ledger;
    private readonly RulesEngine _rules;

    public ShipmentWorkflow(AuditLog audit, NotificationLog notifications, Ledger ledger, RulesEngine rules)
    {
        _audit = audit;
        _notifications = notifications;
        _ledger = ledger;
        _rules = rules;
    }

    public Ledger Ledger => _ledger;

    public RulesEngine Rules => _rules;

    public OperationResult Submit(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (!Ready(shipment, ShipmentState.Draft, actor, Permission.SubmitLoadTender))
        {
            return Refuse(shipment, actor, "submit-refused", "The user cannot submit this draft.", now, correlationId);
        }

        if (!shipment.ShipperVerified)
        {
            return Refuse(shipment, actor, "shipper-unverified", "An unverified shipper cannot post a load.", now, correlationId);
        }

        var errors = LoadTenderValidator.Errors(shipment.Tender);
        if (errors.Count > 0)
        {
            return Refuse(shipment, actor, "tender-invalid", string.Join(" ", errors), now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.ValidationRequired, "shipment.submitted", "shipment.validation-required", now, correlationId, "Tender submitted.");
    }

    public OperationResult AcceptValidation(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.ValidationRequired || !actor.HasRole(PlatformRole.ShipperOrganizationAdministrator))
        {
            return Refuse(shipment, actor, "validation-refused", "Validation was not accepted.", now, correlationId);
        }

        var errors = LoadTenderValidator.Errors(shipment.Tender);
        if (errors.Count > 0)
        {
            return Refuse(shipment, actor, "tender-invalid", string.Join(" ", errors), now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.FundingPending, "shipment.validation-accepted", "shipment.funding-required", now, correlationId, "Tender accepted for funding.");
    }

    public OperationResult RecordFunding(Shipment shipment, Actor actor, IShipmentFundingPort port, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.FundingPending || !PermissionMatrix.Allows(actor, Permission.RecordPaymentRelease))
        {
            return Refuse(shipment, actor, "funding-refused", "Funding was not recorded.", now, correlationId);
        }

        var result = port.Request(shipment.Id, shipment.Tender.Money.TotalAmountCents);
        var applied = shipment.Payment.ApplyFunding(result, _ledger, shipment.Tender.Money.TotalAmountCents);
        if (!applied.Succeeded)
        {
            return Refuse(shipment, actor, applied.Code, applied.Detail, now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.Funded, "payment.funding-confirmed", "shipment.funded", now, correlationId, result.ProviderLabel);
    }

    public OperationResult Publish(Shipment shipment, Actor actor, string? rulePackageId, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.Funded || !PermissionMatrix.Allows(actor, Permission.PublishLoad))
        {
            return Refuse(shipment, actor, "publish-refused", "The load was not published.", now, correlationId);
        }

        if (shipment.Payment.State != PaymentState.FundsReserved)
        {
            return Refuse(shipment, actor, "unfunded", "An unfunded load cannot be published.", now, correlationId);
        }

        if (shipment.Tender.HazmatDesignated && !shipment.HazmatExercise)
        {
            return Refuse(shipment, actor, "hazmat-live-disabled", "Hazmat loads cannot be offered for live transportation.", now, correlationId);
        }

        shipment.Bind(rulePackageId);
        if (shipment.Tender.HazmatDesignated)
        {
            shipment.BlockDispatch();
        }

        return Advance(shipment, actor, ShipmentState.Open, "shipment.opened", "shipment.open", now, correlationId, "Load opened.");
    }

    public OperationResult SubmitBid(Shipment shipment, Actor actor, CarrierAccount carrier, long amountCents, DateTimeOffset now, string correlationId)
    {
        if (shipment.State is not (ShipmentState.Open or ShipmentState.Bidding) || !PermissionMatrix.Allows(actor, Permission.SubmitBid))
        {
            return Refuse(shipment, actor, "bid-refused", "The bid was not accepted.", now, correlationId);
        }

        if (!string.Equals(actor.OrganizationId, carrier.OrganizationId, StringComparison.Ordinal))
        {
            return Refuse(shipment, actor, "bid-refused", "The user is not acting for that carrier.", now, correlationId);
        }

        var gate = CarrierGate(carrier, DateOnly.FromDateTime(shipment.Tender.PickupWindow.Start.UtcDateTime));
        if (gate is not null)
        {
            return Refuse(shipment, actor, gate, "The carrier cannot bid.", now, correlationId);
        }

        shipment.Bids.Add(new Bid(carrier.OrganizationId, amountCents, now));
        if (shipment.State == ShipmentState.Open)
        {
            return Advance(shipment, actor, ShipmentState.Bidding, "bid.submitted", "bid.received", now, correlationId, "Bid submitted.");
        }

        Audit(shipment, actor, "bid.submitted", shipment.State.ToString(), shipment.State.ToString(), now, correlationId, "Another bid was submitted.");
        _notifications.Add("bid.received", shipment.Id, correlationId, now);
        return OperationResult.Ok("bid-submitted", carrier.OrganizationId);
    }

    public OperationResult SelectCarrier(Shipment shipment, Actor actor, CarrierAccount carrier, DateTimeOffset now, string correlationId)
    {
        if (!PermissionMatrix.Allows(actor, Permission.SelectCarrier))
        {
            return Refuse(shipment, actor, "selection-refused", "The carrier was not selected.", now, correlationId);
        }

        if (shipment.Payment.State != PaymentState.FundsReserved)
        {
            return Refuse(shipment, actor, "unfunded-award", "An unfunded load cannot be awarded.", now, correlationId);
        }

        if (shipment.State != ShipmentState.Bidding)
        {
            return Refuse(shipment, actor, "selection-refused", "The load is not ready for carrier selection.", now, correlationId);
        }

        if (shipment.Bids.All(bid => bid.CarrierOrganizationId != carrier.OrganizationId))
        {
            return Refuse(shipment, actor, "bid-missing", "That carrier has no bid.", now, correlationId);
        }

        var gate = CarrierGate(carrier, DateOnly.FromDateTime(shipment.Tender.PickupWindow.Start.UtcDateTime));
        if (gate is not null)
        {
            return Refuse(shipment, actor, gate, "The carrier is no longer eligible.", now, correlationId);
        }

        shipment.Select(carrier.OrganizationId);
        return Advance(shipment, actor, ShipmentState.CarrierConditionallySelected, "carrier.conditionally-selected", "carrier.selected", now, correlationId, "Carrier conditionally selected.");
    }

    public OperationResult RequestAssignment(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.CarrierConditionallySelected || !PermissionMatrix.Allows(actor, Permission.AssignEquipment))
        {
            return Refuse(shipment, actor, "assignment-refused", "Assignment did not start.", now, correlationId);
        }

        if (!string.Equals(actor.OrganizationId, shipment.CarrierOrganizationId, StringComparison.Ordinal))
        {
            return Refuse(shipment, actor, "assignment-refused", "Another carrier cannot assign equipment.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.EquipmentAssignmentPending, "equipment.assignment-requested", "equipment.assignment-pending", now, correlationId, "Assignment requested.");
    }

    public OperationResult AssignTrailer(Shipment shipment, Actor actor, CarrierAccount carrier, string trailerId, DateTimeOffset now, string correlationId)
    {
        if (shipment.State is not (ShipmentState.EquipmentAssignmentPending or ShipmentState.EquipmentSubstitutionRequested)
            || !PermissionMatrix.Allows(actor, Permission.AssignEquipment))
        {
            return Refuse(shipment, actor, "assign-refused", "The trailer was not assigned.", now, correlationId);
        }

        if (!carrier.TrailerIds.Contains(trailerId) || carrier.OrganizationId != shipment.CarrierOrganizationId)
        {
            return Refuse(shipment, actor, "trailer-not-owned", "The trailer is not on this carrier account.", now, correlationId);
        }

        shipment.AssignTrailer(trailerId);
        return Advance(shipment, actor, ShipmentState.EquipmentAssigned, "equipment.assigned", "equipment.assigned", now, correlationId, trailerId);
    }

    public OperationResult ReviewEquipment(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.EquipmentAssigned || !AuthorizedShipper(actor, shipment))
        {
            return Refuse(shipment, actor, "review-refused", "Equipment review did not start.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.EquipmentReview, "equipment.review-started", "equipment.review-started", now, correlationId, shipment.AssignedTrailerId);
    }

    public OperationResult RequestCleaning(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.EquipmentReview || !AuthorizedShipper(actor, shipment))
        {
            return Refuse(shipment, actor, "cleaning-refused", "Cleaning was not requested.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.CleaningRequested, "washout.requested", "washout.requested", now, correlationId, shipment.Tender.Washout.ToString());
    }

    public OperationResult SubmitWashout(Shipment shipment, Actor actor, WashoutEvidence evidence, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.CleaningRequested || !PermissionMatrix.Allows(actor, Permission.SubmitWashoutEvidence))
        {
            return Refuse(shipment, actor, "washout-refused", "Washout evidence was not accepted.", now, correlationId);
        }

        var accepted = WashoutReview.Accept(evidence, shipment.AssignedTrailerId ?? "");
        if (!accepted.Succeeded)
        {
            return Refuse(shipment, actor, accepted.Code, accepted.Detail, now, correlationId);
        }

        shipment.AcceptWashout();
        return Advance(shipment, actor, ShipmentState.EquipmentReview, "washout.submitted", "washout.submitted", now, correlationId, evidence.TrailerId);
    }

    public OperationResult ApproveEquipment(Shipment shipment, Actor actor, EquipmentHistoryBook history, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.EquipmentReview || !AuthorizedShipper(actor, shipment))
        {
            return Refuse(shipment, actor, "approval-refused", "Equipment was not approved.", now, correlationId);
        }

        if (shipment.Tender.Washout != WashoutRequirement.None && !shipment.WashoutAccepted)
        {
            return Refuse(shipment, actor, "washout-required", "Required cleaning evidence is missing.", now, correlationId);
        }

        var shown = history.Entries.Where(entry => entry.TrailerId == shipment.AssignedTrailerId).Select(entry => entry.Id).ToArray();
        shipment.RememberHistory(ContentHash.Sha256(string.Join("|", shown)));
        return Advance(shipment, actor, ShipmentState.EquipmentApproved, "equipment.approved", "equipment.approved", now, correlationId, shipment.ReliedUponHistoryHash);
    }

    public OperationResult RequestSubstitution(Shipment shipment, Actor actor, string reason, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.EquipmentApproved || string.IsNullOrWhiteSpace(reason))
        {
            return Refuse(shipment, actor, "substitution-refused", "Substitution was not requested.", now, correlationId);
        }

        if (!AuthorizedShipper(actor, shipment) && !PermissionMatrix.Allows(actor, Permission.AssignEquipment))
        {
            return Refuse(shipment, actor, "substitution-refused", "The user cannot request substitution.", now, correlationId);
        }

        var carrierId = shipment.CarrierOrganizationId;
        var moved = Advance(shipment, actor, ShipmentState.EquipmentSubstitutionRequested, "equipment.substitution-requested", "equipment.substitution-requested", now, correlationId, reason);
        if (moved.Succeeded && shipment.CarrierOrganizationId != carrierId)
        {
            return Refuse(shipment, actor, "carrier-changed", "Substitution changed the carrier.", now, correlationId);
        }

        return moved;
    }

    public OperationResult RequestAcknowledgement(Shipment shipment, Actor actor, DriverProfile driver, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.EquipmentApproved || !PermissionMatrix.Allows(actor, Permission.AssignEquipment))
        {
            return Refuse(shipment, actor, "driver-refused", "The driver was not assigned.", now, correlationId);
        }

        if (driver.CarrierOrganizationId != shipment.CarrierOrganizationId)
        {
            return Refuse(shipment, actor, "driver-refused", "The driver is not on the selected carrier.", now, correlationId);
        }

        shipment.AssignDriver(driver.UserId);
        return Advance(shipment, actor, ShipmentState.DriverAcknowledgmentPending, "driver.ack-requested", "driver.ack-requested", now, correlationId, driver.UserId);
    }

    public OperationResult Acknowledge(Shipment shipment, Actor actor, DriverProfile driver, CarrierAccount carrier, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.DriverAcknowledgmentPending || actor.UserId != shipment.AssignedDriverUserId)
        {
            return Refuse(shipment, actor, "ack-refused", "This driver cannot acknowledge the load.", now, correlationId);
        }

        if (!shipment.Packet.MatchesServer)
        {
            return Refuse(shipment, actor, "packet-stale", "The local packet does not match the current load.", now, correlationId);
        }

        var missing = shipment.Tender.RequiredDriverEndorsements.Where(item => !driver.Endorsements.Contains(item)).ToArray();
        if (missing.Length > 0)
        {
            return Refuse(shipment, actor, "endorsement-missing", "The driver record is missing a shipper-required endorsement.", now, correlationId);
        }

        var pickup = DateOnly.FromDateTime(shipment.Tender.PickupWindow.Start.UtcDateTime);
        if (carrier.InsuranceExpiresOn <= pickup)
        {
            return Refuse(shipment, actor, "insurance-expired", "Insurance does not cover the pickup.", now, correlationId);
        }

        shipment.Acknowledge(shipment.MaterialRevision);
        if (shipment.Tender.HazmatDesignated || shipment.DispatchBlocked)
        {
            var decision = LiveHazmatDecision(shipment);
            Audit(shipment, actor, "driver.acknowledged", shipment.State.ToString(), shipment.State.ToString(), now, correlationId, decision.DecisionCode);
            _notifications.Add("driver.acknowledged", shipment.Id, correlationId, now);
            return OperationResult.Fail(decision.DecisionCode, "Hazmat dispatch is blocked. The acknowledgement does not release the load.");
        }

        return Advance(shipment, actor, ShipmentState.ReadyForPickup, "driver.acknowledged", "driver.acknowledged", now, correlationId, "Driver acknowledged.");
    }

    public OperationResult ArrivePickup(Shipment shipment, Actor actor, CarrierAccount carrier, DateTimeOffset now, string correlationId)
    {
        if (!DriverState(shipment, actor, ShipmentState.ReadyForPickup))
        {
            return Refuse(shipment, actor, "arrival-refused", "Pickup arrival was not recorded.", now, correlationId);
        }

        if (carrier.InsuranceExpiresOn <= DateOnly.FromDateTime(shipment.Tender.PickupWindow.Start.UtcDateTime))
        {
            return Refuse(shipment, actor, "insurance-expired", "Insurance expired before pickup.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.AtPickup, "shipment.at-pickup", "shipment.at-pickup", now, correlationId, "Arrived at pickup.");
    }

    public OperationResult MarkLoaded(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId) =>
        DriverAdvance(shipment, actor, ShipmentState.AtPickup, ShipmentState.Loaded, "shipment.loaded", now, correlationId);

    public OperationResult Depart(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (!DriverState(shipment, actor, ShipmentState.Loaded))
        {
            return Refuse(shipment, actor, "depart-refused", "Departure was not recorded.", now, correlationId);
        }

        if (shipment.Tender.PermitDeclaredRequired && string.IsNullOrWhiteSpace(shipment.Tender.PermitReference))
        {
            return Refuse(shipment, actor, "permit-missing", "A declared permit reference is missing.", now, correlationId);
        }

        if (!string.IsNullOrWhiteSpace(shipment.Tender.DeclaredRouteRestrictionRegion)
            && string.IsNullOrWhiteSpace(shipment.Tender.PermitReference))
        {
            return Refuse(shipment, actor, "route-restriction", "A declared route restriction has no permit reference.", now, correlationId);
        }

        var route = _rules.Evaluate(
            shipment.BoundRulePackageId,
            "route",
            new Dictionary<string, string> { ["region"] = shipment.Tender.DeclaredRouteRestrictionRegion ?? "" },
            live: true);
        if (shipment.Tender.HazmatDesignated && route.BlocksDispatch)
        {
            return Refuse(shipment, actor, route.DecisionCode, route.Explanation, now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.InTransit, "shipment.in-transit", "shipment.in-transit", now, correlationId, "In transit.");
    }

    public OperationResult ArriveDelivery(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId) =>
        DriverAdvance(shipment, actor, ShipmentState.InTransit, ShipmentState.AtDelivery, "shipment.at-delivery", now, correlationId);

    public OperationResult MarkDelivered(Shipment shipment, Actor actor, string evidenceId, DateTimeOffset now, string correlationId)
    {
        if (!DriverState(shipment, actor, ShipmentState.AtDelivery) || string.IsNullOrWhiteSpace(evidenceId))
        {
            return Refuse(shipment, actor, "delivery-refused", "Delivery evidence is required.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.Delivered, "shipment.delivered", "shipment.delivered", now, correlationId, evidenceId);
    }

    public OperationResult SubmitDocuments(Shipment shipment, Actor actor, string documentHash, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.Delivered || string.IsNullOrWhiteSpace(documentHash))
        {
            return Refuse(shipment, actor, "documents-refused", "Documents were not submitted.", now, correlationId);
        }

        if (actor.UserId != shipment.AssignedDriverUserId && !PermissionMatrix.Allows(actor, Permission.SubmitBid))
        {
            return Refuse(shipment, actor, "documents-refused", "The user cannot submit these documents.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.DocumentReview, "documents.submitted", "documents.submitted", now, correlationId, documentHash);
    }

    public OperationResult AcceptDocuments(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.DocumentReview || !actor.HasRole(PlatformRole.ShipperOrganizationAdministrator))
        {
            return Refuse(shipment, actor, "documents-refused", "Documents were not accepted.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.PaymentReleasePending, "documents.accepted", "payment.release-pending", now, correlationId, "Documents accepted.");
    }

    public OperationResult Release(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State != ShipmentState.PaymentReleasePending || !PermissionMatrix.Allows(actor, Permission.RecordPaymentRelease))
        {
            return Refuse(shipment, actor, "release-refused", "Payment was not released.", now, correlationId);
        }

        var released = shipment.Payment.Release(
            _ledger,
            shipment.Tender.Money.CarrierPayoutCents,
            shipment.Tender.Money.PlatformChargeCents,
            disputed: false);
        if (!released.Succeeded)
        {
            return Refuse(shipment, actor, released.Code, released.Detail, now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.Paid, "payment.released", "payment.released", now, correlationId, "Sandbox release recorded.");
    }

    public OperationResult Dispute(Shipment shipment, Actor actor, string subject, DateTimeOffset now, string correlationId)
    {
        if (string.IsNullOrWhiteSpace(subject) || !PermissionMatrix.Allows(actor, Permission.OpenDispute))
        {
            return Refuse(shipment, actor, "dispute-refused", "The dispute was not opened.", now, correlationId);
        }

        if (shipment.State is ShipmentState.Draft or ShipmentState.Cancelled or ShipmentState.Closed)
        {
            return Refuse(shipment, actor, "dispute-refused", "This shipment cannot be disputed.", now, correlationId);
        }

        return Advance(shipment, actor, ShipmentState.Disputed, "shipment.disputed", "shipment.disputed", now, correlationId, subject);
    }

    public OperationResult Cancel(Shipment shipment, Actor actor, string reason, DateTimeOffset now, string correlationId)
    {
        if (!actor.HasRole(PlatformRole.ShipperOrganizationAdministrator) || string.IsNullOrWhiteSpace(reason))
        {
            return Refuse(shipment, actor, "cancel-refused", "The shipment was not cancelled.", now, correlationId);
        }

        if (shipment.State is ShipmentState.AtPickup or ShipmentState.Loaded or ShipmentState.InTransit or ShipmentState.AtDelivery or ShipmentState.Delivered or ShipmentState.Paid or ShipmentState.Closed)
        {
            return Refuse(shipment, actor, "cancel-refused", "Pickup has started. Cancellation does not withdraw reserved funds.", now, correlationId);
        }

        if (Awarded(shipment.State))
        {
            shipment.Payment.RequestShipperWithdrawal(awarded: true);
        }

        return Advance(shipment, actor, ShipmentState.Cancelled, "shipment.cancelled", "shipment.cancelled", now, correlationId, reason);
    }

    public OperationResult Close(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        if (shipment.State is not (ShipmentState.Paid or ShipmentState.Cancelled))
        {
            return Refuse(shipment, actor, "close-refused", "The shipment is not ready to close.", now, correlationId);
        }

        if (!PermissionMatrix.Allows(actor, Permission.RecordPaymentRelease)
            && !PermissionMatrix.Allows(actor, Permission.ManageEnforcement))
        {
            return Refuse(shipment, actor, "close-refused", "The user cannot close the shipment.", now, correlationId);
        }

        shipment.ExpireHistory(now);
        return Advance(shipment, actor, ShipmentState.Closed, "shipment.closed", "shipment.closed", now, correlationId, "Closed.");
    }

    public OperationResult ChangeHazmatQuantity(Shipment shipment, Actor actor, int expectedVersion, long quantity, DateTimeOffset now, string correlationId)
    {
        if (expectedVersion != shipment.Version)
        {
            return Refuse(shipment, actor, "concurrent-edit", "Another user already changed this load.", now, correlationId);
        }

        if (!PermissionMatrix.Allows(actor, Permission.SubmitLoadTender) || !shipment.Tender.HazmatDesignated || shipment.Tender.Hazmat is null)
        {
            return Refuse(shipment, actor, "hazmat-change-refused", "Hazmat quantity was not changed.", now, correlationId);
        }

        var prior = shipment.State;
        var facts = shipment.Tender.Hazmat with
        {
            Quantity = quantity,
            ClassificationConfirmedByShipper = false,
            ConfirmingUserId = null,
            ConfirmedAt = null
        };
        shipment.ReplaceTender(shipment.Tender with { Hazmat = facts });
        shipment.Revise();
        shipment.Packet.ReplaceServerToken("rev-" + shipment.MaterialRevision);
        shipment.Bump();
        shipment.BlockDispatch();
        if (prior == ShipmentState.ReadyForPickup)
        {
            shipment.Move(ShipmentState.DriverAcknowledgmentPending);
        }

        Audit(shipment, actor, "shipment.material-fact-changed", prior.ToString(), shipment.State.ToString(), now, correlationId, "Hazmat quantity changed. Confirmation was cleared.");
        _notifications.Add("driver.ack-required", shipment.Id, correlationId, now);
        return OperationResult.Ok("hazmat-quantity-changed", quantity.ToString());
    }

    public OperationResult EditCommodity(Shipment shipment, Actor actor, int expectedVersion, string description, DateTimeOffset now, string correlationId)
    {
        if (expectedVersion != shipment.Version)
        {
            return Refuse(shipment, actor, "concurrent-edit", "Another user already changed this load.", now, correlationId);
        }

        if (!PermissionMatrix.Allows(actor, Permission.SubmitLoadTender) || string.IsNullOrWhiteSpace(description))
        {
            return Refuse(shipment, actor, "edit-refused", "The tender was not edited.", now, correlationId);
        }

        shipment.ReplaceTender(shipment.Tender with { CommodityDescription = description });
        shipment.Bump();
        Audit(shipment, actor, "tender.edited", expectedVersion.ToString(), shipment.Version.ToString(), now, correlationId, "Commodity description edited.");
        return OperationResult.Ok("tender-edited", description);
    }

    public OperationResult ViewAssignedHistory(
        Shipment shipment,
        Actor actor,
        EquipmentHistoryBook history,
        string trailerId,
        string action,
        DateTimeOffset now,
        string correlationId)
    {
        var decision = EquipmentHistoryAccess.View(
            actor,
            shipment.ShipperOrganizationId,
            shipment.AuthorizedShipperUserIds,
            shipment.CarrierOrganizationId,
            shipment.AssignedTrailerId,
            trailerId,
            shipment.CarrierOrganizationId is not null,
            shipment.HistoryAccessExpiresAt,
            now);
        if (!decision.Succeeded)
        {
            return Refuse(shipment, actor, decision.Code, decision.Detail, now, correlationId);
        }

        shipment.HistoryAccesses.Add(new HistoryAccessEvent(shipment.Id, actor.UserId, actor.OrganizationId, trailerId, action, now));
        Audit(shipment, actor, "equipment-history." + action, null, trailerId, now, correlationId, "Shipment-scoped history access.");
        return OperationResult.Ok("history-" + action, trailerId);
    }

    public OperationResult RequestWithdrawal(Shipment shipment, Actor actor, DateTimeOffset now, string correlationId)
    {
        var result = shipment.Payment.RequestShipperWithdrawal(Awarded(shipment.State) || shipment.State is ShipmentState.AtPickup or ShipmentState.Loaded or ShipmentState.InTransit);
        return Refuse(shipment, actor, result.Code, result.Detail, now, correlationId);
    }

    public ComplianceDecision LiveHazmatDecision(Shipment shipment)
    {
        if (!shipment.Tender.HazmatDesignated)
        {
            return ComplianceDecision.NotApplicable("This tender is not designated hazardous.");
        }

        var hazmat = shipment.Tender.Hazmat;
        var facts = new Dictionary<string, string>
        {
            ["classificationConfirmed"] = hazmat?.ClassificationConfirmedByShipper.ToString() ?? "false",
            ["quantity"] = hazmat?.Quantity?.ToString() ?? ""
        };
        if (hazmat is null || !hazmat.ClassificationConfirmedByShipper)
        {
            return ComplianceDecision.Unable("Hazmat classification is missing a shipper confirmation.", facts.Select(pair => pair.Key + "=" + pair.Value).ToArray(), true);
        }

        return _rules.Evaluate(shipment.BoundRulePackageId, "placard", facts, live: true);
    }

    private OperationResult DriverAdvance(Shipment shipment, Actor actor, ShipmentState from, ShipmentState to, string eventName, DateTimeOffset now, string correlationId)
    {
        if (!DriverState(shipment, actor, from))
        {
            return Refuse(shipment, actor, "transition-refused", "The driver transition was refused.", now, correlationId);
        }

        return Advance(shipment, actor, to, eventName, eventName, now, correlationId, to.ToString());
    }

    private static bool DriverState(Shipment shipment, Actor actor, ShipmentState expected) =>
        shipment.State == expected && actor.UserId == shipment.AssignedDriverUserId;

    private static bool Awarded(ShipmentState state) => state is
        ShipmentState.CarrierConditionallySelected
        or ShipmentState.EquipmentAssignmentPending
        or ShipmentState.EquipmentAssigned
        or ShipmentState.EquipmentReview
        or ShipmentState.CleaningRequested
        or ShipmentState.EquipmentSubstitutionRequested
        or ShipmentState.EquipmentApproved
        or ShipmentState.DriverAcknowledgmentPending
        or ShipmentState.ReadyForPickup
        or ShipmentState.AtPickup
        or ShipmentState.Loaded
        or ShipmentState.InTransit
        or ShipmentState.AtDelivery
        or ShipmentState.Delivered
        or ShipmentState.DocumentReview
        or ShipmentState.PaymentReleasePending
        or ShipmentState.Paid
        or ShipmentState.Disputed;

    private static string? CarrierGate(CarrierAccount carrier, DateOnly pickup)
    {
        if (carrier.Suspended)
        {
            return "carrier-suspended";
        }

        if (!carrier.AuthorityActive)
        {
            return "authority-inactive";
        }

        if (carrier.InsuranceExpiresOn <= pickup)
        {
            return "insurance-expired";
        }

        return null;
    }

    private static bool AuthorizedShipper(Actor actor, Shipment shipment) =>
        PermissionMatrix.Allows(actor, Permission.ReviewAssignedEquipment)
        && actor.OrganizationId == shipment.ShipperOrganizationId
        && shipment.AuthorizedShipperUserIds.Contains(actor.UserId);

    private static bool Ready(Shipment shipment, ShipmentState expected, Actor actor, Permission permission) =>
        shipment.State == expected && PermissionMatrix.Allows(actor, permission);

    private OperationResult Advance(Shipment shipment, Actor actor, ShipmentState to, string auditEvent, string notification, DateTimeOffset now, string correlationId, string? reason)
    {
        var from = shipment.State;
        shipment.Move(to);
        Audit(shipment, actor, auditEvent, from.ToString(), to.ToString(), now, correlationId, reason);
        _notifications.Add(notification, shipment.Id, correlationId, now);
        return OperationResult.Ok(auditEvent, to.ToString());
    }

    private OperationResult Refuse(Shipment shipment, Actor actor, string code, string detail, DateTimeOffset now, string correlationId)
    {
        Audit(shipment, actor, "shipment.transition-refused", shipment.State.ToString(), code, now, correlationId, detail);
        return OperationResult.Fail(code, detail);
    }

    private void Audit(Shipment shipment, Actor actor, string eventName, string? originalValue, string? newValue, DateTimeOffset now, string correlationId, string? reason)
    {
        _audit.Append(
            new ActorContext(actor.UserId, actor.OrganizationId, actor.Roles.First().ToString()),
            eventName,
            originalValue,
            newValue,
            now,
            reason ?? eventName,
            shipment.Id,
            shipment.AssignedTrailerId,
            null,
            shipment.BoundRulePackageId,
            correlationId);
    }
}
