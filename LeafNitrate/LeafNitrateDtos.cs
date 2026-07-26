namespace AmsRecords.LeafNitrate;

public static class LeafNitrateDtos
{
    public static class MeasurementBases
    {
        public const string No3 = "NO3";
        public const string No3N = "NO3-N";
    }

    public static class MeasurementUnits
    {
        public const string Ppm = "ppm";
        public const string MgPerL = "mg/L";
    }

    public static class SampleConditions
    {
        public const string Dry = "Dry";
        public const string Moist = "Moist";
        public const string WetFreeWater = "WetFreeWater";
    }

    public static class ThresholdScopes
    {
        public const string GrassSpecies = "GrassSpecies";
        public const string Area = "Area";
        public const string Surface = "Surface";
    }

    public sealed record LeafNitrateReadingInputDto(
        [property: JsonPropertyName("rawValue")] decimal RawValue,
        [property: JsonPropertyName("rawBasis")] string RawBasis,
        [property: JsonPropertyName("rawUnit")] string RawUnit,
        [property: JsonPropertyName("stabilized")] bool Stabilized = true,
        [property: JsonPropertyName("rejected")] bool Rejected = false,
        [property: JsonPropertyName("rejectionReason")] string? RejectionReason = null);

    public sealed record LeafNitrateMeasurementCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("sampledAtUtc")] DateTime SampledAtUtc,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("deviceModel")] string DeviceModel,
        [property: JsonPropertyName("sampleMethod")] string SampleMethod,
        [property: JsonPropertyName("sampleCondition")] string SampleCondition,
        [property: JsonPropertyName("recentlyIrrigated")] bool RecentlyIrrigated,
        [property: JsonPropertyName("recentlyRained")] bool RecentlyRained,
        [property: JsonPropertyName("dewRemoved")] bool DewRemoved,
        [property: JsonPropertyName("calibrationAtUtc")] DateTime? CalibrationAtUtc,
        [property: JsonPropertyName("calibrationLowPpmNo3")] decimal? CalibrationLowPpmNo3,
        [property: JsonPropertyName("calibrationHighPpmNo3")] decimal? CalibrationHighPpmNo3,
        [property: JsonPropertyName("sampleTemperatureC")] decimal? SampleTemperatureC,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("readings")] IReadOnlyList<LeafNitrateReadingInputDto> Readings);

    public sealed record LeafNitrateReadingDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sequence")] int Sequence,
        [property: JsonPropertyName("rawValue")] decimal RawValue,
        [property: JsonPropertyName("rawBasis")] string RawBasis,
        [property: JsonPropertyName("rawUnit")] string RawUnit,
        [property: JsonPropertyName("normalizedNo3NPpm")] decimal NormalizedNo3NPpm,
        [property: JsonPropertyName("stabilized")] bool Stabilized,
        [property: JsonPropertyName("rejected")] bool Rejected,
        [property: JsonPropertyName("rejectionReason")] string? RejectionReason);

    public sealed record LeafNitrateMeasurementDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("sampledAtUtc")] DateTime SampledAtUtc,
        [property: JsonPropertyName("recordedAtUtc")] DateTime RecordedAtUtc,
        [property: JsonPropertyName("recordedByName")] string? RecordedByName,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("deviceModel")] string DeviceModel,
        [property: JsonPropertyName("sampleMethod")] string SampleMethod,
        [property: JsonPropertyName("sampleCondition")] string SampleCondition,
        [property: JsonPropertyName("recentlyIrrigated")] bool RecentlyIrrigated,
        [property: JsonPropertyName("recentlyRained")] bool RecentlyRained,
        [property: JsonPropertyName("dewRemoved")] bool DewRemoved,
        [property: JsonPropertyName("calibrationAtUtc")] DateTime? CalibrationAtUtc,
        [property: JsonPropertyName("calibrationLowPpmNo3")] decimal? CalibrationLowPpmNo3,
        [property: JsonPropertyName("calibrationHighPpmNo3")] decimal? CalibrationHighPpmNo3,
        [property: JsonPropertyName("sampleTemperatureC")] decimal? SampleTemperatureC,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("averageNo3NPpm")] decimal? AverageNo3NPpm,
        [property: JsonPropertyName("medianNo3NPpm")] decimal? MedianNo3NPpm,
        [property: JsonPropertyName("minimumNo3NPpm")] decimal? MinimumNo3NPpm,
        [property: JsonPropertyName("maximumNo3NPpm")] decimal? MaximumNo3NPpm,
        [property: JsonPropertyName("uncertaintyPercent")] decimal UncertaintyPercent,
        [property: JsonPropertyName("qualityFlags")] IReadOnlyList<string> QualityFlags,
        [property: JsonPropertyName("readings")] IReadOnlyList<LeafNitrateReadingDto> Readings);

    public sealed record LeafNitrateTimelineQueryDto(
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("includeWet")] bool IncludeWet = true);

    public sealed record LeafNitrateThresholdDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("scopeType")] string ScopeType,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("areaName")] string? AreaName,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("grassSpeciesName")] string? GrassSpeciesName,
        [property: JsonPropertyName("sampleMethod")] string SampleMethod,
        [property: JsonPropertyName("minimumNo3NPpm")] decimal MinimumNo3NPpm,
        [property: JsonPropertyName("maximumNo3NPpm")] decimal MaximumNo3NPpm,
        [property: JsonPropertyName("startMonth")] int? StartMonth,
        [property: JsonPropertyName("endMonth")] int? EndMonth,
        [property: JsonPropertyName("mowingHeightMm")] decimal? MowingHeightMm,
        [property: JsonPropertyName("sourceReference")] string? SourceReference,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("effectiveFromUtc")] DateTime EffectiveFromUtc,
        [property: JsonPropertyName("effectiveToUtc")] DateTime? EffectiveToUtc,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("createdByName")] string? CreatedByName,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record LeafNitrateThresholdSaveDto(
        [property: JsonPropertyName("scopeType")] string ScopeType,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("sampleMethod")] string SampleMethod,
        [property: JsonPropertyName("minimumNo3NPpm")] decimal MinimumNo3NPpm,
        [property: JsonPropertyName("maximumNo3NPpm")] decimal MaximumNo3NPpm,
        [property: JsonPropertyName("startMonth")] int? StartMonth,
        [property: JsonPropertyName("endMonth")] int? EndMonth,
        [property: JsonPropertyName("mowingHeightMm")] decimal? MowingHeightMm,
        [property: JsonPropertyName("sourceReference")] string? SourceReference,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("effectiveFromUtc")] DateTime EffectiveFromUtc,
        [property: JsonPropertyName("contextFieldPubId")] Guid? ContextFieldPubId = null);

    public sealed record LeafNitrateThresholdSuggestionQueryDto(
        [property: JsonPropertyName("scopeType")] string ScopeType,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId);

    public sealed record LeafNitrateThresholdSuggestionFactorDto(
        [property: JsonPropertyName("factor")] string Factor,
        [property: JsonPropertyName("value")] string? Value,
        [property: JsonPropertyName("available")] bool Available);

    public sealed record LeafNitrateThresholdSuggestionDto(
        [property: JsonPropertyName("scopeType")] string ScopeType,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("sampleMethod")] string SampleMethod,
        [property: JsonPropertyName("measurementBasis")] string MeasurementBasis,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("minimumNo3NPpm")] decimal MinimumNo3NPpm,
        [property: JsonPropertyName("maximumNo3NPpm")] decimal MaximumNo3NPpm,
        [property: JsonPropertyName("startMonth")] int StartMonth,
        [property: JsonPropertyName("endMonth")] int EndMonth,
        [property: JsonPropertyName("seasonLabel")] string SeasonLabel,
        [property: JsonPropertyName("localMeasurementCount")] int LocalMeasurementCount,
        [property: JsonPropertyName("localExcludedMeasurementCount")] int LocalExcludedMeasurementCount,
        [property: JsonPropertyName("localMedianNo3NPpm")] decimal? LocalMedianNo3NPpm,
        [property: JsonPropertyName("confidence")] string Confidence,
        [property: JsonPropertyName("rationale")] string Rationale,
        [property: JsonPropertyName("provenance")] string Provenance,
        [property: JsonPropertyName("warning")] string Warning,
        [property: JsonPropertyName("factors")] IReadOnlyList<LeafNitrateThresholdSuggestionFactorDto> Factors,
        [property: JsonPropertyName("requiresSeparateSave")] bool RequiresSeparateSave);

    public sealed record LeafNitrateTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("measurements")] IReadOnlyList<LeafNitrateMeasurementDto> Measurements,
        [property: JsonPropertyName("thresholds")] IReadOnlyList<LeafNitrateThresholdDto> Thresholds);
}
