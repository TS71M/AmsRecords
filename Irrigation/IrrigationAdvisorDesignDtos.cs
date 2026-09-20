namespace AmsRecords.Irrigation;

public static class IrrigationAdvisorDesignDtos
{
    public const string ChangeNozzles = "nozzles";
    public const string ReplaceSprinklers = "replace";

    public sealed record DesignRequest(Guid FieldPubId, Guid SurfacePubId, string Mode,
        Guid? ModelPubId = null, decimal? MaximumPressureBar = null, double TargetDepthMm = 5, bool UseRoleEstimates = false,
        string? T7FlowFamily = null, bool OptimizeLocations = false);
    public sealed record ModelOption(Guid PubId, string Brand, string Name, string Code, string SourceUrl);
    public sealed record Position(Guid PubId, string Name, Guid? ModelPubId, string ModelName,
        double Latitude, double Longitude);
    public sealed record SurfaceOption(Guid PubId, string Name, int SprinklerCount);
    public sealed record Workspace(Guid FieldPubId, Guid SurfacePubId, string SurfaceName,
        IrrigationDigitalTwinDtos.IrrigationAreaBoundaryDto Boundary, IrrigationBoundaryMapDto Map,
        IReadOnlyList<Position> Positions, IReadOnlyList<ModelOption> ReplacementModels,
        IReadOnlyList<SurfaceOption>? Surfaces = null);
    public sealed record NozzlePart(string Position, string PartNumber, string Name, string Color = "",
        string? EstimatedRangeRole = null, double? AssumedFlowSharePercent = null,
        double? AssumedStartRadiusFraction = null, double? AssumedPeakRadiusFraction = null, double? AssumedEndRadiusFraction = null);
    public sealed record Setting(Guid HeadPubId, string Name, Guid ModelPubId, string ModelName,
        Guid NozzlePubId, string NozzleName, IReadOnlyList<NozzlePart> Parts,
        decimal PressureBar, decimal ArcDegrees, decimal BearingDegrees, double RadiusM, double FlowM3H,
        string Evidence, double Latitude = 0, double Longitude = 0, string NozzleCode = "", string NozzleColor = "", double? RuntimeMinutes = null,
        string? DistributionAssumptions = null, bool Enabled = true, string LayoutAction = "Keep",
        double? OriginalLatitude = null, double? OriginalLongitude = null);
    public sealed record DesignOption(int Rank, double Score, double CoveragePercent,
        double? UniformityPercent, double? LowQuarterPercent, double OutsidePercent,
        IReadOnlyList<Setting> Settings, IrrigationVisualSimulatorDtos.IrrigationSimulatorGridDto Grid, double? TargetDepthMm = null);
    public sealed record CurrentSetup(IReadOnlyList<IrrigationVisualSimulatorDtos.IrrigationSimulatorHeadDto> Heads,
        DesignOption? Option, IReadOnlyList<string> Notes);
    public sealed record DesignResult(Guid SurfacePubId, string SurfaceName, string Mode,
        IReadOnlyList<DesignOption> Options, int TestedCandidates, int TestedCombinations,
        double GridResolutionM, IReadOnlyList<string> Notes, string Fingerprint = "", Workspace? Basis = null, CurrentSetup? Current = null);
    public sealed record SaveScenario(DesignRequest Design, int Rank, string Name, string Fingerprint);
    public sealed record ScenarioSummary(Guid PubId, string Name, string Mode, DateTime CreatedUtc);
    public sealed record ScenarioSnapshot(int Version, DesignRequest Request, Workspace Workspace, DesignResult Result);
    public sealed record SavedScenario(ScenarioSummary Summary, ScenarioSnapshot Snapshot);
}

// A candidate is already resolved against the model's supported performance and arc range.
public sealed record IrrigationDesignCandidate(
    IrrigationAdvisorDesignDtos.Setting Setting, IrrigationSimulationHead SimulationHead, decimal? PreferredPressureBar = null);
public sealed record IrrigationDesignPosition(Guid PubId, string Name,
    IReadOnlyList<IrrigationDesignCandidate> Candidates);
