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
    )
    {
        [JsonPropertyName("imageCompositions")]
        public IReadOnlyList<AreaCompositionAiImageCompositionDto> ImageCompositions { get; init; } = [];
    }

    public sealed record AreaCompositionImageQualityDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("photoCount")] int PhotoCount,
        [property: JsonPropertyName("readyForAnalysis")] bool ReadyForAnalysis,
        [property: JsonPropertyName("modelUsed")] string? ModelUsed,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("retakeInstructions")] IReadOnlyList<string> RetakeInstructions,
        [property: JsonPropertyName("images")] IReadOnlyList<AreaCompositionAiImageSuitabilityDto> Images
    );

    public sealed record AreaCompositionAiSpeciesEstimateDto(
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("chartGroupKey")] string? ChartGroupKey = null,
        [property: JsonPropertyName("chartGroupName")] string? ChartGroupName = null,
        [property: JsonPropertyName("taxonRank")] string? TaxonRank = null,
        [property: JsonPropertyName("isChartable")] bool IsChartable = true
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

    public sealed record AreaCompositionAiImageCompositionDto(
        [property: JsonPropertyName("imageIndex")] int ImageIndex,
        [property: JsonPropertyName("photoPubId")] Guid PhotoPubId,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("suitable")] bool Suitable,
        [property: JsonPropertyName("rating")] string Rating,
        [property: JsonPropertyName("weight")] decimal Weight,
        [property: JsonPropertyName("summary")] string? Summary,
        [property: JsonPropertyName("species")] IReadOnlyList<AreaCompositionAiSpeciesEstimateDto> Species
    );
}
