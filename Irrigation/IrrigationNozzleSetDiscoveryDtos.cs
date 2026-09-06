namespace AmsRecords.Irrigation;

public sealed record IrrigationNozzleSetDiscoveryRow(
    Guid PubId, Guid? BatchPubId, string Manufacturer, string Summary, string Status,
    string DocumentTitle, string Revision, int SourcePage, string Check = "New");

public sealed record IrrigationNozzleSetDiscoveryJob(
    Guid PubId, string DocumentTitle, string Status, DateTime? CompletedAtUtc,
    int AddedCount, int ConfirmedCount, string FailureSummary, IrrigationCatalogImportDtos.IrrigationCatalogImportProgressDto? Progress);

public sealed record IrrigationNozzleSetDiscoveryPage(
    IReadOnlyList<IrrigationNozzleSetDiscoveryRow> Items, int TotalCount, int Page,
    bool SearchLimitReached, IReadOnlyList<IrrigationNozzleSetDiscoveryJob> Jobs);
