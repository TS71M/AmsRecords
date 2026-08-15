namespace AmsRecords.Ibus;

public static class SystemIbuRules
{
    public const string BusinessUnitName = "System";

    public static bool IsSystemIbu(string? businessUnitName)
        => string.Equals(
            businessUnitName?.Trim(),
            BusinessUnitName,
            StringComparison.OrdinalIgnoreCase);
}
