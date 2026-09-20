using System.Globalization;
using AmsRecords.Units;
using static AmsRecords.Weather.WeatherCurrentDtos;
using static AmsRecords.Weather.WeatherForecastDtos;
using static AmsRecords.Weather.WeatherObservationDtos;

namespace AmsRecords.Weather;

/// <summary>Equal-weight spatial means, never sums of precipitation depths across locations.</summary>
public static class WeatherLocationAggregation
{
    static UnitValueDto Mean(IEnumerable<UnitValueDto?> values, bool maximum = false)
    {
        var valid = values.Where(x => x?.Value is not null).Select(x => x!).ToArray();
        if (valid.Length == 0) return new();
        var value = maximum ? valid.Max(x => x.Value!.Value) : valid.Average(x => x.Value!.Value);
        return new(value, value.ToString("0.#", CultureInfo.CurrentCulture) + " " + valid[0].UnitShort, valid[0].UnitShort);
    }

    public static WeatherCurrentDto Current(IReadOnlyList<WeatherCurrentDto> source)
    {
        var rows = source.Where(x => x.HasData).ToArray();
        if (rows.Length == 0) return source.FirstOrDefault() ?? new();
        var first = rows.GroupBy(x => x.WeatherCode).OrderByDescending(x => x.Count()).First().First();
        return first with
        {
            Temp = Mean(rows.Select(x => x.Temp)), TempC = rows.Average(x => x.TempC),
            DewPoint = Mean(rows.Select(x => x.DewPoint)), RelativeHumidity = Mean(rows.Select(x => x.RelativeHumidity)),
            WindSpeedMs = Mean(rows.Select(x => x.WindSpeedMs)), WindGustMs = Mean(rows.Select(x => x.WindGustMs), true),
            WindDeg = rows.Length == 1 ? first.WindDeg : null, CloudPct = (short?)rows.Average(x => (decimal?)x.CloudPct),
            RainMm1h = Mean(rows.Select(x => x.RainMm1h)), SnowMm1h = Mean(rows.Select(x => x.SnowMm1h)),
            ObservedAtUtc = rows.Min(x => x.ObservedAtUtc), RainedLast24Hours = rows.Any(x => x.RainedLast24Hours),
            Source = null, Messages = []
        };
    }

    public static List<WeatherForecastHourDisplayDto> Forecast(IEnumerable<WeatherForecastHourDisplayDto> source)
        => source.GroupBy(x => x.ForecastForUtc).OrderBy(x => x.Key).Select(group =>
        {
            var rows = group.ToArray();
            var first = rows.GroupBy(x => x.WeatherCode).OrderByDescending(x => x.Count()).First().First();
            return first with
            {
                Temp = Mean(rows.Select(x => x.Temp)), DewPoint = Mean(rows.Select(x => x.DewPoint)),
                Rh = Mean(rows.Select(x => x.Rh)), Pressure = Mean(rows.Select(x => x.Pressure)),
                Wind = Mean(rows.Select(x => x.Wind)), Gust = Mean(rows.Select(x => x.Gust), true),
                Cloud = Mean(rows.Select(x => x.Cloud)), RainMm1h = Mean(rows.Select(x => x.RainMm1h)),
                SnowMm1h = Mean(rows.Select(x => x.SnowMm1h)), Pop = Mean(rows.Select(x => x.Pop), true)
            };
        }).ToList();

    public static WeatherObservationMiniDto? Observations(IReadOnlyList<WeatherObservationMiniDto> source)
    {
        if (source.Count == 0) return null;
        var points = source.SelectMany(x => x.Points).GroupBy(x => x.ObservedAtUtc).OrderBy(x => x.Key)
            .Select(g => new WeatherObservationMiniPointDto(g.Key, g.Average(x => x.Temp),
                (int?)g.Average(x => (decimal?)x.Rh), g.Average(x => x.Wind), g.Average(x => x.Rain))).ToList();
        return source[0] with
        {
            Count = points.Count, Points = points,
            TempMin = Mean(source.Select(x => x.TempMin)), TempAvg = Mean(source.Select(x => x.TempAvg)), TempMax = Mean(source.Select(x => x.TempMax)),
            RhMin = (int?)source.Average(x => (decimal?)x.RhMin), RhAvg = (int?)source.Average(x => (decimal?)x.RhAvg), RhMax = (int?)source.Average(x => (decimal?)x.RhMax),
            RainSum = Mean(source.Select(x => x.RainSum)), WindMax = Mean(source.Select(x => x.WindMax), true)
        };
    }
}
