namespace AmsRecords.RiskManagement;

/// <summary>Selects dated forecast rows without treating row count as a calendar.</summary>
public static class RiskDisplayDays
{
    public static IReadOnlyList<T> Select<T>(IEnumerable<T>? days, DateOnly today)
        => days?.Where(day => HasData(day) && Date(day) >= today)
            .OrderBy(Date).Take(6).ToList() ?? [];

    static DateOnly Date<T>(T day) => day switch
    {
        DollarSpotDtos.DollarSpotDayRiskDto x => x.DateLocal,
        PythiumDtos.PythiumBlightDayDto x => x.DateLocal,
        MicrodochiumDtos.MicrodochiumDayDto x => x.DateLocal,
        DewPointDtos.DewPointDayDto x => x.DateLocal,
        FrostDtos.FrostDayDto x => x.DateLocal,
        HeatStressDtos.HeatStressDayDto x => x.DateLocal,
        _ => throw new ArgumentException("Unsupported risk day type.", nameof(day))
    };

    static bool HasData<T>(T day) => day switch
    {
        DollarSpotDtos.DollarSpotDayRiskDto x => x.HasData,
        PythiumDtos.PythiumBlightDayDto x => x.HasData,
        MicrodochiumDtos.MicrodochiumDayDto x => x.HasData,
        DewPointDtos.DewPointDayDto x => x.HasData,
        FrostDtos.FrostDayDto x => x.HasData,
        HeatStressDtos.HeatStressDayDto x => x.HasData,
        _ => false
    };
}
