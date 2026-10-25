using AmsRecords.Units;
using AmsRecords.Weather;
using static AmsRecords.RiskManagement.RiskDtos;
using static AmsRecords.Weather.WeatherForecastDtos;

namespace AmsRecords.RiskManagement;

public sealed record RiskTimingHour(DateTime ForecastForUtc, IReadOnlyDictionary<string, string> Levels,
    UnitValueDto Temperature, UnitValueDto Humidity, UnitValueDto DewPoint);
public sealed record RiskTimingWindow(DateTime StartUtc, DateTime EndUtc);
public sealed record RiskMapTiming(DateTime WindowStartUtc, DateTime GeneratedAtUtc, IReadOnlyList<RiskTimingHour> Hours,
    IReadOnlyDictionary<string, IReadOnlyList<RiskTimingWindow>> Windows)
{
    // This is threshold exposure timing, not an hourly probability or a replacement
    // for daily duration-based assessments. No disease calculation participates.
    public static RiskMapTiming? Build(RiskSummaryDto summary, WeatherForecastMiniDto forecast, DateTime utcNow)
    {
        if (summary.FieldPubId == Guid.Empty || forecast.FieldPubId != summary.FieldPubId) return null;
        var points = WeatherMapForecast.BuildHours(forecast.Hours, utcNow);
        if (points.Count == 0) return null;
        var frostModerate = summary.Frost?.ModerateThresholdTempC;
        var frostHigh = summary.Frost?.HighThresholdTempC;
        var heatModerate = summary.HeatStress?.ModerateThresholdTempC;
        var heatHigh = summary.HeatStress?.HighThresholdTempC;
        var frostAvailable = frostHigh.HasValue && frostModerate.HasValue && frostHigh <= frostModerate;
        var heatAvailable = heatHigh.HasValue && heatModerate.HasValue && heatModerate <= heatHigh;
        var start = WeatherMapForecast.HourlyWindowStart(utcNow);
        var byHour = points.ToDictionary(point => point.ForecastForUtc);
        var hours = Enumerable.Range(0, 24).Select(index =>
        {
            var time = start.AddHours(index);
            byHour.TryGetValue(time, out var point);
            var temp = point?.TemperatureComparableValue;
            var rh = point?.Humidity.Value;
            var levels = new Dictionary<string, string> { ["dew"] = "na" };
            if (frostAvailable) levels["frost"] = temp is null ? "na" : temp <= frostHigh ? "high" : temp <= frostModerate ? "moderate" : "low";
            if (heatAvailable) levels["heatStress"] = temp is null ? "na" : temp >= heatHigh ? "high" : temp >= heatModerate ? "moderate" : "low";
            if (temp.HasValue && rh is >= 0 and <= 100)
            {
                var dew = point?.DewPointComparableValue ?? RiskHelper.CalculateDewPoint(temp.Value, (short)Math.Round(rh.Value));
                levels["dew"] = temp.Value - dew <= 1m ? "moderate" : "low";
            }
            return new RiskTimingHour(time, levels, point?.Temperature ?? new(), point?.Humidity ?? new(), point?.DewPoint ?? new());
        }).ToList();
        var windows = hours.SelectMany(hour => hour.Levels.Keys).Distinct().ToDictionary(key => key,
            key => WindowsFor(hours, key));
        return new(start, forecast.GeneratedAtUtc, hours, windows);
    }

    static IReadOnlyList<RiskTimingWindow> WindowsFor(IReadOnlyList<RiskTimingHour> hours, string key)
    {
        var windows = new List<RiskTimingWindow>();
        DateTime? start = null;
        foreach (var hour in hours)
        {
            var exposed = hour.Levels.TryGetValue(key, out var level) && level is "moderate" or "high";
            if (exposed) start ??= hour.ForecastForUtc;
            else if (start.HasValue) { windows.Add(new(start.Value, hour.ForecastForUtc)); start = null; }
        }
        if (start.HasValue) windows.Add(new(start.Value, hours[^1].ForecastForUtc.AddHours(1)));
        return windows;
    }
}
