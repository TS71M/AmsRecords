namespace AmsRecords.Weather.OpenWeather;

// /data/2.5/forecast (5 day / 3 hour)
public record Forecast5Response(
    [property: JsonPropertyName("list")] Forecast5Item[] List
);

public record Forecast5Item(
    [property: JsonPropertyName("dt")] long Dt,
    [property: JsonPropertyName("main")] Forecast5Main Main,
    [property: JsonPropertyName("wind")] Forecast5Wind Wind,
    [property: JsonPropertyName("clouds")] Forecast5Clouds Clouds,
    [property: JsonPropertyName("pop")] decimal? Pop,
    [property: JsonPropertyName("weather")] Forecast5Weather[] Weather,
    [property: JsonPropertyName("rain")] Forecast5Rain? Rain,
    [property: JsonPropertyName("snow")] Forecast5Snow? Snow
);

public record Forecast5Main(
    [property: JsonPropertyName("temp")] decimal Temp,
    [property: JsonPropertyName("pressure")] short Pressure,
    [property: JsonPropertyName("humidity")] short Humidity
);

public record Forecast5Wind(
    [property: JsonPropertyName("speed")] decimal Speed,
    [property: JsonPropertyName("deg")] short? Deg,
    [property: JsonPropertyName("gust")] decimal? Gust
);

public record Forecast5Clouds(
    [property: JsonPropertyName("all")] short? All
);

public record Forecast5Weather(
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonPropertyName("main")] string? Main
);

public record Forecast5Rain(
    // Volume for last 3 hours (mm)
    [property: JsonPropertyName("3h")] decimal? ThreeH
);

public record Forecast5Snow(
    // Volume for last 3 hours (mm)
    [property: JsonPropertyName("3h")] decimal? ThreeH
);
