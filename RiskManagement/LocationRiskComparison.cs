namespace AmsRecords.RiskManagement;

public enum LocalRiskDifference { None, Elevated, Hotspot, SharedHighest }

public sealed record LocationRiskValue(Guid LocationPubId, string Name, string Level, bool HasData);

public sealed record LocationRiskComparison(
    string RiskKey, DateOnly Date, IReadOnlyList<LocationRiskValue> Locations,
    IReadOnlyList<LocationRiskValue> HighestLocations, LocalRiskDifference Difference)
{
    public int AvailableCount => Locations.Count(x => x.HasData);
}

/// <summary>Category comparisons, not statistical significance or additive probabilities.</summary>
public static class LocationRiskComparisons
{
    public static IReadOnlyList<LocationRiskComparison> Build(IReadOnlyList<LocationRiskSummary> locations,
        DateTimeOffset? now = null)
    {
        LocationRiskAggregation.ValidateField(locations);
        var result = new List<LocationRiskComparison>();
        Add<DollarSpotDtos.DollarSpotDayRiskDto>("dollarSpot", x => x.Summary.DollarSpot?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.Level.ToString());
        Add<PythiumDtos.PythiumBlightDayDto>("pythium", x => x.Summary.Pythium?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.RiskLevel);
        Add<MicrodochiumDtos.MicrodochiumDayDto>("microdochium", x => x.Summary.Microdochium?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.RiskLevel);
        Add<DewPointDtos.DewPointDayDto>("dew", x => x.Summary.DewPoint?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.RiskLevel.ToString());
        Add<FrostDtos.FrostDayDto>("frost", x => x.Summary.Frost?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.RiskLevel);
        Add<HeatStressDtos.HeatStressDayDto>("heatStress", x => x.Summary.HeatStress?.Days ?? [], x => x.DateLocal, x => x.HasData, x => x.RiskLevel);
        return result;

        void Add<T>(string key, Func<LocationRiskSummary, IReadOnlyList<T>> days,
            Func<T, DateOnly> date, Func<T, bool> available, Func<T, string> level)
        {
            foreach (var day in locations.SelectMany(days).Select(date).Distinct().Order())
            {
                var values = locations.Select(location =>
                {
                    var rows = days(location).Where(x => date(x) == day).ToArray();
                    var valid = LocationRiskAggregation.IsFresh(location.Summary, now)
                        && rows.Length > 0 && available(rows[0]) && LocationRiskAggregation.Severity(level(rows[0])) >= 0;
                    return new LocationRiskValue(location.LocationPubId, location.Name,
                        valid ? level(rows[0]) : "na", valid);
                }).ToArray();
                result.Add(Compare(key, day, values));
            }
        }
    }

    public static LocationRiskComparison Compare(string key, DateOnly date, IReadOnlyList<LocationRiskValue> locations)
    {
        var valid = locations.Where(x => x.HasData && LocationRiskAggregation.Severity(x.Level) >= 0).ToArray();
        var max = valid.Select(x => LocationRiskAggregation.Severity(x.Level)).DefaultIfEmpty(-1).Max();
        var highest = valid.Where(x => LocationRiskAggregation.Severity(x.Level) == max).ToArray();
        var lower = valid.Where(x => LocationRiskAggregation.Severity(x.Level) < max).ToArray();
        // Missing peers cannot establish a unique local hotspot.
        var difference = valid.Length != locations.Count || lower.Length == 0 ? LocalRiskDifference.None
            : highest.Length > 1 ? LocalRiskDifference.SharedHighest
            : max == 3 && lower.All(x => LocationRiskAggregation.Severity(x.Level) <= 1) ? LocalRiskDifference.Hotspot
            : LocalRiskDifference.Elevated;
        return new(key, date, locations, highest, difference);
    }
}
