using AmsModels;

namespace AmsRecords.FieldZones;

public sealed record FieldZoneDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("zoneName")] string ZoneName,
    [property: JsonPropertyName("zoneType")] FieldZoneType ZoneType,
    [property: JsonPropertyName("geoJson")] string GeoJson,
    [property: JsonPropertyName("active")] bool Active
);

public sealed record FieldZoneSatelliteSnapshotDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("fieldZonePubId")] Guid FieldZonePubId,
    [property: JsonPropertyName("capturedUtc")] DateTime CapturedUtc,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("ndviMean")] decimal? NdviMean,
    [property: JsonPropertyName("ndviMin")] decimal? NdviMin,
    [property: JsonPropertyName("ndviMax")] decimal? NdviMax,
    [property: JsonPropertyName("ndviStdDev")] decimal? NdviStdDev,
    [property: JsonPropertyName("ndreMean")] decimal? NdreMean,
    [property: JsonPropertyName("moistureIndexMean")] decimal? MoistureIndexMean,
    [property: JsonPropertyName("cloudCoveragePercent")] decimal? CloudCoveragePercent
);

public sealed record FieldZoneSatelliteSummaryDto(
    [property: JsonPropertyName("fieldZonePubId")] Guid FieldZonePubId,
    [property: JsonPropertyName("zoneName")] string ZoneName,
    [property: JsonPropertyName("zoneType")] FieldZoneType ZoneType,
    [property: JsonPropertyName("latestCapturedUtc")] DateTime? LatestCapturedUtc,
    [property: JsonPropertyName("latestSource")] string? LatestSource,
    [property: JsonPropertyName("ndviMean")] decimal? NdviMean,
    [property: JsonPropertyName("ndviMin")] decimal? NdviMin,
    [property: JsonPropertyName("ndviMax")] decimal? NdviMax,
    [property: JsonPropertyName("ndviStdDev")] decimal? NdviStdDev,
    [property: JsonPropertyName("ndviVs14DayAveragePercent")] decimal? NdviVs14DayAveragePercent,
    [property: JsonPropertyName("moistureIndexMean")] decimal? MoistureIndexMean,
    [property: JsonPropertyName("moistureSignal")] string MoistureSignal,
    [property: JsonPropertyName("cloudCoveragePercent")] decimal? CloudCoveragePercent,
    [property: JsonPropertyName("confidence")] string Confidence,
    [property: JsonPropertyName("suggestedAction")] string SuggestedAction
);

public sealed record FieldZoneStressRasterOverlayDto(
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("capturedUtc")] DateTime CapturedUtc,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("imageDataUrl")] string ImageDataUrl,
    [property: JsonPropertyName("south")] double South,
    [property: JsonPropertyName("west")] double West,
    [property: JsonPropertyName("north")] double North,
    [property: JsonPropertyName("east")] double East
);

public sealed record FieldMaskDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("maskName")] string MaskName,
    [property: JsonPropertyName("maskType")] FieldMaskType MaskType,
    [property: JsonPropertyName("reason")] FieldMaskReason Reason,
    [property: JsonPropertyName("geoJson")] string GeoJson,
    [property: JsonPropertyName("active")] bool Active
);
