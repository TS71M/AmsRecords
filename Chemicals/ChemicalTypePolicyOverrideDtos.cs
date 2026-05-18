namespace AmsRecords.Chemicals;

public static class ChemicalTypePolicyOverrideDtos
{
    public sealed record ChemicalTypePolicyOverrideDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ownerType")] ChemPolicyOwnerType OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("reason")] string? Reason,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ChemicalTypePolicyOverrideCreateDto(
        [property: JsonPropertyName("ownerType")] ChemPolicyOwnerType OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("reason")] string? Reason
    );

    public sealed record ChemicalTypePolicyOverrideUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("allowed")] bool Allowed,
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("reason")] string? Reason,
        [property: JsonPropertyName("active")] bool Active
    );
}
