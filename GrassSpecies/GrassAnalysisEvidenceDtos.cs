namespace AmsRecords.GrassSpecies;

public static class GrassAnalysisEvidenceDtos
{
    public sealed record Evidence(
        [property: JsonPropertyName("version")] string Version,
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("photos")] IReadOnlyList<PhotoInput> Photos,
        [property: JsonPropertyName("detailViews")] IReadOnlyList<DetailView> DetailViews,
        [property: JsonPropertyName("referencesPresented")] IReadOnlyList<ReferenceInput> ReferencesPresented)
    {
        [JsonPropertyName("pointCoverage")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public GrassCanopyPointDtos.Analysis? PointCoverage { get; init; }
    }

    public sealed record PhotoInput(
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("fileName")] string FileName,
        [property: JsonPropertyName("sourceHash")] string SourceHash,
        [property: JsonPropertyName("inputHash")] string InputHash);

    public sealed record DetailView(
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("fileName")] string FileName,
        [property: JsonPropertyName("inputHash")] string InputHash,
        [property: JsonPropertyName("sourceWidth")] uint SourceWidth,
        [property: JsonPropertyName("sourceHeight")] uint SourceHeight,
        [property: JsonPropertyName("x")] int X,
        [property: JsonPropertyName("y")] int Y,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height);

    public sealed record ReferenceInput(
        [property: JsonPropertyName("referencePubId")] Guid ReferencePubId,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("fileName")] string FileName,
        [property: JsonPropertyName("sourceHash")] string SourceHash,
        [property: JsonPropertyName("inputHash")] string InputHash,
        [property: JsonPropertyName("kind")] string Kind,
        [property: JsonPropertyName("taxa")] IReadOnlyList<GrassReferenceDtos.Taxon> Taxa,
        [property: JsonPropertyName("sourceUrl")] string SourceUrl,
        [property: JsonPropertyName("attribution")] string Attribution,
        [property: JsonPropertyName("license")] string License);
}
