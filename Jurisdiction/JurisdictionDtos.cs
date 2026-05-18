namespace AmsRecords.Jurisdictions;

public static class JurisdictionDtos
{
    public sealed record JurisdictionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("countryPubId")] Guid CountryPubId,
        [property: JsonPropertyName("countryCode")] string CountryCode,
        [property: JsonPropertyName("countryName")] string CountryName,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record JurisdictionCreateDto(
        [property: JsonPropertyName("countryPubId")] Guid CountryPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active = true
    );

    public sealed record JurisdictionUpdateDto(
        [property: JsonPropertyName("countryPubId")] Guid CountryPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record JurisdictionMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name
    );

    public sealed record JurisdictionSetActiveDto(
       [property: JsonPropertyName("setActive")] bool Active
   );
}
