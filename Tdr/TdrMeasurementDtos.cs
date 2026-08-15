namespace AmsRecords.Tdr;

public static class TdrMeasurementDtos
{
    public sealed record Upload(
        Guid ClientMeasurementId, Guid CourseId, Guid FieldId, Guid? ZoneId, Guid? SessionId,
        Guid OperatorId, string? DeviceSerialNumber, string? DeviceBluetoothAddress,
        DateTimeOffset CapturedAtUtc, DateTimeOffset TriggeredAtUtc, DateTimeOffset CaptureStartedUtc,
        DateTimeOffset CaptureCompletedUtc, decimal? VwcPercent, int? Period, decimal? BulkEc,
        decimal? SoilTemperatureC, decimal? IrTemperatureC, decimal? Latitude, decimal? Longitude,
        string? RawLatitude, string? RawLongitude, int? SatelliteCount, string? GpsFix,
        string GpsQuality, string? RodLength, string? SoilType, int? BatteryLevel,
        string ProtocolVersion, string ParserVersion, int PayloadSchemaVersion,
        string RawBlePayloadJson, IReadOnlyList<string>? UnavailableFields,
        decimal? PhoneLatitude = null, decimal? PhoneLongitude = null, decimal? PhoneHorizontalAccuracyMetres = null,
        DateTimeOffset? PhoneCapturedAtUtc = null, decimal? PreferredLatitude = null, decimal? PreferredLongitude = null,
        string PositionSource = "Tdr", decimal? SourceDistanceMetres = null, bool PositionWarning = false,
        string PositionStatus = "TdrOnly");

    public sealed record BatchRequest(IReadOnlyList<Upload> Measurements);
    public sealed record Result(Guid ClientMeasurementId, bool Success, bool AlreadyExisted,
        Guid? ServerMeasurementId, int StatusCode, string? Error, bool LocationWarning = false,
        decimal? DistanceFromFieldKm = null, string LocationCheckStatus = "NotChecked");
    public sealed record BatchResponse(IReadOnlyList<Result> Results);

    public sealed record TimelineQuery(DateTimeOffset? FromUtc = null, DateTimeOffset? ToUtc = null,
        Guid? SessionId = null, Guid? SurfacePubId = null);

    public sealed record TimelineSurface(Guid SurfacePubId, Guid AreaPubId, string AreaName,
        Guid HolePubId, int HoleNumber, string SurfaceName, string Label,
        string? EffectiveGeoJson, string? MappingApprovalState,
        decimal DryVwcThreshold = 15m, decimal WetVwcThreshold = 25m,
        bool HasSurfaceVwcOverride = false);

    public sealed record TimelineItem(Guid ServerMeasurementId, Guid ClientMeasurementId, Guid? SessionId,
        Guid? SurfacePubId, Guid? AreaPubId, string? AreaName, int? HoleNumber, string? SurfaceName,
        DateTimeOffset CapturedAtUtc, decimal? VwcPercent, int? Period, decimal? BulkEc,
        decimal? SoilTemperatureC, decimal? IrTemperatureC, decimal? Latitude, decimal? Longitude,
        int? SatelliteCount, string? GpsFix, string GpsQuality, string? DeviceSerialNumber,
        bool LocationWarning, decimal? DistanceFromFieldKm, string LocationCheckStatus,
        string SurfaceAssignmentStatus, Guid? SuggestedSurfacePubId, string? SuggestedSurfaceName,
        decimal? TdrLatitude = null, decimal? TdrLongitude = null, decimal? PhoneLatitude = null,
        decimal? PhoneLongitude = null, decimal? PhoneHorizontalAccuracyMetres = null,
        decimal? SourceDistanceMetres = null, string PositionSource = "Tdr", bool PositionWarning = false,
        string PositionStatus = "TdrOnly", decimal? DryVwcThreshold = 15m,
        decimal? WetVwcThreshold = 25m, string? VwcThresholdSource = "Default");

    public sealed record SurfaceAssignmentUpdate(Guid? SurfacePubId);

    public sealed record PositionCorrectionUpdate(
        decimal? Latitude = null,
        decimal? Longitude = null,
        bool ResetToCapturedPosition = false);

    public sealed record PositionCorrectionResult(
        Guid MeasurementPubId,
        decimal? Latitude,
        decimal? Longitude,
        string PositionSource,
        string PositionStatus,
        bool LocationWarning,
        decimal? DistanceFromFieldKm,
        Guid? SuggestedSurfacePubId,
        string? SuggestedSurfaceName);

    public sealed record TimelineSession(Guid SessionId, DateTimeOffset FirstReadingUtc,
        DateTimeOffset LastReadingUtc, int MeasurementCount);

    public sealed record TimelineResponse(Guid FieldPubId, string FieldName,
        IReadOnlyList<TimelineSurface> Surfaces, IReadOnlyList<TimelineItem> Measurements,
        IReadOnlyList<TimelineSession> Sessions);
}
