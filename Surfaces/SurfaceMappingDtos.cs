namespace AmsRecords.Surfaces;

using static AmsRecords.Locations.MapCenterDtos;

public static class SurfaceMappingDtos
{
    public sealed record CoordinateDto(double Latitude, double Longitude);
    public sealed record RingDto(Guid RingId, bool IsExclusion, IReadOnlyList<CoordinateDto> Points);
    public sealed record SubsectionGeometryDto(Guid SubsectionPubId, string Name, IReadOnlyList<RingDto> Rings);
    /// <summary>A mapping vertex only. TDR sensor readings and device payloads belong to the measurement module.</summary>
    public sealed record RawBoundaryPointDto(Guid CaptureId, Guid SubsectionPubId, Guid RingId, bool IsExclusion,
        int PointOrder, double Latitude, double Longitude, double? HorizontalAccuracyMetres,
        string? GpsFixQuality, int? SatelliteCount, bool GpsValid, DateTimeOffset CapturedAtUtc,
        double? PhoneLatitude = null, double? PhoneLongitude = null, double? PhoneHorizontalAccuracyMetres = null,
        DateTimeOffset? PhoneCapturedAtUtc = null, double? PreferredLatitude = null, double? PreferredLongitude = null,
        string PositionSource = "Tdr", double? SourceDistanceMetres = null, bool PositionWarning = false,
        string PositionStatus = "TdrOnly", decimal BoundaryVwcPercent = -1m);
    public sealed record SurfaceMapSyncRequestDto(Guid IdempotencyKey, Guid SurfacePubId,
        IReadOnlyList<SubsectionGeometryDto> Subsections, IReadOnlyList<RawBoundaryPointDto> RawPoints,
        bool CompleteSurface);
    public sealed record SurfaceMapRefineRequestDto(Guid IdempotencyKey,
        IReadOnlyList<SubsectionGeometryDto> Subsections, string? Note);
    public sealed record SurfaceMapApproveRequestDto(Guid RevisionPubId);
    public sealed record SurfaceMapRevisionDto(Guid PubId, Guid SurfacePubId, int RevisionNumber,
        string GrossGeoJson, string DeductionGeoJson, string EffectiveGeoJson, decimal GrossAreaM2,
        decimal DeductionAreaM2, decimal EffectiveAreaM2, string ApprovalState, IReadOnlyList<string> Warnings,
        DateTimeOffset CreatedAtUtc, DateTimeOffset? ApprovedAtUtc);
    public sealed record SurfaceMapDetailsDto(Guid FieldPubId, Guid AreaPubId, int HoleNumber,
        bool UseForClippingMeasurements, bool Irrigated, bool ClippingsRemoved);
    public sealed record SurfaceMapWorkspaceDto(Guid SurfacePubId, string SurfaceName, decimal ManualAreaM2,
        bool SubtractFromContainingSurface, IReadOnlyList<SubsectionGeometryDto> Subsections,
        IReadOnlyList<RawBoundaryPointDto> RawPoints, IReadOnlyList<SurfaceMapRevisionDto> Revisions,
        DateTimeOffset? ClearedAtUtc = null, MapCenterDto? MapCenter = null,
        SurfaceMapDetailsDto? Details = null);
    public sealed record SurfaceMapLayerDto(Guid FieldPubId, IReadOnlyList<SurfaceMapLayerItemDto> Layers,
        MapCenterDto? MapCenter = null);
    public sealed record SurfaceMapLayerItemDto(string LayerType, Guid PubId, Guid? ParentPubId, string Name,
        string GeoJson, string? ApprovalState);
    public sealed record SurfaceMapAssignmentOptionDto(Guid SurfacePubId, string SurfaceName, int HoleNumber);
    public sealed record SurfaceMapAssignmentWorkspaceDto(Guid FieldPubId, string FieldName, Guid AreaPubId,
        string AreaName, Guid SourceSurfacePubId, string SourceSurfaceName, int SourceHoleNumber,
        IReadOnlyList<SurfaceMapAssignmentOptionDto> Surfaces);
    public sealed record SurfaceMapReassignRequestDto(Guid TargetSurfacePubId);
    public sealed record SurfaceMapReassignResultDto(Guid VacatedSurfacePubId, Guid AssignedSurfacePubId);
}
