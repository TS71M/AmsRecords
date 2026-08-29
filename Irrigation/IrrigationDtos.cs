using Lib.Enums;

namespace AmsRecords.Irrigation;

public static class IrrigationRules
{
    public const int MaximumNozzlesPerSprinkler = 5;
    public const int MaximumIdentifierLength = 80;
    public const decimal AiReviewConfidenceThreshold = 0.70m;

    public static IrrigationNozzlePositionKind PositionKindForSlot(int position) => position switch
    {
        1 => IrrigationNozzlePositionKind.MainFront,
        2 => IrrigationNozzlePositionKind.IntermediateFront,
        3 => IrrigationNozzlePositionKind.AuxiliaryFront,
        4 or 5 => IrrigationNozzlePositionKind.Rear,
        _ => IrrigationNozzlePositionKind.Side
    };

    public static string PositionLabelForSlot(int position) => position switch
    {
        1 => "Main front",
        2 => "Side left",
        3 => "Side right",
        4 => "Rear left",
        5 => "Rear right",
        _ => "Nozzle"
    };

    public static bool AreNozzlePositionsInterchangeable(int firstPosition, int secondPosition)
        => firstPosition == secondPosition ||
           (firstPosition is 2 or 3 && secondPosition is 2 or 3) ||
           (firstPosition is 4 or 5 && secondPosition is 4 or 5);

    public static bool PositionKindFitsSlot(int position, IrrigationNozzlePositionKind positionKind)
        => position switch
        {
            2 or 3 => positionKind is IrrigationNozzlePositionKind.IntermediateFront or IrrigationNozzlePositionKind.AuxiliaryFront,
            4 or 5 => positionKind == IrrigationNozzlePositionKind.Rear,
            _ => positionKind == PositionKindForSlot(position)
        };

