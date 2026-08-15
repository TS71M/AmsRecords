namespace AmsRecords.Procural;

public static class ProcurementSetupReadiness
{
    public const int TotalSteps = 4;

    public static IReadOnlyList<string> FindMissingCapabilities(IEnumerable<string> assignedCapabilities)
    {
        var assigned = assignedCapabilities.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return ProcurementPermissionCatalog.All
            .Where(x => !assigned.Contains(x.Key))
            .Select(x => x.Key)
            .ToList();
    }

    public static bool IsHubStepComplete(bool hubActive, int activeMemberCount)
        => !hubActive || activeMemberCount > 0;

    public static bool IsRoutingReady(int activeFieldCount, int configuredFieldCount)
        => activeFieldCount > 0 && configuredFieldCount == activeFieldCount;

    public static bool IsCatalogReady(
        int activeSupplierCount,
        int activeProductCount,
        int activeProductSupplierCount)
        => activeSupplierCount > 0 &&
           activeProductCount > 0 &&
           activeProductSupplierCount > 0;
}

public sealed record ProcurementSetupReadinessDto(
    [property: JsonPropertyName("moduleEnabled")] bool ModuleEnabled,
    [property: JsonPropertyName("missingCapabilities")] IReadOnlyList<string> MissingCapabilities,
    [property: JsonPropertyName("hubActive")] bool HubActive,
    [property: JsonPropertyName("activeHubMemberCount")] int ActiveHubMemberCount,
    [property: JsonPropertyName("activeFieldCount")] int ActiveFieldCount,
    [property: JsonPropertyName("configuredFieldCount")] int ConfiguredFieldCount,
    [property: JsonPropertyName("activeSupplierCount")] int ActiveSupplierCount,
    [property: JsonPropertyName("activeProductCount")] int ActiveProductCount,
    [property: JsonPropertyName("activeProductSupplierCount")] int ActiveProductSupplierCount)
{
    [JsonPropertyName("responsibilitiesComplete")]
    public bool ResponsibilitiesComplete => MissingCapabilities.Count == 0;

    [JsonPropertyName("hubComplete")]
    public bool HubComplete => ProcurementSetupReadiness.IsHubStepComplete(HubActive, ActiveHubMemberCount);

    [JsonPropertyName("routingComplete")]
    public bool RoutingComplete => ProcurementSetupReadiness.IsRoutingReady(ActiveFieldCount, ConfiguredFieldCount);

    [JsonPropertyName("catalogComplete")]
    public bool CatalogComplete => ProcurementSetupReadiness.IsCatalogReady(
        ActiveSupplierCount,
        ActiveProductCount,
        ActiveProductSupplierCount);

    [JsonPropertyName("completedStepCount")]
    public int CompletedStepCount => new[]
    {
        ResponsibilitiesComplete,
        HubComplete,
        RoutingComplete,
        CatalogComplete
    }.Count(x => x);

    [JsonPropertyName("totalStepCount")]
    public int TotalStepCount => ProcurementSetupReadiness.TotalSteps;

    [JsonPropertyName("isComplete")]
    public bool IsComplete => ModuleEnabled && CompletedStepCount == TotalStepCount;
}
