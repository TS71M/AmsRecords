using System.Text.Json;

namespace AmsRecords.ApplicationEvidence;

public static class ApplicationEvidenceDtos
{
    public sealed record ImportEvidenceRequest(
        [property: JsonPropertyName("token"), Required, StringLength(100000)] string Token);

    public sealed class RegisterEvidenceRequest
    {
        [JsonPropertyName("evidenceKind")]
        public ApplicationEvidenceKind EvidenceKind { get; set; }

        [JsonPropertyName("sourceKind")]
        public ApplicationEvidenceSourceKind SourceKind { get; set; }

        [JsonPropertyName("title"), Required, StringLength(250, MinimumLength = 1)]
        public string Title { get; set; } = "";

        [JsonPropertyName("sourceUri"), StringLength(1000)]
        public string SourceUri { get; set; } = "";

        [JsonPropertyName("storageReference"), StringLength(1000)]
        public string StorageReference { get; set; } = "";

        [JsonPropertyName("contentType"), StringLength(120)]
        public string ContentType { get; set; } = "";

        [JsonPropertyName("contentLengthBytes"), Range(0, long.MaxValue)]
        public long? ContentLengthBytes { get; set; }

        [JsonPropertyName("contentSha256"), Required, RegularExpression("^[A-Fa-f0-9]{64}$")]
        public string ContentSha256 { get; set; } = "";

        [JsonPropertyName("documentRevision"), StringLength(80)]
        public string DocumentRevision { get; set; } = "";

        [JsonPropertyName("jurisdictionCode"), StringLength(80)]
        public string JurisdictionCode { get; set; } = "";

        [JsonPropertyName("effectiveFrom")]
        public DateOnly? EffectiveFrom { get; set; }

        [JsonPropertyName("effectiveTo")]
        public DateOnly? EffectiveTo { get; set; }

        [JsonPropertyName("supersedesEvidencePubId")]
        public Guid? SupersedesEvidencePubId { get; set; }
    }

