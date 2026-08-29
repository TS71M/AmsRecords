namespace AmsRecords.Irrigation;

public static class IrrigationDemandDtos
{
    public const string ClimaticBalanceBasis = "climatic_balance";
    public const string SoilMoistureBasis = "soil_moisture";
    public const string EffectiveRainfallSource = "effective_rainfall";
    public const string MeasuredRainfallSource = "measured_rainfall";
    public const string RequestSource = "request";
    public const string WeatherDaySummarySource = "weather_day_summary";

    public sealed record IrrigationDemandRequestDto(
        [property: JsonPropertyName("fromDateUtc")] DateOnly FromDateUtc,
        [property: JsonPropertyName("toDateUtc")] DateOnly ToDateUtc,
        [property: JsonPropertyName("et0Mm")] decimal? Et0Mm,
        [property: JsonPropertyName("cropCoefficient")] decimal? CropCoefficient,
        [property: JsonPropertyName("rainfallMm")] decimal? RainfallMm = null,
        [property: JsonPropertyName("effectiveRainfallMm")] decimal? EffectiveRainfallMm = null,
        [property: JsonPropertyName("rootZoneDepthMm")] decimal? RootZoneDepthMm = null,
        [property: JsonPropertyName("availableWaterCapacityMmPerMetre")] decimal? AvailableWaterCapacityMmPerMetre = null,
        [property: JsonPropertyName("allowableDepletionFraction")] decimal? AllowableDepletionFraction = null,
        [property: JsonPropertyName("currentVwcPercent")] decimal? CurrentVwcPercent = null,
        [property: JsonPropertyName("targetVwcPercent")] decimal? TargetVwcPercent = null,
        [property: JsonPropertyName("useLatestSoilMoistureObservation")] bool UseLatestSoilMoistureObservation = false,
        [property: JsonPropertyName("soilMoistureSurfacePubId")] Guid? SoilMoistureSurfacePubId = null,
        [property: JsonPropertyName("soilMoistureMaximumAgeHours")] int SoilMoistureMaximumAgeHours = 72,
        [property: JsonPropertyName("applicationEfficiencyFraction")] decimal? ApplicationEfficiencyFraction = null);

    public sealed record SoilMoistureObservationDto(
        [property: JsonPropertyName("vwcPercent")] decimal VwcPercent,
        [property: JsonPropertyName("observedAtUtc")] DateTime ObservedAtUtc,
        [property: JsonPropertyName("sourceCode")] string SourceCode,
        [property: JsonPropertyName("sourcePubId")] Guid? SourcePubId,
        [property: JsonPropertyName("scopePubId")] Guid? ScopePubId);

    public sealed record IrrigationDemandComponentDto(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("formula")] string Formula,
        [property: JsonPropertyName("valueMm")] decimal ValueMm);

    public sealed record IrrigationAreaDemandResultDto(
        [property: JsonPropertyName("irrigationAreaPubId")] Guid IrrigationAreaPubId,
        [property: JsonPropertyName("irrigationAreaName")] string IrrigationAreaName,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("fromDateUtc")] DateOnly FromDateUtc,
        [property: JsonPropertyName("toDateUtc")] DateOnly ToDateUtc,
        [property: JsonPropertyName("recommendedNetMm")] decimal RecommendedNetMm,
        [property: JsonPropertyName("recommendedGrossMm")] decimal? RecommendedGrossMm,
        [property: JsonPropertyName("basisCode")] string BasisCode,
        [property: JsonPropertyName("et0Mm")] decimal? Et0Mm,
        [property: JsonPropertyName("cropCoefficient")] decimal? CropCoefficient,
        [property: JsonPropertyName("cropEvapotranspirationMm")] decimal? CropEvapotranspirationMm,
        [property: JsonPropertyName("rainfallMm")] decimal? RainfallMm,
        [property: JsonPropertyName("effectiveRainfallMm")] decimal? EffectiveRainfallMm,
        [property: JsonPropertyName("creditedRainfallMm")] decimal? CreditedRainfallMm,
        [property: JsonPropertyName("rainfallBasisCode")] string? RainfallBasisCode,
        [property: JsonPropertyName("rainfallSourceCode")] string? RainfallSourceCode,
        [property: JsonPropertyName("weatherDayCount")] int WeatherDayCount,
        [property: JsonPropertyName("climaticWaterDeficitMm")] decimal? ClimaticWaterDeficitMm,
        [property: JsonPropertyName("rootZoneDepthMm")] decimal? RootZoneDepthMm,
        [property: JsonPropertyName("totalAvailableWaterMm")] decimal? TotalAvailableWaterMm,
        [property: JsonPropertyName("allowableDepletionMm")] decimal? AllowableDepletionMm,
        [property: JsonPropertyName("depletionTriggerReached")] bool? DepletionTriggerReached,
        [property: JsonPropertyName("currentVwcPercent")] decimal? CurrentVwcPercent,
        [property: JsonPropertyName("targetVwcPercent")] decimal? TargetVwcPercent,
        [property: JsonPropertyName("soilMoistureDeficitMm")] decimal? SoilMoistureDeficitMm,
        [property: JsonPropertyName("soilMoistureObservation")] SoilMoistureObservationDto? SoilMoistureObservation,
        [property: JsonPropertyName("applicationEfficiencyFraction")] decimal? ApplicationEfficiencyFraction,
        [property: JsonPropertyName("components")] IReadOnlyList<IrrigationDemandComponentDto> Components,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings,
        [property: JsonPropertyName("isAdvisory")] bool IsAdvisory = true);
}
