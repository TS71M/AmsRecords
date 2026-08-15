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
        [property: JsonPropertyName("currentGrowthPotential")] CurrentGrowthPotentialDto? CurrentGrowthPotential,
        [property: JsonPropertyName("materialPlanLines")] IReadOnlyList<AnnualProductPlanLineDto> MaterialPlanLines,
        [property: JsonPropertyName("nutrientTotals")] IReadOnlyList<AnnualNutrientTotalDto> NutrientTotals,
        [property: JsonPropertyName("upcomingApplications")] IReadOnlyList<ApplicationPlanItemSummaryDto> UpcomingApplications,
        [property: JsonPropertyName("openTriggerEvents")] IReadOnlyList<PlanTriggerEventSummaryDto> OpenTriggerEvents,
        [property: JsonPropertyName("recentExecutions")] IReadOnlyList<ApplicationExecutionSummaryDto> RecentExecutions,
        [property: JsonPropertyName("recentDeviations")] IReadOnlyList<PlanDeviationSummaryDto> RecentDeviations,
        [property: JsonPropertyName("availablePlans")] IReadOnlyList<AnnualApplicationPlanSummaryDto>? AvailablePlans = null,
        [property: JsonPropertyName("selectedPlanPubId")] Guid? SelectedPlanPubId = null,
        [property: JsonPropertyName("isoWeekCount")] int IsoWeekCount = 52,
        [property: JsonPropertyName("calendarWeeks")] IReadOnlyList<FertilizerPlanWeekDto>? CalendarWeeks = null,
        [property: JsonPropertyName("selectedPlanRevisions")] IReadOnlyList<ApplicationPlanRevisionSummaryDto>? SelectedPlanRevisions = null,
        [property: JsonPropertyName("readiness")] ApplicationPlannerReadinessDto? Readiness = null,
        [property: JsonPropertyName("zones")] IReadOnlyList<ApplicationPlanZoneSummaryDto>? Zones = null,
        [property: JsonPropertyName("areaSnapshots")] IReadOnlyList<ApplicationPlanAreaSnapshotDto>? AreaSnapshots = null,
        [property: JsonPropertyName("productSnapshots")] IReadOnlyList<ApplicationPlanProductSnapshotDto>? ProductSnapshots = null,
        [property: JsonPropertyName("performance")] ApplicationPlanPerformanceDto? Performance = null);

    public sealed record FertilizerPlanWeekDto(
        [property: JsonPropertyName("weekNumber")] int WeekNumber,
        [property: JsonPropertyName("startsOn")] DateOnly StartsOn,
        [property: JsonPropertyName("endsOn")] DateOnly EndsOn,
        [property: JsonPropertyName("displayMonth")] int DisplayMonth);

    public sealed record ApplicationPlanCalendarDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("year")] int Year,
        [property: JsonPropertyName("annualPlanPubId")] Guid? AnnualPlanPubId,
        [property: JsonPropertyName("planName")] string? PlanName,
        [property: JsonPropertyName("revisionPubId")] Guid? RevisionPubId,
        [property: JsonPropertyName("revisionNumber")] int RevisionNumber,
        [property: JsonPropertyName("revisionStatus")] string RevisionStatus,
        [property: JsonPropertyName("isEditable")] bool IsEditable,
        [property: JsonPropertyName("selectedZonePubId")] Guid? SelectedZonePubId,
        [property: JsonPropertyName("weeks")] IReadOnlyList<FertilizerPlanWeekDto> Weeks,
        [property: JsonPropertyName("zones")] IReadOnlyList<ApplicationPlanCalendarZoneDto> Zones,
        [property: JsonPropertyName("rows")] IReadOnlyList<ApplicationPlanCalendarRowDto> Rows,
        [property: JsonPropertyName("availableProducts")] IReadOnlyList<ApplicationPlanCalendarProductOptionDto> AvailableProducts,
        [property: JsonPropertyName("nutrientTotals")] IReadOnlyList<ApplicationPlanCalendarNutrientTotalDto> NutrientTotals,
        [property: JsonPropertyName("plannedCost")] decimal PlannedCost);

    public sealed record ApplicationPlanCalendarZoneDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("treatedAreaM2")] decimal TreatedAreaM2,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public sealed record ApplicationPlanCalendarProductOptionDto(
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("rateUnit")] string? RateUnit,
        [property: JsonPropertyName("rateUnitLabel")] string RateUnitLabel,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("isAvailable")] bool IsAvailable);

    public sealed record ApplicationPlanCalendarRowDto(
        [property: JsonPropertyName("zoneProductPubId")] Guid ZoneProductPubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("sourceProductPubId")] Guid? SourceProductPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("rateUnit")] string? RateUnit,
        [property: JsonPropertyName("rateUnitLabel")] string RateUnitLabel,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("defaultRate")] decimal DefaultRate,
        [property: JsonPropertyName("defaultWaterVolume")] decimal DefaultWaterVolume,
        [property: JsonPropertyName("unitCost")] decimal UnitCost,
        [property: JsonPropertyName("packSize")] decimal PackSize,
        [property: JsonPropertyName("packUnitLabel")] string PackUnitLabel,
        [property: JsonPropertyName("annualProductQuantity")] decimal AnnualProductQuantity,
        [property: JsonPropertyName("productQuantityUnit")] string? ProductQuantityUnit,
        [property: JsonPropertyName("plannedCost")] decimal PlannedCost,
        [property: JsonPropertyName("cells")] IReadOnlyList<ApplicationPlanCalendarCellDto> Cells,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public sealed record ApplicationPlanCalendarCellDto(
        [property: JsonPropertyName("planItemPubId")] Guid PlanItemPubId,
        [property: JsonPropertyName("isoWeekNumber")] int IsoWeekNumber,
        [property: JsonPropertyName("plannedLocalDate")] DateOnly PlannedLocalDate,
        [property: JsonPropertyName("currentScheduledLocalDate")] DateOnly CurrentScheduledLocalDate,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("rateUnit")] string? RateUnit,
        [property: JsonPropertyName("waterVolume")] decimal WaterVolume,
        [property: JsonPropertyName("triggerType")] string TriggerType,
        [property: JsonPropertyName("scheduleStatus")] string ScheduleStatus,
        [property: JsonPropertyName("validationStatus")] string ValidationStatus,
        [property: JsonPropertyName("isDone")] bool IsDone,
        [property: JsonPropertyName("plannedProductQuantity")] decimal PlannedProductQuantity,
        [property: JsonPropertyName("productQuantityUnit")] string? ProductQuantityUnit,
        [property: JsonPropertyName("plannedCost")] decimal PlannedCost,
        [property: JsonPropertyName("targetProblem")] string TargetProblem,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("instructions")] string Instructions,
        [property: JsonPropertyName("restrictions")] string Restrictions,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null,
        [property: JsonPropertyName("machineryName")] string MachineryName = "",
        [property: JsonPropertyName("machineryCapacity")] decimal? MachineryCapacity = null,
        [property: JsonPropertyName("machineryCapacityUnit")] string MachineryCapacityUnit = "",
        [property: JsonPropertyName("totalCarrierVolumeLitres")] decimal? TotalCarrierVolumeLitres = null,
        [property: JsonPropertyName("fullTankLoads")] int? FullTankLoads = null,
        [property: JsonPropertyName("partialTankVolumeLitres")] decimal? PartialTankVolumeLitres = null,
        [property: JsonPropertyName("totalTankLoads")] int? TotalTankLoads = null);

    public sealed record ApplicationPlanCalendarNutrientTotalDto(
        [property: JsonPropertyName("nutrientCode")] string NutrientCode,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("totalGrams")] decimal TotalGrams,
        [property: JsonPropertyName("gramsPerM2")] decimal GramsPerM2);

    public sealed record CurrentGrowthPotentialDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("dayUtc")] DateTime DayUtc,
        [property: JsonPropertyName("tempMeanC")] decimal TempMeanC,
        [property: JsonPropertyName("gpPct")] decimal GpPct,
        [property: JsonPropertyName("optC")] decimal OptC,
        [property: JsonPropertyName("varC")] decimal VarC,
        [property: JsonPropertyName("pathway")] string Pathway,
        [property: JsonPropertyName("calculationSource")] string CalculationSource,
        [property: JsonPropertyName("weatherSource")] string? WeatherSource,
        [property: JsonPropertyName("profile")] AmsRecords.Weather.GrowthPotentialDtos.GrowthPotentialProfileDto? Profile = null);

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
        [property: JsonPropertyName("updatedAtUtc")] DateTime? UpdatedAtUtc,
        [property: JsonPropertyName("planName")] string PlanName = "Plan 1",
        [property: JsonPropertyName("planPurpose")] string PlanPurpose = "",
        [property: JsonPropertyName("latestRevisionNumber")] int LatestRevisionNumber = 0,
        [property: JsonPropertyName("latestRevisionStatus")] string LatestRevisionStatus = "Legacy");

    public sealed record ApplicationPlanRevisionSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("revisionNumber")] int RevisionNumber,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("budgetTotal")] decimal BudgetTotal,
        [property: JsonPropertyName("strategySummary")] string StrategySummary,
        [property: JsonPropertyName("changeSummary")] string ChangeSummary,
        [property: JsonPropertyName("plannedApplicationCount")] int PlannedApplicationCount,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("approvedAtUtc")] DateTime? ApprovedAtUtc);

    public sealed record ApplicationPlanComparisonDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("year")] int Year,
        [property: JsonPropertyName("isoWeekCount")] int IsoWeekCount,
        [property: JsonPropertyName("plans")] IReadOnlyList<ApplicationPlanComparisonColumnDto> Plans);

    public sealed record ApplicationPlanComparisonColumnDto(
        [property: JsonPropertyName("plan")] AnnualApplicationPlanSummaryDto Plan,
        [property: JsonPropertyName("plannedCost")] decimal PlannedCost,
        [property: JsonPropertyName("actualCost")] decimal ActualCost,
        [property: JsonPropertyName("products")] IReadOnlyList<AnnualProductPlanLineDto> Products,
        [property: JsonPropertyName("nutrientTotals")] IReadOnlyList<AnnualNutrientTotalDto> NutrientTotals);

    public sealed record ApplicationPlannerReadinessDto(
        [property: JsonPropertyName("isReady")] bool IsReady,
        [property: JsonPropertyName("checks")] IReadOnlyList<ApplicationPlannerReadinessCheckDto> Checks,
        [property: JsonPropertyName("fieldEvidence")] ApplicationPlannerFieldEvidenceDto FieldEvidence,
        [property: JsonPropertyName("eligibleAreas")] IReadOnlyList<ApplicationPlannerAreaEvidenceDto> EligibleAreas);

    public sealed record ApplicationPlannerReadinessCheckDto(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("message")] string Message);

    public sealed record ApplicationPlannerFieldEvidenceDto(
        [property: JsonPropertyName("hasCoordinate")] bool HasCoordinate,
        [property: JsonPropertyName("elevationM")] decimal ElevationM,
        [property: JsonPropertyName("climateZone")] string? ClimateZone,
        [property: JsonPropertyName("jurisdiction")] string? Jurisdiction,
        [property: JsonPropertyName("growthPotentialConfigured")] bool GrowthPotentialConfigured,
        [property: JsonPropertyName("growthPotentialProfile")] AmsRecords.Weather.GrowthPotentialDtos.GrowthPotentialProfileDto? GrowthPotentialProfile,
        [property: JsonPropertyName("climateNormalMonthCount")] int ClimateNormalMonthCount,
        [property: JsonPropertyName("climateNormalFromYear")] int? ClimateNormalFromYear,
        [property: JsonPropertyName("climateNormalToYear")] int? ClimateNormalToYear);

    public sealed record ApplicationPlannerAreaEvidenceDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("surfaceCount")] int SurfaceCount,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2,
        [property: JsonPropertyName("soilTestSurfaceCount")] int SoilTestSurfaceCount,
        [property: JsonPropertyName("latestSoilTestDate")] DateTime? LatestSoilTestDate,
        [property: JsonPropertyName("grassCompositionConfigured")] bool GrassCompositionConfigured,
        [property: JsonPropertyName("grassSpecies")] IReadOnlyList<ApplicationPlannerGrassComponentDto> GrassSpecies);

    public sealed record ApplicationPlannerGrassComponentDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent);

    public sealed record ApplicationPlanAreaSnapshotDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sourceAreaPubId")] Guid? SourceAreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("surfaceCount")] int SurfaceCount,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2,
        [property: JsonPropertyName("surfaces")] IReadOnlyList<ApplicationPlanSurfaceSnapshotDto> Surfaces);

    public sealed record ApplicationPlanSurfaceSnapshotDto(
        [property: JsonPropertyName("sourceSurfacePubId")] Guid? SourceSurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2);

    public sealed record ApplicationPlanProductSnapshotDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sourceProductPubId")] Guid? SourceProductPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("rateUnit")] string? RateUnit,
        [property: JsonPropertyName("rateUnitLabel")] string RateUnitLabel,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("densityKgPerL")] decimal? DensityKgPerL,
        [property: JsonPropertyName("packSize")] decimal PackSize,
        [property: JsonPropertyName("packUnitLabel")] string PackUnitLabel,
        [property: JsonPropertyName("unitCost")] decimal UnitCost,
        [property: JsonPropertyName("inventoryQuantitySnapshot")] decimal InventoryQuantitySnapshot,
        [property: JsonPropertyName("isAvailable")] bool IsAvailable,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<ApplicationPlanProductNutrientSnapshotDto> Nutrients);

    public sealed record ApplicationPlanProductNutrientSnapshotDto(
        [property: JsonPropertyName("sourceNutrientPubId")] Guid? SourceNutrientPubId,
        [property: JsonPropertyName("nutrientCode")] string NutrientCode,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("analysisAmount")] decimal AnalysisAmount,
        [property: JsonPropertyName("analysisBasis")] string AnalysisBasis,
        [property: JsonPropertyName("analysisSource")] string AnalysisSource,
        [property: JsonPropertyName("isVerified")] bool IsVerified);

    public sealed record ApplicationPlanPerformanceDto(
        [property: JsonPropertyName("plannedApplicationCount")] int PlannedApplicationCount,
        [property: JsonPropertyName("appliedApplicationCount")] int AppliedApplicationCount,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<ApplicationNutrientPerformanceDto> Nutrients,
        [property: JsonPropertyName("products")] IReadOnlyList<ApplicationProductPerformanceDto> Products,
        [property: JsonPropertyName("zones")] IReadOnlyList<ApplicationZonePerformanceDto> Zones,
        [property: JsonPropertyName("donePlannedApplicationCount")] int DonePlannedApplicationCount = 0,
        [property: JsonPropertyName("outstandingPlannedApplicationCount")] int OutstandingPlannedApplicationCount = 0,
        [property: JsonPropertyName("unplannedApplicationCount")] int UnplannedApplicationCount = 0,
        [property: JsonPropertyName("skippedPlannedApplicationCount")] int SkippedPlannedApplicationCount = 0);

    public sealed record ApplicationNutrientPerformanceDto(
        [property: JsonPropertyName("nutrientCode")] string NutrientCode,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("plannedKilograms")] decimal PlannedKilograms,
        [property: JsonPropertyName("appliedKilograms")] decimal AppliedKilograms,
        [property: JsonPropertyName("varianceKilograms")] decimal VarianceKilograms,
        [property: JsonPropertyName("plannedApplicationGramsPerM2")] decimal PlannedApplicationGramsPerM2,
        [property: JsonPropertyName("appliedApplicationGramsPerM2")] decimal AppliedApplicationGramsPerM2);

    public sealed record ApplicationProductPerformanceDto(
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("quantityUnit")] string QuantityUnit,
        [property: JsonPropertyName("plannedQuantity")] decimal PlannedQuantity,
        [property: JsonPropertyName("appliedQuantity")] decimal AppliedQuantity,
        [property: JsonPropertyName("varianceQuantity")] decimal VarianceQuantity);

    public sealed record ApplicationZonePerformanceDto(
        [property: JsonPropertyName("zonePubId")] Guid? ZonePubId,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("treatedAreaM2")] decimal TreatedAreaM2,
        [property: JsonPropertyName("plannedApplicationCount")] int PlannedApplicationCount,
        [property: JsonPropertyName("appliedApplicationCount")] int AppliedApplicationCount,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<ApplicationNutrientPerformanceDto> Nutrients);

    public sealed record ApplicationPlanZoneSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("treatedAreaM2")] decimal TreatedAreaM2,
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("areas")] IReadOnlyList<ApplicationPlanZoneAreaSummaryDto> Areas,
        [property: JsonPropertyName("nutrientTargets")] IReadOnlyList<ApplicationPlanZoneNutrientTargetDto> NutrientTargets);

    public sealed record ApplicationPlanZoneAreaSummaryDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("areaM2")] decimal AreaM2);

    public sealed record ApplicationPlanZoneNutrientTargetDto(
        [property: JsonPropertyName("nutrientPubId")] Guid NutrientPubId,
        [property: JsonPropertyName("nutrientCode")] string NutrientCode,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("annualTargetGramsPerM2")] decimal AnnualTargetGramsPerM2,
        [property: JsonPropertyName("minimumGramsPerM2")] decimal? MinimumGramsPerM2,
        [property: JsonPropertyName("maximumGramsPerM2")] decimal? MaximumGramsPerM2,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("rationale")] string Rationale);

    public sealed record SaveApplicationPlanZoneDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("zonePubId")] Guid? ZonePubId,
        [property: JsonPropertyName("zoneName")][property: MaxLength(120)] string ZoneName,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("areaPubIds")] IReadOnlyList<Guid> AreaPubIds,
        [property: JsonPropertyName("notes")][property: MaxLength(1000)] string Notes);

    public sealed record SaveApplicationPlanZoneTargetsDto(
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("targets")] IReadOnlyList<SaveApplicationPlanZoneTargetDto> Targets);

    public sealed record SaveApplicationPlanZoneTargetDto(
        [property: JsonPropertyName("nutrientPubId")] Guid NutrientPubId,
        [property: JsonPropertyName("annualTargetGramsPerM2")] decimal AnnualTargetGramsPerM2,
        [property: JsonPropertyName("minimumGramsPerM2")] decimal? MinimumGramsPerM2,
        [property: JsonPropertyName("maximumGramsPerM2")] decimal? MaximumGramsPerM2,
        [property: JsonPropertyName("source")][property: MaxLength(120)] string Source,
        [property: JsonPropertyName("rationale")][property: MaxLength(1000)] string Rationale);

    public sealed record UpdateApplicationPlanAreaSnapshotDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("areaSnapshotPubId")] Guid AreaSnapshotPubId,
        [property: JsonPropertyName("areaName")][property: MaxLength(120)] string AreaName,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2,
        [property: JsonPropertyName("surfaces")] IReadOnlyList<UpdateApplicationPlanSurfaceSnapshotDto>? Surfaces = null);

    public sealed record UpdateApplicationPlanSurfaceSnapshotDto(
        [property: JsonPropertyName("sourceSurfacePubId")] Guid? SourceSurfacePubId,
        [property: JsonPropertyName("surfaceName")][property: MaxLength(120)] string SurfaceName,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2);

    public sealed record UpdateApplicationPlanProductSnapshotDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("productName")][property: MaxLength(250)] string ProductName,
        [property: JsonPropertyName("productTypeName")][property: MaxLength(120)] string ProductTypeName,
        [property: JsonPropertyName("rateUnit")] string? RateUnit,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("densityKgPerL")] decimal? DensityKgPerL,
        [property: JsonPropertyName("packSize")] decimal PackSize,
        [property: JsonPropertyName("packUnitLabel")][property: MaxLength(20)] string PackUnitLabel,
        [property: JsonPropertyName("unitCost")] decimal UnitCost,
        [property: JsonPropertyName("inventoryQuantitySnapshot")] decimal InventoryQuantitySnapshot,
        [property: JsonPropertyName("isAvailable")] bool IsAvailable,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<UpdateApplicationPlanProductNutrientSnapshotDto> Nutrients);

    public sealed record UpdateApplicationPlanProductNutrientSnapshotDto(
        [property: JsonPropertyName("sourceNutrientPubId")] Guid? SourceNutrientPubId,
        [property: JsonPropertyName("nutrientCode")][property: MaxLength(20)] string NutrientCode,
        [property: JsonPropertyName("nutrientName")][property: MaxLength(120)] string NutrientName,
        [property: JsonPropertyName("analysisAmount")] decimal AnalysisAmount,
        [property: JsonPropertyName("analysisBasis")] string AnalysisBasis,
        [property: JsonPropertyName("analysisSource")][property: MaxLength(250)] string AnalysisSource,
        [property: JsonPropertyName("isVerified")] bool IsVerified);

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
        [property: JsonPropertyName("quantity")] decimal Quantity,
        [property: JsonPropertyName("analysisBasis")] string AnalysisBasis = "PercentByMass");

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
        [property: JsonPropertyName("validationStatus")] string ValidationStatus,
        [property: JsonPropertyName("zonePubId")] Guid? ZonePubId = null,
        [property: JsonPropertyName("zoneName")] string? ZoneName = null,
        [property: JsonPropertyName("plannedLocalDate")] DateOnly? PlannedLocalDate = null,
        [property: JsonPropertyName("isoWeekNumber")] int? IsoWeekNumber = null,
        [property: JsonPropertyName("rateUnit")] string? RateUnit = null,
        [property: JsonPropertyName("treatedAreaM2")] decimal TreatedAreaM2 = 0m,
        [property: JsonPropertyName("plannedProductQuantity")] decimal PlannedProductQuantity = 0m,
        [property: JsonPropertyName("plannedProductQuantityUnit")] string? PlannedProductQuantityUnit = null,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<ApplicationPlanItemNutrientDto>? Nutrients = null,
        [property: JsonPropertyName("executionCount")] int ExecutionCount = 0,
        [property: JsonPropertyName("lastExecutedLocalDate")] DateOnly? LastExecutedLocalDate = null,
        [property: JsonPropertyName("currentScheduledLocalDate")] DateOnly? CurrentScheduledLocalDate = null,
        [property: JsonPropertyName("scheduleStatus")] string ScheduleStatus = "Planned",
        [property: JsonPropertyName("latestScheduleReason")] string? LatestScheduleReason = null,
        [property: JsonPropertyName("latestScheduleNote")] string? LatestScheduleNote = null,
        [property: JsonPropertyName("scheduleEvents")] IReadOnlyList<ApplicationPlanItemScheduleEventDto>? ScheduleEvents = null,
        [property: JsonPropertyName("isDone")] bool IsDone = false,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null,
        [property: JsonPropertyName("machineryName")] string MachineryName = "");

    public sealed record ApplicationPlanItemNutrientDto(
        [property: JsonPropertyName("nutrientCode")] string NutrientCode,
        [property: JsonPropertyName("gramsPerM2")] decimal GramsPerM2,
        [property: JsonPropertyName("totalGrams")] decimal TotalGrams);

    public sealed record ApplicationPlanItemScheduleEventDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("eventType")] string EventType,
        [property: JsonPropertyName("fromStatus")] string FromStatus,
        [property: JsonPropertyName("toStatus")] string ToStatus,
        [property: JsonPropertyName("fromScheduledLocalDate")] DateOnly FromScheduledLocalDate,
        [property: JsonPropertyName("toScheduledLocalDate")] DateOnly? ToScheduledLocalDate,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("note")] string Note,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc);

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
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("zoneName")] string? ZoneName = null,
        [property: JsonPropertyName("actualRateUnit")] string? ActualRateUnit = null,
        [property: JsonPropertyName("treatedAreaM2")] decimal TreatedAreaM2 = 0m,
        [property: JsonPropertyName("actualProductQuantity")] decimal ActualProductQuantity = 0m,
        [property: JsonPropertyName("actualProductQuantityUnit")] string? ActualProductQuantityUnit = null,
        [property: JsonPropertyName("nutrients")] IReadOnlyList<ApplicationPlanItemNutrientDto>? Nutrients = null,
        [property: JsonPropertyName("completesPlannedApplication")] bool CompletesPlannedApplication = true,
        [property: JsonPropertyName("plannedApplicationPubId")] Guid? PlannedApplicationPubId = null,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null,
        [property: JsonPropertyName("machineryName")] string MachineryName = "",
        [property: JsonPropertyName("machineryCapacity")] decimal? MachineryCapacity = null,
        [property: JsonPropertyName("machineryCapacityUnit")] string MachineryCapacityUnit = "",
        [property: JsonPropertyName("machineryLastCalibratedOn")] DateOnly? MachineryLastCalibratedOn = null,
        [property: JsonPropertyName("machineryCalibrationDueOn")] DateOnly? MachineryCalibrationDueOn = null);

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
        [property: JsonPropertyName("strategySummary")][property: MaxLength(4000)] string StrategySummary,
        [property: JsonPropertyName("planName")][property: MaxLength(120)] string? PlanName = null,
        [property: JsonPropertyName("planPurpose")][property: MaxLength(1000)] string? PlanPurpose = null);

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
        [property: JsonPropertyName("restrictions")][property: MaxLength(2000)] string Restrictions,
        [property: JsonPropertyName("zonePubId")] Guid? ZonePubId = null,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null);

    public sealed record AddApplicationPlanCalendarProductDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("defaultRate")] decimal DefaultRate,
        [property: JsonPropertyName("defaultWaterVolume")] decimal DefaultWaterVolume,
        [property: JsonPropertyName("notes")][property: MaxLength(1000)] string Notes);

    public sealed record SaveApplicationPlanCalendarCellDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("planItemPubId")] Guid? PlanItemPubId,
        [property: JsonPropertyName("isoWeekNumber")] int IsoWeekNumber,
        [property: JsonPropertyName("scheduledLocalDate")] DateOnly? ScheduledLocalDate,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("waterVolume")] decimal WaterVolume,
        [property: JsonPropertyName("triggerType")] string TriggerType,
        [property: JsonPropertyName("productCategory")] string ProductCategory,
        [property: JsonPropertyName("targetProblem")][property: MaxLength(250)] string TargetProblem,
        [property: JsonPropertyName("reason")][property: MaxLength(2000)] string Reason,
        [property: JsonPropertyName("instructions")][property: MaxLength(2000)] string Instructions,
        [property: JsonPropertyName("restrictions")][property: MaxLength(2000)] string Restrictions,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null);

    public sealed record BulkSaveApplicationPlanCalendarRowDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("startWeek")] int StartWeek,
        [property: JsonPropertyName("endWeek")] int EndWeek,
        [property: JsonPropertyName("intervalWeeks")] int IntervalWeeks,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("waterVolume")] decimal WaterVolume,
        [property: JsonPropertyName("triggerType")] string TriggerType,
        [property: JsonPropertyName("productCategory")] string ProductCategory,
        [property: JsonPropertyName("reason")][property: MaxLength(2000)] string Reason,
        [property: JsonPropertyName("overwriteExisting")] bool OverwriteExisting,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null);

    public sealed record ClearApplicationPlanCalendarRangeDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("productSnapshotPubId")] Guid ProductSnapshotPubId,
        [property: JsonPropertyName("startWeek")] int StartWeek,
        [property: JsonPropertyName("endWeek")] int EndWeek,
        [property: JsonPropertyName("removeProductRow")] bool RemoveProductRow);

    public sealed record DeleteApplicationPlanItemDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("planItemPubId")] Guid PlanItemPubId);

    public sealed record CopyApplicationPlanCalendarDto(
        [property: JsonPropertyName("sourceAnnualPlanPubId")] Guid SourceAnnualPlanPubId,
        [property: JsonPropertyName("targetAnnualPlanPubId")] Guid TargetAnnualPlanPubId,
        [property: JsonPropertyName("sourceZonePubId")] Guid SourceZonePubId,
        [property: JsonPropertyName("targetZonePubId")] Guid TargetZonePubId,
        [property: JsonPropertyName("includeValues")] bool IncludeValues,
        [property: JsonPropertyName("overwriteExisting")] bool OverwriteExisting);

    public sealed record ShiftApplicationPlanCalendarDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("zonePubId")] Guid ZonePubId,
        [property: JsonPropertyName("fromWeek")] int FromWeek,
        [property: JsonPropertyName("weekOffset")] int WeekOffset,
        [property: JsonPropertyName("includeFollowing")] bool IncludeFollowing,
        [property: JsonPropertyName("overwriteExisting")] bool OverwriteExisting,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("note")][property: MaxLength(1000)] string Note);

    public sealed record ApplicationPlanCalendarMutationResultDto(
        [property: JsonPropertyName("createdCount")] int CreatedCount,
        [property: JsonPropertyName("updatedCount")] int UpdatedCount,
        [property: JsonPropertyName("deletedCount")] int DeletedCount,
        [property: JsonPropertyName("skippedCount")] int SkippedCount,
        [property: JsonPropertyName("conflictWeeks")] IReadOnlyList<int> ConflictWeeks);

    public sealed record CreateApplicationPlanDraftRevisionDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("sourceRevisionPubId")] Guid? SourceRevisionPubId,
        [property: JsonPropertyName("changeSummary")][property: MaxLength(1000)] string ChangeSummary);

    public sealed record ApproveApplicationPlanRevisionDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("revisionPubId")] Guid RevisionPubId,
        [property: JsonPropertyName("changeSummary")][property: MaxLength(1000)] string ChangeSummary);

    public sealed record CloneAnnualApplicationPlanDto(
        [property: JsonPropertyName("sourceAnnualPlanPubId")] Guid SourceAnnualPlanPubId,
        [property: JsonPropertyName("targetYear")] int TargetYear,
        [property: JsonPropertyName("planName")][property: MaxLength(120)] string PlanName,
        [property: JsonPropertyName("planPurpose")][property: MaxLength(1000)] string PlanPurpose,
        [property: JsonPropertyName("includeValues")] bool IncludeValues);

    public sealed record UpdateApplicationPlanItemScheduleDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("plannedApplicationPubId")] Guid PlannedApplicationPubId,
        [property: JsonPropertyName("action")] string Action,
        [property: JsonPropertyName("scheduledLocalDate")] DateOnly? ScheduledLocalDate,
        [property: JsonPropertyName("expectedScheduledLocalDate")] DateOnly? ExpectedScheduledLocalDate,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("note")][property: MaxLength(1000)] string Note);

    public sealed record RecordApplicationExecutionDto(
        [property: JsonPropertyName("annualPlanPubId")] Guid AnnualPlanPubId,
        [property: JsonPropertyName("plannedApplicationPubId")] Guid? PlannedApplicationPubId,
        [property: JsonPropertyName("executedDate")] DateTime ExecutedDate,
        [property: JsonPropertyName("actualProductPubId")] Guid? ActualProductPubId,
        [property: JsonPropertyName("actualRate")] decimal ActualRate,
        [property: JsonPropertyName("actualCost")] decimal ActualCost,
        [property: JsonPropertyName("weatherAtExecutionJson")][property: MaxLength(4000)] string WeatherAtExecutionJson,
        [property: JsonPropertyName("notes")][property: MaxLength(2000)] string Notes,
        [property: JsonPropertyName("zonePubId")] Guid? ZonePubId = null,
        [property: JsonPropertyName("completesPlannedApplication")] bool CompletesPlannedApplication = true,
        [property: JsonPropertyName("machineryPubId")] Guid? MachineryPubId = null);

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
