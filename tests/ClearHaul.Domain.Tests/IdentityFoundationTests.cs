using ClearHaul.Domain.Identity;
using ClearHaul.Domain.Records;

namespace ClearHaul.Domain.Tests;

public sealed class IdentityFoundationTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 2, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Audit_events_form_a_hash_chain()
    {
        var audit = new AuditLog();
        var actor = new ActorContext("user-1", "org-1", "ReadOnlyAuditor");
        var first = audit.Append(actor, "LOAD_DRAFT_CREATED", null, "draft", Now, "created", "ship-1", null, null, null, "c1");
        var second = audit.Append(actor, "LOAD_DRAFT_UPDATED", "draft", "draft", Now, "updated", "ship-1", null, null, null, "c2");
        Assert.Null(first.PreviousEventHash);
        Assert.False(string.IsNullOrWhiteSpace(first.EventHash));
        Assert.Equal(first.EventHash, second.PreviousEventHash);
        Assert.NotEqual(first.EventHash, second.EventHash);
    }

    [Fact]
    public void Qualified_carrier_can_participate_and_a_suspended_carrier_cannot()
    {
        var ready = Carrier("carrier-ok", suspended: false, verified: true, review: false, insurance: new DateOnly(2026, 12, 1), driver: true, equipment: true);
        var suspended = Carrier("carrier-suspended", suspended: true, verified: true, review: false, insurance: new DateOnly(2026, 12, 1), driver: true, equipment: true);
        var expired = Carrier("carrier-expired", suspended: false, verified: true, review: false, insurance: new DateOnly(2026, 10, 1), driver: true, equipment: true);
        var review = Carrier("carrier-review", suspended: false, verified: true, review: true, insurance: new DateOnly(2026, 12, 1), driver: true, equipment: true);
        var day = new DateOnly(2026, 10, 10);
        Assert.True(QualificationCalculator.Evaluate(ready, day, Now).MayParticipate);
        Assert.Equal(QualificationOutcome.Suspended, QualificationCalculator.Evaluate(suspended, day, Now).Outcome);
        Assert.Equal(QualificationOutcome.InsuranceExpired, QualificationCalculator.Evaluate(expired, day, Now).Outcome);
        Assert.Equal(QualificationOutcome.ManualReviewOpen, QualificationCalculator.Evaluate(review, day, Now).Outcome);
        Assert.False(QualificationCalculator.Evaluate(ready, day, Now).IsFreshExternalCheck);
    }

    [Fact]
    public void Manual_review_appends_evidence_and_does_not_remove_the_organization()
    {
        var registry = new OrganizationRegistry();
        registry.RegisterOrganization(new OrganizationRecord
        {
            Id = "carrier-review",
            Kind = "carrier",
            LegalName = "Synthetic Carrier Review",
            Verified = false,
            ManualReviewOpen = true,
            AuthorityActive = true,
            InsuranceExpiresOn = new DateOnly(2026, 12, 1)
        });
        var result = registry.RecordReview("carrier-review", "Reviewed fictional documents.", "reviewer-1", Now);
        Assert.True(result.Succeeded);
        Assert.Single(registry.Evidence);
        Assert.True(registry.FindOrganization("carrier-review")!.Verified);
        Assert.False(registry.FindOrganization("carrier-review")!.ManualReviewOpen);
    }

    private static OrganizationRecord Carrier(string id, bool suspended, bool verified, bool review, DateOnly insurance, bool driver, bool equipment)
    {
        var organization = new OrganizationRecord
        {
            Id = id,
            Kind = "carrier",
            LegalName = id,
            Verified = verified,
            Suspended = suspended,
            AuthorityActive = true,
            InsuranceExpiresOn = insurance,
            ManualReviewOpen = review
        };
        if (driver)
        {
            organization.DriverLabels.Add("Synthetic Driver");
        }

        if (equipment)
        {
            organization.EquipmentLabels.Add("Synthetic Trailer");
        }

        return organization;
    }
}
