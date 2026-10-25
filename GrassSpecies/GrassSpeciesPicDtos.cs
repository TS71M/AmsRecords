namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesPicDtos
{
    public sealed record GrassSpeciesPicDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("updatedAtUtc")] DateTime UpdatedAtUtc,

        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,

        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("relativePath")] string? RelativePath
    )
    {
        [JsonPropertyName("isGlobal")]
        public bool IsGlobal { get; init; }
    }

    public sealed record GrassSpeciesPicCreateDto(
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId
    );
}