    public sealed record SourceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("targetPubId")] Guid TargetPubId,
        [property: JsonPropertyName("targetType")] string TargetType,
        [property: JsonPropertyName("evidenceKind")] ApplicationEvidenceKind EvidenceKind,
        [property: JsonPropertyName("sourceKind")] ApplicationEvidenceSourceKind SourceKind,
        [property: JsonPropertyName("trustLevel")] ApplicationEvidenceTrustLevel TrustLevel,
        [property: JsonPropertyName("reviewStatus")] ApplicationEvidenceReviewStatus ReviewStatus,
        [property: JsonPropertyName("reviewOrigin")] ApplicationEvidenceReviewOrigin ReviewOrigin,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("sourceUri")] string SourceUri,
        [property: JsonPropertyName("contentSha256")] string ContentSha256,
        [property: JsonPropertyName("documentRevision")] string DocumentRevision,
        [property: JsonPropertyName("jurisdictionCode")] string JurisdictionCode,
        [property: JsonPropertyName("effectiveFrom")] DateOnly? EffectiveFrom,
        [property: JsonPropertyName("effectiveTo")] DateOnly? EffectiveTo,
        [property: JsonPropertyName("retrievedAtUtc")] DateTime RetrievedAtUtc,
        [property: JsonPropertyName("extractionConfidence")] decimal ExtractionConfidence,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("concurrencyToken")] Guid ConcurrencyToken,
        [property: JsonPropertyName("suggestions")] IReadOnlyList<SuggestionDto> Suggestions,
        [property: JsonPropertyName("canReview")] bool CanReview = false,
        [property: JsonPropertyName("hasStoredContent")] bool HasStoredContent = false,
        [property: JsonPropertyName("productProfilePubId")] Guid? ProductProfilePubId = null,
        [property: JsonPropertyName("productProfileIsCurrent")] bool ProductProfileIsCurrent = false,
        [property: JsonPropertyName("canCommitProductProfile")] bool CanCommitProductProfile = false,
        [property: JsonPropertyName("machineProfilePubId")] Guid? MachineProfilePubId = null,
        [property: JsonPropertyName("machineProfileIsCurrent")] bool MachineProfileIsCurrent = false,
        [property: JsonPropertyName("canCommitMachineProfile")] bool CanCommitMachineProfile = false,
        [property: JsonPropertyName("freshnessStatus")] ApplicationEvidenceFreshnessStatus FreshnessStatus = ApplicationEvidenceFreshnessStatus.NotMonitored,
        [property: JsonPropertyName("sourceCheckedAtUtc")] DateTime? SourceCheckedAtUtc = null,
        [property: JsonPropertyName("sourceCheckDueAtUtc")] DateTime? SourceCheckDueAtUtc = null,
        [property: JsonPropertyName("sourceContentChangedAtUtc")] DateTime? SourceContentChangedAtUtc = null);

    public sealed record EvidenceListDto(
        [property: JsonPropertyName("canReview")] bool CanReview,
        [property: JsonPropertyName("sources")] IReadOnlyList<SourceDto> Sources);

    public sealed record SuggestionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldKey")] string FieldKey,
        [property: JsonPropertyName("proposedValue")] JsonElement ProposedValue,
        [property: JsonPropertyName("reviewedValue")] JsonElement? ReviewedValue,
        [property: JsonPropertyName("unitCode")] string UnitCode,
        [property: JsonPropertyName("sourceLocation")] string SourceLocation,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("isSafetyCritical")] bool IsSafetyCritical,
        [property: JsonPropertyName("requiresHumanReview")] bool RequiresHumanReview,
        [property: JsonPropertyName("reviewStatus")] ApplicationEvidenceReviewStatus ReviewStatus,
        [property: JsonPropertyName("reviewOrigin")] ApplicationEvidenceReviewOrigin ReviewOrigin,
        [property: JsonPropertyName("concurrencyToken")] Guid ConcurrencyToken);

    public sealed class ReviewSuggestionRequest
    {
        [JsonPropertyName("concurrencyToken")]
        public Guid ConcurrencyToken { get; set; }

        [JsonPropertyName("decision")]
        public ApplicationEvidenceReviewStatus Decision { get; set; }

        [JsonPropertyName("approvedValue")]
        public JsonElement? ApprovedValue { get; set; }

        [JsonPropertyName("note"), StringLength(1000)]
        public string Note { get; set; } = "";
    }

    public sealed class ProductRegulatoryProfileRequest
    {
        [JsonPropertyName("evidencePubId")]
        public Guid EvidencePubId { get; set; }

        [JsonPropertyName("jurisdictionCode"), StringLength(80)]
        public string JurisdictionCode { get; set; } = "";

        [JsonPropertyName("revision"), StringLength(80)]
        public string Revision { get; set; } = "";

        [JsonPropertyName("effectiveFrom")]
        public DateOnly? EffectiveFrom { get; set; }

        [JsonPropertyName("effectiveTo")]
        public DateOnly? EffectiveTo { get; set; }

    }

    public sealed record ProductUseRule(
        [property: JsonPropertyName("target")] string Target,
        [property: JsonPropertyName("siteOrCrop")] string SiteOrCrop,
        [property: JsonPropertyName("minimumRate")] decimal? MinimumRate,
        [property: JsonPropertyName("maximumRate")] decimal? MaximumRate,
        [property: JsonPropertyName("rateUnit")] string RateUnit,
        [property: JsonPropertyName("maximumApplications")] int? MaximumApplications,
        [property: JsonPropertyName("minimumIntervalDays")] int? MinimumIntervalDays,
        [property: JsonPropertyName("reEntryHours")] decimal? ReEntryHours,
        [property: JsonPropertyName("preHarvestDays")] int? PreHarvestDays,
        [property: JsonPropertyName("bufferZoneMetres")] decimal? BufferZoneMetres,
        [property: JsonPropertyName("instructions")] string Instructions,
        [property: JsonPropertyName("sourceLocation")] string SourceLocation);

    public sealed record WeatherRestriction(
        [property: JsonPropertyName("metric")] string Metric,
        [property: JsonPropertyName("minimum")] decimal? Minimum,
        [property: JsonPropertyName("maximum")] decimal? Maximum,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("instructions")] string Instructions,
        [property: JsonPropertyName("sourceLocation")] string SourceLocation);

    public sealed class MachineApplicationProfileRequest
    {
        [JsonPropertyName("evidencePubId")]
        public Guid EvidencePubId { get; set; }

        [JsonPropertyName("revision"), StringLength(80)]
        public string Revision { get; set; } = "";

        [JsonPropertyName("effectiveFrom")]
        public DateOnly? EffectiveFrom { get; set; }

        [JsonPropertyName("effectiveTo")]
        public DateOnly? EffectiveTo { get; set; }

    }
}

public static class ApplicationEvidenceLimits
{
    public const int MaximumStoredContentBytes = 15 * 1024 * 1024;
}

/// <summary>
/// Shared decision rule: safety-relevant facts are never silently accepted;
/// only high-confidence facts from first-party or regulatory evidence may skip review.
/// </summary>
public static class ApplicationEvidenceReviewPolicy
{
    public const decimal TrustedAutoAcceptConfidence = 0.95m;

    public static bool RequiresHumanReview(
        bool isSafetyCritical,
        decimal confidence,
        ApplicationEvidenceTrustLevel trustLevel)
        => isSafetyCritical
            || confidence < TrustedAutoAcceptConfidence
            || trustLevel is not (ApplicationEvidenceTrustLevel.Manufacturer
                or ApplicationEvidenceTrustLevel.RegulatoryAuthority
                or ApplicationEvidenceTrustLevel.MachineReported);
}
