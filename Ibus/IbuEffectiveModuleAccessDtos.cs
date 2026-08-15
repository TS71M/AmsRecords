namespace AmsRecords.Ibus;

public static class IbuEffectiveModuleAccessDtos
{
    public sealed record IbuEffectiveModuleAccessDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isReleased")] bool IsReleased,
        [property: JsonPropertyName("isOrganizationEnabled")] bool IsOrganizationEnabled,
        [property: JsonPropertyName("canView")] bool CanView,
        [property: JsonPropertyName("canEdit")] bool CanEdit,
        [property: JsonPropertyName("canDelete")] bool CanDelete,
        [property: JsonPropertyName("isFieldEnabled")] bool IsFieldEnabled = true,
        [property: JsonPropertyName("allowedActions")] IReadOnlyList<string>? AllowedActions = null);

    public sealed record IbuEffectiveModuleAccessResponseDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("isAdministrator")] bool IsAdministrator,
        [property: JsonPropertyName("hasAccessProfile")] bool HasAccessProfile,
        [property: JsonPropertyName("accessProfilePubId")] Guid? AccessProfilePubId,
        [property: JsonPropertyName("modules")] IReadOnlyList<IbuEffectiveModuleAccessDto> Modules);

    public sealed record IbuEffectiveModuleAccessBatchRequestDto(
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds);

    public sealed record IbuEffectiveFieldModuleAccessDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("access")] IbuEffectiveModuleAccessResponseDto Access);

    public sealed record IbuEffectiveModuleAccessBatchResponseDto(
        [property: JsonPropertyName("fields")] IReadOnlyList<IbuEffectiveFieldModuleAccessDto> Fields);
}
