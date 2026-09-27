namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanReviewRevisionDto(Guid PubId, int Number);
public sealed record ApplicationPlanReviewNutrientDto(string Code, decimal Kilograms);
public sealed record ApplicationPlanReviewProductDto(Guid? ProductSnapshotPubId, string ProductName,
    string? QuantityUnit, decimal? Quantity, int ApplicationCount);
public sealed record ApplicationPlanReviewMeasureDto(int ApplicationCount, decimal? Cost,
    bool NutrientsComplete, IReadOnlyList<ApplicationPlanReviewNutrientDto> Nutrients,
    IReadOnlyList<ApplicationPlanReviewProductDto>? Products = null);
public sealed record ApplicationPlanReviewMonthDto(int Year, int Month,
    ApplicationPlanReviewMeasureDto? Original,
    ApplicationPlanReviewMeasureDto? Scheduled,
    ApplicationPlanReviewMeasureDto Actual);
public sealed record ApplicationPlanReviewYearDto(int Year,
    ApplicationPlanReviewMeasureDto? Original, ApplicationPlanReviewMeasureDto? Scheduled,
    ApplicationPlanReviewMeasureDto Actual);
public sealed record ApplicationPlanMonthlyReviewDto(Guid PlanPubId, int PlanningYear,
    ApplicationPlanReviewRevisionDto? Baseline, ApplicationPlanReviewRevisionDto? Operational,
    IReadOnlyList<ApplicationPlanReviewMonthDto> Months,
    ApplicationPlanReviewMeasureDto? OriginalTotal, ApplicationPlanReviewMeasureDto? ScheduledTotal,
    ApplicationPlanReviewMeasureDto ActualTotal,
    IReadOnlyList<ApplicationPlanReviewYearDto>? Years = null);
