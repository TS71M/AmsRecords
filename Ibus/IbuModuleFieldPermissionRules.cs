namespace AmsRecords.Ibus;

public static class IbuModuleFieldPermissionRules
{
    public static bool IsFieldEnabled(bool restrictToSelectedFields, bool hasFieldPermission)
        => !restrictToSelectedFields || hasFieldPermission;

    public static bool MustRevokeFieldPermissions(bool moduleIsEnabled)
        => !moduleIsEnabled;
}