    public static IrrigationNozzleState ObservedStateForSelection(
        Guid? nozzleOptionPubId,
        string? nozzleCode,
        string? nozzleName,
        string? color)
    {
        if (nozzleOptionPubId.HasValue ||
            new[] { nozzleCode, nozzleName, color }.Any(value =>
                !string.IsNullOrWhiteSpace(value) &&
                !string.Equals(value.Trim(), "unknown", StringComparison.OrdinalIgnoreCase)))
        {
            return IrrigationNozzleState.Installed;
        }

        return IrrigationNozzleState.Unknown;
    }

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
    public sealed record IrrigationSprinklerRecognitionFactDto(
        [property: JsonPropertyName("factType")] string FactType,
        [property: JsonPropertyName("value")] string Value,
        [property: JsonPropertyName("evidenceWeight")] decimal EvidenceWeight,
        [property: JsonPropertyName("isRequiredForExactMatch")] bool IsRequiredForExactMatch,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public sealed record IrrigationSprinklerRecognitionProfileDto(
        [property: JsonPropertyName("recognitionFamily")] string RecognitionFamily,
        [property: JsonPropertyName("visibleModelMarking")] string VisibleModelMarking,
        [property: JsonPropertyName("driveType")] string DriveType,
        [property: JsonPropertyName("arcType")] string ArcType,
        [property: JsonPropertyName("bodyStyle")] string BodyStyle,
        [property: JsonPropertyName("inlineFrontNozzlePortCountMin")] int? InlineFrontNozzlePortCountMin,
        [property: JsonPropertyName("inlineFrontNozzlePortCountMax")] int? InlineFrontNozzlePortCountMax,
        [property: JsonPropertyName("rearNozzlePortCount")] int? RearNozzlePortCount,
        [property: JsonPropertyName("dimensionBoundary")] string DimensionBoundary,
        [property: JsonPropertyName("outerBodyDiameterMinMm")] decimal? OuterBodyDiameterMinMm,
        [property: JsonPropertyName("outerBodyDiameterMaxMm")] decimal? OuterBodyDiameterMaxMm,
        [property: JsonPropertyName("referenceFeature")] string ReferenceFeature,
        [property: JsonPropertyName("referenceDiameterMm")] decimal? ReferenceDiameterMm,
        [property: JsonPropertyName("bodyHeightMinMm")] decimal? BodyHeightMinMm,
        [property: JsonPropertyName("bodyHeightMaxMm")] decimal? BodyHeightMaxMm,
        [property: JsonPropertyName("inletSizeMillimeters")] decimal? InletSizeMillimeters,
        [property: JsonPropertyName("dimensionalToleranceMm")] decimal? DimensionalToleranceMm,
        [property: JsonPropertyName("verificationStatus")] string VerificationStatus,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("distinguishingSummary")] string DistinguishingSummary,
        [property: JsonPropertyName("facts")] IReadOnlyList<IrrigationSprinklerRecognitionFactDto> Facts);

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
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("defaultMinPressureBar")] decimal? DefaultMinPressureBar = null,
        [property: JsonPropertyName("defaultMaxPressureBar")] decimal? DefaultMaxPressureBar = null,
        [property: JsonPropertyName("recognitionProfile")] IrrigationSprinklerRecognitionProfileDto? RecognitionProfile = null);

    public sealed record IrrigationSprinklerModelSaveDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("manufacturerName")][param: Required, MaxLength(120)] string ManufacturerName,
        [property: JsonPropertyName("modelName")][param: Required, MaxLength(160)] string ModelName,
        [property: JsonPropertyName("modelCode")][param: MaxLength(80)] string ModelCode,
        [property: JsonPropertyName("maximumNozzleCount")][param: Range(1, 5)] int MaximumNozzleCount,
        [property: JsonPropertyName("sourceUrl")][param: MaxLength(500)] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")][param: MaxLength(2000)] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("active")] bool Active = true,
        [property: JsonPropertyName("defaultMinPressureBar")][param: Range(typeof(decimal), "0", "100")] decimal? DefaultMinPressureBar = null,
        [property: JsonPropertyName("defaultMaxPressureBar")][param: Range(typeof(decimal), "0", "100")] decimal? DefaultMaxPressureBar = null);

    public sealed record IrrigationSprinklerNozzleOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid SprinklerModelPubId,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("sprinklerModelName")] string SprinklerModelName,
        [property: JsonPropertyName("sprinklerModelCode")] string SprinklerModelCode,
        [property: JsonPropertyName("positionKind")] IrrigationNozzlePositionKind PositionKind,
        [property: JsonPropertyName("nozzleCode")] string NozzleCode,
        [property: JsonPropertyName("nozzleName")] string NozzleName,
        [property: JsonPropertyName("color")] string Color,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("referenceNotes")] string ReferenceNotes,
        [property: JsonPropertyName("isLegacy")] bool IsLegacy,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("nominalFlowM3H")] decimal? NominalFlowM3H = null,
        [property: JsonPropertyName("nominalRadiusM")] decimal? NominalRadiusM = null,
        [property: JsonPropertyName("nominalPressureBar")] decimal? NominalPressureBar = null,
        [property: JsonPropertyName("referenceImageUrl")] string? ReferenceImageUrl = null);

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
        [property: JsonPropertyName("active")] bool Active = true,
        [property: JsonPropertyName("nominalFlowM3H")][param: Range(typeof(decimal), "0", "10000")] decimal? NominalFlowM3H = null,
        [property: JsonPropertyName("nominalRadiusM")][param: Range(typeof(decimal), "0", "10000")] decimal? NominalRadiusM = null,
        [property: JsonPropertyName("nominalPressureBar")][param: Range(typeof(decimal), "0", "100")] decimal? NominalPressureBar = null,
        [property: JsonPropertyName("referenceImageUrl")][param: MaxLength(500)] string? ReferenceImageUrl = null);

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
        [property: JsonPropertyName("nozzleOptionPubId")] Guid? NozzleOptionPubId = null,
        [property: JsonPropertyName("compatibilityOverride")] bool CompatibilityOverride = false);

    public sealed record SurfaceSprinklerImageLocationDto(
        [property: JsonPropertyName("view")] string View,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("latitude")] decimal? Latitude,
        [property: JsonPropertyName("longitude")] decimal? Longitude,
        [property: JsonPropertyName("locationAccuracyMeters")] decimal? LocationAccuracyMeters,
        [property: JsonPropertyName("capturedAtUtc")] DateTime? CapturedAtUtc);

    public sealed record SurfaceSprinklerDimensionalMeasurementDto(
        [property: JsonPropertyName("direction")] string Direction,
        [property: JsonPropertyName("referenceRadiusPixels")] decimal ReferenceRadiusPixels,
        [property: JsonPropertyName("outerRadiusPixels")] decimal OuterRadiusPixels,
        [property: JsonPropertyName("ratio")] decimal Ratio,
        [property: JsonPropertyName("estimatedOuterDiameterMm")] decimal EstimatedOuterDiameterMm,
        [property: JsonPropertyName("boundaryConfidence")] decimal BoundaryConfidence,
        [property: JsonPropertyName("centerXNormalized")] decimal CenterXNormalized,
        [property: JsonPropertyName("centerYNormalized")] decimal CenterYNormalized,
        [property: JsonPropertyName("referenceBoundaryXNormalized")] decimal ReferenceBoundaryXNormalized,
        [property: JsonPropertyName("referenceBoundaryYNormalized")] decimal ReferenceBoundaryYNormalized,
        [property: JsonPropertyName("outerBoundaryXNormalized")] decimal OuterBoundaryXNormalized,
        [property: JsonPropertyName("outerBoundaryYNormalized")] decimal OuterBoundaryYNormalized);

    public sealed record SurfaceSprinklerDimensionalReviewDto(
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("favouredModelCode")] string? FavouredModelCode,
        [property: JsonPropertyName("knownReferenceDiameterMm")] decimal? KnownReferenceDiameterMm,
        [property: JsonPropertyName("combinedRatio")] decimal? CombinedRatio,
        [property: JsonPropertyName("estimatedOuterDiameterMm")] decimal? EstimatedOuterDiameterMm,
        [property: JsonPropertyName("estimatedUncertaintyMm")] decimal? EstimatedUncertaintyMm,
        [property: JsonPropertyName("confidence")] decimal RecognitionConfidence,
        [property: JsonPropertyName("measurements")] IReadOnlyList<SurfaceSprinklerDimensionalMeasurementDto> Measurements,
        [property: JsonPropertyName("limitations")] IReadOnlyList<string> Limitations);

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
        [property: JsonPropertyName("operatingPressureKpa")] decimal? OperatingPressureKpa = null,
        [property: JsonPropertyName("dimensionalReview")] SurfaceSprinklerDimensionalReviewDto? DimensionalReview = null);

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
        [property: JsonPropertyName("existingSprinklerPubId")] Guid? ExistingSprinklerPubId = null,
        [property: JsonPropertyName("selectedFlex800BodyFamily")][param: MaxLength(20)] string? SelectedFlex800BodyFamily = null,
        [property: JsonPropertyName("selectedSprinklerModelPubId")] Guid? SelectedSprinklerModelPubId = null);

    public sealed record IrrigationPhotoAnalysisPreflightRequestDto(
        [property: JsonPropertyName("topImagePubId")] Guid TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid BackImagePubId,
        [property: JsonPropertyName("selectedSprinklerModelPubId")] Guid? SelectedSprinklerModelPubId = null);

    public sealed record IrrigationPhotoAnalysisPreflightDto(
        [property: JsonPropertyName("requiresBodyFamilySelection")] bool RequiresBodyFamilySelection,
        [property: JsonPropertyName("selectedFlex800BodyFamily")] string? SelectedFlex800BodyFamily,
        [property: JsonPropertyName("dimensionalReview")] SurfaceSprinklerDimensionalReviewDto? DimensionalReview);

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

    public sealed record IrrigationRecognitionPatternProposalDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sourceSprinklerPubId")] Guid SourceSprinklerPubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("organizationName")] string OrganizationName,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("identifier")] string Identifier,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("factType")] string FactType,
        [property: JsonPropertyName("proposedValue")] string ProposedValue,
        [property: JsonPropertyName("evidenceViews")] IReadOnlyList<string> EvidenceViews,
        [property: JsonPropertyName("evidenceSummary")] string EvidenceSummary,
        [property: JsonPropertyName("aiConfidence")] decimal AiConfidence,
        [property: JsonPropertyName("aiModel")] string AiModel,
        [property: JsonPropertyName("promptVersion")] string PromptVersion,
        [property: JsonPropertyName("suggestedRequiredForExactMatch")] bool SuggestedRequiredForExactMatch,
        [property: JsonPropertyName("topImagePubId")] Guid? TopImagePubId,
        [property: JsonPropertyName("frontImagePubId")] Guid? FrontImagePubId,
        [property: JsonPropertyName("backImagePubId")] Guid? BackImagePubId,
        [property: JsonPropertyName("occurrenceCount")] int OccurrenceCount,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("lastObservedAtUtc")] DateTime LastObservedAtUtc,
        [property: JsonPropertyName("reviewedAtUtc")] DateTime? ReviewedAtUtc,
        [property: JsonPropertyName("reviewerNotes")] string ReviewerNotes);

    public sealed record IrrigationRecognitionPatternApprovalDto(
        [property: JsonPropertyName("sprinklerModelPubId")] Guid SprinklerModelPubId,
        [property: JsonPropertyName("factType")][param: Required, MaxLength(64)] string FactType,
        [property: JsonPropertyName("value")][param: Required, MaxLength(500)] string Value,
        [property: JsonPropertyName("evidenceWeight")][param: Range(typeof(decimal), "0", "1")] decimal EvidenceWeight,
        [property: JsonPropertyName("isRequiredForExactMatch")] bool IsRequiredForExactMatch,
        [property: JsonPropertyName("reviewerNotes")][param: MaxLength(1000)] string ReviewerNotes);

    public sealed record IrrigationRecognitionPatternRejectionDto(
        [property: JsonPropertyName("reviewerNotes")][param: MaxLength(1000)] string ReviewerNotes);

    public sealed record IrrigationRecognitionLearningSummaryDto(
        [property: JsonPropertyName("activeGlobalExampleCount")] int ActiveGlobalExampleCount,
        [property: JsonPropertyName("lastApprovedAtUtc")] DateTime? LastApprovedAtUtc,
        [property: JsonPropertyName("recurringErrors")] IReadOnlyList<IrrigationRecognitionRecurringErrorDto> RecurringErrors);

    public sealed record IrrigationRecognitionRecurringErrorDto(
        [property: JsonPropertyName("fingerprint")] string Fingerprint,
        [property: JsonPropertyName("occurrences")] int Occurrences,
        [property: JsonPropertyName("lastApprovedAtUtc")] DateTime LastApprovedAtUtc);

    public sealed record IrrigationRecognitionLearningDatasetDto(
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("itemCount")] int ItemCount,
        [property: JsonPropertyName("truncated")] bool Truncated,
        [property: JsonPropertyName("items")] IReadOnlyList<IrrigationRecognitionLearningDatasetItemDto> Items);

    public sealed record IrrigationRecognitionLearningDatasetItemDto(
        [property: JsonPropertyName("examplePubId")] Guid ExamplePubId,
        [property: JsonPropertyName("approvedAtUtc")] DateTime ApprovedAtUtc,
        [property: JsonPropertyName("promptVersion")] string PromptVersion,
        [property: JsonPropertyName("originalConfidence")] decimal OriginalConfidence,
        [property: JsonPropertyName("predictedManufacturerName")] string PredictedManufacturerName,
        [property: JsonPropertyName("predictedModelName")] string PredictedModelName,
        [property: JsonPropertyName("predictedConfigurationName")] string PredictedConfigurationName,
        [property: JsonPropertyName("approvedManufacturerName")] string ApprovedManufacturerName,
        [property: JsonPropertyName("approvedModelName")] string ApprovedModelName,
        [property: JsonPropertyName("approvedConfigurationName")] string ApprovedConfigurationName,
        [property: JsonPropertyName("observedNozzleColors")] IReadOnlyList<string> ObservedNozzleColors,
        [property: JsonPropertyName("evidenceSummary")] string EvidenceSummary,
        [property: JsonPropertyName("errorFingerprint")] string ErrorFingerprint,
        [property: JsonPropertyName("originalAnalysisJson")] string OriginalAnalysisJson,
        [property: JsonPropertyName("approvedCorrectionJson")] string ApprovedCorrectionJson,
        [property: JsonPropertyName("images")] IReadOnlyList<IrrigationRecognitionLearningImageDto> Images);

    public sealed record IrrigationRecognitionLearningImageDto(
        [property: JsonPropertyName("view")] string View,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId);
}
