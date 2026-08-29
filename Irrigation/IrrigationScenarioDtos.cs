namespace AmsRecords.Irrigation;

public static class IrrigationScenarioDtos
{
    public const string InstalledSource = "installed";
    public const string ScenarioSource = "scenario";

    public sealed record IrrigationScenarioSnapshotDto(
        [property: JsonPropertyName("meanMm")] decimal MeanMm,
        [property: JsonPropertyName("dUlq")] decimal? DUlq,
        [property: JsonPropertyName("cu")] decimal? CU,
        [property: JsonPropertyName("targetDeviation")] decimal TargetDeviation,
        [property: JsonPropertyName("outsideTargetPercent")] decimal OutsideTargetPercent,
        [property: JsonPropertyName("flowM3H")] decimal FlowM3H);

    public sealed record IrrigationScenarioHeadSettingDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("headName")] string HeadName,
        [property: JsonPropertyName("nozzleOverridePubId")] Guid? NozzleOverridePubId,
        [property: JsonPropertyName("nozzleOverrideName")] string NozzleOverrideName,
        [property: JsonPropertyName("runtimeMinutes")] decimal RuntimeMinutes,
        [property: JsonPropertyName("pressureOverrideBar")] decimal? PressureOverrideBar,
        [property: JsonPropertyName("enabled")] bool Enabled,
        [property: JsonPropertyName("arcOverrideDegrees")] decimal? ArcOverrideDegrees);

    public sealed record IrrigationScenarioDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("defaultRuntimeMinutes")] decimal DefaultRuntimeMinutes,
        [property: JsonPropertyName("targetDepthMm")] decimal TargetDepthMm,
        [property: JsonPropertyName("gridResolutionM")] decimal GridResolutionM,
        [property: JsonPropertyName("createdUtc")] DateTime CreatedUtc,
        [property: JsonPropertyName("createdByPubId")] Guid CreatedByPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("snapshot")] IrrigationScenarioSnapshotDto Snapshot,
        [property: JsonPropertyName("headSettings")] IReadOnlyList<IrrigationScenarioHeadSettingDto> HeadSettings);

    public sealed record IrrigationScenarioCreateDto(
        [property: JsonPropertyName("name")][param: Required, MaxLength(160)] string Name,
        [property: JsonPropertyName("description")][param: MaxLength(2000)] string Description,
        [property: JsonPropertyName("simulation")][param: Required] IrrigationVisualSimulatorDtos.IrrigationSimulatorRequestDto Simulation);

    public sealed record IrrigationScenarioCompareRequestDto(
        [property: JsonPropertyName("beforeScenarioPubId")] Guid? BeforeScenarioPubId,
        [property: JsonPropertyName("afterScenarioPubId")] Guid? AfterScenarioPubId);

    public sealed record IrrigationScenarioComparisonSourceDto(
        [property: JsonPropertyName("sourceType")] string SourceType,
        [property: JsonPropertyName("scenarioPubId")] Guid? ScenarioPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("snapshot")] IrrigationScenarioSnapshotDto Snapshot);

    public sealed record IrrigationScenarioMetricDifferenceDto(
        [property: JsonPropertyName("metric")] string Metric,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("beforeValue")] decimal? BeforeValue,
        [property: JsonPropertyName("afterValue")] decimal? AfterValue,
        [property: JsonPropertyName("delta")] decimal? Delta);

    public sealed record IrrigationScenarioHeadConfigurationDto(
        [property: JsonPropertyName("nozzlePubId")] Guid? NozzlePubId,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("runtimeMinutes")] decimal RuntimeMinutes,
        [property: JsonPropertyName("pressureBar")] decimal? PressureBar,
        [property: JsonPropertyName("enabled")] bool Enabled,
        [property: JsonPropertyName("arcDegrees")] decimal ArcDegrees);

    public sealed record IrrigationScenarioHeadDifferenceDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("headName")] string HeadName,
        [property: JsonPropertyName("before")] IrrigationScenarioHeadConfigurationDto Before,
        [property: JsonPropertyName("after")] IrrigationScenarioHeadConfigurationDto After,
        [property: JsonPropertyName("changedFields")] IReadOnlyList<string> ChangedFields);

    public sealed record IrrigationScenarioComparisonDto(
        [property: JsonPropertyName("before")] IrrigationScenarioComparisonSourceDto Before,
        [property: JsonPropertyName("after")] IrrigationScenarioComparisonSourceDto After,
        [property: JsonPropertyName("metricDifferences")] IReadOnlyList<IrrigationScenarioMetricDifferenceDto> MetricDifferences,
        [property: JsonPropertyName("headDifferences")] IReadOnlyList<IrrigationScenarioHeadDifferenceDto> HeadDifferences,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings);
}
