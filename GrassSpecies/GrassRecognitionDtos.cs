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
        "reopen" => "Reopen annual survey", "apply" => "Apply approved area composition", _ => value
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

    public sealed record Photo(Guid PhotoPubId, string Role, string SampleKey, Guid? IdentificationPubId = null, int? IdentificationVersion = null);
    public sealed record Submit(Guid AreaPubId, Guid? SurfacePubId, string Mode, Guid IdempotencyKey,
        IReadOnlyList<Photo> Photos, IReadOnlyList<string>? UnavailableFeatures = null);
    public sealed record Candidate(Guid? SpeciesPubId, string Name, string Rank, decimal ModelScore,
        decimal? Percent, string Evidence, string Conflicts);
    public sealed record Result(string Outcome, string Summary, IReadOnlyList<Candidate> Candidates,
        IReadOnlyList<string> RetakeInstructions, decimal? WeedCoveragePercent, string Limitations,
        IReadOnlyList<PhotoQuality>? PhotoQuality = null);
    public sealed record PhotoQuality(Guid PhotoPubId, string Status, string Guidance, decimal? SharpnessIndicator, decimal? MeanExposure);
    public sealed record Observation(Guid PubId, Guid AreaPubId, Guid? SurfacePubId, string Mode,
        string Status, string ReviewStatus, int Version, DateTime CreatedAtUtc, Result? Result,
        Result? ReviewedResult, string? ReviewNote, DateTime? ApprovedAtUtc, bool CanReview,
        IReadOnlyList<Photo> Photos, DateTime? AppliedAtUtc = null, DateTime? ReopenedAtUtc = null);
    public sealed record Review(int ExpectedVersion, string Action, string Reason, Result? Correction = null);
    public sealed record ReviewPage(IReadOnlyList<Observation> Observations, int? NextSkip);
    public sealed record EvidenceImage(byte[] Bytes, string MimeType);
    public sealed record AggregateSpecies(Guid? SpeciesPubId, string Name, decimal Percent);
    public sealed record Aggregate(Guid AreaPubId, int ApprovedSurfaces, int TotalSurfaces,
        decimal? SurveyedAreaPercent, bool IsAreaWeighted, IReadOnlyList<AggregateSpecies> Species,
        string Limitation);

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
        return null;
    }
}
