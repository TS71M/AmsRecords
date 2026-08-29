namespace AmsRecords.Irrigation;

public static class IrrigationVisualSimulatorDtos
{
    public sealed record IrrigationSimulatorAreaOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("hasBoundary")] bool HasBoundary);

    public sealed record IrrigationSimulatorNozzleOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("nominalPressureBar")] decimal? NominalPressureBar,
        [property: JsonPropertyName("minimumPressureBar")] decimal MinimumPressureBar,
        [property: JsonPropertyName("maximumPressureBar")] decimal MaximumPressureBar);

    public sealed record IrrigationSimulatorHeadDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("stationName")] string StationName,
        [property: JsonPropertyName("controllerName")] string ControllerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("installedNozzlePubId")] Guid? InstalledNozzlePubId,
        [property: JsonPropertyName("installedNozzleName")] string InstalledNozzleName,
        [property: JsonPropertyName("installedPressureBar")] decimal? InstalledPressureBar,
        [property: JsonPropertyName("mapX")] double? MapX,
        [property: JsonPropertyName("mapY")] double? MapY,
        [property: JsonPropertyName("arcDegrees")] decimal ArcDegrees,
        [property: JsonPropertyName("orientationDegrees")] decimal OrientationDegrees,
        [property: JsonPropertyName("isAreaMember")] bool IsAreaMember,
        [property: JsonPropertyName("nozzles")] IReadOnlyList<IrrigationSimulatorNozzleOptionDto> Nozzles,
        [property: JsonPropertyName("limitation")] string? Limitation);

    public sealed record IrrigationSimulatorWorkspaceDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("systemName")] string SystemName,
        [property: JsonPropertyName("areas")] IReadOnlyList<IrrigationSimulatorAreaOptionDto> Areas,
        [property: JsonPropertyName("boundary")] IrrigationDigitalTwinDtos.IrrigationAreaBoundaryDto Boundary,
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationSimulatorHeadDto> Heads,
        [property: JsonPropertyName("defaultRuntimeMinutes")] double DefaultRuntimeMinutes,
        [property: JsonPropertyName("defaultTargetDepthMm")] double DefaultTargetDepthMm,
        [property: JsonPropertyName("defaultGridResolutionM")] double DefaultGridResolutionM,
        [property: JsonPropertyName("targetToleranceFraction")] double TargetToleranceFraction,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings,
        [property: JsonPropertyName("calibration")] CatchCanCalibrationDtos.IrrigationModelCalibrationSummaryDto? Calibration = null);

    public sealed record IrrigationSimulatorHeadOverrideDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("enabled")] bool Enabled,
        [property: JsonPropertyName("runtimeMinutes")] double? RuntimeMinutes,
        [property: JsonPropertyName("nozzlePubId")] Guid? NozzlePubId,
        [property: JsonPropertyName("pressureBar")] decimal? PressureBar,
        [property: JsonPropertyName("arcDegrees")] decimal? ArcDegrees = null);

    public sealed record IrrigationSimulatorRequestDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("runtimeMinutes")] double RuntimeMinutes,
        [property: JsonPropertyName("targetDepthMm")] double TargetDepthMm,
        [property: JsonPropertyName("gridResolutionM")] double GridResolutionM,
        [property: JsonPropertyName("includeInstalledComparison")] bool IncludeInstalledComparison,
        [property: JsonPropertyName("headOverrides")] IReadOnlyList<IrrigationSimulatorHeadOverrideDto> HeadOverrides,
        [property: JsonPropertyName("applySiteCalibration")] bool? ApplySiteCalibration = null);

    public sealed record IrrigationSimulatorHeadResultDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("enabled")] bool Enabled,
        [property: JsonPropertyName("runtimeMinutes")] double RuntimeMinutes,
        [property: JsonPropertyName("nozzlePubId")] Guid? NozzlePubId,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("pressureBar")] decimal? PressureBar,
        [property: JsonPropertyName("flowM3H")] double? FlowM3H,
        [property: JsonPropertyName("radiusM")] double? RadiusM,
        [property: JsonPropertyName("arcDegrees")] decimal ArcDegrees,
        [property: JsonPropertyName("performanceStatus")] string PerformanceStatus,
        [property: JsonPropertyName("simulated")] bool Simulated,
        [property: JsonPropertyName("limitation")] string? Limitation);

    public sealed record IrrigationSimulatorGridDto(
        [property: JsonPropertyName("originX")] double OriginX,
        [property: JsonPropertyName("originY")] double OriginY,
        [property: JsonPropertyName("cellSizeM")] double CellSizeM,
        [property: JsonPropertyName("width")] int Width,
        [property: JsonPropertyName("height")] int Height,
        [property: JsonPropertyName("applicationDepthMm")] IReadOnlyList<double> ApplicationDepthMm,
        [property: JsonPropertyName("targetMask")] IReadOnlyList<bool> TargetMask);

    public sealed record IrrigationSimulatorResultDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("grid")] IrrigationSimulatorGridDto Grid,
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationSimulatorHeadResultDto> Heads,
        [property: JsonPropertyName("metrics")] IrrigationDistributionMetrics Metrics,
        [property: JsonPropertyName("installedMetrics")] IrrigationDistributionMetrics? InstalledMetrics,
        [property: JsonPropertyName("comparison")] IrrigationDistributionComparison? Comparison,
        [property: JsonPropertyName("confidence")] IrrigationSimulationConfidence Confidence,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings,
        [property: JsonPropertyName("calibration")] CatchCanCalibrationDtos.IrrigationModelCalibrationSummaryDto? Calibration = null);
}
