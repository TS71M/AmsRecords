namespace AmsRecords.Weather;

public record OpenWeatherOptions(
    [property: JsonPropertyName("apiKey")] string ApiKey,
    [property: JsonPropertyName("units")] string Units,
    [property: JsonPropertyName("language")] string Language
    );