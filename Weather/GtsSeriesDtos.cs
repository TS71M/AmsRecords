namespace AmsRecords.Weather;
using AmsRecords.Units;

public static class GtsSeriesDtos
{
    public sealed record GtsSeriesDto(
        [property: JsonPropertyName("year")] short Year,
        [property: JsonPropertyName("points")] IReadOnlyList<GtsPointDto> Points,
        [property: JsonPropertyName("completeness")] GtsDtos.GtsDataCompletenessDto? Completeness = null
    );

    public sealed record GtsPointDto(
        [property: JsonPropertyName("date")] DateOnly Date,
        [property: JsonPropertyName("dailyMeanTempC")] decimal? DailyMeanTempC,
        [property: JsonPropertyName("dailyMeanTemp")] UnitValueDto? DailyMeanTemp,
        [property: JsonPropertyName("monthWeight")] decimal MonthWeight,
        [property: JsonPropertyName("dailyContribution")] decimal DailyContribution,
        [property: JsonPropertyName("runningGts")] decimal RunningGts
    );
}
