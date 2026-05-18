namespace AmsRecords.Ibus;

public static class IbuModuleSettingDtos
{
    public sealed record IbuModulePreferenceDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isEnabled")] bool IsEnabled);

    public sealed record IbuModulePreferencesDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules,
        [property: JsonPropertyName("canEdit")] bool CanEdit);

    public sealed record SaveIbuModulePreferencesDto(
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules);
}
