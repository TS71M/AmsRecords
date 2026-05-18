using static AmsRecords.RiskManagement.DewPointDtos;
using static AmsRecords.RiskManagement.RiskDtos;
using static Lib.Enums.RiskLevels;

namespace AmsRecords.RiskManagement;

public static class RiskHelper
{
    public static decimal NormalizeDollarSpotThreshold(decimal? thresholdPct)
    {
        var t = thresholdPct.GetValueOrDefault(20m);
        return t <= 0 ? 20m : t;
    }

    public static List<DailyMean> BuildDailyMeans(IReadOnlyList<HourPoint> hourly)
        => [.. hourly
            .GroupBy(h => DateOnly.FromDateTime(h.HourUtc.UtcDateTime.Date))
            .Select(g => new DailyMean(
                Date: g.Key,
                AvgTempC: g.Average(x => x.TempC),
                AvgRh: g.Average(x => (decimal)x.Rh),
                HourCount: g.Count()
            ))
            .OrderBy(d => d.Date)];

    public static DewPointNDaysDto ComputeDewPointNDays(
        Guid fieldPubId,
        DateTimeOffset nowUtc,
        DateOnly todayUtc,
        int daysToReturn,
        IReadOnlyList<HourPoint> hourly)
    {
        daysToReturn = Math.Clamp(daysToReturn, 1, 7);

        var targetDays = Enumerable.Range(0, daysToReturn)
            .Select(i => todayUtc.AddDays(i))
            .ToArray();

        var days = new List<DewPointDayDto>(daysToReturn);

        foreach (var d in targetDays)
        {
            var dayHours = hourly
                .Where(h => DateOnly.FromDateTime(h.HourUtc.UtcDateTime.Date) == d)
                .ToList();

            if (dayHours.Count < 18)
            {
                days.Add(new DewPointDayDto(
                    DateLocal: d,
                    HasData: false,
                    AvgDewPointC: null,
                    AvgDewPoint: null,
                    MinDewPointDepressionC: null,
                    MinDewPointDepression: null,
                    HoursNearDew: null,
                    RiskLevel: RiskLevel.None));
                continue;
            }

            decimal sumDew = 0m;
            decimal minDep = decimal.MaxValue;
            var hoursNear = 0;

            foreach (var h in dayHours)
            {
                var dew = CalculateDewPoint(h.TempC, h.Rh);
                sumDew += dew;

                var dep = h.TempC - dew;
                if (dep < minDep)
                    minDep = dep;

                if (dep <= 1.0m)
                    hoursNear++;
            }

            days.Add(new DewPointDayDto(
                DateLocal: d,
                HasData: true,
                AvgDewPointC: Math.Round(sumDew / dayHours.Count, 1),
                AvgDewPoint: null,
                MinDewPointDepressionC: Math.Round(minDep, 1),
                MinDewPointDepression: null,
                HoursNearDew: hoursNear,
                RiskLevel: DewRiskLevel(hoursNear, Math.Round(minDep, 1))
            ));
        }

        return new DewPointNDaysDto(
            FieldPubId: fieldPubId,
            GeneratedAtUtc: nowUtc,
            Days: days,
            MessageCategories:
            [
                new(
            AppMessageCategories.Notes,
            [
                new(RiskMessageCodes.DewPoint.Model, null, null),
                new(RiskMessageCodes.DewPoint.BlendedSeries, null, null),
                new(RiskMessageCodes.DewPoint.HoursNearDewDefinition, null, null)
            ])
            ]
        );
    }

    public static decimal CalculateDewPoint(decimal tempC, short rhPct)
    {
        // Magnus formula
        const double a = 17.27;
        const double b = 237.7;

        var t = (double)tempC;
        var rh = Math.Clamp(rhPct / 100.0, 0.0001, 1.0);

        var alpha = (a * t) / (b + t) + Math.Log(rh);
        var dewPoint = (b * alpha) / (a - alpha);

        return Math.Round((decimal)dewPoint, 1);
    }

    public static decimal LogisticPct(decimal logit)
    {
        var x = Math.Exp((double)logit);
        return Math.Round((decimal)(x / (1.0 + x) * 100.0), 1);
    }

    public static DateTimeOffset TruncateToUtcHour(DateTimeOffset utc)
         => new(utc.UtcDateTime.Year, utc.UtcDateTime.Month, utc.UtcDateTime.Day, utc.UtcDateTime.Hour, 0, 0, TimeSpan.Zero);

    public static decimal NormalizeThreshold(decimal? thresholdPct)
    {
        var t = thresholdPct.GetValueOrDefault(20m);
        return t <= 0 ? 20m : t;
    }

    public static RiskLevel DewRiskLevel(int hoursNearDew, decimal minDewPointDepressionC)
    {
        // Guard: if depression never got tight, cap the risk.
        // This reduces false positives from rounding / noisy RH.
        if (minDewPointDepressionC > 2.0m)
            return hoursNearDew >= 2 ? RiskLevel.Low : RiskLevel.None;

        return hoursNearDew switch
        {
            <= 1 => RiskLevel.None,
            <= 4 => RiskLevel.Low,
            <= 7 => RiskLevel.Moderate,
            <= 11 => RiskLevel.High,
            _ => RiskLevel.High
        };
    }
}
