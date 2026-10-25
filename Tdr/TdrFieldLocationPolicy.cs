using AmsRecords.Locations;

namespace AmsRecords.Tdr;

/// <summary>Advisory field-distance check shared by capture guidance and uploaded readings.</summary>
public static class TdrFieldLocationPolicy
{
    public const decimal WarningThresholdKm = 5m;

    public static (decimal? DistanceKm, bool Warning, string Status) ForPhone(
        MapCenterDtos.MapCenterDto? field, PhonePositionDto? phone, DateTimeOffset now)
    {
        static bool Valid(double latitude, double longitude) => double.IsFinite(latitude) && latitude is >= -90 and <= 90
            && double.IsFinite(longitude) && longitude is >= -180 and <= 180;
        if (field?.Source != MapCenterDtos.Sources.Field || !Valid(field.Latitude, field.Longitude))
            return (null, false, "FieldCoordinatesUnavailable");
        if (phone is null || !Valid(phone.Latitude, phone.Longitude)
            || phone.HorizontalAccuracyMetres is not >= 0 or > 100 || !double.IsFinite(phone.HorizontalAccuracyMetres.Value)
            || now - phone.CapturedAtUtc > TimeSpan.FromSeconds(15) || phone.CapturedAtUtc - now > TimeSpan.FromSeconds(2))
            return (null, false, "MeasurementCoordinatesInvalid");
        return Check((decimal)field.Latitude, (decimal)field.Longitude, (decimal)phone.Latitude, (decimal)phone.Longitude);
    }

    public static (decimal? DistanceKm, bool Warning, string Status) Check(
        decimal? fieldLatitude, decimal? fieldLongitude, decimal? latitude, decimal? longitude)
    {
        if (!fieldLatitude.HasValue || !fieldLongitude.HasValue || !ValidCoordinate(fieldLatitude.Value, fieldLongitude.Value))
            return (null, false, "FieldCoordinatesUnavailable");
        if (!latitude.HasValue || !longitude.HasValue || !ValidCoordinate(latitude.Value, longitude.Value))
            return (null, false, "MeasurementCoordinatesInvalid");
        var distance = (decimal)DistanceKm(fieldLatitude.Value, fieldLongitude.Value, latitude.Value, longitude.Value);
        return (decimal.Round(distance, 3), distance > WarningThresholdKm,
            distance > WarningThresholdKm ? "OutsideFieldMargin" : "WithinFieldMargin");
    }

    public static bool ValidCoordinate(decimal latitude, decimal longitude) =>
        latitude is >= -90 and <= 90 && longitude is >= -180 and <= 180 && (latitude != 0 || longitude != 0);

    public static double DistanceKm(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        const double earthRadiusKm = 6371.0088;
        static double Radians(decimal degrees) => (double)degrees * Math.PI / 180d;
        var latitudeDelta = Radians(lat2 - lat1);
        var longitudeDelta = Radians(lon2 - lon1);
        var a = Math.Pow(Math.Sin(latitudeDelta / 2d), 2d) +
            Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * Math.Pow(Math.Sin(longitudeDelta / 2d), 2d);
        a = Math.Clamp(a, 0d, 1d);
        return earthRadiusKm * 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
    }
}
