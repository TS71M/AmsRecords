using AmsRecords.Ibus;

namespace AmsRecords.FieldMeasurements;

public static class OrganizationMeasurementHubDtos
{
    public const string Route = "organization-measurement-hubs";
    public const int MaximumDays = 366;
    public const int MaximumReadings = 10000;

    public static bool Supports(string module) => module is ModuleAccessCatalog.SoilTests or ModuleAccessCatalog.WaterTests
        or ModuleAccessCatalog.ClippingVolume or ModuleAccessCatalog.CuttingHeight;

    public sealed record Overview(IReadOnlyList<Site> Sites, IReadOnlyList<Reading> Readings, bool LimitExceeded = false);
    public sealed record Site(Guid FieldPubId, string FieldName, string OwnerName);
    public sealed record Reading(Guid FieldPubId, Guid RecordPubId, DateTime DateUtc, string Location,
        string MetricKey, string Metric, string? Unit, string? Method, decimal? Value, string? Text,
        Guid? AreaPubId = null, string? MethodKey = null, string? UnitKey = null,
        string? AreaGroupCode = null, string? AreaGroupName = null);
}
