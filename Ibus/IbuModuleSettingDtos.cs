namespace AmsRecords.Ibus;

public static class IbuModuleSettingDtos
{
    public sealed record IbuModulePreferenceDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isEnabled")] bool IsEnabled,
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields = false);

    public sealed record IbuModulePreferencesDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules,
        [property: JsonPropertyName("canEdit")] bool CanEdit);

    public sealed record SaveIbuModulePreferencesDto(
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules);

    public sealed record SaveIbuModulePreferenceDto(
        [property: JsonPropertyName("isEnabled")] bool IsEnabled);

    public sealed record IbuModuleFieldItemDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("isSelected")] bool IsSelected);

    public sealed record IbuModuleFieldPreferencesDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isEnabled")] bool IsEnabled,
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields,
        [property: JsonPropertyName("fields")] IReadOnlyList<IbuModuleFieldItemDto> Fields,
        [property: JsonPropertyName("canEdit")] bool CanEdit);

    public sealed record SaveIbuModuleFieldPreferencesDto(
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds);
}
