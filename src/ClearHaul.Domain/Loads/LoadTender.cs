namespace ClearHaul.Domain.Loads;

public sealed record LocationStop(
    string Role,
    string Line1,
    string Locality,
    string Region,
    string PostalCode,
    string Country,
    string AddressValidationResult,
    double Latitude,
    double Longitude,
    string TimeZone,
    string? FacilityEntrance,
    string? TruckEntrance,
    string? CheckIn,
    string? OperatingHours,
    string? AppointmentRequirement,
    string? ContactInstructions);

public sealed record ScheduleWindow(DateTimeOffset Start, DateTimeOffset End, string TimeZone);

public sealed record MoneyTerms(
    long PostedLinehaulCents,
    long FuelSurchargeCents,
    long StopChargesCents,
    long SpecialHandlingCents,
    long OtherAccessorialCents,
    long TotalAmountCents,
    long CarrierPayoutCents,
    long PlatformChargeCents,
    string DetentionTerms,
    string LayoverTerms,
    string TruckOrderedNotUsedTerms,
    string CancellationTerms,
    string PaymentReleaseCondition);

public sealed record HazmatFacts(
    bool ClassificationConfirmedByShipper,
    string? ConfirmingUserId,
    DateTimeOffset? ConfirmedAt,
    string? ProperShippingName,
    string? IdentificationNumber,
    string? HazardClass,
    string? PackingGroup,
    long? Quantity,
    string? QuantityUnit);

public enum WashoutRequirement
{
    None,
    SweepOut,
    StandardWashout,
    FoodGradeWashout,
    Sanitization,
    AllergenSpecific,
    KosherCertified,
    HalalCertified,
    PharmaceuticalOrCustomerSpecific,
    ManualShipperApproval
}

public sealed record LoadTender(
    string LegalShipperName,
    string BillToParty,
    LocationStop Pickup,
    LocationStop Delivery,
    string CommodityDescription,
    string CommodityCategory,
    int PieceCount,
    string PackageType,
    int PalletCount,
    long WeightGrams,
    string? Dimensions,
    bool Stackable,
    bool FoodOrFeed,
    bool HazmatDesignated,
    HazmatFacts? Hazmat,
    string TrailerType,
    bool FoodGradeRequired,
    WashoutRequirement Washout,
    string SealRequirement,
    ScheduleWindow PickupWindow,
    ScheduleWindow DeliveryWindow,
    MoneyTerms Money,
    bool PermitDeclaredRequired,
    string? PermitReference,
    IReadOnlyList<string> RequiredDriverEndorsements,
    string? DeclaredRouteRestrictionRegion,
    string? Notes);

public static class LoadTenderValidator
{
    public static IReadOnlyList<string> Errors(LoadTender tender)
    {
        var errors = new List<string>();
        Require(errors, tender.LegalShipperName, "Legal shipper name is required.");
        Require(errors, tender.BillToParty, "Bill-to party is required.");
        ValidateStop(errors, tender.Pickup, "Pickup");
        ValidateStop(errors, tender.Delivery, "Delivery");
        if (string.IsNullOrWhiteSpace(tender.CommodityDescription))
        {
            errors.Add("Commodity description is required. Notes cannot replace it.");
        }

        Require(errors, tender.CommodityCategory, "Commodity category is required.");
        if (tender.PieceCount <= 0)
        {
            errors.Add("Piece count must be greater than zero.");
        }

        Require(errors, tender.PackageType, "Package type is required.");
        if (tender.WeightGrams <= 0)
        {
            errors.Add("Weight is required.");
        }

        Require(errors, tender.TrailerType, "Trailer type is required.");
        Require(errors, tender.SealRequirement, "Seal requirement is required.");
        ValidateWindow(errors, tender.PickupWindow, "Pickup");
        ValidateWindow(errors, tender.DeliveryWindow, "Delivery");
        if (tender.DeliveryWindow.Start < tender.PickupWindow.Start)
        {
            errors.Add("Delivery window starts before the pickup window.");
        }

        ValidateMoney(errors, tender.Money);
        if (tender.HazmatDesignated && tender.Hazmat is null)
        {
            errors.Add("Hazmat designation requires structured hazmat facts.");
        }

        if (tender.PermitDeclaredRequired && string.IsNullOrWhiteSpace(tender.PermitReference))
        {
            errors.Add("A declared permit requirement needs a permit reference.");
        }

        return errors;
    }

    private static void ValidateMoney(List<string> errors, MoneyTerms money)
    {
        var components = money.PostedLinehaulCents + money.FuelSurchargeCents + money.StopChargesCents
            + money.SpecialHandlingCents + money.OtherAccessorialCents;
        if (components != money.TotalAmountCents)
        {
            errors.Add("Line items do not add up to the total amount.");
        }

        if (money.CarrierPayoutCents + money.PlatformChargeCents != money.TotalAmountCents)
        {
            errors.Add("Carrier payout and platform charge do not add up to the total amount.");
        }

        if (money.PostedLinehaulCents < 0 || money.TotalAmountCents < 0 || money.PlatformChargeCents < 0)
        {
            errors.Add("Money amounts cannot be negative.");
        }

        Require(errors, money.DetentionTerms, "Detention terms are required.");
        Require(errors, money.CancellationTerms, "Cancellation terms are required.");
        Require(errors, money.PaymentReleaseCondition, "Payment-release condition is required.");
        Require(errors, money.LayoverTerms, "Layover terms are required.");
        Require(errors, money.TruckOrderedNotUsedTerms, "Truck-ordered-not-used terms are required.");
    }

    private static void ValidateWindow(List<string> errors, ScheduleWindow window, string label)
    {
        Require(errors, window.TimeZone, label + " time zone is required.");
        if (window.End <= window.Start)
        {
            errors.Add(label + " window is not a positive range.");
        }
    }

    private static void ValidateStop(List<string> errors, LocationStop stop, string label)
    {
        Require(errors, stop.Line1, label + " address is required.");
        Require(errors, stop.Locality, label + " locality is required.");
        Require(errors, stop.Region, label + " region is required.");
        Require(errors, stop.PostalCode, label + " postal code is required.");
        Require(errors, stop.TimeZone, label + " time zone is required.");
        Require(errors, stop.AddressValidationResult, label + " address-validation result is required.");
        if (stop.Latitude is < -90 or > 90 || stop.Longitude is < -180 or > 180)
        {
            errors.Add(label + " coordinates are out of range.");
        }
    }

    private static void Require(List<string> errors, string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(message);
        }
    }
}
