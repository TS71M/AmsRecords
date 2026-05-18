using AmsRecords.RiskManagement;

namespace AmsRecords.Weather;

public static class WeatherExtensions
{
    public static decimal CalculateDewPoint(this WeatherObservation o)
        => RiskHelper.CalculateDewPoint(o.TempC, o.RelativeHumidity);
}