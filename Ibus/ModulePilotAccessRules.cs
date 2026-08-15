namespace AmsRecords.Ibus;

public static class ModulePilotAccessRules
{
    public static bool IsGrantActive(bool active, DateTime? expiresAtUtc, DateTime utcNow)
        => active && (!expiresAtUtc.HasValue || expiresAtUtc.Value > utcNow);

    public static bool IsAvailable(
        bool isGloballyReleased,
        bool isPilotCapable,
        bool grantActive,
        DateTime? expiresAtUtc,
        DateTime utcNow)
        => isGloballyReleased ||
           (isPilotCapable && IsGrantActive(grantActive, expiresAtUtc, utcNow));
}
