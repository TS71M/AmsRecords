using AmsRecords.Messages;
using AmsRecords.Units;

namespace AmsRecords.Weather;

public static class WeatherCurrentDtos
{
    public sealed record WeatherCurrentDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("noDataReason")] string? NoDataReason,

        [property: JsonPropertyName("observedAtUtc")] DateTime? ObservedAtUtc,

        [property: JsonPropertyName("temp")] UnitValueDto? Temp,
        [property: JsonPropertyName("dewPoint")] UnitValueDto? DewPoint,
        [property: JsonPropertyName("relativeHumidity")] UnitValueDto? RelativeHumidity,

        [property: JsonPropertyName("windSpeedMs")] UnitValueDto? WindSpeedMs,
        [property: JsonPropertyName("windGustMs")] UnitValueDto? WindGustMs,
        [property: JsonPropertyName("windDeg")] short? WindDeg,

        [property: JsonPropertyName("cloudPct")] short? CloudPct,
        [property: JsonPropertyName("rainMm1h")] UnitValueDto? RainMm1h,
        [property: JsonPropertyName("snowMm1h")] UnitValueDto? SnowMm1h,

        [property: JsonPropertyName("weatherCode")] int? WeatherCode,
        [property: JsonPropertyName("weatherMain")] string? WeatherMain,
        [property: JsonPropertyName("iconKey")] string? IconKey,
        [property: JsonPropertyName("weatherTitle")] string? WeatherTitle,

        [property: JsonPropertyName("source")] string? Source,
        [property: JsonPropertyName("messages")] IReadOnlyList<AppMessageDto> Messages
    )
    {
        public WeatherCurrentDto() : this(
            Guid.Empty,
            string.Empty,
            false,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            [])
        { }

        public static WeatherCurrentDto Empty(
            Guid fieldPubId,
            string fieldName,
            string reason,
            IReadOnlyList<AppMessageDto>? messages = null)
            => new(
                FieldPubId: fieldPubId,
                FieldName: fieldName,
                HasData: false,
                NoDataReason: reason,
                ObservedAtUtc: null,
                Temp: null,
                DewPoint: null,
                RelativeHumidity: null,
                WindSpeedMs: null,
                WindGustMs: null,
                WindDeg: null,
                CloudPct: null,
                RainMm1h: null,
                SnowMm1h: null,
                WeatherCode: null,
                WeatherMain: null,
                IconKey: null,
                WeatherTitle: null,
                Source: null,
                Messages: messages ?? []
            );
    }
}
