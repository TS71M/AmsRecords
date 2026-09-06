namespace AmsRecords.Irrigation;

public static class IrrigationNozzleConfigurationMatchStatuses
{
    public const string DocumentedCurrentMatch = "documented-current-match";
    public const string DocumentedHistoricalMatch = "documented-historical-match";
    public const string MixedDocumentedComponents = "mixed-documented-components";
    public const string UndocumentedConfiguration = "undocumented-configuration";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.Ordinal)
    {
        DocumentedCurrentMatch,
        DocumentedHistoricalMatch,
        MixedDocumentedComponents,
        UndocumentedConfiguration
    };
}
