namespace AmsRecords.Ibus;

public static class IbuEffectiveModuleAccessDtos
{
    public sealed record IbuEffectiveModuleAccessDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isReleased")] bool IsReleased,
        [property: JsonPropertyName("isOrganizationEnabled")] bool IsOrganizationEnabled,
        [property: JsonPropertyName("canView")] bool CanView,
        [property: JsonPropertyName("canEdit")] bool CanEdit);

    public sealed record IbuEffectiveModuleAccessResponseDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("isAdministrator")] bool IsAdministrator,
        [property: JsonPropertyName("hasAccessProfile")] bool HasAccessProfile,
        [property: JsonPropertyName("accessProfilePubId")] Guid? AccessProfilePubId,
        [property: JsonPropertyName("modules")] IReadOnlyList<IbuEffectiveModuleAccessDto> Modules);
}
