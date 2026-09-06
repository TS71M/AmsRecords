namespace AmsRecords.Irrigation;

// Optional, transient visual evidence. Absence never implies a completed analysis.
public sealed record IrrigationRecognitionCrop(double X, double Y, double Width, double Height);
public sealed record IrrigationRecognitionCandidate(Guid ImagePubId, string PartNumber, string Color,
    string Assessment, string Reason)
{
    // Content-addressed, nozzle-only preview. Older servers omit these additive fields.
    public string? PreviewVersion { get; init; }
    public bool? PreviewAvailable { get; init; }
}
public sealed record IrrigationRecognitionNozzleFrame(int Position, string Label, Guid? SourceImagePubId,
    IrrigationRecognitionCrop? Crop, string Color, string Geometry, string PartNumber,
    string Outcome, string OrientationWarning, IReadOnlyList<IrrigationRecognitionCandidate> Candidates)
{
    public IReadOnlyList<IrrigationRecognitionCandidate> Shortlist => Candidates
        .Where(c => c.Assessment is "supported" or "uncertain")
        .OrderByDescending(c => c.Assessment == "supported")
        .DistinctBy(c => c.PartNumber, StringComparer.OrdinalIgnoreCase).Take(3).ToList();
}
public sealed record IrrigationRecognitionProgressDto(Guid RunId, int Revision, string Stage,
    IReadOnlyList<IrrigationRecognitionNozzleFrame> Nozzles, Guid? SprinklerPubId = null);
