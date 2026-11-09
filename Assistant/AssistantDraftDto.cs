namespace AmsRecords.Assistant;

public static class AssistantDraftKinds
{
    public const string Field = "field";
    public const string GreenSpeed = "green_speed";
    public const string CuttingHeight = "cutting_height";
    public const string ClippingVolume = "clipping_volume";
    public const string Fertilizer = "fertilizer_gp";
    public const string FertilizerAlternative = "fertilizer_alternative";
    public const string Product = "product";
    public const string ProductList = "product_list";
    public const string SoilImport = "soil_import";
    public const string WaterImport = "water_import";
    public const string ModuleAccess = "module_access";
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
    AmsRecords.Products.ProductsDtos.ProductListImportPreviewDto? ProductList = null);

public sealed record AssistantModuleAccessDraftDto(string ModuleKey, Guid IbuPubId, string IbuName,
    bool WasGranted, DateTime? ExpiresAtUtc);

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
