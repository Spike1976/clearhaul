using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Payments;

public enum PaymentState
{
    FundingRequested,
    FundingPending,
    FundingConfirmed,
    FundingFailed,
    FundsReserved,
    RateChangeProposed,
    RateChangeAccepted,
    AccessorialProposed,
    AccessorialAccepted,
    PartialRelease,
    FullRelease,
    Hold,
    Dispute,
    Refund,
    Chargeback,
    Payout,
    Reconciliation,
    FailedPayout,
    ManualReview
}

public sealed record FundingResult(bool Accepted, bool Simulated, string ProviderLabel, string Reference);

public interface IShipmentFundingPort
{
    FundingResult Request(string shipmentId, long amountCents);
}

public sealed class SandboxFundingAdapter : IShipmentFundingPort
{
    public FundingResult Request(string shipmentId, long amountCents)
    {
        if (amountCents <= 0)
        {
            return new FundingResult(false, true, "sandbox-not-a-bank", "rejected");
        }

        return new FundingResult(true, true, "sandbox-not-a-bank", "sim-" + shipmentId);
    }
}

public sealed record LedgerLine(string EventId, string Account, long SignedCents, bool Simulated);

public sealed class Ledger
{
    private readonly List<LedgerLine> _lines = [];
    private readonly HashSet<string> _events = [];

    public IReadOnlyList<LedgerLine> Lines => _lines;

    public long Balance(string account) => _lines.Where(line => line.Account == account).Sum(line => line.SignedCents);

    public OperationResult Post(string eventId, IReadOnlyList<(string Account, long SignedCents)> lines, bool simulated)
    {
        if (!simulated)
        {
            return OperationResult.Fail("production-funds-refused", "Production funding is not connected.");
        }

        if (_events.Contains(eventId))
        {
            return OperationResult.Ok("duplicate-ignored", eventId);
        }

        if (lines.Count == 0 || lines.Sum(line => line.SignedCents) != 0)
        {
            return OperationResult.Fail("unbalanced-ledger", "A ledger event must balance to zero.");
        }

        foreach (var line in lines)
        {
            _lines.Add(new LedgerLine(eventId, line.Account, line.SignedCents, simulated));
        }

        _events.Add(eventId);
        return OperationResult.Ok("ledger-posted", eventId);
    }
}

public enum AccessorialStatus
{
    Proposed,
    Accepted,
    Disputed
}

public sealed record AccessorialLine(string Id, string Kind, long AmountCents, AccessorialStatus Status, string EvidenceId);

public sealed class PaymentAccount
{
    private readonly List<AccessorialLine> _accessorials = [];
    private readonly HashSet<string> _webhooks = [];

    public PaymentAccount(string shipmentId)
    {
        ShipmentId = shipmentId;
        State = PaymentState.FundingRequested;
    }

    public string ShipmentId { get; }

    public PaymentState State { get; private set; }

    public IReadOnlyList<AccessorialLine> Accessorials => _accessorials;

    public OperationResult ApplyFunding(FundingResult result, Ledger ledger, long amountCents)
    {
        if (!result.Simulated || result.ProviderLabel != "sandbox-not-a-bank")
        {
            State = PaymentState.ManualReview;
            return OperationResult.Fail("funding-not-sandbox", "Only the labeled sandbox adapter can move this foundation.");
        }

        if (!result.Accepted)
        {
            State = PaymentState.FundingFailed;
            return OperationResult.Fail("funding-failed", result.Reference);
        }

        var posted = ledger.Post(
            result.Reference,
            [("ExternalSandboxClearing", -amountCents), ("ShipmentReserved", amountCents)],
            simulated: true);
        if (!posted.Succeeded)
        {
            State = PaymentState.FundingFailed;
            return posted;
        }

        State = PaymentState.FundsReserved;
        return OperationResult.Ok("funds-reserved", result.Reference);
    }

    public OperationResult ApplyWebhook(string eventId, FundingResult result, Ledger ledger, long amountCents)
    {
        if (_webhooks.Contains(eventId))
        {
            return OperationResult.Ok("duplicate-webhook-ignored", eventId);
        }

        var applied = ApplyFunding(result, ledger, amountCents);
        if (applied.Succeeded)
        {
            _webhooks.Add(eventId);
        }

        return applied;
    }

    public OperationResult ProposeAccessorial(string kind, long amountCents, string evidenceId)
    {
        _accessorials.Add(new AccessorialLine(Guid.NewGuid().ToString("N"), kind, amountCents, AccessorialStatus.Proposed, evidenceId));
        return OperationResult.Ok("accessorial-proposed", kind);
    }

    public OperationResult DisputeAccessorial(string accessorialId)
    {
        var index = _accessorials.FindIndex(line => line.Id == accessorialId);
        if (index < 0)
        {
            return OperationResult.Fail("accessorial-missing", accessorialId);
        }

        var current = _accessorials[index];
        _accessorials[index] = current with { Status = AccessorialStatus.Disputed };
        State = PaymentState.Dispute;
        return OperationResult.Ok("accessorial-disputed", accessorialId);
    }

    public OperationResult Release(Ledger ledger, long carrierPayoutCents, long platformChargeCents, bool disputed)
    {
        if (disputed || State == PaymentState.Dispute)
        {
            return OperationResult.Fail("release-blocked", "Disputed funds stay reserved.");
        }

        if (State != PaymentState.FundsReserved)
        {
            return OperationResult.Fail("release-blocked", "Funds are not reserved.");
        }

        var reserved = ledger.Balance("ShipmentReserved");
        if (reserved != carrierPayoutCents + platformChargeCents)
        {
            return OperationResult.Fail("release-blocked", "Reserved funds do not match the payout split.");
        }

        var posted = ledger.Post(
            "release-" + ShipmentId,
            [
                ("ShipmentReserved", -reserved),
                ("CarrierPayable", carrierPayoutCents),
                ("PlatformOperating", platformChargeCents)
            ],
            simulated: true);
        if (!posted.Succeeded)
        {
            State = PaymentState.FailedPayout;
            return posted;
        }

        State = PaymentState.FullRelease;
        return posted;
    }

    public OperationResult RequestShipperWithdrawal(bool awarded)
    {
        if (awarded && State is PaymentState.FundsReserved or PaymentState.Hold or PaymentState.Dispute)
        {
            State = PaymentState.ManualReview;
            return OperationResult.Fail(
                "withdrawal-refused",
                "Reserved shipment funds are not unilaterally withdrawable after award.");
        }

        return OperationResult.Fail("withdrawal-refused", "This foundation has no shipper withdrawal path.");
    }
}
