using AmsModels;

namespace AmsRecords.FieldZones;

public static class FieldZoneExtensions
{
    public static FieldZoneDto ToDto(this FieldZone zone)
        => new(
            zone.PubId,
            zone.Field.PubId,
            zone.ZoneName,
            zone.ZoneType,
            zone.GeoJson,
            zone.Active);

    public static FieldZoneSatelliteSnapshotDto ToDto(this FieldZoneSatelliteSnapshot snapshot)
        => new(
            snapshot.PubId,
            snapshot.FieldZone.PubId,
            snapshot.CapturedUtc,
            snapshot.Source,
            snapshot.NdviMean,
            snapshot.NdviMin,
            snapshot.NdviMax,
            snapshot.NdviStdDev,
            snapshot.NdreMean,
            snapshot.MoistureIndexMean,
            snapshot.CloudCoveragePercent);

    public static SatelliteStressScanRunDto ToDto(this SatelliteStressScanRun run)
        => new(
            run.PubId,
            run.Field.PubId,
            run.StartedUtc,
            run.CompletedUtc,
            run.Trigger,
            run.Status,
            run.RequestCount,
            run.ProcessingUnitsEstimate,
            run.SceneId,
            run.Message);

    public static FieldMaskDto ToDto(this FieldMask mask)
        => new(
            mask.PubId,
            mask.Field.PubId,
            mask.MaskName,
            mask.MaskType,
            mask.Reason,
            mask.GeoJson,
            mask.Active);
}
