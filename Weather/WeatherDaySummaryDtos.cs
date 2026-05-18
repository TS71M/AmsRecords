using AmsRecords.Units;

namespace AmsRecords.Weather;

public static class WeatherDaySummaryDtos
{
    public sealed record WeatherDaySummaryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("dayUtc")] DateTime DayUtc,
        [property: JsonPropertyName("tempMinC")] decimal? TempMinC,
        [property: JsonPropertyName("tempMaxC")] decimal? TempMaxC,
        [property: JsonPropertyName("tempMeanC")] decimal? TempMeanC,
        [property: JsonPropertyName("precipMm")] decimal? PrecipMm,
        [property: JsonPropertyName("tempMin")] UnitValueDto? TempMin,
        [property: JsonPropertyName("tempMax")] UnitValueDto? TempMax,
        [property: JsonPropertyName("tempMean")] UnitValueDto? TempMean,
        [property: JsonPropertyName("precip")] UnitValueDto? Precip,
        [property: JsonPropertyName("humidityMeanPct")] short? HumidityMeanPct,
        [property: JsonPropertyName("source")] string? Source,
        [property: JsonPropertyName("updatedAtUtc")] DateTime UpdatedAtUtc
    );

    public sealed record WeatherDaySummaryQueryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("take")] int Take = 200,
        [property: JsonPropertyName("skip")] int Skip = 0
    );
}
