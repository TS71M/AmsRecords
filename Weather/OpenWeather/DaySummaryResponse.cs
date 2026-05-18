namespace AmsRecords.Weather.OpenWeather;

public sealed record DaySummaryResponse(
    [property: JsonPropertyName("lat")] double? Lat,
    [property: JsonPropertyName("lon")] double? Lon,
    [property: JsonPropertyName("tz")] string? Tz,
    [property: JsonPropertyName("date")] string? Date,
    [property: JsonPropertyName("units")] string? Units,
    [property: JsonPropertyName("cloud_cover")] DaySummaryCloudCover? CloudCover,
    [property: JsonPropertyName("humidity")] DaySummaryHumidity? Humidity,
    [property: JsonPropertyName("precipitation")] DaySummaryPrecipitation? Precipitation,
    [property: JsonPropertyName("temperature")] DaySummaryTemperature? Temperature,
    [property: JsonPropertyName("wind")] DaySummaryWind? Wind
);

public sealed record DaySummaryTemperature(
    [property: JsonPropertyName("min")] double? Min,
    [property: JsonPropertyName("max")] double? Max,
    [property: JsonPropertyName("afternoon")] double? Afternoon,
    [property: JsonPropertyName("night")] double? Night,
    [property: JsonPropertyName("evening")] double? Evening,
    [property: JsonPropertyName("morning")] double? Morning
);

public sealed record DaySummaryPrecipitation(
    [property: JsonPropertyName("total")] double? Total
);

public sealed record DaySummaryHumidity(
    [property: JsonPropertyName("morning")] double? Morning,
    [property: JsonPropertyName("afternoon")] double? Afternoon,
    [property: JsonPropertyName("evening")] double? Evening,
    [property: JsonPropertyName("night")] double? Night
);

public sealed record DaySummaryCloudCover(
    [property: JsonPropertyName("afternoon")] double? Afternoon
);

public sealed record DaySummaryWind(
    [property: JsonPropertyName("max")] DaySummaryWindMax? Max
);

public sealed record DaySummaryWindMax(
    [property: JsonPropertyName("speed")] double? Speed,
    [property: JsonPropertyName("direction")] double? Direction
);
