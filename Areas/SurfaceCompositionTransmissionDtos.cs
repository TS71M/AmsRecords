namespace AmsRecords.Areas;

public static class SurfaceCompositionTransmissionDtos
{
    public sealed record SurfaceCompositionTransmissionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("nextAllowedAtUtc")] DateTime NextAllowedAtUtc,
        [property: JsonPropertyName("blocksNewAnalysis")] bool BlocksNewAnalysis,
        [property: JsonPropertyName("photoCount")] int PhotoCount,
        [property: JsonPropertyName("modelUsed")] string? ModelUsed,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("summary")] string? Summary,
        [property: JsonPropertyName("suitabilitySummary")] string? SuitabilitySummary,
        [property: JsonPropertyName("weedCoveragePercent")] decimal WeedCoveragePercent,
        [property: JsonPropertyName("species")] IReadOnlyList<SurfaceCompositionTransmissionSpeciesDto> Species
    );

    public sealed record SurfaceCompositionTransmissionSpeciesDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("notes")] string? Notes
    );
}
