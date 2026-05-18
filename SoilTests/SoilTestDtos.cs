namespace AmsRecords.SoilTests;

public static class SoilTestDtos
{
    public sealed record SoilTestIndexDto(
         [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
         [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
         [property: JsonPropertyName("fieldName")] string FieldName,
         [property: JsonPropertyName("tests")] List<SoilTestIndexItemDto> Tests
     );

    public sealed record SoilTestIndexItemDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("holeName")] string HoleName,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("resultCount")] int ResultCount,
        [property: JsonPropertyName("results")] List<SoilTestResultDto> Results
    );

    public sealed record SoilTestChemUpsertDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("value")] decimal Value
    );

    public sealed record SoilTestChemDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("chemTypeName")] string? ChemTypeName,
        [property: JsonPropertyName("value")] decimal Value
    );

    public sealed record SoilTestCreateDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("methodRaw")] string? MethodRaw,
        [property: JsonPropertyName("results")] List<SoilTestResultUpsertDto> Results
    );

    public sealed record SoilTestUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("methodRaw")] string? MethodRaw,
        [property: JsonPropertyName("results")] List<SoilTestResultUpsertDto> Results
    );

    public sealed record SoilTestDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holeName")] string HoleName,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("methodRaw")] string? MethodRaw,
        [property: JsonPropertyName("results")] List<SoilTestResultDto> Results
    );
    public sealed record SoilTestResultDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("analyteName")] string AnalyteName,
        [property: JsonPropertyName("analyteGroup")] string? AnalyteGroup,
        [property: JsonPropertyName("value")] decimal? Value,
        [property: JsonPropertyName("textValue")] string? TextValue,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort
    );

    public sealed record SoilTestResultUpsertDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId
    );

    public sealed record SoilExtractMethodDto(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name
    );

    public sealed record SoilTestTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("selectedMethodCode")] string? SelectedMethodCode,
        [property: JsonPropertyName("availableMethods")] List<SoilExtractMethodDto> AvailableMethods,
        [property: JsonPropertyName("series")] List<SoilTestTimelineSeriesDto> Series
    );

    public sealed record SoilTestTimelineSeriesDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("points")] List<SoilTestTimelinePointDto> Points
    );

    public sealed record SoilTestTimelinePointDto(
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("sampleCount")] int SampleCount
    );

    public sealed record SoilTestScanImportDto(
        [property: JsonPropertyName("fileName")] string FileName,
        [property: JsonPropertyName("matches")] List<SoilTestScanImportMatchDto> Matches,
        [property: JsonPropertyName("warnings")] List<string> Warnings,
        [property: JsonPropertyName("extractedTextPreview")] string? ExtractedTextPreview
    );

    public sealed record SoilTestScanImportMatchDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("analyteName")] string AnalyteName,
        [property: JsonPropertyName("detectedLabel")] string DetectedLabel,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("confidence")] decimal Confidence
    );

    public sealed record SoilTestDocumentImportPreviewDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("reportedFieldName")] string? ReportedFieldName,
        [property: JsonPropertyName("fieldSimilarityScore")] decimal? FieldSimilarityScore,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime? SoilTestDateUtc,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("methodRaw")] string? MethodRaw,
        [property: JsonPropertyName("availableSurfaces")] List<SoilTestDocumentImportSurfaceOptionDto> AvailableSurfaces,
        [property: JsonPropertyName("surfaces")] List<SoilTestDocumentImportSurfacePreviewDto> Surfaces,
        [property: JsonPropertyName("warnings")] List<string> Warnings,
        [property: JsonPropertyName("extractedTextPreview")] string? ExtractedTextPreview
    );

    public sealed record SoilTestDocumentImportSurfaceOptionDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName
    );

    public sealed record SoilTestDocumentImportSurfacePreviewDto(
        [property: JsonPropertyName("rawSurfaceLabel")] string RawSurfaceLabel,
        [property: JsonPropertyName("matchedSurfacePubId")] Guid? MatchedSurfacePubId,
        [property: JsonPropertyName("matchedSurfaceName")] string? MatchedSurfaceName,
        [property: JsonPropertyName("matchScore")] decimal? MatchScore,
        [property: JsonPropertyName("canCreate")] bool CanCreate,
        [property: JsonPropertyName("warnings")] List<string> Warnings,
        [property: JsonPropertyName("results")] List<SoilTestDocumentImportResultPreviewDto> Results
    );

    public sealed record SoilTestDocumentImportResultPreviewDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("analyteName")] string AnalyteName,
        [property: JsonPropertyName("detectedLabel")] string DetectedLabel,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("confidence")] decimal Confidence
    );

    public sealed record SoilTestDocumentImportCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("reportedFieldName")] string? ReportedFieldName,
        [property: JsonPropertyName("soilTestDateUtc")] DateTime SoilTestDateUtc,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("methodRaw")] string? MethodRaw,
        [property: JsonPropertyName("items")] List<SoilTestDocumentImportCreateItemDto> Items
    );

    public sealed record SoilTestDocumentImportCreateItemDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("rawSurfaceLabel")] string RawSurfaceLabel,
        [property: JsonPropertyName("selected")] bool Selected,
        [property: JsonPropertyName("results")] List<SoilTestDocumentImportCreateResultInputDto> Results
    );

    public sealed record SoilTestDocumentImportCreateResultInputDto(
        [property: JsonPropertyName("analytePubId")] Guid AnalytePubId,
        [property: JsonPropertyName("value")] decimal Value,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("detectedLabel")] string? DetectedLabel
    );

    public sealed record SoilTestDocumentImportCreateResultDto(
        [property: JsonPropertyName("createdCount")] int CreatedCount,
        [property: JsonPropertyName("skippedCount")] int SkippedCount,
        [property: JsonPropertyName("items")] List<SoilTestDocumentImportCreateResultItemDto> Items
    );

    public sealed record SoilTestDocumentImportCreateResultItemDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("created")] bool Created,
        [property: JsonPropertyName("message")] string Message
    );
}
