using Lib.Enums;

namespace AmsRecords.Irrigation;

public static class IrrigationRules
{
    public const int MaximumNozzlesPerSprinkler = 5;
    public const int MaximumIdentifierLength = 80;
    public const decimal AiReviewConfidenceThreshold = 0.70m;

    public static bool RequiresAdministratorReview(decimal confidence)
        => confidence < AiReviewConfidenceThreshold;

    public static string? ValidateSlots<T>(
        IReadOnlyList<T>? slots,
        Func<T, int> position,
        Func<T, string?> label)
    {
        if (slots is null || slots.Count == 0)
            return "Add at least one nozzle position.";
        if (slots.Count > MaximumNozzlesPerSprinkler)
            return $"A sprinkler may have at most {MaximumNozzlesPerSprinkler} nozzle positions.";
        if (slots.Any(x => position(x) is < 1 or > MaximumNozzlesPerSprinkler))
            return $"Nozzle positions must be between 1 and {MaximumNozzlesPerSprinkler}.";
        if (slots.GroupBy(position).Any(x => x.Count() > 1))
            return "Each nozzle position may be used only once.";
        if (slots.Any(x => string.IsNullOrWhiteSpace(label(x))))
            return "Every nozzle position needs a label.";
        return null;
    }
}

public static class IrrigationDtos
{
    public sealed record IrrigationSprinklerModelDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("modelCode")] string ModelCode,
        [property: JsonPropertyName("maximumNozzleCount")] int MaximumNozzleCount,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("isAiDiscovered")] bool IsAiDiscovered,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationSprinklerModelSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("manufacturerName")][param: Required, MaxLength(120)] string ManufacturerName,
        [property: JsonPropertyName("modelName")][param: Required, MaxLength(160)] string ModelName,
        [property: JsonPropertyName("modelCode")][param: MaxLength(80)] string ModelCode,
        [property: JsonPropertyName("maximumNozzleCount")][param: Range(1, 5)] int MaximumNozzleCount,
        [property: JsonPropertyName("sourceUrl")][param: MaxLength(500)] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")][param: MaxLength(2000)] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("active")] bool Active = true);

    public sealed record IrrigationSprinklerNozzleOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid SprinklerModelPubId,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("sprinklerModelName")] string SprinklerModelName,
        [property: JsonPropertyName("positionKind")] IrrigationNozzlePositionKind PositionKind,
        [property: JsonPropertyName("nozzleCode")] string NozzleCode,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("color")] string Color,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationSprinklerNozzleOptionSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid SprinklerModelPubId,
        [property: JsonPropertyName("positionKind")] IrrigationNozzlePositionKind PositionKind,
        [property: JsonPropertyName("nozzleCode")][param: Required, MaxLength(80)] string NozzleCode,
        [property: JsonPropertyName("nozzleName")][param: Required, MaxLength(160)] string NozzleName,
        [property: JsonPropertyName("color")][param: MaxLength(80)] string Color,
        [property: JsonPropertyName("sourceUrl")][param: MaxLength(500)] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")][param: MaxLength(2000)] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("active")] bool Active = true);

    public sealed record IrrigationNozzleConfigurationSlotDto(
        [property: JsonPropertyName("position")] int Position,
        [property: JsonPropertyName("positionKind")] IrrigationNozzlePositionKind PositionKind,
        [property: JsonPropertyName("positionLabel")] string PositionLabel,
        [property: JsonPropertyName("nozzleCode")] string NozzleCode,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("color")] string Color,
        [property: JsonPropertyName("isOptional")] bool IsOptional,
        [property: JsonPropertyName("nozzleOptionPubId")] Guid? NozzleOptionPubId = null);

    public sealed record IrrigationNozzleConfigurationDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModel")] IrrigationSprinklerModelDto? SprinklerModel,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("isUnknownSprinkler")] bool IsUnknownSprinkler,
        [property: JsonPropertyName("recognitionHints")] string RecognitionHints,
        [property: JsonPropertyName("scope")] IrrigationConfigurationScope Scope,
        [property: JsonPropertyName("scopeLabel")] string ScopeLabel,
        [property: JsonPropertyName("isApprovedReference")] bool IsApprovedReference,
        [property: JsonPropertyName("isAiDiscovered")] bool IsAiDiscovered,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("slots")] IReadOnlyList<IrrigationNozzleConfigurationSlotDto> Slots);

    public sealed record IrrigationNozzleConfigurationSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("name")][param: Required, MaxLength(160)] string Name,
        [property: JsonPropertyName("isUnknownSprinkler")] bool IsUnknownSprinkler,
        [property: JsonPropertyName("recognitionHints")][param: MaxLength(2000)] string RecognitionHints,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("slots")] IReadOnlyList<IrrigationNozzleConfigurationSlotDto> Slots,
        [property: JsonPropertyName("isApprovedReference")] bool IsApprovedReference = true);

    public sealed record SurfaceSprinklerNozzleDto(
        [property: JsonPropertyName("position")] int Position,
        [property: JsonPropertyName("positionKind")] IrrigationNozzlePositionKind PositionKind,
        [property: JsonPropertyName("state")] IrrigationNozzleState State,
        [property: JsonPropertyName("positionLabel")] string PositionLabel,
        [property: JsonPropertyName("nozzleCode")] string NozzleCode,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("color")] string Color,
        [property: JsonPropertyName("recognitionConfidence")] decimal? RecognitionConfidence,
        [property: JsonPropertyName("nozzleOptionPubId")] Guid? NozzleOptionPubId = null);

    public sealed record SurfaceSprinklerImageLocationDto(
        [property: JsonPropertyName("view")] string View,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("latitude")] decimal? Latitude,
        [property: JsonPropertyName("longitude")] decimal? Longitude,
        [property: JsonPropertyName("locationAccuracyMeters")] decimal? LocationAccuracyMeters,
        [property: JsonPropertyName("capturedAtUtc")] DateTime? CapturedAtUtc);

    public sealed record SurfaceSprinklerDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("configurationPubId")] Guid? ConfigurationPubId,
        [property: JsonPropertyName("identifier")] string Identifier,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("configurationName")] string ConfigurationName,
        [property: JsonPropertyName("topImagePubId")] Guid? TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid? FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid? BackImagePubId,
        [property: JsonPropertyName("latitude")] decimal? Latitude,
        [property: JsonPropertyName("longitude")] decimal? Longitude,
        [property: JsonPropertyName("locationAccuracyMeters")] decimal? LocationAccuracyMeters,
        [property: JsonPropertyName("locationCapturedAtUtc")] DateTime? LocationCapturedAtUtc,
        [property: JsonPropertyName("locationSource")] string LocationSource,
        [property: JsonPropertyName("imageLocations")] IReadOnlyList<SurfaceSprinklerImageLocationDto> ImageLocations,
        [property: JsonPropertyName("recognitionConfidence")] decimal? RecognitionConfidence,
        [property: JsonPropertyName("recognitionSummary")] string RecognitionSummary,
        [property: JsonPropertyName("conditionFlags")] IReadOnlyList<string> ConditionFlags,
        [property: JsonPropertyName("configurationAssessment")] IrrigationNozzleConfigurationAssessment ConfigurationAssessment,
        [property: JsonPropertyName("configurationIssues")] IReadOnlyList<string> ConfigurationIssues,
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("needsReview")] bool NeedsReview,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("confirmedAtUtc")] DateTime? ConfirmedAtUtc,
        [property: JsonPropertyName("lastInspectedAtUtc")] DateTime LastInspectedAtUtc,
        [property: JsonPropertyName("nozzles")] IReadOnlyList<SurfaceSprinklerNozzleDto> Nozzles,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId = null,
        [property: JsonPropertyName("arcDegrees")] decimal? ArcDegrees = null,
        [property: JsonPropertyName("operatingPressureKpa")] decimal? OperatingPressureKpa = null);

    public sealed record SurfaceIrrigationInventoryDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("sprinklers")] IReadOnlyList<SurfaceSprinklerDto> Sprinklers);

    public sealed record FieldIrrigationInventoryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("surfaces")] IReadOnlyList<SurfaceIrrigationInventoryDto> Surfaces);

    public sealed record SurfaceSprinklerSaveDto(
        [property: JsonPropertyName("configurationPubId")] Guid? ConfigurationPubId,
        [property: JsonPropertyName("identifier")][param: Required, MaxLength(80)] string Identifier,
        [property: JsonPropertyName("manufacturerName")][param: MaxLength(120)] string ManufacturerName,
        [property: JsonPropertyName("modelName")][param: MaxLength(160)] string ModelName,
        [property: JsonPropertyName("configurationName")][param: MaxLength(160)] string ConfigurationName,
        [property: JsonPropertyName("notes")][param: MaxLength(2000)] string Notes,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("nozzles")] IReadOnlyList<SurfaceSprinklerNozzleDto> Nozzles,
        [property: JsonPropertyName("saveAsCatalogScope")] IrrigationLocalConfigurationSaveScope SaveAsCatalogScope = IrrigationLocalConfigurationSaveScope.None,
        [property: JsonPropertyName("latitude")] decimal? Latitude = null,
        [property: JsonPropertyName("longitude")] decimal? Longitude = null,
        [property: JsonPropertyName("locationAccuracyMeters")] decimal? LocationAccuracyMeters = null,
        [property: JsonPropertyName("locationCapturedAtUtc")] DateTime? LocationCapturedAtUtc = null,
        [property: JsonPropertyName("locationSource")][param: MaxLength(80)] string? LocationSource = null,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId = null,
        [property: JsonPropertyName("arcDegrees")] decimal? ArcDegrees = null,
        [property: JsonPropertyName("operatingPressureKpa")] decimal? OperatingPressureKpa = null);

    public sealed record IrrigationPhotoAnalysisRequestDto(
        [property: JsonPropertyName("topImagePubId")] Guid TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid BackImagePubId,
        [property: JsonPropertyName("identifier")][param: MaxLength(80)] string? Identifier = null,
        [property: JsonPropertyName("existingSprinklerPubId")] Guid? ExistingSprinklerPubId = null);

    public sealed record SurfaceSprinklerImagesSaveDto(
        [property: JsonPropertyName("topImagePubId")] Guid? TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid? FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid? BackImagePubId);

    public sealed record IrrigationSprinklerReviewDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("organizationName")] string OrganizationName,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("identifier")] string Identifier,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("configurationPubId")] Guid? ConfigurationPubId,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("configurationName")] string ConfigurationName,
        [property: JsonPropertyName("recognitionConfidence")] decimal RecognitionConfidence,
        [property: JsonPropertyName("recognitionSummary")] string RecognitionSummary,
        [property: JsonPropertyName("conditionFlags")] IReadOnlyList<string> ConditionFlags,
        [property: JsonPropertyName("topImagePubId")] Guid? TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid? FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid? BackImagePubId,
        [property: JsonPropertyName("latitude")] decimal? Latitude,
        [property: JsonPropertyName("longitude")] decimal? Longitude,
        [property: JsonPropertyName("locationAccuracyMeters")] decimal? LocationAccuracyMeters,
        [property: JsonPropertyName("lastInspectedAtUtc")] DateTime LastInspectedAtUtc,
        [property: JsonPropertyName("nozzles")] IReadOnlyList<SurfaceSprinklerNozzleDto> Nozzles);

    public sealed record IrrigationSprinklerReviewSaveDto(
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("configurationPubId")] Guid? ConfigurationPubId,
        [property: JsonPropertyName("manufacturerName")][param: MaxLength(120)] string ManufacturerName,
        [property: JsonPropertyName("modelName")][param: MaxLength(160)] string ModelName,
        [property: JsonPropertyName("configurationName")][param: MaxLength(160)] string ConfigurationName);
}
