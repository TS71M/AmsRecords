namespace AmsRecords.ContactDetails;

public static class ContactDetailDtos
{
    public record ContactDetailDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("website")] string? Website,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId
        );

    public record ContactDetailCreateDto(
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("website")] string? Website,
        [property: JsonPropertyName("ownerType")][Required] ParentEntityType OwnerType,
        [property: JsonPropertyName("ownerPubId")][Required] Guid OwnerPubId
        )
    {
        public ContactDetailCreateDto() : this(null, null, null, ParentEntityType.Ibu, Guid.Empty) { }
    };

    public record ContactDetailUpdateDto(
            [property: JsonPropertyName("publicId")] Guid PubId,
            [property: JsonPropertyName("phone")] string? Phone,
            [property: JsonPropertyName("email")] string? Email,
            [property: JsonPropertyName("website")] string? Website,
            [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
            [property: JsonPropertyName("ownerType")] ParentEntityType? OwnerType = null,
            [property: JsonPropertyName("ownerPubId")] Guid? OwnerPubId = null
            )
    {
        public ContactDetailUpdateDto() : this(Guid.Empty, null, null, null, Guid.Empty) { }
    };

}
