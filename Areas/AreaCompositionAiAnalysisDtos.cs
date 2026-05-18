namespace AmsRecords.Areas;

public static class AreaCompositionAiAnalysisDtos
{
    public sealed record AreaCompositionAiAnalysisDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("photoCount")] int PhotoCount,
        [property: JsonPropertyName("readyForAnalysis")] bool ReadyForAnalysis,
        [property: JsonPropertyName("modelUsed")] string? ModelUsed,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("suitabilitySummary")] string SuitabilitySummary,
        [property: JsonPropertyName("weedCoveragePercent")] decimal WeedCoveragePercent,
        [property: JsonPropertyName("species")] IReadOnlyList<AreaCompositionAiSpeciesEstimateDto> Species,
        [property: JsonPropertyName("weeds")] IReadOnlyList<AreaCompositionAiWeedEstimateDto> Weeds,
        [property: JsonPropertyName("images")] IReadOnlyList<AreaCompositionAiImageSuitabilityDto> Images
    );

    public sealed record AreaCompositionAiSpeciesEstimateDto(
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("notes")] string? Notes
    );

    public sealed record AreaCompositionAiWeedEstimateDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("notes")] string? Notes
    );

    public sealed record AreaCompositionAiImageSuitabilityDto(
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("suitable")] bool Suitable,
        [property: JsonPropertyName("rating")] string Rating,
        [property: JsonPropertyName("issues")] IReadOnlyList<string> Issues
    );
}
