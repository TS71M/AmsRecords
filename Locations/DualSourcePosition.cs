namespace AmsRecords.Locations;

public sealed record PhonePositionDto(double Latitude, double Longitude, double? HorizontalAccuracyMetres,
    DateTimeOffset CapturedAtUtc);

public sealed record PositionSelectionDto(double? Latitude, double? Longitude, string Source,
    double? SourceDistanceMetres, bool Warning, string Status);

public static class DualSourcePosition
{
    public const double GoodPhoneAccuracyMetres = 3d;
    public const double MaximumAcceptablePhoneAccuracyMetres = 5d;
    public const double MaximumPhoneAgeSeconds = 2d;
    public const double DisagreementWarningMetres = 5d;
    public const double StrongDisagreementMetres = 15d;

    public static PositionSelectionDto Select(double? tdrLatitude, double? tdrLongitude,
        PhonePositionDto? phone, DateTimeOffset eventAtUtc)
    {
        var tdrValid = IsValid(tdrLatitude, tdrLongitude);
        var phoneValid = phone is not null && IsValid(phone.Latitude, phone.Longitude);
        var phoneAge = phone is null ? double.PositiveInfinity : Math.Abs((phone.CapturedAtUtc - eventAtUtc).TotalSeconds);
        var phoneFresh = phoneValid && phoneAge <= MaximumPhoneAgeSeconds;
        var phoneAcceptable = phoneFresh && phone!.HorizontalAccuracyMetres is >= 0 and <= MaximumAcceptablePhoneAccuracyMetres;
        double? distance = tdrValid && phoneValid
            ? DistanceMetres(tdrLatitude!.Value, tdrLongitude!.Value, phone!.Latitude, phone.Longitude)
            : null;
        var disagreement = distance > DisagreementWarningMetres;
        var strongDisagreement = distance > StrongDisagreementMetres;

        if (phoneAcceptable)
            return new(phone!.Latitude, phone.Longitude, "Phone", distance, disagreement,
                strongDisagreement ? "StrongSourceDisagreement" : disagreement ? "SourceDisagreement" :
                phone.HorizontalAccuracyMetres <= GoodPhoneAccuracyMetres ? "PhoneGood" : "PhoneAcceptable");
        if (tdrValid)
            return new(tdrLatitude, tdrLongitude, "Tdr", distance, disagreement || (phoneValid && !phoneAcceptable),
                !phoneValid ? "TdrOnly" : !phoneFresh ? "PhoneStale" : "PhoneAccuracyRejected");
        if (phoneFresh)
            return new(phone!.Latitude, phone.Longitude, "PhoneLowAccuracy", null, true, "PhoneAccuracyRejectedTdrUnavailable");
        return new(null, null, "None", distance, true, phoneValid ? "PhoneStaleTdrUnavailable" : "NoValidPosition");
    }

    public static bool IsValid(double? latitude, double? longitude) => latitude is >= -90 and <= 90 &&
        longitude is >= -180 and <= 180 && (Math.Abs(latitude.Value) > 0.0000001 || Math.Abs(longitude.Value) > 0.0000001);

    public static double DistanceMetres(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadius = 6371008.8;
        static double Radians(double degrees) => degrees * Math.PI / 180d;
        var dLat = Radians(lat2 - lat1); var dLon = Radians(lon2 - lon1);
        var a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * Math.Pow(Math.Sin(dLon / 2), 2);
        return earthRadius * 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
    }
}
