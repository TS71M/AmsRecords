using AmsRecords.Units;

namespace AmsRecords.Weather;

public static class WeatherForecastDtos
{
    public sealed record ForecastPointVm(
        string HourLabel,
        DateTime ForecastForUtc,
        decimal TempC,
        string TempCTxt,
        decimal? DewPoint,
        string? DewPointTxt,
        decimal Rh,
        string RhTxt,
        decimal? RainMm1h,
        string? RainMm1hTxt,
        decimal? Pop,
        string? PopTxt,
        decimal WindMs,
        string WindMsTxt,
        decimal? GustMs,
        string? GustMsTxt,
        int? WeatherCode,
        string? WeatherMain
        );

    public sealed record WeatherForecastHourDisplayDto(
        [property: JsonPropertyName("forecastForUtc")] DateTime ForecastForUtc,
        [property: JsonPropertyName("temp")] UnitValueDto Temp,
        [property: JsonPropertyName("dewPoint")] UnitValueDto DewPoint,
        [property: JsonPropertyName("rh")] UnitValueDto Rh,
        [property: JsonPropertyName("pressure")] UnitValueDto Pressure,
        [property: JsonPropertyName("wind")] UnitValueDto Wind,
        [property: JsonPropertyName("gust")] UnitValueDto Gust,
        [property: JsonPropertyName("cloud")] UnitValueDto Cloud,
        [property: JsonPropertyName("rainMm1h")] UnitValueDto RainMm1h,
        [property: JsonPropertyName("snowMm1h")] UnitValueDto SnowMm1h,
        [property: JsonPropertyName("pop")] UnitValueDto Pop,
        [property: JsonPropertyName("code")] int? WeatherCode,
        [property: JsonPropertyName("main")] string? WeatherMain
    );

    public sealed record WeatherForecastHourDto(
        [property: JsonPropertyName("forecastForUtc")] DateTime ForecastForUtc,
        [property: JsonPropertyName("tempC")] decimal TempC,
        [property: JsonPropertyName("relativeHumidity")] short RelativeHumidity,
        [property: JsonPropertyName("pressureHPa")] short PressureHPa,
        [property: JsonPropertyName("windSpeedMs")] decimal WindSpeedMs,
        [property: JsonPropertyName("windGustMs")] decimal? WindGustMs,
        [property: JsonPropertyName("cloudPct")] short? CloudPct,
        [property: JsonPropertyName("rainMm1h")] decimal? RainMm1h,
        [property: JsonPropertyName("snowMm1h")] decimal? SnowMm1h,
        [property: JsonPropertyName("pop")] decimal? Pop,
        [property: JsonPropertyName("weatherMain")] string? WeatherMain
    );

    public sealed record WeatherForecastMiniDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("hours")] IReadOnlyList<WeatherForecastMiniHourDto> Hours
    );

    public sealed record WeatherForecastMiniHourDto(
        [property: JsonPropertyName("forecastForUtc")] DateTime ForecastForUtc,
        [property: JsonPropertyName("temp")] UnitValueDto Temp,
        [property: JsonPropertyName("dewPoint")] UnitValueDto DewPoint,
        [property: JsonPropertyName("rh")] UnitValueDto Rh,
        [property: JsonPropertyName("wind")] UnitValueDto Wind,
        [property: JsonPropertyName("gust")] UnitValueDto Gust,
        [property: JsonPropertyName("rainMm1h")] UnitValueDto RainMm1h,
        [property: JsonPropertyName("pop")] UnitValueDto Pop,
        [property: JsonPropertyName("code")] int? WeatherCode,
        [property: JsonPropertyName("main")] string? WeatherMain
    );
}
