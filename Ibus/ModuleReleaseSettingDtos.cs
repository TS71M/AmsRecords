namespace AmsRecords.Ibus;

public static class ModuleReleaseSettingDtos
{
    public sealed record ModuleReleaseSettingDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isReleased")] bool IsReleased);

    public sealed record ModuleReleaseSettingsDto(
        [property: JsonPropertyName("modules")] List<ModuleReleaseSettingDto> Modules);

    public sealed record SaveModuleReleaseSettingsDto(
        [property: JsonPropertyName("modules")] List<ModuleReleaseSettingDto> Modules);
}
