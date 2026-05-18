namespace AmsRecords.Weather;

public static class OpenWeatherRequestLogDtos
{
    public sealed record OpenWeatherDailyUsageDto(
        [property: JsonPropertyName("dayUtc")] DateTime DayUtc,
        [property: JsonPropertyName("requestsUsed")] int RequestsUsed,
        [property: JsonPropertyName("cumulatedRequestsUsed")] int CumulatedRequestsUsed
    );
}
