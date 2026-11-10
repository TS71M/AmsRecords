using AmsRecords.LeafNitrate;
using static AmsRecords.LeafNitrate.LeafNitrateDtos;
using static AmsRecords.RiskManagement.DollarSpotDtos;

namespace AmsRecords.Assistant;

public static class AssistantDraftKinds
{
    public const string Field = "field";
    public const string GreenSpeed = "green_speed";
    public const string CuttingHeight = "cutting_height";
    public const string ClippingVolume = "clipping_volume";
    public const string LeafNitrate = "leaf_nitrate";
    public const string DollarSpotObservation = "dollar_spot_observation";
    public const string CatchCanTest = "catch_can_test";
    public const string Fertilizer = "fertilizer_gp";
    public const string FertilizerAlternative = "fertilizer_alternative";
    public const string Product = "product";
    public const string ProductList = "product_list";
    public const string SoilImport = "soil_import";
    public const string WaterImport = "water_import";
    public const string ModuleAccess = "module_access";
    public const string ModulePilotRevocation = "module_pilot_revocation";
    public const string ModulePilotExpiry = "module_pilot_expiry";
}

// An editable proposal, never a command to save or a permission grant.
public sealed record AssistantDraftDto(
    Guid Id, string Kind, Guid? FieldPubId = null, string? FieldName = null,
    Guid? AreaPubId = null, Guid? SurfacePubId = null, decimal? Value = null,
    Guid? UnitPubId = null, string? Unit = null, DateTimeOffset? MeasuredAt = null,
    DateOnly? MeasuredOn = null, Guid? FieldTypePubId = null, Guid? CountryPubId = null,
    AssistantFieldDetailsDto? Details = null,
    IReadOnlyList<SupportAssistantLinkDto>? Sources = null,
    Guid? IbuPubId = null,
    AssistantFertilizerDraftDto? Fertilizer = null, AssistantFertilizerAlternativeDraftDto? FertilizerAlternative = null,
    AmsRecords.Products.ProductsDtos.ProductWebsiteImportPreviewDto? Product = null,
    AssistantProductSetupDto? ProductSetup = null,
    string? TargetLabel = null, DateTimeOffset? PreviousCutAt = null, int? CutIntervalSource = null,
    AmsRecords.SoilTests.SoilTestDtos.SoilTestDocumentImportPreviewDto? SoilImport = null,
    AmsRecords.WaterTests.WaterTestDtos.WaterTestDocumentImportPreviewDto? WaterImport = null,
    AmsRecords.SoilTests.SoilTestDtos.SoilTestDocumentImportCreateDto? SoilReview = null,
    AmsRecords.WaterTests.WaterTestDtos.WaterTestDocumentImportCreateDto? WaterReview = null,
    AssistantModuleAccessDraftDto? ModuleAccess = null,
    AmsRecords.Products.ProductsDtos.ProductListImportPreviewDto? ProductList = null)
{
    public AssistantLeafNitrateDraftDto? LeafNitrate { get; init; }
    public AssistantDollarSpotObservationDraftDto? DollarSpotObservation { get; init; }
    public AmsRecords.Irrigation.CatchCanCalibrationDtos.CatchCanTestSaveDto? CatchCanTest { get; init; }
}

public sealed record AssistantDollarSpotObservationDraftDto(string? MorningMyceliumObserved,
    string? LeafLesionsObserved, string? ActiveExpansionObserved, string? LdsOrHydrophobicityPresent)
{
    public const string Present = "present";
    public const string Absent = "absent";
    public const string NotChecked = "not_checked";
    public AssistantDollarSpotDewRemovalDraftDto? DewRemoval { get; init; }

    public bool IsValid()
    {
        string?[] answers = [MorningMyceliumObserved, LeafLesionsObserved, ActiveExpansionObserved, LdsOrHydrophobicityPresent];
        return (DewRemoval is not null || answers.Any(answer => answer is not null))
            && answers.All(answer => answer is null or Present or Absent or NotChecked)
            && (DewRemoval is null || DewRemoval.IsValid());
    }

    public static DollarSpotObservationDto? PrepareReview(AssistantDraftDto draft, DollarSpotEarlyWarningStatusDto status)
    {
        if (draft.Kind != AssistantDraftKinds.DollarSpotObservation || status.FieldPubId == Guid.Empty || draft.FieldPubId != status.FieldPubId
            || !status.FieldEnabled || draft.MeasuredOn != status.TodayLocal
            || status.TodayObservation is { } prior && prior.DateLocal != status.TodayLocal
            || draft.DollarSpotObservation is not { } answers || !answers.IsValid()) return null;

        var current = status.TodayObservation ?? new DollarSpotObservationDto(null, status.TodayLocal,
            null, null, null, null, null, null, null);
        if (answers.DewRemoval is { } dew)
            current = current with
            {
                DewManuallyRemoved = dew.Status switch { AssistantDollarSpotDewRemovalDraftDto.No => false, NotChecked => null, _ => true },
                DewRemovedAtLocal = dew.FinishLocal,
                DewRemovalStartedAtLocal = dew.StartLocal,
                DewRemovalCompletedAtLocal = dew.FinishLocal,
                DewRemovalCoveragePct = dew.CoveragePercent
            };
        return current with
        {
            MorningMyceliumObserved = Apply(answers.MorningMyceliumObserved, current.MorningMyceliumObserved),
            LeafLesionsObserved = Apply(answers.LeafLesionsObserved, current.LeafLesionsObserved),
            ActiveExpansionObserved = Apply(answers.ActiveExpansionObserved, current.ActiveExpansionObserved),
            LdsOrHydrophobicityPresent = Apply(answers.LdsOrHydrophobicityPresent, current.LdsOrHydrophobicityPresent)
        };
    }

    static bool? Apply(string? answer, bool? current) => answer switch
    {
        Present => true,
        Absent => false,
        NotChecked => null,
        _ => current
    };
}

