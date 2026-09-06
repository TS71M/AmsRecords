namespace AmsRecords.Irrigation;

/// <summary>
/// Geometric area-average application, not a measured distribution or an overlapping-layout rate.
/// Flow and radius must describe the same application at the same pressure. For an arc comparison,
/// the caller explicitly assumes that flow and radius remain unchanged as the arc changes.
/// </summary>
public static class IrrigationApplicationRate
{
    public static double? AverageMmPerHour(decimal? flowM3H, decimal? radiusM, decimal? arcDegrees = 360m)
    {
        if (flowM3H is null or < 0m || radiusM is null or <= 0m || arcDegrees is null or <= 0m or > 360m)
            return null;

        var radius = (double)radiusM.Value;
        var sectorAreaM2 = Math.PI * radius * radius * ((double)arcDegrees.Value / 360d);
        var rate = 1000d * (double)flowM3H.Value / sectorAreaM2;
        return double.IsFinite(rate) ? rate : null;
    }
}
