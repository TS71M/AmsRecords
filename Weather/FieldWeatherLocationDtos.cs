namespace AmsRecords.Weather;

public static class FieldWeatherLocationRules
{
    public const int MaxAdditionalLocations = AmsModels.FieldWeatherLocationConstraints.MaxAdditionalLocations;
    public const int MaxNameLength = AmsModels.FieldWeatherLocationConstraints.MaxNameLength;
    public static readonly Guid PrimaryLocationPubId = Guid.Empty;

    public static string? Validate(string? name, decimal latitude, decimal longitude)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Location name is required.";
        if (name.Trim().Length > MaxNameLength)
            return $"Location name must not exceed {MaxNameLength} characters.";
        if (latitude is < -90m or > 90m)
            return "Latitude must be between -90 and 90.";
        if (longitude is < -180m or > 180m)
            return "Longitude must be between -180 and 180.";

        return null;
    }
}

public sealed record FieldWeatherLocationDto(
    Guid PubId,
    Guid FieldPubId,
    string Name,
    decimal Latitude,
    decimal Longitude,
    bool IsPrimary,
    int DisplayOrder);

public sealed record FieldWeatherLocationCreateDto(
    string Name,
    decimal Latitude,
    decimal Longitude);

public sealed record FieldWeatherLocationUpdateDto(
    string Name,
    decimal Latitude,
    decimal Longitude);
