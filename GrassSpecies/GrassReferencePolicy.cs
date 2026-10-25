using static AmsRecords.GrassSpecies.GrassReferenceDtos;

namespace AmsRecords.GrassSpecies;

public static class GrassReferencePolicy
{
    public const string Unverified = "unverified";
    public const string SourceLabelled = "source_labelled";
    public const string Verified = "verified";
    public const string ReferencePartition = "reference";
    public static readonly IReadOnlyList<string> Kinds = ["uncategorized", "leaf_blade", "turf_canopy", "supporting"];
    public static readonly IReadOnlyList<string> Partitions = ["reference", "calibration", "holdout"];
    public static readonly IReadOnlyList<string> VerificationMethods = ["authoritative_publication", "expert_confirmation"];
    public static readonly IReadOnlyList<string> Licenses = ["CC0-1.0", "CC-BY-2.0", "CC-BY-3.0", "CC-BY-4.0", "CC-BY-SA-2.0", "CC-BY-SA-3.0", "CC-BY-SA-4.0", "public_domain", "permission"];

    public static string Label(string value) => value switch {
        "uncategorized" => "Uncategorized", "leaf_blade" => "Leaf blade", "turf_canopy" => "Turf canopy",
        "supporting" => "Supporting botanical view", "unverified" => "Unverified", "source_labelled" => "Source-labelled",
        "verified" => "Verified", "reference" => "Reference", "calibration" => "Calibration", "holdout" => "Holdout",
        "authoritative_publication" => "Authoritative publication", "expert_confirmation" => "Expert confirmation",
        "public_domain" => "Public domain", "permission" => "Written permission", "metadata_updated" => "Metadata updated",
        "retired" => "Retired", "reactivated" => "Reactivated", _ => value
    };

    public static string? Validate(Update request, bool forVerification = false)
    {
        if (request.ExpectedVersion < 0 || !Kinds.Contains(request.Kind) || !Partitions.Contains(request.Partition))
            return "Choose a supported reference category and dataset partition.";
        if (request.Metadata is not { } m || request.Taxa is null || request.Taxa.Count is < 1 or > 12
            || request.Taxa.Any(x => x is null || x.SpeciesPubId == Guid.Empty)
            || request.Taxa.Select(x => x.SpeciesPubId).Distinct().Count() != request.Taxa.Count)
            return "Associate between one and twelve distinct grass species.";
        foreach (var (value, limit) in new (string?, int)[] {
            (m.SourceUrl, 2000), (m.LicenseUrl, 2000), (m.SourceOrganization, 250), (m.Attribution, 1000),
            (m.License, 80), (m.VerificationMethod, 40), (m.VerificationEvidence, 2000), (m.Morphology, 2000),
            (m.Cultivar, 250), (m.CaptureConditions, 1000), (m.Season, 120), (m.SurfaceUse, 120),
            (m.ProvenanceGroup, 120), (m.CoverageMethod, 1000) })
            if (value is null || value.Length > limit) return "Reference metadata is missing or exceeds its field length limit.";
        if (m.MowingHeightMm is < 0 or > 2000) return "Mowing height must be between zero and 2000 mm.";
        if ((!string.IsNullOrWhiteSpace(m.SourceUrl) && !PublicationUrl(m.SourceUrl))
            || (!string.IsNullOrWhiteSpace(m.LicenseUrl) && !PublicationUrl(m.LicenseUrl)))
            return "Source and license URLs must use HTTPS without embedded credentials.";
        if (request.Kind == "leaf_blade" && request.Taxa.Count != 1)
            return "A leaf-blade reference must identify one species.";
        if (request.Taxa.Any(x => x.MeasuredCanopyPercent is < 0 or > 100))
            return "Measured canopy percentages must be between zero and 100.";
        if (request.Taxa.Any(x => x.MeasuredCanopyPercent.HasValue)
            && (request.Kind != "turf_canopy" || request.Taxa.Any(x => !x.MeasuredCanopyPercent.HasValue)
                || Math.Abs(request.Taxa.Sum(x => x.MeasuredCanopyPercent ?? 0) - 100) > .01m
                || string.IsNullOrWhiteSpace(m.CoverageMethod)))
            return "Measured composition requires a canopy image, a measurement method and complete shares totalling 100%.";
        if (forVerification && (request.Kind == "uncategorized" || (m.VerificationMethod == "authoritative_publication" && !PublicationUrl(m.SourceUrl))
            || string.IsNullOrWhiteSpace(m.SourceOrganization) || string.IsNullOrWhiteSpace(m.Attribution)
            || !Licenses.Contains(m.License) || (m.License.StartsWith("CC", StringComparison.Ordinal) && !PublicationUrl(m.LicenseUrl))
            || !VerificationMethods.Contains(m.VerificationMethod) || string.IsNullOrWhiteSpace(m.VerificationEvidence)
            || string.IsNullOrWhiteSpace(m.Morphology) || string.IsNullOrWhiteSpace(m.ProvenanceGroup)))
            return "Verification requires image category, source, attribution, permitted-use evidence, confirmation method, botanical evidence, morphology and provenance group.";
        return null;
    }

    static bool PublicationUrl(string? value)
        => Uri.TryCreate(value, UriKind.Absolute, out var url) && url.Scheme == Uri.UriSchemeHttps
            && string.IsNullOrEmpty(url.UserInfo) && !string.IsNullOrWhiteSpace(url.Host);
}
