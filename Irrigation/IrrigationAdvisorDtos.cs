namespace AmsRecords.Irrigation;

using static IrrigationDemandDtos;

public static class IrrigationAdvisorCodes
{
    public static class OverallStatuses
    {
        public const string Healthy = "HEALTHY";
        public const string Attention = "ATTENTION";
        public const string ActionRequired = "ACTION_REQUIRED";
        public const string InsufficientData = "INSUFFICIENT_DATA";
    }

    public static class Severities
    {
        public const string Info = "INFO";
        public const string Low = "LOW";
        public const string Moderate = "MODERATE";
        public const string High = "HIGH";
        public const string Critical = "CRITICAL";
    }

    public static class Categories
    {
        public const string Distribution = "DISTRIBUTION";
        public const string Hydraulics = "HYDRAULICS";
        public const string Nozzle = "NOZZLE";
        public const string Pressure = "PRESSURE";
        public const string Overspray = "OVERSPRAY";
        public const string Soil = "SOIL";
        public const string Moisture = "MOISTURE";
        public const string Weather = "WEATHER";
        public const string Runtime = "RUNTIME";
        public const string DataQuality = "DATA_QUALITY";
    }

    public static class ClaimTypes
    {
        public const string Fact = "FACT";
        public const string ModelEstimate = "MODEL_ESTIMATE";
        public const string Hypothesis = "HYPOTHESIS";
    }

    public static class EvidenceTypes
    {
        public const string Measured = "MEASURED";
        public const string DeterministicResult = "DETERMINISTIC_RESULT";
        public const string ModelEstimate = "MODEL_ESTIMATE";
        public const string Observed = "OBSERVED";
        public const string DataAvailability = "DATA_AVAILABILITY";
    }

    public static class ActionTypes
    {
        public const string RunSimulation = "RUN_SIMULATION";
        public const string TestAlternativeNozzle = "TEST_ALTERNATIVE_NOZZLE";
        public const string PerformCatchCanTest = "PERFORM_CATCH_CAN_TEST";
        public const string MeasurePressure = "MEASURE_PRESSURE";
        public const string InspectSprinkler = "INSPECT_SPRINKLER";
        public const string CheckArc = "CHECK_ARC";
        public const string CheckLeveling = "CHECK_LEVELING";
        public const string CheckBlockedNozzle = "CHECK_BLOCKED_NOZZLE";
        public const string ReviewIrrigationRuntime = "REVIEW_IRRIGATION_RUNTIME";
        public const string InvestigateSoilRootzone = "INVESTIGATE_SOIL_ROOTZONE";
        public const string TakeTdrReadings = "TAKE_TDR_READINGS";
    }
}

public static class IrrigationAdvisorDtos
{
    public sealed record IrrigationAdvisorRequestDto(
        [property: JsonPropertyName("simulation")] IrrigationVisualSimulatorDtos.IrrigationSimulatorRequestDto Simulation,
        [property: JsonPropertyName("demand")] IrrigationDemandRequestDto? Demand = null,
        [property: JsonPropertyName("includeHydraulicAnalysis")] bool IncludeHydraulicAnalysis = true);

