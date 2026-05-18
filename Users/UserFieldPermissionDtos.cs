namespace AmsRecords.Users;

public static class UserFieldPermissionDtos
{
    public sealed record UserFieldPermissionMatrixDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ibuName")] string? IbuName,
        [property: JsonPropertyName("fields")] IReadOnlyList<FieldPermissionItemDto> Fields,
        [property: JsonPropertyName("users")] IReadOnlyList<UserPermissionRowDto> Users);

    public sealed record FieldPermissionItemDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record UserPermissionRowDto(
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("userName")] string? UserName,
        [property: JsonPropertyName("fullName")] string? FullName,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds,
        [property: JsonPropertyName("automaticFieldPubIds")] IReadOnlyList<Guid> AutomaticFieldPubIds);

    public sealed record SaveUserFieldPermissionMatrixDto(
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId,
        [property: JsonPropertyName("users")][Required] IReadOnlyList<SaveUserPermissionRowDto> Users);

    public sealed record SaveUserPermissionRowDto(
        [property: JsonPropertyName("userPubId")][Required] Guid UserPubId,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds);
}
