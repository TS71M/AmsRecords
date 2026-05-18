namespace AmsRecords.WaterTests;

public static class WaterTestDtos
{
    public sealed record WaterTestIndexDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("tests")] List<WaterTestIndexItemDto> Tests
    );

    public sealed record WaterTestIndexItemDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("reportDateUtc")] DateTime ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("completedResultCount")] int CompletedResultCount,
        [property: JsonPropertyName("totalResultCount")] int TotalResultCount
    );

    public sealed record WaterTestResultUpsertDto(
        [property: JsonPropertyName("resultCode")] string ResultCode,
        [property: JsonPropertyName("value")] decimal? Value,
        [property: JsonPropertyName("textValue")] string? TextValue
    );

    public sealed record WaterTestResultDto(
        [property: JsonPropertyName("resultCode")] string ResultCode,
        [property: JsonPropertyName("sectionName")] string SectionName,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("unitLabel")] string? UnitLabel,
        [property: JsonPropertyName("isNumeric")] bool IsNumeric,
        [property: JsonPropertyName("value")] decimal? Value,
        [property: JsonPropertyName("textValue")] string? TextValue,
        [property: JsonPropertyName("reference1")] string? Reference1,
        [property: JsonPropertyName("reference2")] string? Reference2,
        [property: JsonPropertyName("reference3")] string? Reference3,
        [property: JsonPropertyName("reference4")] string? Reference4,
        [property: JsonPropertyName("methodText")] string? MethodText,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record WaterTestCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportDateUtc")] DateTime ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("customerNumber")] string? CustomerNumber,
        [property: JsonPropertyName("samplerName")] string? SamplerName,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("results")] List<WaterTestResultUpsertDto> Results
    );

    public sealed record WaterTestUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportDateUtc")] DateTime ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("customerNumber")] string? CustomerNumber,
        [property: JsonPropertyName("samplerName")] string? SamplerName,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("results")] List<WaterTestResultUpsertDto> Results
    );

    public sealed record WaterTestDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportDateUtc")] DateTime ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("customerNumber")] string? CustomerNumber,
        [property: JsonPropertyName("samplerName")] string? SamplerName,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("results")] List<WaterTestResultDto> Results
    );

    public sealed record WaterTestDocumentImportPreviewDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("reportedFieldName")] string? ReportedFieldName,
        [property: JsonPropertyName("fieldSimilarityScore")] decimal? FieldSimilarityScore,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportDateUtc")] DateTime? ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("customerNumber")] string? CustomerNumber,
        [property: JsonPropertyName("samplerName")] string? SamplerName,
        [property: JsonPropertyName("results")] List<WaterTestDocumentImportResultPreviewDto> Results,
        [property: JsonPropertyName("warnings")] List<string> Warnings,
        [property: JsonPropertyName("extractedTextPreview")] string? ExtractedTextPreview
    );

    public sealed record WaterTestDocumentImportResultPreviewDto(
        [property: JsonPropertyName("resultCode")] string ResultCode,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("detectedLabel")] string DetectedLabel,
        [property: JsonPropertyName("isNumeric")] bool IsNumeric,
        [property: JsonPropertyName("value")] decimal? Value,
        [property: JsonPropertyName("textValue")] string? TextValue,
        [property: JsonPropertyName("confidence")] decimal Confidence
    );

    public sealed record WaterTestDocumentImportCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("reportedFieldName")] string? ReportedFieldName,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reportDateUtc")] DateTime ReportDateUtc,
        [property: JsonPropertyName("sampleReceivedDateUtc")] DateTime? SampleReceivedDateUtc,
        [property: JsonPropertyName("sampleTakenDateUtc")] DateTime? SampleTakenDateUtc,
        [property: JsonPropertyName("reportNumber")] string? ReportNumber,
        [property: JsonPropertyName("analysisNumber")] string? AnalysisNumber,
        [property: JsonPropertyName("projectName")] string? ProjectName,
        [property: JsonPropertyName("customerNumber")] string? CustomerNumber,
        [property: JsonPropertyName("samplerName")] string? SamplerName,
        [property: JsonPropertyName("ignore")] bool Ignore,
        [property: JsonPropertyName("results")] List<WaterTestDocumentImportResultInputDto> Results
    );

    public sealed record WaterTestDocumentImportResultInputDto(
        [property: JsonPropertyName("resultCode")] string ResultCode,
        [property: JsonPropertyName("value")] decimal? Value,
        [property: JsonPropertyName("textValue")] string? TextValue,
        [property: JsonPropertyName("detectedLabel")] string? DetectedLabel
    );
}
