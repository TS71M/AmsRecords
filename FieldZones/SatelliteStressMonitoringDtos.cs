using AmsModels;

namespace AmsRecords.FieldZones;

public sealed record SatelliteStressMonitoringSettingDto(
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("enabled")] bool Enabled,
    [property: JsonPropertyName("frequency")] SatelliteStressMonitoringFrequency Frequency,
    [property: JsonPropertyName("maxCloudCoveragePercent")] decimal MaxCloudCoveragePercent,
    [property: JsonPropertyName("minDaysBetweenScans")] int MinDaysBetweenScans,
    [property: JsonPropertyName("monthlyRequestBudget")] int MonthlyRequestBudget,
    [property: JsonPropertyName("lastCatalogCheckUtc")] DateTime? LastCatalogCheckUtc,
    [property: JsonPropertyName("lastProcessedUtc")] DateTime? LastProcessedUtc,
    [property: JsonPropertyName("nextEligibleScanUtc")] DateTime? NextEligibleScanUtc,
    [property: JsonPropertyName("lastProcessedSceneId")] string? LastProcessedSceneId,
    [property: JsonPropertyName("requestCountThisMonth")] int RequestCountThisMonth,
    [property: JsonPropertyName("remainingRequestsThisMonth")] int RemainingRequestsThisMonth
);

public sealed record SatelliteStressMonitoringUpdateDto(
    [property: JsonPropertyName("enabled")] bool Enabled,
    [property: JsonPropertyName("frequency")] SatelliteStressMonitoringFrequency Frequency,
    [property: JsonPropertyName("maxCloudCoveragePercent")] decimal MaxCloudCoveragePercent,
    [property: JsonPropertyName("minDaysBetweenScans")] int MinDaysBetweenScans,
    [property: JsonPropertyName("monthlyRequestBudget")] int MonthlyRequestBudget
);

public sealed record SatelliteStressScanRunDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("startedUtc")] DateTime StartedUtc,
    [property: JsonPropertyName("completedUtc")] DateTime? CompletedUtc,
    [property: JsonPropertyName("trigger")] string Trigger,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("requestCount")] int RequestCount,
    [property: JsonPropertyName("processingUnitsEstimate")] decimal ProcessingUnitsEstimate,
    [property: JsonPropertyName("sceneId")] string? SceneId,
    [property: JsonPropertyName("message")] string? Message
);
