using static AmsRecords.Irrigation.SprinklerPerformanceDtos;

namespace AmsRecords.Irrigation;

public sealed record DistributionCoverageRow(
    Guid NozzlePubId, string Manufacturer, string Model, string NozzleCode, string NozzleName,
    string Color, bool Active, bool IsLegacy, int PerformancePoints, int MeasuredProfiles,
    int DerivedProfiles, int ModeledProfiles, int RetainedTests, int InactiveProfiles,
    IReadOnlyList<decimal> MeasuredPressuresBar);

public sealed record DistributionCoveragePage(
    int Total, int MissingMeasured, int WithMeasured, int WithRetainedTests,
    int Page, int PageSize, int FilteredTotal, IReadOnlyList<DistributionCoverageRow> Items);

/// <summary>A versioned import contract, independent of any legacy vendor file format.</summary>
public sealed record DistributionMeasurementImport
{
    public int SchemaVersion { get; init; } = 1;
    public string Manufacturer { get; init; } = "";
    public string ModelCode { get; init; } = "";
    public string NozzleCode { get; init; } = "";
    public string Position { get; init; } = "";
    public string Generation { get; init; } = "";
    public string TestId { get; init; } = "";
    public DateOnly TestDate { get; init; }
    public string Source { get; init; } = "";
    public string SourceSha256 { get; init; } = "";
    public string ReusePermission { get; init; } = "";
    public string TestConditions { get; init; } = "";
    public string ApplicationScope { get; init; } = "NozzleOption";
    public string Pattern { get; init; } = "Radial";
    public decimal PressureBar { get; init; }
    public decimal ArcDegrees { get; init; } = 360;
    public decimal HeightM { get; init; }
    public decimal FlowM3H { get; init; }
    public string DistanceUnit { get; init; } = "m";
    public string ApplicationUnit { get; init; } = "mm/h";
    public IReadOnlyList<DistributionMeasurementPoint> Points { get; init; } = [];
}

public sealed record DistributionMeasurementPoint(decimal Distance, decimal Application);
public sealed record DistributionTestContext(decimal RadiusM, decimal FlowM3H, decimal ArcDegrees, decimal HeightM);
public sealed record DistributionImportRequest(string Json, bool Confirmed = false);
public sealed record DistributionImportPreview(
    DistributionMeasurementImport Evidence, decimal RadiusM,
    IReadOnlyList<SprinklerDistributionPointSaveDto> NormalizedPoints, string Warning,
    double? IntegratedFlowM3H = null, double? FlowRecoveryPercent = null);
