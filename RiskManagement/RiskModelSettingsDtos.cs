namespace AmsRecords.RiskManagement;

public static class RiskModelSettingsDtos
{
    public record RiskModelSettingsDto(
        [property: JsonPropertyName("heatStressModerateTempC")] decimal HeatStressModerateTempC,
        [property: JsonPropertyName("heatStressHighTempC")] decimal HeatStressHighTempC,
        [property: JsonPropertyName("heatStressModerateHours")] int HeatStressModerateHours,
        [property: JsonPropertyName("heatStressHighHours")] int HeatStressHighHours,
        [property: JsonPropertyName("frostModerateTempC")] decimal FrostModerateTempC,
        [property: JsonPropertyName("frostHighTempC")] decimal FrostHighTempC,
        [property: JsonPropertyName("updatedUtc")] DateTime UpdatedUtc);

    public record RiskModelSettingsUpdateDto(
        [property: JsonPropertyName("heatStressModerateTempC")] decimal HeatStressModerateTempC,
        [property: JsonPropertyName("heatStressHighTempC")] decimal HeatStressHighTempC,
        [property: JsonPropertyName("heatStressModerateHours")] int HeatStressModerateHours,
        [property: JsonPropertyName("heatStressHighHours")] int HeatStressHighHours,
        [property: JsonPropertyName("frostModerateTempC")] decimal FrostModerateTempC,
        [property: JsonPropertyName("frostHighTempC")] decimal FrostHighTempC);

    public record RiskHeatStressRuleDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("climateZoneLabel")] string? ClimateZoneLabel,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("grassSpeciesName")] string? GrassSpeciesName,
        [property: JsonPropertyName("grassTypePubId")] Guid? GrassTypePubId,
        [property: JsonPropertyName("grassTypeName")] string? GrassTypeName,
        [property: JsonPropertyName("moderateTempC")] decimal ModerateTempC,
        [property: JsonPropertyName("highTempC")] decimal HighTempC,
        [property: JsonPropertyName("moderateHours")] int ModerateHours,
        [property: JsonPropertyName("highHours")] int HighHours,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("updatedUtc")] DateTime UpdatedUtc);

    public record RiskHeatStressRuleCreateDto(
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("grassTypePubId")] Guid? GrassTypePubId,
        [property: JsonPropertyName("moderateTempC")] decimal ModerateTempC,
        [property: JsonPropertyName("highTempC")] decimal HighTempC,
        [property: JsonPropertyName("moderateHours")] int ModerateHours,
        [property: JsonPropertyName("highHours")] int HighHours,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public record RiskHeatStressRuleUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("climateZonePubId")] Guid? ClimateZonePubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("grassTypePubId")] Guid? GrassTypePubId,
        [property: JsonPropertyName("moderateTempC")] decimal ModerateTempC,
        [property: JsonPropertyName("highTempC")] decimal HighTempC,
        [property: JsonPropertyName("moderateHours")] int ModerateHours,
        [property: JsonPropertyName("highHours")] int HighHours,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active);
}
