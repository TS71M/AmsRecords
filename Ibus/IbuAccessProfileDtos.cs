namespace AmsRecords.Ibus;

public static class IbuAccessProfileDtos
{
    public sealed record IbuAccessProfileDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("profileName")] string ProfileName,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("permissions")] IReadOnlyList<IbuAccessProfilePermissionDto> Permissions);

    public sealed record IbuAccessProfilePermissionDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("permissionKey")] string PermissionKey,
        [property: JsonPropertyName("isAllowed")] bool IsAllowed);

    public sealed record IbuAccessProfileCreateDto(
        [property: JsonPropertyName("profileName")][Required] string ProfileName,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("permissions")] IReadOnlyList<IbuAccessProfilePermissionDto> Permissions);

    public sealed record IbuAccessProfileUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("profileName")][Required] string ProfileName,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("permissions")] IReadOnlyList<IbuAccessProfilePermissionDto> Permissions);
}
