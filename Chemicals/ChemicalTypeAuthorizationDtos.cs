namespace AmsRecords.Chemicals;

public static class ChemicalTypeAuthorizationDtos
{
    public sealed record ChemicalTypeAuthorizationDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("jurisdictionPubId")] Guid JurisdictionPubId,
        [property: JsonPropertyName("jurisdictionName")] string JurisdictionName,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ChemicalTypeAuthorizationCreateDto(
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("jurisdictionPubId")] Guid JurisdictionPubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc
    );

    public sealed record ChemicalTypeAuthorizationUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record EffectiveChemicalTypePolicyDto(
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("source")] string Source // "FieldOverride" | "IbuOverride" | "Jurisdiction" | "Default"
    );
}
