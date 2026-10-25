namespace AmsRecords.GrassSpecies;

public static class GrassRecognition
{
    public const string Identification = "identification";
    public const string Composition = "composition";
    public const string Version = "grass-evidence-v1";
    public const decimal MinimumModelScore = 0.70m;
    public static string DisplayLabel(string value) => value switch {
        Identification => "Identify a grass plant", Composition => "Estimate grass composition",
        "pending" => "Queued for analysis", "running" => "Analyzing photographs", "completed" => "Analysis complete", "failed" => "Analysis failed",
        "unreviewed" => "Awaiting supervisor review", "approved" => "Approved", "needs_evidence" => "More evidence requested", "rejected" => "Rejected",
        "approve" => "Approve estimate", "request_evidence" => "Request more evidence", "reject" => "Reject",
        "reopen" => "Reopen annual survey", "apply" => "Apply approved area composition",
        "species" => "Species", "genus" => "Genus", "species_group" => "Species group",
        "functional_group" => "Unresolved morphotype", "unknown" => "Unidentified grass", _ => value
    };
    public static readonly string[] PhotoRoles = ["sample", "overview", "leaf", "collar", "seedhead", "growth"];
    public static string CaptureGuidance(string role) => role switch {
        "sample" => "Photograph a representative patch from above. Spread locations across the target, including visibly different patches. Use the same location key for its close-ups.",
        "overview" => "Show the whole plant and how its shoots grow. Keep the plant separate from neighboring plants in the frame.",
        "leaf" => "Photograph the leaf blade and tip in focus, then its underside if useful. Include a small ruler for scale without covering the leaf.",
        "collar" => "Show where the leaf blade meets the stem. Gently expose the ligule and any auricles without tearing them. Mark this feature unavailable if it cannot be seen.",
        "seedhead" => "Photograph a mature seedhead on the same plant, if present. Do not substitute a seedhead from a neighboring plant.",
        "growth" => "Show shoots, runners or rhizomes from the same plant when accessible. Do not guess hidden structures.",
        _ => "Choose the view you are photographing."
    };

