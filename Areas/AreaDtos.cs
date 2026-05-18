namespace AmsRecords.Areas;

public static class AreaDtos
{
    public sealed record AreaIndexDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("hasSurface")] bool HasSurface,
        [property: JsonPropertyName("planning")] bool Planning,
        [property: JsonPropertyName("irrigated")] bool Irrigated,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("areaGroupPubId")] Guid AreaGroupPubId,
        [property: JsonPropertyName("areaGroupName")] string AreaGroupName,
        [property: JsonPropertyName("areaGroupCode")] string AreaGroupCode
    );

    public sealed record AreaDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("planning")] bool Planning,
        [property: JsonPropertyName("clippingsRemoved")] bool ClippingsRemoved,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("nMax")] decimal NMax,
        [property: JsonPropertyName("tempOpt")] decimal TempOpt,
        [property: JsonPropertyName("etThreshold")] decimal EtThreshold,
        [property: JsonPropertyName("irrigated")] bool Irrigated,
        [property: JsonPropertyName("report")] bool Report,
        [property: JsonPropertyName("whc")] decimal Whc,
        [property: JsonPropertyName("hasSurface")] bool HasSurface,
        [property: JsonPropertyName("greenSpeed")] bool GreenSpeed,
        [property: JsonPropertyName("baseTemp")] int BaseTemp,
        [property: JsonPropertyName("gddDays")] int GddDays,
        [property: JsonPropertyName("dollarSpotMedium")] int DollarSpotMedium,
        [property: JsonPropertyName("dollarSpotHigh")] int DollarSpotHigh,
        [property: JsonPropertyName("areaGroupPubId")] Guid AreaGroupPubId,
        [property: JsonPropertyName("areaGroupName")] string AreaGroupName,
        [property: JsonPropertyName("areaGroupCode")] string AreaGroupCode,
        [property: JsonPropertyName("grassSpecies")] IReadOnlyList<AreaCompositionDtos.AreaGrassSpeciesRowDto> GrassSpecies,
        [property: JsonPropertyName("soilTypes")] IReadOnlyList<AreaCompositionDtos.AreaSoilTypeRowDto> SoilTypes
    );

    public sealed record AreaCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaGroupPubId")] Guid AreaGroupPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("planning")] bool Planning,
        [property: JsonPropertyName("clippingsRemoved")] bool ClippingsRemoved,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("nMax")] decimal NMax,
        [property: JsonPropertyName("tempOpt")] decimal TempOpt,
        [property: JsonPropertyName("etThreshold")] decimal EtThreshold,
        [property: JsonPropertyName("irrigated")] bool Irrigated,
        [property: JsonPropertyName("report")] bool Report,
        [property: JsonPropertyName("whc")] decimal Whc,
        [property: JsonPropertyName("hasSurface")] bool HasSurface,
        [property: JsonPropertyName("greenSpeed")] bool GreenSpeed,
        [property: JsonPropertyName("baseTemp")] int BaseTemp,
        [property: JsonPropertyName("gddDays")] int GddDays,
        [property: JsonPropertyName("dollarSpotMedium")] int DollarSpotMedium,
        [property: JsonPropertyName("dollarSpotHigh")] int DollarSpotHigh
    );

    public sealed record AreaUpdateDto(
        [property: JsonPropertyName("areaGroupPubId")] Guid AreaGroupPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("planning")] bool Planning,
        [property: JsonPropertyName("clippingsRemoved")] bool ClippingsRemoved,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("nMax")] decimal NMax,
        [property: JsonPropertyName("tempOpt")] decimal TempOpt,
        [property: JsonPropertyName("etThreshold")] decimal EtThreshold,
        [property: JsonPropertyName("irrigated")] bool Irrigated,
        [property: JsonPropertyName("report")] bool Report,
        [property: JsonPropertyName("whc")] decimal Whc,
        [property: JsonPropertyName("hasSurface")] bool HasSurface,
        [property: JsonPropertyName("greenSpeed")] bool GreenSpeed,
        [property: JsonPropertyName("baseTemp")] int BaseTemp,
        [property: JsonPropertyName("gddDays")] int GddDays,
        [property: JsonPropertyName("dollarSpotMedium")] int DollarSpotMedium,
        [property: JsonPropertyName("dollarSpotHigh")] int DollarSpotHigh
    );

    public sealed record AreaMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("hasSurface")] bool HasSurface,
        [property: JsonPropertyName("areaGroupCode")] string AreaGroupCode
    );
}
