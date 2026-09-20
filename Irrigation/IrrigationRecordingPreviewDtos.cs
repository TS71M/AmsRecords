using Lib.Enums;
using static AmsRecords.Irrigation.IrrigationVisualSimulatorDtos;

namespace AmsRecords.Irrigation;

public sealed record IrrigationRecordingPreviewNozzle(int Position, Guid? NozzleOptionPubId, IrrigationNozzleState State);
public sealed record IrrigationRecordingPreviewRequest(Guid FieldPubId, Guid SurfacePubId,
    Guid ModelPubId, Guid? ConfigurationPubId, double Latitude, double Longitude,
    double RuntimeMinutes, decimal? PressureBar, decimal? ArcDegrees, decimal? BearingDegrees,
    IReadOnlyList<IrrigationRecordingPreviewNozzle> Nozzles, decimal? TrajectoryDegrees = null,
    bool UseRecommendedConfiguration = false, bool UseDocumentedPressure = false);
public sealed record IrrigationRecordingPreviewResult(IrrigationSimulatorGridDto Grid,
    IrrigationSimulatorHeadResultDto Head, IReadOnlyList<string> Warnings, IrrigationMapBasis Basis);
