namespace AmsRecords.WeatherDiaryRules;

public static class WeatherDiaryRuleDtos
{
    public static class MetricKeys
    {
        public const string TempMaxC = "tempMaxC";
        public const string TempMinC = "tempMinC";
        public const string TempMeanC = "tempMeanC";
        public const string PrecipMm = "precipMm";
        public const string DewHours = "dewHours";
        public const string HumidityMeanPct = "humidityMeanPct";
        public const string GpPct = "gpPct";

        public static readonly string[] All =
        [
            TempMaxC,
            TempMinC,
            TempMeanC,
            PrecipMm,
            DewHours,
            HumidityMeanPct,
            GpPct
        ];
    }

    public static class ComparisonTypes
    {
        public const string Gte = "gte";
        public const string Lte = "lte";

        public static readonly string[] All = [Gte, Lte];
    }

    public static class BaselineSources
    {
        public const string None = "None";
        public const string FieldMonthlyMeanTempC = "FieldMonthlyMeanTempC";

        public static readonly string[] All = [None, FieldMonthlyMeanTempC];
    }

    public sealed record WeatherDiaryRuleDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("climateZoneCode")] string? ClimateZoneCode,
        [property: JsonPropertyName("climateZoneName")] string? ClimateZoneName,
        [property: JsonPropertyName("ruleName")] string RuleName,
        [property: JsonPropertyName("eventKey")] string EventKey,
        [property: JsonPropertyName("metricKey")] string MetricKey,
        [property: JsonPropertyName("comparisonType")] string ComparisonType,
        [property: JsonPropertyName("baselineSource")] string BaselineSource,
        [property: JsonPropertyName("warningAbsolute")] decimal? WarningAbsolute,
        [property: JsonPropertyName("dangerAbsolute")] decimal? DangerAbsolute,
        [property: JsonPropertyName("warningOffset")] decimal? WarningOffset,
        [property: JsonPropertyName("dangerOffset")] decimal? DangerOffset,
        [property: JsonPropertyName("relatedRiskKey")] string? RelatedRiskKey,
        [property: JsonPropertyName("minimumDurationDays")] int MinimumDurationDays,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("createdUtc")] DateTime CreatedUtc,
        [property: JsonPropertyName("updatedUtc")] DateTime UpdatedUtc,
        [property: JsonPropertyName("createdBy")] string? CreatedBy
    );

    public sealed record WeatherDiaryRuleCreateDto(
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("ruleName")] string RuleName,
        [property: JsonPropertyName("eventKey")] string EventKey,
        [property: JsonPropertyName("metricKey")] string MetricKey,
        [property: JsonPropertyName("comparisonType")] string ComparisonType,
        [property: JsonPropertyName("baselineSource")] string BaselineSource,
        [property: JsonPropertyName("warningAbsolute")] decimal? WarningAbsolute,
        [property: JsonPropertyName("dangerAbsolute")] decimal? DangerAbsolute,
        [property: JsonPropertyName("warningOffset")] decimal? WarningOffset,
        [property: JsonPropertyName("dangerOffset")] decimal? DangerOffset,
        [property: JsonPropertyName("relatedRiskKey")] string? RelatedRiskKey,
        [property: JsonPropertyName("minimumDurationDays")] int MinimumDurationDays,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record WeatherDiaryRuleUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("ruleName")] string RuleName,
        [property: JsonPropertyName("eventKey")] string EventKey,
        [property: JsonPropertyName("metricKey")] string MetricKey,
        [property: JsonPropertyName("comparisonType")] string ComparisonType,
        [property: JsonPropertyName("baselineSource")] string BaselineSource,
        [property: JsonPropertyName("warningAbsolute")] decimal? WarningAbsolute,
        [property: JsonPropertyName("dangerAbsolute")] decimal? DangerAbsolute,
        [property: JsonPropertyName("warningOffset")] decimal? WarningOffset,
        [property: JsonPropertyName("dangerOffset")] decimal? DangerOffset,
        [property: JsonPropertyName("relatedRiskKey")] string? RelatedRiskKey,
        [property: JsonPropertyName("minimumDurationDays")] int MinimumDurationDays,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active
    );
}
