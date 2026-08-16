namespace AmsRecords.Ibus;

public static class IbuContactDtos
{
    public sealed record IbuContactDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("role")] string? Role,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("mobile")] string? Mobile,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("website")] string? Website = null,
        [property: JsonPropertyName("isSystemContact")] bool IsSystemContact = false,
        [property: JsonPropertyName("isAssignedUser")] bool IsAssignedUser = false);

    public sealed record IbuContactCreateDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("role")] string? Role,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("mobile")] string? Mobile,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public sealed record IbuContactUpdateDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("role")] string? Role,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("mobile")] string? Mobile,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IbuContactPrimaryNameUpdateDto(
        [property: JsonPropertyName("name")]
        [param: Required]
        [param: MaxLength(150)]
        string Name);
}
