using System.Globalization;
using AmsRecords.Units;
using static AmsRecords.Weather.WeatherForecastDtos;

namespace AmsRecords.Weather;

public sealed record WeatherMapDay(
    DateOnly DateLocal, UnitValueDto Temperature, UnitValueDto Humidity, UnitValueDto DewPoint,
    string WeatherMain, decimal? ProbabilityPct, decimal? ExpectedRain, string RainUnit, string Wind,
    int HoursAvailable, decimal? TemperatureComparableValue, decimal? DewPointComparableValue);

public sealed record WeatherMapHour(
    DateTime ForecastForUtc, DateTime ForecastForLocal, DateOnly DateLocal,
    UnitValueDto Temperature, UnitValueDto Humidity, UnitValueDto DewPoint,
    string WeatherMain, decimal? ProbabilityPct, decimal? ExpectedRain, string RainUnit, string Wind,
    int HoursAvailable, decimal? TemperatureComparableValue, decimal? DewPointComparableValue);

/// <summary>One forecast projection policy for mobile and web weather maps, using the forecast's own local dates.</summary>
public static class WeatherMapForecast
{
    public static IReadOnlyList<WeatherMapDay> Build(IEnumerable<WeatherForecastMiniHourDto>? hours, DateOnly today)
        => (hours ?? []).Where(hour => hour.DateLocal >= today && hour.DateLocal <= today.AddDays(5))
            .OrderBy(hour => hour.ForecastForUtc).DistinctBy(hour => hour.ForecastForUtc)
            .GroupBy(hour => hour.DateLocal).OrderBy(group => group.Key)
            .Select(group =>
            {
                var rows = group.ToList();
                var temperature = Average(rows.Select(row => row.Temp));
                var dew = Average(rows.Select(row => row.DewPoint));
                var rain = rows.Select(row => row.RainMm1h.Value).Where(value => value.HasValue).Select(value => value!.Value).ToList();
                return new WeatherMapDay(group.Key, temperature, Average(rows.Select(row => row.Rh)), dew,
                    rows.Where(row => !string.IsNullOrWhiteSpace(row.WeatherMain)).GroupBy(row => row.WeatherMain!.Trim(), StringComparer.OrdinalIgnoreCase)
                        .OrderByDescending(group => group.Count()).Select(group => group.Key).FirstOrDefault() ?? "",
                    rows.Max(row => row.Pop.Value), rain.Count == 0 ? null : Math.Round(rain.Sum(), 1),
                    rows.FirstOrDefault(row => !string.IsNullOrWhiteSpace(row.RainMm1h.UnitShort))?.RainMm1h.UnitShort ?? "mm",
                    rows.FirstOrDefault(row => !string.IsNullOrWhiteSpace(row.Wind.Text))?.Wind.Text ?? "",
                    rows.Count, ComparableTemperature(temperature), ComparableTemperature(dew));
            }).ToList();

    // Start at the next whole UTC hour. UTC keys keep repeated/skipped local DST
    // hours distinct; missing slots remain absent rather than borrowing observations.
    public static DateTime HourlyWindowStart(DateTime utcNow)
        => new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0, DateTimeKind.Utc).AddHours(1);

    public static IReadOnlyList<WeatherMapHour> BuildHours(IEnumerable<WeatherForecastMiniHourDto>? hours, DateTime utcNow)
    {
        var start = HourlyWindowStart(utcNow);
        return (hours ?? []).Where(hour => hour.ForecastForUtc >= start && hour.ForecastForUtc < start.AddHours(24))
            .OrderBy(hour => hour.ForecastForUtc).DistinctBy(hour => hour.ForecastForUtc)
            .Select(hour => new WeatherMapHour(DateTime.SpecifyKind(hour.ForecastForUtc, DateTimeKind.Utc),
                hour.ForecastForLocal, hour.DateLocal, hour.Temp, hour.Rh, hour.DewPoint, hour.WeatherMain ?? "",
                hour.Pop.Value, hour.RainMm1h.Value, hour.RainMm1h.UnitShort, hour.Wind.Text, 1,
                ComparableTemperature(hour.Temp), ComparableTemperature(hour.DewPoint))).ToList();
    }

    static UnitValueDto Average(IEnumerable<UnitValueDto> values)
    {
        var available = values.Where(value => value.Value.HasValue).ToList();
        if (available.Count == 0) return new();
        var unit = available[0].UnitShort;
        var average = Math.Round(available.Average(value => value.Value!.Value), 1);
        return new(average, string.IsNullOrWhiteSpace(unit) ? average.ToString("0.#", CultureInfo.CurrentCulture)
            : $"{average.ToString("0.#", CultureInfo.CurrentCulture)} {unit}", unit);
    }

    static decimal? ComparableTemperature(UnitValueDto value)
        => value.UnitShort.Contains("F", StringComparison.OrdinalIgnoreCase) ? (value.Value - 32m) * 5m / 9m : value.Value;
}
