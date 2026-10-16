namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerActualCopySelectionDto(Guid ExecutionPubId, decimal PercentageChange = 0);
public sealed record PlannerActualCopyPreviewRequest(Guid FieldPubId, int Year, string Scope, Guid? PlanPubId,
    string ExpectedSourceToken, IReadOnlyList<PlannerActualCopySelectionDto> Selections,
    string AreaBasis = "PreserveQuantity", decimal? TargetAreaM2 = null,
    int? TargetYear = null, string TimingMode = "QuantitiesOnly", string UnavailableWeekAction = "RequireDecision",
    IReadOnlyList<PlannerActualCopyDateChoiceDto>? DateChoices = null,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    IReadOnlyList<Guid>? DestinationSurfacePubIds = null);
public sealed record PlannerActualCopyDateChoiceDto(Guid ExecutionPubId, DateOnly? TargetDate = null, bool Omit = false);
public sealed record PlannerActualCopyPreviewRowDto(PlannerActualCopyRowDto Source, decimal PercentageChange,
    decimal AreaAdjustedQuantity, decimal TargetQuantity, DateOnly? TargetDate = null,
    int? TargetMonth = null, string MappingStatus = "QuantityOnly",
    IReadOnlyList<PlannerActualCopyNutrientPreviewDto>? NutrientPreview = null,
    PlannerActualCopyRatePreviewDto? RatePreview = null);
public sealed record PlannerActualCopyRatePreviewDto(decimal TreatedAreaM2,
    Lib.Planning.FertilizerApplicationRateUnit? RateUnit, decimal? Rate,
    decimal? ScheduledQuantity, decimal? QuantityDifference, string Status);
// Null is unavailable, never a zero nutrient contribution. Analysis completeness
// remains separately identified on Source; a calculable nutrient is not proof
// that the historical formulation contained no other nutrients.
public sealed record PlannerActualCopyNutrientPreviewDto(string Code, decimal? TargetGrams, string Status);
public sealed record PlannerActualCopyPreviewDto(string SourceToken, string PreviewToken,
    string AreaBasis, decimal? TargetAreaM2, IReadOnlyList<PlannerActualCopyPreviewRowDto> Rows,
    int? TargetYear = null, string TimingMode = "QuantitiesOnly", bool CanCopy = false,
    IReadOnlyList<ApplicationPlanSourceSurfaceDto>? DestinationSurfaces = null);
