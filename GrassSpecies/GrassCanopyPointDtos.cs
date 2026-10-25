namespace AmsRecords.GrassSpecies;

// Point locations are server-owned. Labels are interpretations, not verified botany.
public static class GrassCanopyPointDtos
{
    public sealed record LocationWeight(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("representedAreaM2")] decimal RepresentedAreaM2);

    public sealed record SamplingPlan(
        [property: JsonPropertyName("mode")] string Mode,
        [property: JsonPropertyName("locations")] IReadOnlyList<LocationWeight> Locations);

    public sealed record Analysis(
        [property: JsonPropertyName("version")] string Version,
        [property: JsonPropertyName("sampling")] SamplingPlan Sampling,
        [property: JsonPropertyName("photos")] IReadOnlyList<Estimate> Photos,
        [property: JsonPropertyName("survey")] Survey? Survey)
    {
        [JsonPropertyName("inputs")]
        public IReadOnlyList<BatchInputs> Inputs { get; init; } = [];
        [JsonPropertyName("complete")]
        public bool Complete { get; init; }
        [JsonPropertyName("photoRoles")]
        public IReadOnlyList<GrassRecognition.Photo> PhotoRoles { get; init; } = [];
    }

    public sealed record BatchInputs(
        [property: JsonPropertyName("modelUsed")] string ModelUsed,
        [property: JsonPropertyName("evidence")] GrassAnalysisEvidenceDtos.Evidence Evidence);

    public static string? ValidateSampling(SamplingPlan plan, IEnumerable<string> sampleKeys)
    {
        var keys = sampleKeys.Distinct(StringComparer.Ordinal).ToHashSet(StringComparer.Ordinal);
        if (plan.Mode is not ("per_photo" or "equal_locations" or "area_strata") || plan.Locations is null || plan.Locations.Count > 15)
            return "Choose per-photo results, equal representative locations, or area strata.";
        if (plan.Mode != "area_strata") return plan.Locations.Count == 0 ? null : "Area weights require area-stratum sampling.";
        if (plan.Locations.Count != keys.Count || plan.Locations.Any(x => x is null || !keys.Contains(x.Key)
                || x.RepresentedAreaM2 is <= 0 or > 100_000_000) || plan.Locations.Select(x => x.Key).Distinct(StringComparer.Ordinal).Count() != keys.Count)
            return "Supply one positive represented area for every sample location.";
        return null;
    }

    public sealed record Point(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("x")] int X,
        [property: JsonPropertyName("y")] int Y,
        [property: JsonPropertyName("cropX")] int CropX,
        [property: JsonPropertyName("cropY")] int CropY,
        [property: JsonPropertyName("cropSize")] int CropSize,
        [property: JsonPropertyName("tileX")] int TileX,
        [property: JsonPropertyName("tileY")] int TileY);

    public sealed record Plan(
        [property: JsonPropertyName("version")] string Version,
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("sourceHash")] string SourceHash,
        [property: JsonPropertyName("sourceWidth")] uint SourceWidth,
        [property: JsonPropertyName("sourceHeight")] uint SourceHeight,
        [property: JsonPropertyName("atlasHash")] string AtlasHash,
        [property: JsonPropertyName("points")] IReadOnlyList<Point> Points);

    public sealed record Label(
        [property: JsonPropertyName("pointId")] int PointId,
        [property: JsonPropertyName("coverClass")] string CoverClass,
        [property: JsonPropertyName("morphotypeKey")] string? MorphotypeKey = null);

    public sealed record Morphotype(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("speciesPubId")] Guid? SpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("rank")] string Rank,
        [property: JsonPropertyName("identificationScore")] decimal IdentificationScore,
        [property: JsonPropertyName("visibleEvidence")] string VisibleEvidence,
        [property: JsonPropertyName("alternatives")] string Alternatives);

    public sealed record Interpretation(
        [property: JsonPropertyName("morphotypes")] IReadOnlyList<Morphotype> Morphotypes,
        [property: JsonPropertyName("labels")] IReadOnlyList<Label> Labels);

    public sealed record Coverage(
        [property: JsonPropertyName("morphotype")] Morphotype Morphotype,
        [property: JsonPropertyName("hits")] int Hits,
        [property: JsonPropertyName("grassPercent")] decimal GrassPercent,
        [property: JsonPropertyName("minimumWithUnassessed")] decimal MinimumWithUnassessed,
        [property: JsonPropertyName("maximumWithUnassessed")] decimal MaximumWithUnassessed);

    public sealed record Estimate(
        [property: JsonPropertyName("plan")] Plan Plan,
        [property: JsonPropertyName("labels")] IReadOnlyList<Label> Labels,
        [property: JsonPropertyName("grassPoints")] int GrassPoints,
        [property: JsonPropertyName("weedPoints")] int WeedPoints,
        [property: JsonPropertyName("nonVegetationPoints")] int NonVegetationPoints,
        [property: JsonPropertyName("unassessedPoints")] int UnassessedPoints,
        [property: JsonPropertyName("coverage")] IReadOnlyList<Coverage> Coverage,
        [property: JsonPropertyName("issues")] IReadOnlyList<string> Issues);

    public sealed record Location(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("representedAreaM2")] decimal? RepresentedAreaM2,
        [property: JsonPropertyName("photos")] IReadOnlyList<Estimate> Photos)
    {
        [JsonPropertyName("unavailablePhotos")]
        public int UnavailablePhotos { get; init; }
    }

    public sealed record SurveyCoverage(
        [property: JsonPropertyName("morphotype")] Morphotype Morphotype,
        [property: JsonPropertyName("grassPercent")] decimal GrassPercent,
        [property: JsonPropertyName("minimumWithUnassessed")] decimal MinimumWithUnassessed,
        [property: JsonPropertyName("maximumWithUnassessed")] decimal MaximumWithUnassessed);

    public sealed record Survey(
        [property: JsonPropertyName("samplingBasis")] string SamplingBasis,
        [property: JsonPropertyName("locationCount")] int LocationCount,
        [property: JsonPropertyName("distinctPhotoCount")] int DistinctPhotoCount,
        [property: JsonPropertyName("unassessedSurfacePercent")] decimal? UnassessedSurfacePercent,
        [property: JsonPropertyName("coverage")] IReadOnlyList<SurveyCoverage> Coverage,
        [property: JsonPropertyName("limitations")] IReadOnlyList<string> Limitations)
    {
        [JsonPropertyName("weedSurfacePercent")]
        public decimal? WeedSurfacePercent { get; init; }
        [JsonPropertyName("grassSurfacePercent")]
        public decimal? GrassSurfacePercent { get; init; }
    }
}
