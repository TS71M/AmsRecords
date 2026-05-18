namespace AmsRecords.Weather;

public static class WeatherBackfillTestDtos
{
    public sealed record WeatherBackfillTestResultDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("dayUtc")] DateTime DayUtc,
        [property: JsonPropertyName("action")] string Action,
        [property: JsonPropertyName("source")] string? Source,
        [property: JsonPropertyName("tempMinC")] decimal? TempMinC,
        [property: JsonPropertyName("tempMaxC")] decimal? TempMaxC,
        [property: JsonPropertyName("tempMeanC")] decimal? TempMeanC,
        [property: JsonPropertyName("precipMm")] decimal? PrecipMm,
        [property: JsonPropertyName("humidityMeanPct")] short? HumidityMeanPct
    );
}
