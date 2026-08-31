using Lib.Enums;

namespace AmsRecords.Irrigation;

public static class IrrigationCatalogImportDtos
{
    public sealed record IrrigationCatalogImportBatchDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("documentTitle")] string DocumentTitle,
        [property: JsonPropertyName("documentNumber")] string DocumentNumber,
        [property: JsonPropertyName("revision")] string Revision,
        [property: JsonPropertyName("sourceUrl")] string SourceUrl,
        [property: JsonPropertyName("sourceUrlStatus")] string SourceUrlStatus,
        [property: JsonPropertyName("sourceTraceNotes")] string SourceTraceNotes,
        [property: JsonPropertyName("originalFileName")] string OriginalFileName,
        [property: JsonPropertyName("hasStoredPdf")] bool HasStoredPdf,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("extractorModel")] string ExtractorModel,
        [property: JsonPropertyName("promptVersion")] string PromptVersion,
        [property: JsonPropertyName("failureSummary")] string FailureSummary,
        [property: JsonPropertyName("candidateCount")] int CandidateCount,
        [property: JsonPropertyName("pendingCount")] int PendingCount,
        [property: JsonPropertyName("acceptedCount")] int AcceptedCount,
        [property: JsonPropertyName("rejectedCount")] int RejectedCount,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("candidates")] IReadOnlyList<IrrigationCatalogImportCandidateDto> Candidates);

    public sealed record IrrigationCatalogImportCandidateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("candidateType")] string CandidateType,
        [property: JsonPropertyName("sourcePage")] int SourcePage,
        [property: JsonPropertyName("displaySummary")] string DisplaySummary,
        [property: JsonPropertyName("payloadJson")] string PayloadJson,
        [property: JsonPropertyName("warnings")] IReadOnlyList<string> Warnings,
        [property: JsonPropertyName("extractionConfidence")] decimal ExtractionConfidence,
        [property: JsonPropertyName("evidenceLevel")] IrrigationCompatibilityEvidenceLevel EvidenceLevel,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("reviewerNotes")] string ReviewerNotes,
        [property: JsonPropertyName("reviewedAtUtc")] DateTime? ReviewedAtUtc,
        [property: JsonPropertyName("reconciliationStatus")] string ReconciliationStatus = IrrigationCatalogReconciliationStatuses.NotEvaluated,
        [property: JsonPropertyName("reconciliationDifferences")] IReadOnlyList<IrrigationCatalogReconciliationDifferenceDto>? ReconciliationDifferences = null);

    public sealed record IrrigationCatalogReconciliationDifferenceDto(
        [property: JsonPropertyName("field")] string Field,
        [property: JsonPropertyName("existingValue")] string ExistingValue,
        [property: JsonPropertyName("extractedValue")] string ExtractedValue,
        [property: JsonPropertyName("effect")] string Effect);

    public sealed record IrrigationCatalogCandidateReviewDto(
        [property: JsonPropertyName("sourcePage")][param: Range(1, 10000)] int SourcePage,
        [property: JsonPropertyName("payloadJson")][param: Required] string PayloadJson,
        [property: JsonPropertyName("reviewerNotes")][param: MaxLength(2000)] string ReviewerNotes,
        [property: JsonPropertyName("accept")] bool Accept);

    public sealed record IrrigationCatalogComponentCandidate(
        string ManufacturerName,
        string PartNumber,
        string ManufacturerNumber,
        string ComponentType,
        string Name,
        string Color,
        string Notes);

    public sealed record IrrigationCatalogPlatformCandidate(
        string ManufacturerName,
        string PlatformCode,
        string Name,
        string Description);

    public sealed record IrrigationCatalogModelPlatformCandidate(
        string ManufacturerName,
        string ModelCode,
        string PlatformCode);

    public sealed record IrrigationCatalogPlatformComponentCandidate(
        string ManufacturerName,
        string PlatformCode,
        string PartNumber,
        string RoleCode,
        IrrigationNozzlePositionKind? PositionKind,
        IrrigationCompatibilityEvidenceLevel EvidenceLevel,
        string Notes);

    public sealed record IrrigationCatalogNozzleSetComponentCandidate(
        string PartNumber,
        string RoleCode,
        int? Position,
        IrrigationNozzlePositionKind? PositionKind,
        decimal? RecommendedInstallationAngleDegrees,
        bool IsRequired);

    public sealed record IrrigationCatalogNozzleSetCandidate(
        string ManufacturerName,
        string PlatformCode,
        string SetCode,
        string Name,
        IrrigationCompatibilityEvidenceLevel EvidenceLevel,
        string Notes,
        IReadOnlyList<IrrigationCatalogNozzleSetComponentCandidate> Components);

    public sealed record IrrigationCatalogPerformanceCandidate(
        string ManufacturerName,
        string PlatformCode,
        string SetCode,
        decimal PressureBar,
        decimal FlowM3H,
        decimal RadiusM,
        decimal? PrecipitationRateMmH,
        decimal? TrajectoryDegrees,
        decimal? RotationSeconds,
        IrrigationCompatibilityEvidenceLevel EvidenceLevel);
}
