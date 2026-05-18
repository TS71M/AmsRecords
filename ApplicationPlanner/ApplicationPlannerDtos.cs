namespace AmsRecords.ApplicationPlanner;

public static class ApplicationPlannerDtos
{
    public sealed record ApplicationPlannerWorkspaceDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("year")] int Year,
        [property: JsonPropertyName("hasAnnualPlan")] bool HasAnnualPlan,
        [property: JsonPropertyName("annualPlan")] AnnualApplicationPlanSummaryDto? AnnualPlan,
        [property: JsonPropertyName("stats")] ApplicationPlannerStatsDto Stats,
        [property: JsonPropertyName("materialPlanLines")] IReadOnlyList<AnnualProductPlanLineDto> MaterialPlanLines,
        [property: JsonPropertyName("nutrientTotals")] IReadOnlyList<AnnualNutrientTotalDto> NutrientTotals,
        [property: JsonPropertyName("upcomingApplications")] IReadOnlyList<ApplicationPlanItemSummaryDto> UpcomingApplications,
        [property: JsonPropertyName("openTriggerEvents")] IReadOnlyList<PlanTriggerEventSummaryDto> OpenTriggerEvents,
        [property: JsonPropertyName("recentExecutions")] IReadOnlyList<ApplicationExecutionSummaryDto> RecentExecutions,
        [property: JsonPropertyName("recentDeviations")] IReadOnlyList<PlanDeviationSummaryDto> RecentDeviations);

    public sealed record AnnualApplicationPlanSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("areaName")] string? AreaName,
        [property: JsonPropertyName("year")] int Year,
        [property: JsonPropertyName("budgetTotal")] decimal BudgetTotal,
        [property: JsonPropertyName("strategySummary")] string StrategySummary,
        [property: JsonPropertyName("plannedApplicationCount")] int PlannedApplicationCount,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("updatedAtUtc")] DateTime? UpdatedAtUtc);

    public sealed record ApplicationPlannerStatsDto(
        [property: JsonPropertyName("plannedApplicationCount")] int PlannedApplicationCount,
        [property: JsonPropertyName("executedApplicationCount")] int ExecutedApplicationCount,
        [property: JsonPropertyName("openTriggerCount")] int OpenTriggerCount,
        [property: JsonPropertyName("deviationCount")] int DeviationCount,
        [property: JsonPropertyName("plannedBudget")] decimal PlannedBudget,
        [property: JsonPropertyName("actualCost")] decimal ActualCost,
        [property: JsonPropertyName("costVariance")] decimal CostVariance);

    public sealed record AnnualProductPlanLineDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("unitLabel")] string UnitLabel,
        [property: JsonPropertyName("plannedQuantity")] decimal PlannedQuantity,
        [property: JsonPropertyName("annualPlanQuantity")] decimal AnnualPlanQuantity,
        [property: JsonPropertyName("inventoryOrDeliveredQuantity")] decimal InventoryOrDeliveredQuantity,
        [property: JsonPropertyName("availableQuantity")] decimal AvailableQuantity,
        [property: JsonPropertyName("unitCost")] decimal UnitCost,
        [property: JsonPropertyName("plannedCost")] decimal PlannedCost,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<AnnualProductNutrientContributionDto> Nutrients);

    public sealed record AnnualProductNutrientContributionDto(
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("percentage")] decimal Percentage,
        [property: JsonPropertyName("quantity")] decimal Quantity);

    public sealed record AnnualNutrientTotalDto(
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("quantity")] decimal Quantity);

    public sealed record ApplicationPlanItemSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("plannedDate")] DateTime PlannedDate,
        [property: JsonPropertyName("triggerType")] string TriggerType,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("productCategory")] string ProductCategory,
        [property: JsonPropertyName("targetProblem")] string TargetProblem,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("waterVolume")] decimal WaterVolume,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("validationStatus")] string ValidationStatus);

    public sealed record PlanTriggerEventSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("severity")] string Severity,
        [property: JsonPropertyName("detectedAt")] DateTime DetectedAt,
        [property: JsonPropertyName("suggestedAction")] string SuggestedAction,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("affectedApplicationCount")] int AffectedApplicationCount);

    public sealed record ApplicationExecutionSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("executedDate")] DateTime ExecutedDate,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("actualRate")] decimal ActualRate,
        [property: JsonPropertyName("actualCost")] decimal ActualCost,
        [property: JsonPropertyName("notes")] string Notes);

    public sealed record PlanDeviationSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("deviationType")] string DeviationType,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("nutrientDelta")] decimal NutrientDelta,
        [property: JsonPropertyName("costDelta")] decimal CostDelta,
        [property: JsonPropertyName("reason")] string Reason);

    public sealed record CreateAnnualApplicationPlanDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("year")] int Year,
        [property: JsonPropertyName("budgetTotal")] decimal BudgetTotal,
        [property: JsonPropertyName("strategySummary")][property: MaxLength(4000)] string StrategySummary);

    public sealed record SaveAnnualProductPlanLinesDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("lines")] List<SaveAnnualProductPlanLineDto> Lines);

    public sealed record SaveAnnualProductPlanLineDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("plannedQuantity")] decimal PlannedQuantity,
        [property: JsonPropertyName("annualPlanQuantity")] decimal AnnualPlanQuantity,
        [property: JsonPropertyName("inventoryOrDeliveredQuantity")] decimal InventoryOrDeliveredQuantity);

    public sealed record CreateApplicationPlanItemDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("plannedDate")] DateTime PlannedDate,
        [property: JsonPropertyName("triggerType")] string TriggerType,
        [property: JsonPropertyName("productPubId")] Guid? ProductPubId,
        [property: JsonPropertyName("productCategory")] string ProductCategory,
        [property: JsonPropertyName("targetProblem")][property: MaxLength(250)] string TargetProblem,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("waterVolume")] decimal WaterVolume,
        [property: JsonPropertyName("instructions")][property: MaxLength(2000)] string Instructions,
        [property: JsonPropertyName("reason")][property: MaxLength(2000)] string Reason,
        [property: JsonPropertyName("restrictions")][property: MaxLength(2000)] string Restrictions);

    public sealed record RecordApplicationExecutionDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("plannedApplicationPubId")] Guid? PlannedApplicationPubId,
        [property: JsonPropertyName("executedDate")] DateTime ExecutedDate,
        [property: JsonPropertyName("actualProductPubId")] Guid? ActualProductPubId,
        [property: JsonPropertyName("actualRate")] decimal ActualRate,
        [property: JsonPropertyName("actualCost")] decimal ActualCost,
        [property: JsonPropertyName("weatherAtExecutionJson")][property: MaxLength(4000)] string WeatherAtExecutionJson,
        [property: JsonPropertyName("notes")][property: MaxLength(2000)] string Notes);

    public sealed record CreatePlanDeviationDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("plannedApplicationPubId")] Guid? PlannedApplicationPubId,
        [property: JsonPropertyName("deviationType")] string DeviationType,
        [property: JsonPropertyName("nutrientDelta")] decimal NutrientDelta,
        [property: JsonPropertyName("costDelta")] decimal CostDelta,
        [property: JsonPropertyName("reason")][property: MaxLength(2000)] string Reason);

    public sealed record EvaluateApplicationTriggersDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("detectedAtUtc")] DateTime? DetectedAtUtc,
        [property: JsonPropertyName("gddDeviationDays")] decimal? GddDeviationDays,
        [property: JsonPropertyName("heatStressForecast")] bool HeatStressForecast,
        [property: JsonPropertyName("diseaseRiskForecast")] bool DiseaseRiskForecast,
        [property: JsonPropertyName("soilMoisturePct")] decimal? SoilMoisturePct,
        [property: JsonPropertyName("source")] string? Source);
}
