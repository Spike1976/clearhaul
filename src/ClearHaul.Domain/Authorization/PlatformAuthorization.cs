namespace ClearHaul.Domain.Authorization;

public enum PlatformRole
{
    ShipperOrganizationAdministrator,
    ShipperEmployee,
    ShippingFacilityPersonnel,
    CarrierOrganizationAdministrator,
    Dispatcher,
    SafetyCompliancePersonnel,
    Driver,
    WashoutOrInspectionFacility,
    PlatformInvestigator,
    PlatformComplianceAdministrator,
    PaymentAndDisputeAdministrator,
    ReadOnlyAuditor,
    SystemAdministrator
}

public enum Permission
{
    SubmitLoadTender,
    PublishLoad,
    SubmitBid,
    SelectCarrier,
    AssignEquipment,
    ReviewAssignedEquipment,
    SubmitWashoutEvidence,
    ApproveEquipment,
    AcknowledgeDriverPacket,
    RecordDelivery,
    RequestPaymentRelease,
    RecordPaymentRelease,
    OpenDispute,
    InvestigateReport,
    ReadAudit,
    DraftRulePackage,
    ReviewIdentity,
    ManageEnforcement,
    ReadOrganizationRecord
}

public sealed record Actor(
    string UserId,
    string OrganizationId,
    IReadOnlySet<PlatformRole> Roles,
    bool Removed,
    bool MfaSatisfied)
{
    public bool HasRole(params PlatformRole[] roles) => roles.Any(Roles.Contains);
}

public static class PermissionMatrix
{
    private static readonly IReadOnlyDictionary<PlatformRole, Permission[]> Grants =
        new Dictionary<PlatformRole, Permission[]>
        {
            [PlatformRole.ShipperOrganizationAdministrator] =
            [
                Permission.SubmitLoadTender, Permission.PublishLoad, Permission.SelectCarrier,
                Permission.ReviewAssignedEquipment, Permission.ApproveEquipment,
                Permission.RequestPaymentRelease, Permission.OpenDispute, Permission.ReadOrganizationRecord
            ],
            [PlatformRole.ShipperEmployee] =
            [
                Permission.SubmitLoadTender, Permission.PublishLoad, Permission.SelectCarrier,
                Permission.ReviewAssignedEquipment, Permission.ApproveEquipment, Permission.OpenDispute
            ],
            [PlatformRole.ShippingFacilityPersonnel] = [Permission.SubmitWashoutEvidence, Permission.RecordDelivery],
            [PlatformRole.CarrierOrganizationAdministrator] =
            [
                Permission.SubmitBid, Permission.AssignEquipment, Permission.SubmitWashoutEvidence,
                Permission.OpenDispute, Permission.ReadOrganizationRecord
            ],
            [PlatformRole.Dispatcher] =
            [Permission.SubmitBid, Permission.AssignEquipment, Permission.SubmitWashoutEvidence],
            [PlatformRole.SafetyCompliancePersonnel] =
            [Permission.SubmitWashoutEvidence, Permission.ReadOrganizationRecord],
            [PlatformRole.Driver] =
            [
                Permission.AcknowledgeDriverPacket, Permission.RecordDelivery,
                Permission.SubmitWashoutEvidence, Permission.OpenDispute
            ],
            [PlatformRole.WashoutOrInspectionFacility] = [Permission.SubmitWashoutEvidence],
            [PlatformRole.PlatformInvestigator] = [Permission.InvestigateReport, Permission.ReadAudit],
            [PlatformRole.PlatformComplianceAdministrator] =
            [Permission.DraftRulePackage, Permission.ReviewIdentity, Permission.ReadAudit],
            [PlatformRole.PaymentAndDisputeAdministrator] =
            [Permission.RecordPaymentRelease, Permission.OpenDispute, Permission.ReadAudit],
            [PlatformRole.ReadOnlyAuditor] = [Permission.ReadAudit, Permission.ReadOrganizationRecord],
            [PlatformRole.SystemAdministrator] =
            [Permission.ReviewIdentity, Permission.ReadAudit, Permission.ManageEnforcement]
        };

    private static readonly HashSet<Permission> MfaRequired =
    [
        Permission.RecordPaymentRelease,
        Permission.ManageEnforcement,
        Permission.DraftRulePackage,
        Permission.ReviewIdentity,
        Permission.InvestigateReport
    ];

    public static IReadOnlyList<PlatformRole> AllRoles { get; } = Enum.GetValues<PlatformRole>();

    public static bool Allows(Actor actor, Permission permission)
    {
        if (actor.Removed || string.IsNullOrWhiteSpace(actor.UserId))
        {
            return false;
        }

        if (MfaRequired.Contains(permission) && !actor.MfaSatisfied)
        {
            return false;
        }

        return actor.Roles.Any(role => Grants.TryGetValue(role, out var granted) && granted.Contains(permission));
    }
}
