namespace AmsRecords.Irrigation;

public static class SprinklerPerformanceDtos
{
    public static class PerformanceStatuses
    {
        public const string Exact = "EXACT";
        public const string Interpolated = "INTERPOLATED";
        public const string ClampedLower = "CLAMPED_LOWER";
        public const string ClampedUpper = "CLAMPED_UPPER";
        public const string BelowSupportedRange = "BELOW_SUPPORTED_RANGE";
        public const string AboveSupportedRange = "ABOVE_SUPPORTED_RANGE";
        public const string NoPerformanceData = "NO_PERFORMANCE_DATA";
        public const string InvalidPerformanceCurve = "INVALID_PERFORMANCE_CURVE";
        public const string NozzleNotFound = "NOZZLE_NOT_FOUND";
        public const string InactiveNozzle = "INACTIVE_NOZZLE";
        public const string InvalidPressure = "INVALID_PRESSURE";
    }

    public static class DistributionProfileStatuses
    {
        public const string Exact = "EXACT";
        public const string NoProfileAtPressure = "NO_PROFILE_AT_PRESSURE";
        public const string NoProfileData = "NO_PROFILE_DATA";
        public const string NozzleNotFound = "NOZZLE_NOT_FOUND";
        public const string InactiveNozzle = "INACTIVE_NOZZLE";
        public const string InvalidPressure = "INVALID_PRESSURE";
    }

    public sealed record SprinklerPerformanceDataPointDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("pressureBar")] decimal PressureBar,
        [property: JsonPropertyName("flowM3H")] decimal FlowM3H,
        [property: JsonPropertyName("radiusM")] decimal RadiusM,
        [property: JsonPropertyName("trajectoryDegrees")] decimal? TrajectoryDegrees,
        [property: JsonPropertyName("rotationSeconds")] decimal? RotationSeconds,
        [property: JsonPropertyName("dataSource")] string DataSource,
        [property: JsonPropertyName("dataQualityCode")] string DataQualityCode);

    public sealed record SprinklerPerformanceResult(
        [property: JsonPropertyName("nozzlePubId")] Guid NozzlePubId,
        [property: JsonPropertyName("requestedPressureBar")] decimal RequestedPressureBar,
        [property: JsonPropertyName("supported")] bool Supported,
        [property: JsonPropertyName("flowM3H")] decimal? FlowM3H,
        [property: JsonPropertyName("radiusM")] decimal? RadiusM,
        [property: JsonPropertyName("trajectoryDegrees")] decimal? TrajectoryDegrees,
        [property: JsonPropertyName("rotationSeconds")] decimal? RotationSeconds,
        [property: JsonPropertyName("dataSource")] string? DataSource,
        [property: JsonPropertyName("dataQualityCode")] string? DataQualityCode,
        [property: JsonPropertyName("interpolationStatus")] string InterpolationStatus,
        [property: JsonPropertyName("supportedPressureMinBar")] decimal? SupportedPressureMinBar,
        [property: JsonPropertyName("supportedPressureMaxBar")] decimal? SupportedPressureMaxBar,
        [property: JsonPropertyName("lowerSourcePressureBar")] decimal? LowerSourcePressureBar,
        [property: JsonPropertyName("upperSourcePressureBar")] decimal? UpperSourcePressureBar,
        [property: JsonPropertyName("sourcePerformancePubIds")] IReadOnlyList<Guid> SourcePerformancePubIds,
        [property: JsonPropertyName("sourceDataQualityCodes")] IReadOnlyList<string> SourceDataQualityCodes,
        [property: JsonPropertyName("warning")] string? Warning);

    public sealed record SprinklerNozzlePerformanceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("nozzlePubId")] Guid NozzlePubId,
        [property: JsonPropertyName("pressureBar")] decimal PressureBar,
        [property: JsonPropertyName("flowM3H")] decimal FlowM3H,
        [property: JsonPropertyName("radiusM")] decimal RadiusM,
        [property: JsonPropertyName("trajectoryDegrees")] decimal? TrajectoryDegrees,
        [property: JsonPropertyName("rotationSeconds")] decimal? RotationSeconds,
        [property: JsonPropertyName("dataSource")] string DataSource,
        [property: JsonPropertyName("dataQualityCode")] string DataQualityCode,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record SprinklerNozzlePerformanceSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("pressureBar")][param: Range(typeof(decimal), "0.001", "100")] decimal PressureBar,
        [property: JsonPropertyName("flowM3H")][param: Range(typeof(decimal), "0.0001", "1000")] decimal FlowM3H,
        [property: JsonPropertyName("radiusM")][param: Range(typeof(decimal), "0.001", "200")] decimal RadiusM,
        [property: JsonPropertyName("trajectoryDegrees")][param: Range(typeof(decimal), "0", "90")] decimal? TrajectoryDegrees,
        [property: JsonPropertyName("rotationSeconds")][param: Range(typeof(decimal), "0.001", "3600")] decimal? RotationSeconds,
        [property: JsonPropertyName("dataSource")][param: Required, MaxLength(500)] string DataSource,
        [property: JsonPropertyName("dataQualityCode")][param: Required, MaxLength(40)] string DataQualityCode,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record SprinklerDistributionPointDto(
        [property: JsonPropertyName("normalizedDistance")] decimal NormalizedDistance,
        [property: JsonPropertyName("relativeApplication")] decimal RelativeApplication);

    public sealed record SprinklerDistributionPointSaveDto(
        [property: JsonPropertyName("normalizedDistance")] decimal NormalizedDistance,
        [property: JsonPropertyName("relativeApplication")] decimal RelativeApplication);

    public sealed record SprinklerDistributionProfileDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("nozzlePubId")] Guid NozzlePubId,
        [property: JsonPropertyName("pressureBar")] decimal PressureBar,
        [property: JsonPropertyName("dataSource")] string DataSource,
        [property: JsonPropertyName("confidenceLevelCode")] string ConfidenceLevelCode,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("points")] IReadOnlyList<SprinklerDistributionPointDto> Points);

    public sealed record SprinklerDistributionProfileSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("pressureBar")][param: Range(typeof(decimal), "0.001", "100")] decimal PressureBar,
        [property: JsonPropertyName("dataSource")][param: Required, MaxLength(500)] string DataSource,
        [property: JsonPropertyName("confidenceLevelCode")][param: Required, MaxLength(40)] string ConfidenceLevelCode,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("points")][param: Required] IReadOnlyList<SprinklerDistributionPointSaveDto> Points);

    public sealed record SprinklerDistributionNormalizationResult(
        [property: JsonPropertyName("success")] bool Success,
        [property: JsonPropertyName("points")] IReadOnlyList<SprinklerDistributionPointDto> Points,
        [property: JsonPropertyName("error")] string? Error);

    public sealed record SprinklerDistributionProfileResult(
        [property: JsonPropertyName("nozzlePubId")] Guid NozzlePubId,
        [property: JsonPropertyName("requestedPressureBar")] decimal RequestedPressureBar,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("profilePubId")] Guid? ProfilePubId,
        [property: JsonPropertyName("profilePressureBar")] decimal? ProfilePressureBar,
        [property: JsonPropertyName("dataSource")] string? DataSource,
        [property: JsonPropertyName("confidenceLevelCode")] string? ConfidenceLevelCode,
        [property: JsonPropertyName("supportedPressuresBar")] IReadOnlyList<decimal> SupportedPressuresBar,
        [property: JsonPropertyName("points")] IReadOnlyList<SprinklerDistributionPointDto> Points,
        [property: JsonPropertyName("warning")] string? Warning);
}
