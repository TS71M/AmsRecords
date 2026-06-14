namespace AmsRecords.Weather;

public static class WeatherTimeHelper
{
    public static TimeSpan ResolveFieldLocalOffset(decimal? latitude, decimal? longitude, DateTimeOffset whenUtc)
    {
        if (latitude is null || longitude is null)
            return TimeSpan.Zero;

        var timeZone = ResolveApproximateTimeZone(latitude.Value, longitude.Value);
        if (timeZone is not null)
            return timeZone.GetUtcOffset(whenUtc.UtcDateTime);

        var offsetHours = (int)Math.Round((double)(longitude.Value / 15m), MidpointRounding.AwayFromZero);
        offsetHours = Math.Clamp(offsetHours, -12, 14);

        return TimeSpan.FromHours(offsetHours);
    }

    public static DateOnly LocalDateForUtc(DateTime utc, TimeSpan fieldLocalOffset)
        => DateOnly.FromDateTime(DateTime.SpecifyKind(utc, DateTimeKind.Utc).Add(fieldLocalOffset).Date);

    public static DateOnly LocalDateForUtc(DateTimeOffset utc, TimeSpan fieldLocalOffset)
        => DateOnly.FromDateTime(utc.UtcDateTime.Add(fieldLocalOffset).Date);

    public static DateTime LocalDateStartToUtc(DateOnly localDate, TimeSpan fieldLocalOffset)
        => DateTime.SpecifyKind(localDate.ToDateTime(TimeOnly.MinValue).Subtract(fieldLocalOffset), DateTimeKind.Utc);

    static TimeZoneInfo? ResolveApproximateTimeZone(decimal latitude, decimal longitude)
    {
        if (latitude is < 35m or > 72m || longitude is < -25m or > 45m)
            return null;

        if (longitude < -5m)
            return FindTimeZone("GMT Standard Time", "Europe/London");

        if (longitude >= 22m)
            return FindTimeZone("FLE Standard Time", "Europe/Helsinki");

        return FindTimeZone("W. Europe Standard Time", "Europe/Berlin");
    }

    static TimeZoneInfo? FindTimeZone(params string[] ids)
    {
        foreach (var id in ids)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return null;
    }
}
