namespace AmsRecords.Irrigation;

public static class IrrigationRecordingDefaults
{
    public const decimal PressurePsi = 65m;
    // Match the catalogue's three-decimal bar precision so 65 PSI equals its 4.482 bar row.
    public static readonly decimal PressureBar = decimal.Round(PressurePsi * 0.06894757293168m, 3);
    public const decimal ArcDegrees = 180m;
    public const decimal TrajectoryDegrees = 25m;
}
