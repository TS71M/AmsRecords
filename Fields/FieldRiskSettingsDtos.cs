using static Lib.Enums.RiskLevels;

namespace AmsRecords.Fields;

public sealed record FieldRiskSettingsDto(
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("fieldName")] string FieldName,
    [property: JsonPropertyName("active")] bool Active,
    [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
    [property: JsonPropertyName("dollarSpot")] RiskLevel DollarSpot,
    [property: JsonPropertyName("pythium")] RiskLevel Pythium,
    [property: JsonPropertyName("microdochium")] RiskLevel Microdochium,
    [property: JsonPropertyName("dew")] RiskLevel Dew,
    [property: JsonPropertyName("frost")] RiskLevel Frost,
    [property: JsonPropertyName("heatStress")] RiskLevel HeatStress,
    [property: JsonPropertyName("anthracnose")] RiskLevel Anthracnose
);

public sealed record FieldRiskSettingsUpdateDto(
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("dollarSpot")] RiskLevel DollarSpot,
    [property: JsonPropertyName("pythium")] RiskLevel Pythium,
    [property: JsonPropertyName("microdochium")] RiskLevel Microdochium,
    [property: JsonPropertyName("dew")] RiskLevel Dew,
    [property: JsonPropertyName("frost")] RiskLevel Frost,
    [property: JsonPropertyName("heatStress")] RiskLevel HeatStress,
    [property: JsonPropertyName("anthracnose")] RiskLevel Anthracnose
);

public sealed record FieldRiskSettingsBatchUpdateDto(
    [property: JsonPropertyName("items")] IReadOnlyList<FieldRiskSettingsUpdateDto> Items
);
