namespace AmsRecords.FieldMeasurements;

public static class FieldMeasurementDtos
{
    public sealed record MeasurementTimelineQueryDto(
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId);

    public sealed record ClippingSurfaceDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("surfaceAreaM2")] decimal SurfaceAreaM2 = 0m);

    public sealed record ClippingVolumeCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("volume")] decimal Volume,
        [property: JsonPropertyName("inputUnitPubId")] Guid? InputUnitPubId = null);

    public sealed record ClippingVolumeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("volume")] decimal Volume,
        [property: JsonPropertyName("sampleAreaM2")] decimal? SampleAreaM2 = null,
        [property: JsonPropertyName("yieldMlPerM2")] decimal? YieldMlPerM2 = null);

    public sealed record ClippingVolumeTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("clippingSurfaces")] IReadOnlyList<ClippingSurfaceDto> ClippingSurfaces,
        [property: JsonPropertyName("measurements")] IReadOnlyList<ClippingVolumeDto> Measurements,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId = null,
        [property: JsonPropertyName("normalizedUnit")] string NormalizedUnit = "mL/m²");

    public sealed record GreenSpeedSurfaceDto(
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("label")] string Label);

    public sealed record GreenSpeedCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("speed")] decimal Speed,
        [property: JsonPropertyName("inputUnitPubId")] Guid? InputUnitPubId = null);

    public sealed record GreenSpeedDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("speed")] decimal Speed);

    public sealed record GreenSpeedTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("greenSpeedSurfaces")] IReadOnlyList<GreenSpeedSurfaceDto> GreenSpeedSurfaces,
        [property: JsonPropertyName("measurements")] IReadOnlyList<GreenSpeedDto> Measurements,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId = null);

    public sealed record CuttingHeightCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("measuredOn")] DateTime MeasuredOn,
        [property: JsonPropertyName("height")] decimal Height,
        [property: JsonPropertyName("inputUnitPubId")] Guid? InputUnitPubId = null);

    public sealed record CuttingHeightDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("measuredOn")] DateTime MeasuredOn,
        [property: JsonPropertyName("height")] decimal Height);

    public sealed record CuttingHeightTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("measurements")] IReadOnlyList<CuttingHeightDto> Measurements,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId = null);

    public sealed record CuttingHeightAreaLatestDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("measurementPubId")] Guid? MeasurementPubId,
        [property: JsonPropertyName("measuredOn")] DateTime? MeasuredOn,
        [property: JsonPropertyName("height")] decimal? Height);

    public sealed record CuttingHeightLatestByAreaDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("areas")] IReadOnlyList<CuttingHeightAreaLatestDto> Areas,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId = null);
}