    public sealed record Photo(
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("sampleKey")] string SampleKey,
        [property: JsonPropertyName("identificationPubId")] Guid? IdentificationPubId = null,
        [property: JsonPropertyName("identificationVersion")] int? IdentificationVersion = null);
    public sealed record Capability([property: JsonPropertyName("enabled")] bool Enabled);
    public sealed record Submit(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("mode")] string Mode,
        [property: JsonPropertyName("idempotencyKey")] Guid IdempotencyKey,
        [property: JsonPropertyName("photos")] IReadOnlyList<Photo> Photos,
        [property: JsonPropertyName("unavailableFeatures")] IReadOnlyList<string>? UnavailableFeatures = null)
    {
        [JsonPropertyName("pointSampling")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GrassCanopyPointDtos.SamplingPlan? PointSampling { get; init; }
    }
    public sealed record Candidate(
        [property: JsonPropertyName("speciesPubId")] Guid? SpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("rank")] string Rank,
        [property: JsonPropertyName("modelScore")] decimal ModelScore,
        [property: JsonPropertyName("percent")] decimal? Percent,
        [property: JsonPropertyName("evidence")] string Evidence,
        [property: JsonPropertyName("conflicts")] string Conflicts);
    public sealed record Result(
        [property: JsonPropertyName("outcome")] string Outcome,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("candidates")] IReadOnlyList<Candidate> Candidates,
        [property: JsonPropertyName("retakeInstructions")] IReadOnlyList<string> RetakeInstructions,
        [property: JsonPropertyName("weedCoveragePercent")] decimal? WeedCoveragePercent,
        [property: JsonPropertyName("limitations")] string Limitations,
        [property: JsonPropertyName("photoQuality")] IReadOnlyList<PhotoQuality>? PhotoQuality = null)
    {
        [JsonPropertyName("analysisEvidence")]
        public GrassAnalysisEvidenceDtos.Evidence? AnalysisEvidence { get; init; }

        [JsonPropertyName("pointCoverage")]
        public GrassCanopyPointDtos.Analysis? PointCoverage { get; init; }
    }
    public sealed record PhotoQuality(
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("guidance")] string Guidance,
        [property: JsonPropertyName("sharpnessIndicator")] decimal? SharpnessIndicator,
        [property: JsonPropertyName("meanExposure")] decimal? MeanExposure);
    public sealed record Observation(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("mode")] string Mode,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("reviewStatus")] string ReviewStatus,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("result")] Result? Result,
        [property: JsonPropertyName("reviewedResult")] Result? ReviewedResult,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote,
        [property: JsonPropertyName("approvedAtUtc")] DateTime? ApprovedAtUtc,
        [property: JsonPropertyName("canReview")] bool CanReview,
        [property: JsonPropertyName("photos")] IReadOnlyList<Photo> Photos,
        [property: JsonPropertyName("appliedAtUtc")] DateTime? AppliedAtUtc = null,
        [property: JsonPropertyName("reopenedAtUtc")] DateTime? ReopenedAtUtc = null);
    public sealed record Review(
        [property: JsonPropertyName("expectedVersion")] int ExpectedVersion,
        [property: JsonPropertyName("action")] string Action,
        [property: JsonPropertyName("reason")] string Reason,
        [property: JsonPropertyName("correction")] Result? Correction = null);
    public sealed record ReviewPage(
        [property: JsonPropertyName("observations")] IReadOnlyList<Observation> Observations,
        [property: JsonPropertyName("nextSkip")] int? NextSkip);
    public sealed record EvidenceImage(
        [property: JsonPropertyName("bytes")] byte[] Bytes,
        [property: JsonPropertyName("mimeType")] string MimeType);
    public static bool MatchesSubmission(Observation? saved, Submit request)
        => saved is not null && saved.PubId != Guid.Empty && saved.AreaPubId == request.AreaPubId
            && saved.SurfacePubId == request.SurfacePubId && saved.Mode == request.Mode
            && saved.Photos is not null && saved.Photos.Count == request.Photos.Count
            && request.Photos.ToHashSet().SetEquals(saved.Photos);
    public static string TaxonKey(Guid? speciesPubId, string? rank, string? name)
        => speciesPubId is Guid id ? "species:" + id.ToString("D")
            : rank is "genus" or "species_group" or "functional_group" && !string.IsNullOrWhiteSpace(name)
                ? rank + ":" + NormalizeTaxonName(rank, name).ToUpperInvariant() : "unknown";

    // Only remove explicit unspecified-species notation or a matching common-name
    // gloss. Never turn an ambiguous common name, species or competing genera into
    // a genus claim merely to combine rows or obtain a benchmark match.
    public static string NormalizeTaxonName(string? rank, string? name)
    {
        var value = (name ?? "").Trim();
        if (rank != "genus" || value.Length == 0) return value;
        var space = 0;
        while (space < value.Length && !char.IsWhiteSpace(value[space])) space++;
        var genus = value[..space];
        if (!genus.All(char.IsAsciiLetter)) return value;
        var suffix = value[space..].Trim().ToLowerInvariant();
        var common = genus.ToLowerInvariant() switch {
            "poa" => "bluegrass", "lolium" => "ryegrass", "agrostis" => "bentgrass", "festuca" => "fescue", _ => null
        };
        var unspecified = suffix is "" or "sp" or "sp." or "spp" or "spp." or "species";
        var matchingGloss = common is not null && (suffix == $"({common})" || suffix == $"sp. ({common})"
            || suffix == $"sp ({common})" || suffix == $"spp. ({common})" || suffix == $"spp ({common})");
        if (!unspecified && !matchingGloss) return value;
        return char.ToUpperInvariant(genus[0]) + genus[1..].ToLowerInvariant();
    }
    public sealed record AggregateSpecies(
        [property: JsonPropertyName("speciesPubId")] Guid? SpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent);
    public sealed record Aggregate(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("approvedSurfaces")] int ApprovedSurfaces,
        [property: JsonPropertyName("totalSurfaces")] int TotalSurfaces,
        [property: JsonPropertyName("surveyedAreaPercent")] decimal? SurveyedAreaPercent,
        [property: JsonPropertyName("isAreaWeighted")] bool IsAreaWeighted,
        [property: JsonPropertyName("species")] IReadOnlyList<AggregateSpecies> Species,
        [property: JsonPropertyName("limitation")] string Limitation);

    public static bool HasIdentificationEvidence(decimal score, int usablePhotos, bool hasCandidate)
        => score >= MinimumModelScore && usablePhotos >= 3 && hasCandidate;

    public static string? Validate(Submit request)
    {
        if (request.AreaPubId == Guid.Empty || request.IdempotencyKey == Guid.Empty)
            return "An area and submission identifier are required.";
        if (request.Mode is not (Identification or Composition)) return "Choose an analysis mode.";
        if (request.Photos is null || request.Photos.Count is < 1 or > 15) return "Use between 1 and 15 photos.";
        if (request.Photos.Any(x => x is null)) return "Each photo must be valid.";
        if (request.Photos.Select(x => x.PhotoPubId).Distinct().Count() != request.Photos.Count)
            return "Each photograph may be submitted only once.";
        if (request.Photos.Any(x => x.PhotoPubId == Guid.Empty || !PhotoRoles.Contains(x.Role)
            || string.IsNullOrWhiteSpace(x.SampleKey) || x.SampleKey.Length > 64))
            return "Each photo needs a valid role and sample location.";
        if (request.Mode == Identification && request.Photos.Select(x => x.SampleKey).Distinct().Count() != 1)
            return "Identify one plant per submission. All diagnostic views must use the same plant key.";
        if (request.Mode == Composition && request.Photos.Any(x => x.Role != "sample"
            && !request.Photos.Any(sample => sample.Role == "sample" && sample.SampleKey == x.SampleKey)))
            return "Associate each diagnostic view with a representative sample location.";
        if (request.Photos.Any(x => x.IdentificationPubId.HasValue && (request.Mode != Composition || x.Role == "sample" || x.IdentificationVersion is null or < 1
            || !request.Photos.Any(sample => sample.Role == "sample" && sample.SampleKey == x.SampleKey))))
            return "Link diagnostic evidence to an existing representative sample location.";
        if (request.UnavailableFeatures?.Count > PhotoRoles.Length || request.UnavailableFeatures?.Any(x => x is null || x.Length > 64 || !PhotoRoles.Contains(x)) == true)
            return "Choose a supported unavailable feature.";
        if (request.Mode == Composition && request.Photos.Where(x => x.Role == "sample")
            .Select(x => x.SampleKey).Distinct().Count() < 5)
            return "Composition needs at least five representative sample locations.";
        if (request.Mode == Identification && new[] { "overview", "leaf", "collar" }.Any(role =>
            !request.Photos.Any(x => x.Role == role) && request.UnavailableFeatures?.Contains(role) != true))
            return "Add overview, leaf and collar views, or mark the unavailable feature.";
        if (request.PointSampling is { } sampling) {
            if (request.Mode != Composition) return "Point coverage is available for composition observations.";
            if (request.Photos.Count(x => x.Role == "sample") > 10) return "Use at most ten sampling photographs, plus optional diagnostic views.";
            if (GrassCanopyPointDtos.ValidateSampling(sampling, request.Photos.Where(x => x.Role == "sample").Select(x => x.SampleKey)) is { } samplingError)
                return samplingError;
        }
        return null;
    }
}
