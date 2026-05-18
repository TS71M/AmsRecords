namespace AmsRecords.Weather.OpenWeather;

public record OneCallResponse(
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("timezone")] string Timezone,
    [property: JsonPropertyName("timezone_offset")] int TimezoneOffset,
    [property: JsonPropertyName("current")] OneCallCurrent? Current,
    [property: JsonPropertyName("hourly")] OneCallHourly[]? Hourly
);

public record OneCallCurrent(
    [property: JsonPropertyName("dt")] long Dt,
    [property: JsonPropertyName("temp")] double Temp,
    [property: JsonPropertyName("humidity")] int Humidity,
    [property: JsonPropertyName("pressure")] int Pressure,
    [property: JsonPropertyName("dew_point")] double? DewPoint,
    [property: JsonPropertyName("wind_speed")] double WindSpeed,
    [property: JsonPropertyName("wind_deg")] int? WindDeg,
    [property: JsonPropertyName("wind_gust")] double? WindGust,
    [property: JsonPropertyName("clouds")] int? Clouds,
    [property: JsonPropertyName("weather")] Weather[]? Weather
);

public record OneCallHourly(
    [property: JsonPropertyName("dt")] long Dt,
    [property: JsonPropertyName("temp")] double Temp,
    [property: JsonPropertyName("humidity")] int Humidity,
    [property: JsonPropertyName("pressure")] int Pressure,
    [property: JsonPropertyName("dew_point")] double? DewPoint,
    [property: JsonPropertyName("wind_speed")] double WindSpeed,
    [property: JsonPropertyName("wind_deg")] int? WindDeg,
    [property: JsonPropertyName("wind_gust")] double? WindGust,
    [property: JsonPropertyName("clouds")] int? Clouds,
    [property: JsonPropertyName("pop")] double? Pop,
    [property: JsonPropertyName("rain")] OneCallRain? Rain,
    [property: JsonPropertyName("snow")] OneCallSnow? Snow,
    [property: JsonPropertyName("weather")] Weather[]? Weather
);

public record OneCallRain([property: JsonPropertyName("1h")] double? OneH);
public record OneCallSnow([property: JsonPropertyName("1h")] double? OneH);