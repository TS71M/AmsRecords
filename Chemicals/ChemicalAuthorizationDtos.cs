namespace AmsRecords.Chemicals;

public static class ChemicalAuthorizationDtos
{
    public sealed record ChemicalAuthorizationDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("jurisdictionPubId")] Guid JurisdictionPubId,
        [property: JsonPropertyName("jurisdictionName")] string JurisdictionName,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTime? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTime? ValidToUtc,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ChemicalAuthorizationCreateDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("jurisdictionPubId")] Guid JurisdictionPubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTime? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTime? ValidToUtc
    );

    public sealed record ChemicalAuthorizationUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTime? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTime? ValidToUtc,
        [property: JsonPropertyName("active")] bool Active
    );
}
