using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public static class ApplicationPlanAreaSnapshotExtensions
{
    public static bool UsesSelectedSurfaceArea(this ApplicationPlanAreaSnapshotDto snapshot)
        => snapshot.Surfaces.Count > 0 && snapshot.SurfaceAreaM2 ==
            decimal.Round(snapshot.Surfaces.Where(surface => surface.Included).Sum(surface => surface.SurfaceAreaM2),
                1, MidpointRounding.AwayFromZero);
}
