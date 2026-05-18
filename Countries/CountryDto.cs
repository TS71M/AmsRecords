namespace AmsRecords.Countries;

public record CountryDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("countryCode")] string CountryCode,
    [property: JsonPropertyName("countryName")] string CountryName
    );

public record CountryMiniDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("countryName")] string CountryName
    );

public record CountryCreateDto(
    [property: JsonPropertyName("countryCode")][MaxLength(2)] string CountryCode,
    [property: JsonPropertyName("countryName")][MaxLength(100)] string CountryName
    );

public record CountryUpdateDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("countryCode")][MaxLength(2)] string CountryCode,
    [property: JsonPropertyName("countryName")][MaxLength(100)] string CountryName
    );