    public sealed record IrrigationAnalysisAreaDetailsDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("areaTypeCode")] string AreaTypeCode,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("areaM2")] double AreaM2);

    public sealed record IrrigationAnalysisHeadDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("stationName")] string StationName,
        [property: JsonPropertyName("controllerName")] string ControllerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("installedNozzleName")] string InstalledNozzleName,
        [property: JsonPropertyName("installedPressureBar")] decimal? InstalledPressureBar,
        [property: JsonPropertyName("arcDegrees")] decimal ArcDegrees,
        [property: JsonPropertyName("mapped")] bool Mapped,
        [property: JsonPropertyName("isAreaMember")] bool IsAreaMember,
        [property: JsonPropertyName("limitation")] string? Limitation);

    public sealed record IrrigationAnalysisZoneDto(
        [property: JsonPropertyName("zoneId")] string ZoneId,
        [property: JsonPropertyName("kind")] string Kind,
        [property: JsonPropertyName("cellCount")] int CellCount,
        [property: JsonPropertyName("areaM2")] double AreaM2,
        [property: JsonPropertyName("averageDepthMm")] double AverageDepthMm,
        [property: JsonPropertyName("minimumDepthMm")] double MinimumDepthMm,
        [property: JsonPropertyName("maximumDepthMm")] double MaximumDepthMm,
        [property: JsonPropertyName("differenceFromAreaMeanPercent")] double? DifferenceFromAreaMeanPercent,
        [property: JsonPropertyName("centroidX")] double CentroidX,
        [property: JsonPropertyName("centroidY")] double CentroidY);

    public sealed record IrrigationAnalysisSimulationDto(
        [property: JsonPropertyName("runtimeMinutes")] double RuntimeMinutes,
        [property: JsonPropertyName("targetDepthMm")] double TargetDepthMm,
        [property: JsonPropertyName("gridResolutionM")] double GridResolutionM,
        [property: JsonPropertyName("confidenceCode")] string ConfidenceCode,
        [property: JsonPropertyName("targetCellCount")] int TargetCellCount,
        [property: JsonPropertyName("meanDepthMm")] double MeanDepthMm,
        [property: JsonPropertyName("minimumDepthMm")] double MinimumDepthMm,
        [property: JsonPropertyName("maximumDepthMm")] double MaximumDepthMm,
        [property: JsonPropertyName("distributionUniformityLowQuarter")] double? DistributionUniformityLowQuarter,
        [property: JsonPropertyName("christiansenUniformityCoefficient")] double? ChristiansenUniformityCoefficient,
        [property: JsonPropertyName("coefficientOfVariation")] double? CoefficientOfVariation,
        [property: JsonPropertyName("belowTargetPercent")] double BelowTargetPercent,
        [property: JsonPropertyName("withinTargetPercent")] double WithinTargetPercent,
        [property: JsonPropertyName("aboveTargetPercent")] double AboveTargetPercent,
        [property: JsonPropertyName("outsideTargetPercent")] double OutsideTargetPercent,
        [property: JsonPropertyName("outsideTargetAreaApplicationM3")] double? OutsideTargetAreaApplicationM3,
        [property: JsonPropertyName("targetApplicationEfficiencyPercent")] double? TargetApplicationEfficiencyPercent,
        [property: JsonPropertyName("zones")] IReadOnlyList<IrrigationAnalysisZoneDto> Zones,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings);

    public sealed record IrrigationAnalysisNozzlePerformanceDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("stationName")] string StationName,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("pressureBar")] decimal? PressureBar,
        [property: JsonPropertyName("flowM3H")] double? FlowM3H,
        [property: JsonPropertyName("radiusM")] double? RadiusM,
        [property: JsonPropertyName("arcDegrees")] decimal ArcDegrees,
        [property: JsonPropertyName("performanceStatus")] string PerformanceStatus,
        [property: JsonPropertyName("simulated")] bool Simulated,
        [property: JsonPropertyName("limitation")] string? Limitation);

    public sealed record IrrigationAnalysisHydraulicHeadDto(
        [property: JsonPropertyName("headPubId")] Guid? HeadPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("predictedPressureBar")] double PredictedPressureBar,
        [property: JsonPropertyName("predictedFlowM3H")] double PredictedFlowM3H,
        [property: JsonPropertyName("pressureDeficitBar")] double PressureDeficitBar,
        [property: JsonPropertyName("performanceStatus")] string PerformanceStatus);

    public sealed record IrrigationAnalysisHydraulicsDto(
        [property: JsonPropertyName("scenarioPubId")] Guid? ScenarioPubId,
        [property: JsonPropertyName("scenarioName")] string? ScenarioName,
        [property: JsonPropertyName("valid")] bool Valid,
        [property: JsonPropertyName("converged")] bool Converged,
        [property: JsonPropertyName("calculationMethodCode")] string CalculationMethodCode,
        [property: JsonPropertyName("totalFlowM3H")] double? TotalFlowM3H,
        [property: JsonPropertyName("minimumHeadPressureBar")] double? MinimumHeadPressureBar,
        [property: JsonPropertyName("maximumVelocityMS")] double? MaximumVelocityMS,
        [property: JsonPropertyName("headsBelowTargetCount")] int? HeadsBelowTargetCount,
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationAnalysisHydraulicHeadDto> Heads,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings,
        [property: JsonPropertyName("validationIssues")] IReadOnlyList<string> ValidationIssues);

    public sealed record IrrigationAnalysisWeatherDto(
        [property: JsonPropertyName("fromDateUtc")] DateOnly FromDateUtc,
        [property: JsonPropertyName("toDateUtc")] DateOnly ToDateUtc,
        [property: JsonPropertyName("rainfallMm")] decimal? RainfallMm,
        [property: JsonPropertyName("effectiveRainfallMm")] decimal? EffectiveRainfallMm,
        [property: JsonPropertyName("creditedRainfallMm")] decimal? CreditedRainfallMm,
        [property: JsonPropertyName("rainfallSourceCode")] string? RainfallSourceCode,
        [property: JsonPropertyName("weatherDayCount")] int WeatherDayCount);

    public sealed record IrrigationAnalysisEtDto(
        [property: JsonPropertyName("et0Mm")] decimal? Et0Mm,
        [property: JsonPropertyName("cropCoefficient")] decimal? CropCoefficient,
        [property: JsonPropertyName("cropEvapotranspirationMm")] decimal? CropEvapotranspirationMm,
        [property: JsonPropertyName("climaticWaterDeficitMm")] decimal? ClimaticWaterDeficitMm);

    public sealed record IrrigationAnalysisMoistureDto(
        [property: JsonPropertyName("currentVwcPercent")] decimal? CurrentVwcPercent,
        [property: JsonPropertyName("targetVwcPercent")] decimal? TargetVwcPercent,
        [property: JsonPropertyName("soilMoistureDeficitMm")] decimal? SoilMoistureDeficitMm,
        [property: JsonPropertyName("rootZoneDepthMm")] decimal? RootZoneDepthMm,
        [property: JsonPropertyName("observedAtUtc")] DateTime? ObservedAtUtc,
        [property: JsonPropertyName("sourceCode")] string? SourceCode);

    public sealed record IrrigationAnalysisCatchCanDto(
        [property: JsonPropertyName("testPubId")] Guid TestPubId,
        [property: JsonPropertyName("testDateUtc")] DateTime TestDateUtc,
        [property: JsonPropertyName("runtimeSeconds")] int RuntimeSeconds,
        [property: JsonPropertyName("measurementCount")] int MeasurementCount,
        [property: JsonPropertyName("meanMm")] double MeanMm,
        [property: JsonPropertyName("distributionUniformityLowQuarter")] double? DistributionUniformityLowQuarter,
        [property: JsonPropertyName("christiansenUniformityCoefficient")] double? ChristiansenUniformityCoefficient,
        [property: JsonPropertyName("modelMeanAbsoluteErrorMm")] double ModelMeanAbsoluteErrorMm,
        [property: JsonPropertyName("calibrationStatus")] string CalibrationStatus,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings);

    public sealed record IrrigationAnalysisHistoryDto(
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("runtimeMinutes")] double? RuntimeMinutes,
        [property: JsonPropertyName("appliedDepthMm")] double? AppliedDepthMm,
        [property: JsonPropertyName("sourceCode")] string SourceCode);

    public sealed record IrrigationEvidenceDto(
        [property: JsonPropertyName("evidenceId")] string EvidenceId,
        [property: JsonPropertyName("section")] string Section,
        [property: JsonPropertyName("evidenceType")] string EvidenceType,
        [property: JsonPropertyName("statement")] string Statement,
        [property: JsonPropertyName("metricCode")] string? MetricCode = null,
        [property: JsonPropertyName("value")] double? Value = null,
        [property: JsonPropertyName("unit")] string? Unit = null,
        [property: JsonPropertyName("qualityCode")] string? QualityCode = null);

    public sealed record IrrigationAnalysisContext(
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("area")] IrrigationAnalysisAreaDetailsDto Area,
        [property: JsonPropertyName("installedHeads")] IReadOnlyList<IrrigationAnalysisHeadDto> InstalledHeads,
        [property: JsonPropertyName("simulation")] IrrigationAnalysisSimulationDto Simulation,
        [property: JsonPropertyName("hydraulics")] IrrigationAnalysisHydraulicsDto? Hydraulics,
        [property: JsonPropertyName("nozzlePerformance")] IReadOnlyList<IrrigationAnalysisNozzlePerformanceDto> NozzlePerformance,
        [property: JsonPropertyName("waterDemand")] IrrigationAreaDemandResultDto? WaterDemand,
        [property: JsonPropertyName("weather")] IrrigationAnalysisWeatherDto? Weather,
        [property: JsonPropertyName("et")] IrrigationAnalysisEtDto? Et,
        [property: JsonPropertyName("moisture")] IrrigationAnalysisMoistureDto? Moisture,
        [property: JsonPropertyName("catchCan")] IrrigationAnalysisCatchCanDto? CatchCan,
        [property: JsonPropertyName("recentIrrigationHistory")] IReadOnlyList<IrrigationAnalysisHistoryDto> RecentIrrigationHistory,
        [property: JsonPropertyName("evidence")] IReadOnlyList<IrrigationEvidenceDto> Evidence,
        [property: JsonPropertyName("requiredLimitations")] IReadOnlyList<string> RequiredLimitations);

    public sealed record IrrigationRecommendedActionDto(
        [property: JsonPropertyName("actionType")] string ActionType,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("evidenceIds")] IReadOnlyList<string> EvidenceIds);

    public sealed record IrrigationAdvisorFindingDto(
        [property: JsonPropertyName("severity")] string Severity,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("claimType")] string ClaimType,
        [property: JsonPropertyName("evidence")] IReadOnlyList<IrrigationEvidenceDto> Evidence,
        [property: JsonPropertyName("interpretation")] string Interpretation,
        [property: JsonPropertyName("recommendedActions")] IReadOnlyList<IrrigationRecommendedActionDto> RecommendedActions,
        [property: JsonPropertyName("confidence")] double Confidence);

    public sealed record IrrigationAdvisorResultDto(
        [property: JsonPropertyName("overallStatus")] string OverallStatus,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("findings")] IReadOnlyList<IrrigationAdvisorFindingDto> Findings,
        [property: JsonPropertyName("limitations")] IReadOnlyList<string> Limitations,
        [property: JsonPropertyName("context")] IrrigationAnalysisContext Context,
        [property: JsonPropertyName("advisoryOnly")] bool AdvisoryOnly = true);
}
