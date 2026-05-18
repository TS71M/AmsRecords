namespace AmsRecords.Areas;

public static class AreaCompositionHistoryDtos
{
    public sealed record AreaCompositionHistoryDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("series")] IReadOnlyList<AreaCompositionHistorySeriesDto> Series
    );

    public sealed record AreaCompositionHistorySeriesDto(
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("currentPercent")] decimal CurrentPercent,
        [property: JsonPropertyName("points")] IReadOnlyList<AreaCompositionHistoryPointDto> Points
    );

    public sealed record AreaCompositionHistoryPointDto(
        [property: JsonPropertyName("validFromUtc")] DateTime ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTime? ValidToUtc,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("isCurrent")] bool IsCurrent
    );
}
