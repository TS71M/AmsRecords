namespace AmsRecords.Name;

public static class NameDtos
{
    public record NameDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("middleName")] string? MiddleName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId
    )
    {
        public NameDto() : this(Guid.Empty, string.Empty, null, string.Empty, Guid.Empty) { }

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
    }
    public record NameCreateDto(
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("middleName")] string? MiddleName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ownerType")][Required] ParentEntityType OwnerType,
        [property: JsonPropertyName("ownerPubId")][Required] Guid OwnerPubId
    );

    public record NameUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("middleName")] string? MiddleName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ownerType")] ParentEntityType? OwnerType = null,
        [property: JsonPropertyName("ownerPubId")] Guid? OwnerPubId = null
    )
    {
        public NameUpdateDto() : this(Guid.Empty, string.Empty, null, string.Empty, Guid.Empty) { }

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
    }
}
