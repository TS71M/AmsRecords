using AmsRecords.Units;

namespace AmsRecords.Weather;

public sealed class WeatherObservationDtos
{
    public record WeatherObservationDto(
        [property: JsonPropertyName("observedAtUtc")] DateTime ObservedAtUtc,
        [property: JsonPropertyName("tempC")] decimal TempC,
        [property: JsonPropertyName("relativeHumidity")] short RelativeHumidity,
        [property: JsonPropertyName("pressureHPa")] short PressureHPa,
        [property: JsonPropertyName("windSpeedMs")] decimal WindSpeedMs,
        [property: JsonPropertyName("windGustMs")] decimal? WindGustMs,
        [property: JsonPropertyName("cloudPct")] short? CloudPct,
        [property: JsonPropertyName("rainMm1h")] decimal? RainMm1h,
        [property: JsonPropertyName("snowMm1h")] decimal? SnowMm1h,
        [property: JsonPropertyName("weatherMain")] string? WeatherMain
    );

    public sealed record WeatherObservationDisplayDto(
        [property: JsonPropertyName("observedAtUtc")] DateTime ObservedAtUtc,
        [property: JsonPropertyName("temp")] UnitValueDto Temp,
        [property: JsonPropertyName("relativeHumidity")] UnitValueDto RelativeHumidity,
        [property: JsonPropertyName("pressureHpa")] UnitValueDto? PressureHpa, // ✅ nullable
        [property: JsonPropertyName("wind")] UnitValueDto? Wind,
        [property: JsonPropertyName("gust")] UnitValueDto? Gust,
        [property: JsonPropertyName("cloudPct")] int? CloudPct,
        [property: JsonPropertyName("rainMm1h")] UnitValueDto? RainMm1h,
        [property: JsonPropertyName("snowMm1h")] UnitValueDto? SnowMm1h,
        [property: JsonPropertyName("weatherMain")] string? WeatherMain
    );

    public sealed record WeatherObservationMiniDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fromUtc")] DateTime FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime ToUtc,
        [property: JsonPropertyName("count")] int Count,

        [property: JsonPropertyName("tempMin")] UnitValueDto? TempMin,
        [property: JsonPropertyName("tempAvg")] UnitValueDto? TempAvg,
        [property: JsonPropertyName("tempMax")] UnitValueDto? TempMax,

        [property: JsonPropertyName("rhMin")] int? RhMin,
        [property: JsonPropertyName("rhAvg")] int? RhAvg,
        [property: JsonPropertyName("rhMax")] int? RhMax,

        [property: JsonPropertyName("rainSum")] UnitValueDto? RainSum,
        [property: JsonPropertyName("windMax")] UnitValueDto? WindMax,

        // Optional: downsampled points for quick charts
        [property: JsonPropertyName("points")] IReadOnlyList<WeatherObservationMiniPointDto> Points
    );

    public sealed record WeatherObservationMiniPointDto(
        [property: JsonPropertyName("observedAtUtc")] DateTime ObservedAtUtc,
        [property: JsonPropertyName("temp")] decimal? Temp,
        [property: JsonPropertyName("rh")] int? Rh,
        [property: JsonPropertyName("wind")] decimal? Wind,
        [property: JsonPropertyName("rain")] decimal? Rain
    );

    public sealed record WeatherObservationDayGroupDto(
        [property: JsonPropertyName("dateUtc")] DateOnly DateUtc,
        [property: JsonPropertyName("count")] int Count,

        [property: JsonPropertyName("tempMin")] UnitValueDto? TempMin,
        [property: JsonPropertyName("tempMax")] UnitValueDto? TempMax,

        [property: JsonPropertyName("rhMin")] int? RhMin,
        [property: JsonPropertyName("rhMax")] int? RhMax,

        [property: JsonPropertyName("rainSum")] UnitValueDto? RainSum,
        [property: JsonPropertyName("windMax")] UnitValueDto? WindMax,

        [property: JsonPropertyName("hours")] IReadOnlyList<WeatherObservationDisplayDto> Hours
    );

    public sealed record ObservationRow(
        DateTime ObservedAtUtc,
        decimal TempC,
        short RelativeHumidity,
        short PressureHPa,
        decimal WindSpeedMs,
        decimal? WindGustMs,
        short? WindDeg,
        short? CloudPct,
        decimal? RainMm1h,
        decimal? SnowMm1h,
        string? WeatherMain
    );
}