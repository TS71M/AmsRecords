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
        [property: JsonPropertyName("label")] string Label);

    public sealed record ClippingVolumeCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("volume")] decimal Volume);

    public sealed record ClippingVolumeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc,
        [property: JsonPropertyName("volume")] decimal Volume);

    public sealed record ClippingVolumeTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("unit")] string Unit,
        [property: JsonPropertyName("clippingSurfaces")] IReadOnlyList<ClippingSurfaceDto> ClippingSurfaces,
        [property: JsonPropertyName("measurements")] IReadOnlyList<ClippingVolumeDto> Measurements);

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
        [property: JsonPropertyName("speed")] decimal Speed);

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
        [property: JsonPropertyName("measurements")] IReadOnlyList<GreenSpeedDto> Measurements);

    public sealed record CuttingHeightCreateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("measuredOn")] DateTime MeasuredOn,
        [property: JsonPropertyName("height")] decimal Height);

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
        [property: JsonPropertyName("measurements")] IReadOnlyList<CuttingHeightDto> Measurements);

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
        [property: JsonPropertyName("areas")] IReadOnlyList<CuttingHeightAreaLatestDto> Areas);
}
