using ClearHaul.Domain.Authorization;
using ClearHaul.Domain.Equipment;
using ClearHaul.Domain.Loads;
using ClearHaul.Domain.Payments;
using ClearHaul.Domain.Records;
using ClearHaul.Domain.Rules;
using ClearHaul.Domain.Shipments;
using ClearHaul.Domain.Verification;

namespace ClearHaul.Domain.Tests;

public sealed class PhaseZeroTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 1, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Unverified_shipper_cannot_post()
    {
        var fx = new Fixture();
        var shipment = fx.Shipment(verified: false);
        var result = fx.Workflow.Submit(shipment, fx.ShipperAdmin, Now, "c1");
        Assert.False(result.Succeeded);
        Assert.Equal("shipper-unverified", result.Code);
        Assert.Equal(ShipmentState.Draft, shipment.State);
    }

    [Fact]
    public void Inactive_authority_blocks_selection_after_a_bid()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToBidding();
        fx.Carrier.AuthorityActive = false;
        var result = fx.Workflow.SelectCarrier(shipment, fx.ShipperAdmin, fx.Carrier, Now, "c2");
        Assert.Equal("authority-inactive", result.Code);
        Assert.Equal(ShipmentState.Bidding, shipment.State);
    }

    [Fact]
    public void Expired_insurance_blocks_pickup()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToAcknowledgement();
        fx.Carrier.InsuranceExpiresOn = new DateOnly(2026, 10, 10);
        var result = fx.Workflow.Acknowledge(shipment, fx.DriverActor, fx.Driver, fx.Carrier, Now, "c3");
        Assert.Equal("insurance-expired", result.Code);
        Assert.Equal(ShipmentState.DriverAcknowledgmentPending, shipment.State);
    }

    [Fact]
    public void Unfunded_load_cannot_be_awarded()
    {
        var fx = new Fixture();
        var shipment = fx.Shipment();
        fx.Workflow.Submit(shipment, fx.ShipperAdmin, Now, "c4");
        fx.Workflow.AcceptValidation(shipment, fx.ShipperAdmin, Now, "c4");
        var result = fx.Workflow.SelectCarrier(shipment, fx.ShipperAdmin, fx.Carrier, Now, "c4");
        Assert.Equal("unfunded-award", result.Code);
        Assert.Equal(ShipmentState.FundingPending, shipment.State);
    }

    [Fact]
    public void Unselected_shipper_cannot_view_equipment_history()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToBidding();
        var result = fx.Workflow.ViewAssignedHistory(shipment, fx.ShipperAdmin, fx.History, "TR-1", "view", Now, "c5");
        Assert.Equal("history-hidden", result.Code);
        Assert.Empty(shipment.HistoryAccesses);
    }

    [Fact]
    public void Selected_shipper_cannot_view_an_unassigned_trailer()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToAssigned();
        var result = fx.Workflow.ViewAssignedHistory(shipment, fx.ShipperAdmin, fx.History, "TR-2", "view", Now, "c6");
        Assert.Equal("trailer-not-assigned", result.Code);
        Assert.Equal("fleet-search-forbidden", EquipmentHistoryAccess.SearchFleet().Code);
    }

    [Fact]
    public void Substituting_a_trailer_keeps_the_selected_carrier()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToApproved();
        var carrierId = shipment.CarrierOrganizationId;
        var result = fx.Workflow.RequestSubstitution(shipment, fx.ShipperAdmin, "Shipper rejected the trailer.", Now, "c7");
        Assert.True(result.Succeeded);
        Assert.Equal(ShipmentState.EquipmentSubstitutionRequested, shipment.State);
        Assert.Equal(carrierId, shipment.CarrierOrganizationId);
        Assert.Equal("substitution-requested", EquipmentApprovalStateMachine.Project(shipment.State));
    }

    [Fact]
    public void Washout_for_a_different_trailer_is_rejected()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToCleaning();
        var result = fx.Workflow.SubmitWashout(shipment, fx.CarrierAdmin, SampleWashout("TR-9", altered: false), Now, "c8");
        Assert.Equal("washout-trailer-mismatch", result.Code);
        Assert.Equal(ShipmentState.CleaningRequested, shipment.State);
    }

    [Fact]
    public void Altered_washout_document_is_rejected()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToCleaning();
        var result = fx.Workflow.SubmitWashout(shipment, fx.DriverActor, SampleWashout("TR-1", altered: true), Now, "c9");
        Assert.Equal("washout-altered", result.Code);
        Assert.False(shipment.WashoutAccepted);
    }

    [Fact]
    public void Cargo_history_correction_appends_and_keeps_the_original()
    {
        var fx = new Fixture();
        var original = fx.History.Entries.Single();
        var correction = original with
        {
            Id = "amend-1",
            CargoCategory = "packaged-resin",
            Confidence = HistoryConfidence.SupersededByDocumentedCorrection,
            AmendsEntryId = original.Id,
            AmendmentReason = "Wrong category.",
            EvidenceId = "evidence-1"
        };
        var result = fx.History.Correct(original.Id, correction, "Wrong category.", "evidence-1");
        Assert.True(result.Succeeded);
        Assert.Equal("bagged-mineral", fx.History.Entries[0].CargoCategory);
        Assert.Equal(2, fx.History.Entries.Count);
        Assert.NotEqual(HistoryConfidence.ThirdPartyVerified, fx.History.Entries[0].Confidence);
    }

    [Fact]
    public void Downloaded_packet_remains_readable_offline()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToAcknowledgement();
        var before = shipment.Packet.ReadOffline();
        shipment.Packet.ReplaceServerToken("rev-server");
        var offline = shipment.Packet.ReadOffline();
        var sync = shipment.Packet.Sync(online: false);
        Assert.True(before.Succeeded);
        Assert.Equal(before.Detail, offline.Detail);
        Assert.Equal("offline", sync.Code);
    }

    [Fact]
    public void Hazmat_quantity_change_clears_confirmation_after_acknowledgement()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToHazmatAcknowledgement();
        Assert.Equal(shipment.MaterialRevision, shipment.AcknowledgedRevision);
        var result = fx.Workflow.ChangeHazmatQuantity(shipment, fx.ShipperAdmin, shipment.Version, 50, Now, "c12");
        Assert.True(result.Succeeded);
        Assert.NotEqual(shipment.MaterialRevision, shipment.AcknowledgedRevision);
        Assert.False(shipment.Tender.Hazmat!.ClassificationConfirmedByShipper);
        Assert.Equal(ShipmentState.DriverAcknowledgmentPending, shipment.State);
        Assert.False(shipment.Packet.MatchesServer);
    }

    [Fact]
    public void Live_placard_evaluation_does_not_invent_an_answer()
    {
        var fx = new Fixture();
        fx.Workflow.Rules.AddDraft(Fixture.PlacardPackage("placard-low", "fixture-above-threshold", "10"));
        var above = fx.Workflow.Rules.Evaluate("placard-low", "placard", new Dictionary<string, string> { ["quantity"] = "11" }, live: false);
        var below = fx.Workflow.Rules.Evaluate("placard-low", "placard", new Dictionary<string, string> { ["quantity"] = "10" }, live: false);
        var live = fx.Workflow.Rules.Evaluate("placard-low", "placard", new Dictionary<string, string> { ["quantity"] = "11" }, live: true);
        Assert.Equal("fixture-above-threshold", above.DecisionCode);
        Assert.Equal(ComplianceDecision.UnableOutcome, below.Outcome);
        Assert.Equal(ComplianceDecision.UnableOutcome, live.Outcome);
        Assert.Equal(ComplianceDecision.QualifiedReviewCode, live.DecisionCode);
        Assert.True(live.BlocksDispatch);
        Assert.True(live.HumanConfirmationRequired);
    }

    [Fact]
    public void Missing_hazmat_classification_blocks_dispatch()
    {
        var fx = new Fixture();
        var shipment = fx.HazmatShipment(confirmed: false);
        var decision = fx.Workflow.LiveHazmatDecision(shipment);
        Assert.Equal(ComplianceDecision.UnableOutcome, decision.Outcome);
        Assert.True(decision.BlocksDispatch);
        Assert.True(fx.Workflow.Submit(shipment, fx.ShipperAdmin, Now, "c14").Succeeded);
        Assert.True(fx.Workflow.AcceptValidation(shipment, fx.ShipperAdmin, Now, "c14").Succeeded);
        Assert.True(fx.Workflow.RecordFunding(shipment, fx.PaymentAdmin, new SandboxFundingAdapter(), Now, "c14").Succeeded);
        var publish = fx.Workflow.Publish(shipment, fx.ShipperAdmin, null, Now, "c14");
        Assert.Equal("hazmat-live-disabled", publish.Code);
    }

    [Fact]
    public void Incompatible_hazmat_facts_do_not_receive_an_invented_clearance()
    {
        var fx = new Fixture();
        var shipment = fx.HazmatShipment(confirmed: true);
        var decision = fx.Workflow.LiveHazmatDecision(shipment);
        Assert.Equal(ComplianceDecision.QualifiedReviewCode, decision.DecisionCode);
        Assert.True(decision.BlocksDispatch);
        Assert.False(fx.Workflow.Rules.AddDraft(new RulePackage("x", "1", new DateOnly(2026, 1, 1), null, true, false, [])).Succeeded);
    }

    [Fact]
    public void Missing_declared_permit_fails_validation()
    {
        var tender = Fixture.Tender() with { PermitDeclaredRequired = true, PermitReference = null };
        Assert.Contains(LoadTenderValidator.Errors(tender), error => error.Contains("permit", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Missing_shipper_required_endorsement_blocks_acknowledgement()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToAcknowledgement(requiredEndorsement: "shipper-stated-endorsement");
        var result = fx.Workflow.Acknowledge(shipment, fx.DriverActor, fx.Driver, fx.Carrier, Now, "c17");
        Assert.Equal("endorsement-missing", result.Code);
    }

    [Fact]
    public void Declared_route_restriction_without_a_permit_blocks_departure()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToLoaded(restriction: "TEST-REGION");
        var result = fx.Workflow.Depart(shipment, fx.DriverActor, Now, "c18");
        Assert.Equal("route-restriction", result.Code);
        Assert.Equal(ShipmentState.Loaded, shipment.State);
    }

    [Fact]
    public void Shipper_cannot_withdraw_reserved_funds_after_pickup()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToPickup();
        var reserved = fx.Workflow.Ledger.Balance("ShipmentReserved");
        var result = fx.Workflow.RequestWithdrawal(shipment, fx.ShipperAdmin, Now, "c19");
        Assert.Equal("withdrawal-refused", result.Code);
        Assert.Equal(reserved, fx.Workflow.Ledger.Balance("ShipmentReserved"));
        Assert.Equal(0, fx.Workflow.Ledger.Balance("PlatformOperating"));
    }

    [Fact]
    public void Detention_evidence_proposes_an_accessorial_without_releasing_funds()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToPickup();
        var proposed = shipment.Payment.ProposeAccessorial("detention", 2500, "detention-evidence");
        Assert.Equal("accessorial-proposed", proposed.Code);
        Assert.Equal(PaymentState.FundsReserved, shipment.Payment.State);
        Assert.Equal(AccessorialStatus.Proposed, shipment.Payment.Accessorials.Single().Status);
    }

    [Fact]
    public void Disputing_one_accessorial_leaves_the_other_line_alone()
    {
        var account = new PaymentAccount("ship-21");
        account.ProposeAccessorial("detention", 2500, "e1");
        account.ProposeAccessorial("layover", 1000, "e2");
        var disputed = account.Accessorials[0].Id;
        account.DisputeAccessorial(disputed);
        Assert.Equal(AccessorialStatus.Disputed, account.Accessorials[0].Status);
        Assert.Equal(AccessorialStatus.Proposed, account.Accessorials[1].Status);
    }

    [Fact]
    public void Driver_compensation_dispute_stays_off_the_public_view()
    {
        var disclosure = new CompensationDisclosure("percentage-of-linehaul", 1234500);
        var result = disclosure.Dispute(Actor("driver-1", "carrier-1", PlatformRole.Driver));
        Assert.True(result.Succeeded);
        Assert.True(disclosure.TrustReviewRequired);
        Assert.DoesNotContain("1234500", disclosure.PublicView(), StringComparison.Ordinal);
    }

    [Fact]
    public void Audit_events_cannot_be_deleted()
    {
        var audit = new AuditLog();
        audit.Append(new ActorContext("user", "org", "ReadOnlyAuditor"), "shipment.submitted", null, "draft", Now, "test", "s", null, null, null, "c23");
        var count = audit.Events.Count;
        var result = audit.TryDelete(audit.Events[0].Id, new ActorContext("admin", "platform", "SystemAdministrator"), Now, "c23");
        Assert.False(result.Succeeded);
        Assert.Contains(audit.Events, item => item.Id == audit.Events[0].Id && item.EventName == "shipment.submitted");
        Assert.True(audit.Events.Count > count);
    }

    [Fact]
    public void Suspended_carrier_cannot_bid()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToOpen();
        fx.Carrier.Suspended = true;
        var result = fx.Workflow.SubmitBid(shipment, fx.Dispatcher, fx.Carrier, 9000, Now, "c24");
        Assert.Equal("carrier-suspended", result.Code);
        Assert.Equal(ShipmentState.Open, shipment.State);
    }

    [Fact]
    public void Removed_user_cannot_open_an_old_signed_link()
    {
        var link = new SignedDocumentLink("user-25", Now.AddHours(2));
        var result = link.Open("user-25", userRemoved: true, Now.AddMinutes(5));
        Assert.Equal("link-refused", result.Code);
    }

    [Fact]
    public void Simultaneous_edits_keep_the_first_version()
    {
        var fx = new Fixture();
        var shipment = fx.Shipment();
        var first = fx.Workflow.EditCommodity(shipment, fx.ShipperEmployee, 0, "Bagged mineral", Now, "c26");
        var second = fx.Workflow.EditCommodity(shipment, fx.ShipperAdmin, 0, "Different commodity", Now, "c26");
        Assert.True(first.Succeeded);
        Assert.Equal("concurrent-edit", second.Code);
        Assert.Equal("Bagged mineral", shipment.Tender.CommodityDescription);
    }

    [Fact]
    public void Duplicate_payment_webhook_posts_once()
    {
        var ledger = new Ledger();
        var account = new PaymentAccount("ship-27");
        var funding = new FundingResult(true, true, "sandbox-not-a-bank", "sim-ship-27");
        var first = account.ApplyWebhook("evt-27", funding, ledger, 10000);
        var second = account.ApplyWebhook("evt-27", funding, ledger, 10000);
        Assert.True(first.Succeeded);
        Assert.Equal("duplicate-webhook-ignored", second.Code);
        Assert.Equal(10000, ledger.Balance("ShipmentReserved"));
        Assert.Equal(-10000, ledger.Balance("ExternalSandboxClearing"));
    }

    [Fact]
    public void Unavailable_verification_source_is_not_labeled_fresh()
    {
        var snapshot = new UnavailableVerificationSource().Retrieve("carrier-1", Now);
        Assert.Equal("unavailable-cached-result-is-not-a-live-check", snapshot.Freshness(Now, TimeSpan.FromMinutes(5)));
        Assert.NotEqual("fresh", snapshot.Freshness(Now, TimeSpan.FromMinutes(5)));
    }

    [Fact]
    public void An_active_load_keeps_its_bound_rule_package()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToOpen("rules-old");
        fx.Workflow.Rules.AddDraft(Fixture.RoutePackage("rules-new", "NEW"));
        var bound = fx.Workflow.Rules.Evaluate(shipment.BoundRulePackageId, "route", new Dictionary<string, string> { ["region"] = "TEST-REGION" }, live: false);
        var live = fx.Workflow.Rules.Evaluate(shipment.BoundRulePackageId, "route", new Dictionary<string, string> { ["region"] = "TEST-REGION" }, live: true);
        Assert.Equal("rules-old", shipment.BoundRulePackageId);
        Assert.Equal("OLD", bound.DecisionCode);
        Assert.Equal(ComplianceDecision.UnableOutcome, live.Outcome);
    }

    [Fact]
    public void Conflicted_investigator_cannot_take_the_report()
    {
        var report = new EnforcementCase("ship-30", "shipper-1", "carrier-1", "payment-misconduct", "Reserved funds were disputed.", "evidence-30");
        var conflict = report.AssignInvestigator(Actor("investigator-1", "shipper-1", PlatformRole.PlatformInvestigator));
        var clear = report.AssignInvestigator(Actor("investigator-2", "platform-1", PlatformRole.PlatformInvestigator));
        Assert.Equal("conflict-of-interest", conflict.Code);
        Assert.True(clear.Succeeded);
        Assert.Equal(EnforcementAction.PatternReview, report.Action);
    }

    [Fact]
    public void Catalog_defines_every_state_and_transition_contract()
    {
        foreach (var state in Enum.GetValues<ShipmentState>())
        {
            Assert.Contains(TransitionCatalog.All, contract => contract.From == state || contract.To == state);
        }

        foreach (var contract in TransitionCatalog.All)
        {
            Assert.False(string.IsNullOrWhiteSpace(contract.Actors));
            Assert.False(string.IsNullOrWhiteSpace(contract.RequiredData));
            Assert.False(string.IsNullOrWhiteSpace(contract.Validation));
            Assert.False(string.IsNullOrWhiteSpace(contract.AuditEvent));
            Assert.False(string.IsNullOrWhiteSpace(contract.NotificationEvent));
            Assert.False(string.IsNullOrWhiteSpace(contract.Reversal));
            Assert.False(string.IsNullOrWhiteSpace(contract.Failure));
        }
    }

    [Fact]
    public void Authorized_history_view_is_logged_and_expires()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToAssigned();
        var viewed = fx.Workflow.ViewAssignedHistory(shipment, fx.ShipperAdmin, fx.History, "TR-1", "view", Now, "c-view");
        var outsider = fx.Workflow.ViewAssignedHistory(shipment, fx.OtherShipper, fx.History, "TR-1", "view", Now, "c-view");
        var admin = fx.Workflow.ViewAssignedHistory(shipment, fx.SystemAdmin, fx.History, "TR-1", "view", Now, "c-view");
        Assert.True(viewed.Succeeded);
        Assert.Equal("history-hidden", outsider.Code);
        Assert.Equal("history-hidden", admin.Code);
        Assert.Single(shipment.HistoryAccesses);
        var frozen = shipment.ReliedUponHistoryHash;
        fx.Workflow.ApproveEquipment(shipment, fx.ShipperAdmin, fx.History, Now, "c-view");
        frozen = shipment.ReliedUponHistoryHash;
        fx.History.Correct(fx.History.Entries[0].Id, fx.History.Entries[0] with { Id = "later", AmendsEntryId = fx.History.Entries[0].Id, CargoCategory = "changed" }, "later", "evidence-later");
        Assert.Equal(frozen, shipment.ReliedUponHistoryHash);
        fx.Workflow.Cancel(shipment, fx.ShipperAdmin, "Cancelled before pickup.", Now, "c-view");
        fx.Workflow.Close(shipment, fx.PaymentAdmin, Now, "c-view");
        var expired = fx.Workflow.ViewAssignedHistory(shipment, fx.ShipperAdmin, fx.History, "TR-1", "view", Now, "c-view");
        Assert.Equal("history-access-expired", expired.Code);
    }

    [Fact]
    public void Sandbox_release_balances_and_keeps_operating_funds_separate_until_release()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToRelease();
        Assert.Equal(0, fx.Workflow.Ledger.Balance("PlatformOperating"));
        var released = fx.Workflow.Release(shipment, fx.PaymentAdmin, Now, "c-pay");
        Assert.True(released.Succeeded);
        Assert.Equal(ShipmentState.Paid, shipment.State);
        Assert.Equal(0, fx.Workflow.Ledger.Balance("ShipmentReserved"));
        Assert.Equal(9000, fx.Workflow.Ledger.Balance("CarrierPayable"));
        Assert.Equal(1000, fx.Workflow.Ledger.Balance("PlatformOperating"));
        Assert.Equal("not-sent-no-provider", fx.Notifications.Events[^1].DeliveryStatus);
    }

    [Fact]
    public void Notes_cannot_replace_a_structured_commodity()
    {
        var tender = Fixture.Tender() with { CommodityDescription = " ", Notes = "Bagged mineral in the notes." };
        Assert.Contains(LoadTenderValidator.Errors(tender), error => error.Contains("Notes cannot replace", StringComparison.Ordinal));
    }

    [Fact]
    public void Sensitive_actions_require_the_mfa_flag_and_retention_is_not_a_delete()
    {
        var fx = new Fixture();
        var shipment = fx.AdvanceToFundingPending();
        var withoutMfa = fx.PaymentAdmin with { MfaSatisfied = false };
        var funding = fx.Workflow.RecordFunding(shipment, withoutMfa, new SandboxFundingAdapter(), Now, "c-mfa");
        Assert.Equal("funding-refused", funding.Code);
        var policy = RetentionPolicy.Unset(RetentionClass.Audit);
        Assert.Null(policy.ApprovedDurationDays);
        Assert.False(RetentionActions.RequestPhysicalDelete(policy).Succeeded);
        Assert.Equal("no-universal-strap-count", SecurementGuidance.UniversalStrapCount().Code);
        Assert.Equal("3000", SecurementGuidance.Aggregate([1000, 2000]).Detail);
        Assert.Empty(PaperworkReminders.Visible([new PaperworkReminder("papers-in-cab", null, null)]));
    }

    [Fact]
    public void No_role_receives_every_permission()
    {
        foreach (var role in PermissionMatrix.AllRoles)
        {
            var actor = Actor("user", "org", role);
            Assert.False(PermissionMatrix.Allows(actor, Permission.SubmitBid) && PermissionMatrix.Allows(actor, Permission.RecordPaymentRelease) && PermissionMatrix.Allows(actor, Permission.ApproveEquipment));
        }
    }

    private static Actor Actor(string userId, string organizationId, params PlatformRole[] roles) =>
        new(userId, organizationId, roles.ToHashSet(), false, true);

    private static WashoutEvidence SampleWashout(string trailerId, bool altered) =>
        new(trailerId, "Synthetic Wash Facility", "Example Yard", Now, "standard-washout", "receipt-hash", "certificate-hash", altered, true, true);

    private sealed class Fixture
    {
        public Fixture()
        {
            Audit = new AuditLog();
            Notifications = new NotificationLog();
            Workflow = new ShipmentWorkflow(Audit, Notifications, new Ledger(), new RulesEngine());
            Workflow.Rules.AddDraft(RoutePackage("rules-old", "OLD"));
            Carrier = new CarrierAccount
            {
                OrganizationId = "carrier-1",
                AuthorityActive = true,
                Suspended = false,
                InsuranceExpiresOn = new DateOnly(2026, 12, 1)
            };
            Carrier.TrailerIds.Add("TR-1");
            Driver = new DriverProfile { UserId = "driver-1", CarrierOrganizationId = "carrier-1" };
            History = new EquipmentHistoryBook();
            History.Append(new EquipmentHistoryEntry(
                "hist-1", "TR-1", "UNIT-1", "dry-van", "bagged-mineral", false, false, false, false, false, false, false,
                null, "sweep", HistoryConfidence.CarrierDeclared, "carrier", "carrier-admin", "carrier-1", Now, null, null, null));
        }

        public AuditLog Audit { get; }

        public NotificationLog Notifications { get; }

        public ShipmentWorkflow Workflow { get; }

        public CarrierAccount Carrier { get; }

        public DriverProfile Driver { get; }

        public EquipmentHistoryBook History { get; }

        public Actor ShipperAdmin => Actor("shipper-admin", "shipper-1", PlatformRole.ShipperOrganizationAdministrator);

        public Actor ShipperEmployee => Actor("shipper-employee", "shipper-1", PlatformRole.ShipperEmployee);

        public Actor OtherShipper => Actor("other-shipper", "shipper-2", PlatformRole.ShipperOrganizationAdministrator);

        public Actor CarrierAdmin => Actor("carrier-admin", "carrier-1", PlatformRole.CarrierOrganizationAdministrator);

        public Actor Dispatcher => Actor("dispatcher-1", "carrier-1", PlatformRole.Dispatcher);

        public Actor DriverActor => Actor("driver-1", "carrier-1", PlatformRole.Driver);

        public Actor PaymentAdmin => Actor("pay-admin", "platform-1", PlatformRole.PaymentAndDisputeAdministrator);

        public Actor SystemAdmin => Actor("sys-admin", "platform-1", PlatformRole.SystemAdministrator);

        public Shipment Shipment(bool verified = true, string? endorsement = null, string? restriction = null, bool hazmatExercise = false, bool hazmat = false, bool confirmed = true)
        {
            var tender = hazmat ? HazmatTender(confirmed) : Tender(endorsement, restriction);
            return new Shipment("ship-1", "shipper-1", verified, tender, ["shipper-admin", "shipper-employee"], hazmatExercise);
        }

        public Shipment HazmatShipment(bool confirmed) => Shipment(hazmat: true, confirmed: confirmed);

        public Shipment AdvanceToFundingPending()
        {
            var shipment = Shipment();
            Assert.True(Workflow.Submit(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.AcceptValidation(shipment, ShipperAdmin, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToOpen(string? packageId = "rules-old")
        {
            var shipment = AdvanceToFundingPending();
            Assert.True(Workflow.RecordFunding(shipment, PaymentAdmin, new SandboxFundingAdapter(), Now, "setup").Succeeded);
            Assert.True(Workflow.Publish(shipment, ShipperAdmin, packageId, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToBidding()
        {
            var shipment = AdvanceToOpen();
            Assert.True(Workflow.SubmitBid(shipment, Dispatcher, Carrier, 9000, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToAssigned()
        {
            var shipment = AdvanceToBidding();
            Assert.True(Workflow.SelectCarrier(shipment, ShipperAdmin, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAssignment(shipment, Dispatcher, Now, "setup").Succeeded);
            Assert.True(Workflow.AssignTrailer(shipment, Dispatcher, Carrier, "TR-1", Now, "setup").Succeeded);
            Assert.True(Workflow.ReviewEquipment(shipment, ShipperAdmin, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToCleaning()
        {
            var shipment = AdvanceToAssigned();
            Assert.True(Workflow.RequestCleaning(shipment, ShipperEmployee, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToApproved()
        {
            var shipment = AdvanceToAssigned();
            Assert.True(Workflow.ApproveEquipment(shipment, ShipperAdmin, History, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToAcknowledgement(string? requiredEndorsement = null)
        {
            var shipment = requiredEndorsement is null
                ? AdvanceToApproved()
                : AdvanceToApprovedWithEndorsement(requiredEndorsement);
            Assert.True(Workflow.RequestAcknowledgement(shipment, Dispatcher, Driver, Now, "setup").Succeeded);
            shipment.Packet.Download();
            return shipment;
        }

        public Shipment AdvanceToHazmatAcknowledgement()
        {
            var shipment = Shipment(hazmatExercise: true, hazmat: true, confirmed: true);
            Assert.True(Workflow.Submit(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.AcceptValidation(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.RecordFunding(shipment, PaymentAdmin, new SandboxFundingAdapter(), Now, "setup").Succeeded);
            Assert.True(Workflow.Publish(shipment, ShipperAdmin, "rules-old", Now, "setup").Succeeded);
            Assert.True(shipment.DispatchBlocked);
            Assert.True(Workflow.SubmitBid(shipment, Dispatcher, Carrier, 9000, Now, "setup").Succeeded);
            Assert.True(Workflow.SelectCarrier(shipment, ShipperAdmin, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAssignment(shipment, Dispatcher, Now, "setup").Succeeded);
            Assert.True(Workflow.AssignTrailer(shipment, Dispatcher, Carrier, "TR-1", Now, "setup").Succeeded);
            Assert.True(Workflow.ReviewEquipment(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.ApproveEquipment(shipment, ShipperAdmin, History, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAcknowledgement(shipment, Dispatcher, Driver, Now, "setup").Succeeded);
            shipment.Packet.Download();
            var acknowledged = Workflow.Acknowledge(shipment, DriverActor, Driver, Carrier, Now, "setup");
            Assert.False(acknowledged.Succeeded);
            Assert.Equal(ShipmentState.DriverAcknowledgmentPending, shipment.State);
            return shipment;
        }

        public Shipment AdvanceToLoaded(string? restriction)
        {
            var shipment = new Shipment("ship-1", "shipper-1", true, Tender(restriction: restriction), ["shipper-admin", "shipper-employee"]);
            Assert.True(Workflow.Submit(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.AcceptValidation(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.RecordFunding(shipment, PaymentAdmin, new SandboxFundingAdapter(), Now, "setup").Succeeded);
            Assert.True(Workflow.Publish(shipment, ShipperAdmin, null, Now, "setup").Succeeded);
            Assert.True(Workflow.SubmitBid(shipment, Dispatcher, Carrier, 9000, Now, "setup").Succeeded);
            Assert.True(Workflow.SelectCarrier(shipment, ShipperAdmin, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAssignment(shipment, Dispatcher, Now, "setup").Succeeded);
            Assert.True(Workflow.AssignTrailer(shipment, Dispatcher, Carrier, "TR-1", Now, "setup").Succeeded);
            Assert.True(Workflow.ReviewEquipment(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.ApproveEquipment(shipment, ShipperAdmin, History, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAcknowledgement(shipment, Dispatcher, Driver, Now, "setup").Succeeded);
            shipment.Packet.Download();
            Assert.True(Workflow.Acknowledge(shipment, DriverActor, Driver, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.ArrivePickup(shipment, DriverActor, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.MarkLoaded(shipment, DriverActor, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToPickup()
        {
            var shipment = AdvanceToAcknowledgement();
            Assert.True(Workflow.Acknowledge(shipment, DriverActor, Driver, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.ArrivePickup(shipment, DriverActor, Carrier, Now, "setup").Succeeded);
            return shipment;
        }

        public Shipment AdvanceToRelease()
        {
            var shipment = AdvanceToPickup();
            Assert.True(Workflow.MarkLoaded(shipment, DriverActor, Now, "setup").Succeeded);
            Assert.True(Workflow.Depart(shipment, DriverActor, Now, "setup").Succeeded);
            Assert.True(Workflow.ArriveDelivery(shipment, DriverActor, Now, "setup").Succeeded);
            Assert.True(Workflow.MarkDelivered(shipment, DriverActor, "pod-1", Now, "setup").Succeeded);
            Assert.True(Workflow.SubmitDocuments(shipment, DriverActor, "doc-hash", Now, "setup").Succeeded);
            Assert.True(Workflow.AcceptDocuments(shipment, ShipperAdmin, Now, "setup").Succeeded);
            return shipment;
        }

        private Shipment AdvanceToApprovedWithEndorsement(string endorsement)
        {
            var shipment = new Shipment("ship-1", "shipper-1", true, Tender(endorsement), ["shipper-admin", "shipper-employee"]);
            Assert.True(Workflow.Submit(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.AcceptValidation(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.RecordFunding(shipment, PaymentAdmin, new SandboxFundingAdapter(), Now, "setup").Succeeded);
            Assert.True(Workflow.Publish(shipment, ShipperAdmin, "rules-old", Now, "setup").Succeeded);
            Assert.True(Workflow.SubmitBid(shipment, Dispatcher, Carrier, 9000, Now, "setup").Succeeded);
            Assert.True(Workflow.SelectCarrier(shipment, ShipperAdmin, Carrier, Now, "setup").Succeeded);
            Assert.True(Workflow.RequestAssignment(shipment, Dispatcher, Now, "setup").Succeeded);
            Assert.True(Workflow.AssignTrailer(shipment, Dispatcher, Carrier, "TR-1", Now, "setup").Succeeded);
            Assert.True(Workflow.ReviewEquipment(shipment, ShipperAdmin, Now, "setup").Succeeded);
            Assert.True(Workflow.ApproveEquipment(shipment, ShipperAdmin, History, Now, "setup").Succeeded);
            return shipment;
        }

        public static LoadTender Tender(string? endorsement = null, string? restriction = null) =>
            new(
                "Synthetic Shipper One",
                "Synthetic Bill To",
                Stop("pickup"),
                Stop("delivery"),
                "Bagged mineral",
                "dry-nonhazardous",
                10,
                "bag",
                10,
                1000000,
                "48x40x40",
                true,
                false,
                false,
                null,
                "dry-van",
                false,
                WashoutRequirement.None,
                "seal-required",
                new ScheduleWindow(new DateTimeOffset(2026, 10, 10, 15, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 10, 10, 18, 0, 0, TimeSpan.Zero), "America/Chicago"),
                new ScheduleWindow(new DateTimeOffset(2026, 10, 12, 15, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 10, 12, 18, 0, 0, TimeSpan.Zero), "America/Chicago"),
                new MoneyTerms(10000, 0, 0, 0, 0, 10000, 9000, 1000, "Detention terms stated.", "Layover terms stated.", "TONU terms stated.", "Cancellation terms stated.", "Release after accepted delivery documents."),
                false,
                null,
                endorsement is null ? [] : [endorsement],
                restriction,
                null);

        public static LoadTender HazmatTender(bool confirmed) =>
            Tender() with
            {
                HazmatDesignated = true,
                CommodityCategory = "designated-hazmat-exercise",
                Hazmat = new HazmatFacts(confirmed, confirmed ? "shipper-admin" : null, confirmed ? Now : null, null, null, null, null, 10, "kg")
            };

        public static RulePackage PlacardPackage(string id, string decision, string operand) =>
            new(id, "fixture-0", new DateOnly(2026, 1, 1), null, false, true,
            [
                new RuleDefinition("rule-" + id, "placard", "quantity", "gt", operand, decision, "fixture-citation")
            ]);

        public static RulePackage RoutePackage(string id, string decision) =>
            new(id, id, new DateOnly(2026, 1, 1), null, false, true,
            [
                new RuleDefinition("route-" + id, "route", "region", "eq", "TEST-REGION", decision, "fixture-citation")
            ]);

        private static LocationStop Stop(string role) =>
            new(role, "1 Example Road", "Example", "IL", "60601", "US", "unvalidated-placeholder", 41.88, -87.63, "America/Chicago", "main gate", "truck gate", "office", "08:00-16:00", "appointment", "Ask for the desk.");
    }
}
