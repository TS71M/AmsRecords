namespace AmsRecords.Ibus;

public static class IbuTravelDirectoryDtos
{
    public sealed record TravelDirectoryOrderUpdateDto(
        [property: JsonPropertyName("orderedPubIds")]
        [param: Required]
        IReadOnlyList<Guid> OrderedPubIds);

    public sealed record TravelDirectoryDto(
        [property: JsonPropertyName("courses")] IReadOnlyList<TravelCourseDto> Courses);

    public sealed record TravelCourseDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("destinations")] IReadOnlyList<TravelDestinationDto> Destinations,
        [property: JsonPropertyName("contacts")] IReadOnlyList<TravelContactDto> Contacts);

    public sealed record TravelDestinationDto(
        [property: JsonPropertyName("pubId")] Guid? PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("address")] string? Address,
        [property: JsonPropertyName("latitude")] double? Latitude,
        [property: JsonPropertyName("longitude")] double? Longitude,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("source")] string Source);

    public sealed record TravelContactDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("department")] string? Department,
        [property: JsonPropertyName("role")] string? Role,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("website")] string? Website = null,
        [property: JsonPropertyName("mobile")] string? Mobile = null);

    public sealed record ContactSyncDto(
        [property: JsonPropertyName("targetPubId")][param: Required] Guid TargetPubId,
        [property: JsonPropertyName("source")][param: Required, MaxLength(20)] string Source,
        [property: JsonPropertyName("phone")][param: MaxLength(60)] string? Phone,
        [property: JsonPropertyName("email")][param: MaxLength(250), EmailAddress] string? Email);
}
