namespace AmsRecords.Weather.OpenWeather;

public record CurrentWeatherResponse(
    [property: JsonPropertyName("dt")] long Dt,
    [property: JsonPropertyName("coord")] Coord Coord,
    [property: JsonPropertyName("weather")] Weather[] Weather,
    [property: JsonPropertyName("main")] Main Main,
    [property: JsonPropertyName("wind")] Wind Wind,
    [property: JsonPropertyName("clouds")] Clouds? Clouds,
    [property: JsonPropertyName("rain")] Rain? Rain,
    [property: JsonPropertyName("snow")] Snow? Snow
);

public record Coord(
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon
);

public record Weather(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("main")] string Main,
    [property: JsonPropertyName("description")] string Description
);

public record Main(
    [property: JsonPropertyName("temp")] double Temp,
    [property: JsonPropertyName("humidity")] int Humidity,
    [property: JsonPropertyName("pressure")] int Pressure
);

public record Wind(
    [property: JsonPropertyName("speed")] double Speed,
    [property: JsonPropertyName("deg")] int? Deg,
    [property: JsonPropertyName("gust")] double? Gust
);

public record Clouds([property: JsonPropertyName("all")] int All);

public record Rain([property: JsonPropertyName("1h")] double? OneH);
public record Snow([property: JsonPropertyName("1h")] double? OneH);