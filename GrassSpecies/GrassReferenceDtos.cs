namespace AmsRecords.GrassSpecies;

public static class GrassReferenceDtos
{
    public sealed record Metadata(
        [property: JsonPropertyName("sourceUrl")] string SourceUrl = "",
        [property: JsonPropertyName("sourceOrganization")] string SourceOrganization = "",
        [property: JsonPropertyName("attribution")] string Attribution = "",
        [property: JsonPropertyName("license")] string License = "",
        [property: JsonPropertyName("licenseUrl")] string LicenseUrl = "",
        [property: JsonPropertyName("verificationMethod")] string VerificationMethod = "",
        [property: JsonPropertyName("verificationEvidence")] string VerificationEvidence = "",
        [property: JsonPropertyName("morphology")] string Morphology = "",
        [property: JsonPropertyName("cultivar")] string Cultivar = "",
        [property: JsonPropertyName("mowingHeightMm")] decimal? MowingHeightMm = null,
        [property: JsonPropertyName("captureConditions")] string CaptureConditions = "",
        [property: JsonPropertyName("season")] string Season = "",
        [property: JsonPropertyName("surfaceUse")] string SurfaceUse = "",
        [property: JsonPropertyName("provenanceGroup")] string ProvenanceGroup = "",
        [property: JsonPropertyName("coverageMethod")] string CoverageMethod = "");

    public sealed record Taxon(
        [property: JsonPropertyName("speciesPubId")] Guid SpeciesPubId,
        [property: JsonPropertyName("measuredCanopyPercent")] decimal? MeasuredCanopyPercent = null,
        [property: JsonPropertyName("scientificName")] string? ScientificName = null);

    public sealed record Reference(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("speciesPubId")] Guid SpeciesPubId,
        [property: JsonPropertyName("speciesName")] string SpeciesName,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("kind")] string Kind,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("partition")] string Partition,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("contentHash")] string? ContentHash,
        [property: JsonPropertyName("metadata")] Metadata Metadata,
        [property: JsonPropertyName("taxa")] IReadOnlyList<Taxon> Taxa);

    public sealed record Update(
        [property: JsonPropertyName("expectedVersion")] int ExpectedVersion,
        [property: JsonPropertyName("kind")] string Kind,
        [property: JsonPropertyName("partition")] string Partition,
        [property: JsonPropertyName("metadata")] Metadata Metadata,
        [property: JsonPropertyName("taxa")] IReadOnlyList<Taxon> Taxa);

    public sealed record Verify(
        [property: JsonPropertyName("expectedVersion")] int ExpectedVersion,
        [property: JsonPropertyName("expectedContentHash")] string ExpectedContentHash,
        [property: JsonPropertyName("imageInspected")] bool ImageInspected,
        [property: JsonPropertyName("labelConfirmed")] bool LabelConfirmed,
        [property: JsonPropertyName("rightsConfirmed")] bool RightsConfirmed);

    public sealed record Revision(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("action")] string Action,
        [property: JsonPropertyName("actorPubId")] Guid? ActorPubId,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("snapshot")] Reference Snapshot);

    public sealed record Page(
        [property: JsonPropertyName("items")] IReadOnlyList<Reference> Items,
        [property: JsonPropertyName("nextSkip")] int? NextSkip);

    public sealed record Inspection(
        [property: JsonPropertyName("contentHash")] string ContentHash,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height);
}