public sealed record AssistantDollarSpotDewRemovalDraftDto(string Status, TimeOnly? StartLocal, TimeOnly? FinishLocal, decimal? CoveragePercent)
{
    public const string No = "no";
    public const string InProgress = "inProgress";
    public const string Completed = "completed";
    public const string PartiallyCompleted = "partiallyCompleted";

    public bool IsValid()
    {
        if (StartLocal is { } start && start.Ticks % TimeSpan.TicksPerMinute != 0
            || FinishLocal is { } finish && finish.Ticks % TimeSpan.TicksPerMinute != 0
            || CoveragePercent is { } coverage && (coverage is < 0m or > 100m || coverage != decimal.Truncate(coverage))) return false;
        return Status switch
        {
            No or AssistantDollarSpotObservationDraftDto.NotChecked => StartLocal is null && FinishLocal is null && CoveragePercent is null,
            InProgress => StartLocal.HasValue && FinishLocal is null && CoveragePercent.HasValue,
            Completed or PartiallyCompleted => StartLocal.HasValue && FinishLocal.HasValue && FinishLocal >= StartLocal
                && (Status == Completed ? CoveragePercent == 100m : CoveragePercent is < 100m),
            _ => false
        };
    }
}

public sealed record AssistantLeafNitrateDraftDto(decimal Reading1, decimal? Reading2, string Basis, string Unit)
{
    public bool IsValid() => Basis is MeasurementBases.No3 or MeasurementBases.No3N
        && Unit is MeasurementUnits.Ppm or MeasurementUnits.MgPerL
        && LeafNitrateRules.IsWithinMeterRange(Reading1, Basis)
        && (Reading2 is not { } second || LeafNitrateRules.IsWithinMeterRange(second, Basis));
}

public sealed record AssistantModuleAccessDraftDto(string ModuleKey, Guid IbuPubId, string IbuName,
    bool WasGranted, DateTime? ExpiresAtUtc, DateOnly? AccessThrough = null);

public sealed record AssistantProductSetupDto(Guid? AccountPubId, Guid? RateUnitPubId, Guid? AreaUnitPubId,
    Guid? PackUnitPubId, decimal? Pack, decimal? Price, Guid? FieldPubId, string? SourceImageSha256 = null);

public sealed record AssistantFertilizerAlternativeProductDto(Guid? ProductSnapshotPubId, string ProductName, decimal? NitrogenSharePercent);
public sealed record AssistantFertilizerAlternativeDraftDto(Guid PlanPubId, Guid RevisionPubId, Guid ZonePubId, int Year,
    string AlternativeName, IReadOnlyList<Guid> SourceProductSnapshotPubIds,
    IReadOnlyList<AssistantFertilizerAlternativeProductDto> Products, DateOnly? StartDate, DateOnly? EndDate,
    int? IntervalDays, decimal? CutoffPercent, string NitrogenBasis, bool FindCheaper = false);

public sealed record AssistantFertilizerDraftDto(Guid PlanPubId, Guid RevisionPubId, Guid ZonePubId,
    Guid? ProductSnapshotPubId, int Year, string ProductName, DateOnly? StartDate, DateOnly? EndDate,
    decimal NitrogenAtFullGrowthGramsPerM2, decimal CutoffPercent, int IntervalDays = 14, bool AdjustExisting = false);

public sealed record AssistantFieldDetailsDto
{
    [System.ComponentModel.DataAnnotations.StringLength(200)] public string? Website { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(200)] public string? Address1 { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(200)] public string? Address2 { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(60)] public string? Town { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(100)] public string? StateOrRegion { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(20)] public string? PostalCode { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(60)] public string? Email { get; init; }
    [System.ComponentModel.DataAnnotations.StringLength(50)] public string? Phone { get; init; }
    [System.ComponentModel.DataAnnotations.Range(0, 10000)] public decimal? ElevationMetres { get; init; }
    [System.ComponentModel.DataAnnotations.Range(1, 999)] public int? NumberOfHoles { get; init; }
    [System.ComponentModel.DataAnnotations.Range(1900, 2100)] public int? YearOfConstruction { get; init; }
    public decimal? TotalSurfaceM2 { get; init; }
}

public static class AssistantDraftNumbers
{
    // Never interpret a comma as a thousands separator in a spoken measurement.
    public static bool TryParse(string? text, out decimal value)
        => decimal.TryParse(text?.Trim().Replace(',', '.'),
            System.Globalization.NumberStyles.AllowDecimalPoint,
            System.Globalization.CultureInfo.InvariantCulture, out value) && value > 0;
}
