namespace AmsRecords.Irrigation;

public static class IrrigationOptimizationObjectiveCodes
{
    public const string MaximumDistributionUniformityLowQuarter = "MAX_DULQ";
    public const string MaximumChristiansenUniformity = "MAX_CU";
    public const string MinimumTargetDeviation = "MIN_TARGET_DEVIATION";
    public const string MinimumOutsideTargetApplication = "MIN_OUTSIDE_TARGET_APPLICATION";
    public const string MinimumTotalWaterVolume = "MIN_TOTAL_WATER_VOLUME";
    public const string MinimumNozzleChanges = "MIN_NOZZLE_CHANGES";
    public const string CompositeWeighted = "COMPOSITE_WEIGHTED";

    public static IReadOnlyList<string> All { get; } =
    [
        MaximumDistributionUniformityLowQuarter,
        MaximumChristiansenUniformity,
        MinimumTargetDeviation,
        MinimumOutsideTargetApplication,
        MinimumTotalWaterVolume,
        MinimumNozzleChanges,
        CompositeWeighted
    ];

    public static bool IsValid(string? value)
        => All.Contains(value?.Trim(), StringComparer.OrdinalIgnoreCase);

    public static string Normalize(string value)
        => All.First(x => string.Equals(x, value.Trim(), StringComparison.OrdinalIgnoreCase));
}

public static class IrrigationOptimizationReasonCodes
{
    public const string ImprovedLowQuarter = "IMPROVED_LOW_QUARTER";
    public const string ImprovedCu = "IMPROVED_CU";
    public const string ReducedTargetDeviation = "REDUCED_TARGET_DEVIATION";
    public const string ReducedOutsideTargetApplication = "REDUCED_OUTSIDE_TARGET_APPLICATION";
    public const string ReducedOverspray = "REDUCED_OVERSPRAY";
    public const string ReducedWaterVolume = "REDUCED_WATER_VOLUME";
    public const string FlowLimitReached = "FLOW_LIMIT_REACHED";
    public const string NozzleChangeLimitReached = "NOZZLE_CHANGE_LIMIT_REACHED";
    public const string RuntimeLimitReached = "RUNTIME_LIMIT_REACHED";
    public const string TargetDepthSatisfied = "TARGET_DEPTH_SATISFIED";
    public const string SearchLimitReached = "SEARCH_LIMIT_REACHED";
    public const string GridResolutionCoarsened = "GRID_RESOLUTION_COARSENED";
    public const string TargetDepthUnreachable = "TARGET_DEPTH_UNREACHABLE";
    public const string NoFeasibleNozzleCandidates = "NO_FEASIBLE_NOZZLE_CANDIDATES";
    public const string NoBetterScenarioFound = "NO_BETTER_SCENARIO_FOUND";
}

public static class IrrigationNozzleOptimizerDtos
{
    public sealed record IrrigationOptimizationCompositeWeightsDto(
        [property: JsonPropertyName("dUlq")] double DUlq = 1d,
        [property: JsonPropertyName("cu")] double CU = 1d,
        [property: JsonPropertyName("targetDeviation")] double TargetDeviation = 1d,
        [property: JsonPropertyName("outsideTargetApplication")] double OutsideTargetApplication = 1d,
        [property: JsonPropertyName("totalWaterVolume")] double TotalWaterVolume = 1d,
        [property: JsonPropertyName("nozzleChanges")] double NozzleChanges = 1d);

    public sealed record IrrigationOptimizationHeadControlDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("currentRuntimeMinutes")] double? CurrentRuntimeMinutes = null,
        [property: JsonPropertyName("keepExistingNozzle")] bool KeepExistingNozzle = false);

    public sealed record IrrigationNozzleOptimizationRequestDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("targetDepthMm")] double TargetDepthMm,
        [property: JsonPropertyName("maximumSimultaneousFlowM3H")] double? MaximumSimultaneousFlowM3H = null,
        [property: JsonPropertyName("allowedNozzleFamilies")] IReadOnlyList<string>? AllowedNozzleFamilies = null,
        [property: JsonPropertyName("maximumNozzleChanges")] int MaximumNozzleChanges = 2,
        [property: JsonPropertyName("maximumRuntimeMinutes")] double MaximumRuntimeMinutes = 30d,
        [property: JsonPropertyName("minimumPressureSuitability")] double MinimumPressureSuitability = 0d,
        [property: JsonPropertyName("objective")] string Objective = IrrigationOptimizationObjectiveCodes.MaximumDistributionUniformityLowQuarter,
        [property: JsonPropertyName("compositeWeights")] IrrigationOptimizationCompositeWeightsDto? CompositeWeights = null,
        [property: JsonPropertyName("headControls")] IReadOnlyList<IrrigationOptimizationHeadControlDto>? HeadControls = null,
        [property: JsonPropertyName("defaultCurrentRuntimeMinutes")] double DefaultCurrentRuntimeMinutes = 10d,
        [property: JsonPropertyName("targetToleranceFraction")] double TargetToleranceFraction = 0.1d,
        [property: JsonPropertyName("gridResolutionM")] double GridResolutionM = 0.5d,
        [property: JsonPropertyName("requestedOptionCount")] int RequestedOptionCount = 3);

    public sealed record IrrigationOptimizationMetricsDto(
        [property: JsonPropertyName("flowM3H")] double FlowM3H,
        [property: JsonPropertyName("dUlq")] double? DUlq,
        [property: JsonPropertyName("cu")] double? CU,
        [property: JsonPropertyName("meanDepthMm")] double MeanDepthMm,
        [property: JsonPropertyName("targetDeviationMm")] double TargetDeviationMm,
        [property: JsonPropertyName("outsideTargetPercent")] double OutsideTargetPercent,
        [property: JsonPropertyName("outsideTargetAreaVolumeM3")] double OutsideTargetAreaVolumeM3,
        [property: JsonPropertyName("totalWaterVolumeM3")] double TotalWaterVolumeM3,
        [property: JsonPropertyName("nozzleChangeCount")] int NozzleChangeCount);

    public sealed record IrrigationOptimizationChangedHeadDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("headName")] string HeadName,
        [property: JsonPropertyName("oldNozzlePubId")] Guid? OldNozzlePubId,
        [property: JsonPropertyName("oldNozzleName")] string OldNozzleName,
        [property: JsonPropertyName("newNozzlePubId")] Guid NewNozzlePubId,
        [property: JsonPropertyName("newNozzleName")] string NewNozzleName,
        [property: JsonPropertyName("oldRuntimeMinutes")] double OldRuntimeMinutes,
        [property: JsonPropertyName("newRuntimeMinutes")] double NewRuntimeMinutes);

    public sealed record IrrigationOptimizationBaselineDto(
        [property: JsonPropertyName("metrics")] IrrigationOptimizationMetricsDto Metrics,
        [property: JsonPropertyName("headSettings")] IReadOnlyList<IrrigationVisualSimulatorDtos.IrrigationSimulatorHeadOverrideDto> HeadSettings);

    public sealed record IrrigationOptimizationOptionDto(
        [property: JsonPropertyName("rank")] int Rank,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("score")] double Score,
        [property: JsonPropertyName("metrics")] IrrigationOptimizationMetricsDto Metrics,
        [property: JsonPropertyName("changedHeads")] IReadOnlyList<IrrigationOptimizationChangedHeadDto> ChangedHeads,
        [property: JsonPropertyName("headSettings")] IReadOnlyList<IrrigationVisualSimulatorDtos.IrrigationSimulatorHeadOverrideDto> HeadSettings,
        [property: JsonPropertyName("simulation")] IrrigationVisualSimulatorDtos.IrrigationSimulatorRequestDto Simulation,
        [property: JsonPropertyName("reasonCodes")] IReadOnlyList<string> ReasonCodes);

    public sealed record IrrigationNozzleOptimizationResultDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("objective")] string Objective,
        [property: JsonPropertyName("baseline")] IrrigationOptimizationBaselineDto Baseline,
        [property: JsonPropertyName("options")] IReadOnlyList<IrrigationOptimizationOptionDto> Options,
        [property: JsonPropertyName("availableNozzleFamilies")] IReadOnlyList<string> AvailableNozzleFamilies,
        [property: JsonPropertyName("evaluatedScenarioCount")] int EvaluatedScenarioCount,
        [property: JsonPropertyName("searchLimitReached")] bool SearchLimitReached,
        [property: JsonPropertyName("reasonCodes")] IReadOnlyList<string> ReasonCodes,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings);
}
