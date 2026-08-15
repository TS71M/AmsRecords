namespace AmsRecords.Ibus;

public static class ModuleReleaseSettingDtos
{
    public sealed record ModuleReleaseSettingDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isReleased")] bool IsReleased);

    public sealed record ModuleReleaseSettingsDto(
        [property: JsonPropertyName("modules")] List<ModuleReleaseSettingDto> Modules,
        [property: JsonPropertyName("activePilotGrants")] List<ModulePilotSummaryDto>? ActivePilotGrants = null);

    public sealed record ModulePilotSummaryDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("expiresAtUtc")] DateTime? ExpiresAtUtc);

    public sealed record SaveModuleReleaseSettingsDto(
        [property: JsonPropertyName("modules")] List<ModuleReleaseSettingDto> Modules);

    public sealed record SaveModuleReleaseStateDto(
        [property: JsonPropertyName("isReleased")] bool IsReleased);

    public sealed record ModuleAvailabilityDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isGloballyReleased")] bool IsGloballyReleased,
        [property: JsonPropertyName("isPilotCapable")] bool IsPilotCapable,
        [property: JsonPropertyName("isPilotGranted")] bool IsPilotGranted,
        [property: JsonPropertyName("pilotExpiresAtUtc")] DateTime? PilotExpiresAtUtc,
        [property: JsonPropertyName("isAvailable")] bool IsAvailable);

    public sealed record ModuleAvailabilitySettingsDto(
        [property: JsonPropertyName("modules")] List<ModuleAvailabilityDto> Modules);

    public sealed record ModulePilotGrantItemDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("isGranted")] bool IsGranted,
        [property: JsonPropertyName("expiresAtUtc")] DateTime? ExpiresAtUtc);

    public sealed record ModulePilotAccessDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isPilotCapable")] bool IsPilotCapable,
        [property: JsonPropertyName("isGloballyReleased")] bool IsGloballyReleased,
        [property: JsonPropertyName("ibus")] List<ModulePilotGrantItemDto> Ibus);

    public sealed record ModulePilotGrantSelectionDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("expiresAtUtc")] DateTime? ExpiresAtUtc);

    public sealed record SaveModulePilotAccessDto(
        [property: JsonPropertyName("ibus")] List<ModulePilotGrantSelectionDto> Ibus);
}
