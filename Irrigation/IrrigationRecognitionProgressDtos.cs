namespace AmsRecords.Irrigation;

// Optional, transient visual evidence. Absence never implies a completed analysis.
public sealed record IrrigationRecognitionCrop(double X, double Y, double Width, double Height);
public sealed record IrrigationRecognitionCandidate(Guid ImagePubId, string PartNumber, string Color,
    string Assessment, string Reason)
{
    public string ManufacturerName { get; init; } = "";
    public Guid? NozzleOptionPubId { get; init; }
    public string SprinklerModelCode { get; init; } = "";
    public string DisplayName => string.Join(" · ", new[] { ManufacturerName, SprinklerModelCode, PartNumber }.Where(x => !string.IsNullOrWhiteSpace(x)));
    [System.Text.Json.Serialization.JsonIgnore]
    public string SelectionKey => NozzleOptionPubId is { } id && id != Guid.Empty
        ? $"option:{id:D}" : System.Text.Json.JsonSerializer.Serialize(new[] { Identity.Manufacturer, Identity.Part });
    public IrrigationDtos.IrrigationSprinklerNozzleOptionDto? ResolveCatalogueOption(IEnumerable<IrrigationDtos.IrrigationSprinklerNozzleOptionDto> options)
    {
        static string Normalize(string? value) => (value ?? "").Trim().ToUpperInvariant();
        var matches = options.Where(option => option.Active &&
            (!NozzleOptionPubId.HasValue || option.PubId == NozzleOptionPubId.Value) &&
            (string.IsNullOrWhiteSpace(ManufacturerName) || Normalize(option.ManufacturerName) == Normalize(ManufacturerName)) &&
            Normalize(option.NozzleCode) == Normalize(PartNumber) &&
            Normalize(option.Color).Replace("GREY", "GRAY") == Normalize(Color).Replace("GREY", "GRAY")).Take(2).ToList();
        return matches.Count == 1 ? matches[0] : null;
    }
    [System.Text.Json.Serialization.JsonIgnore]
    public (string Manufacturer, string Part) Identity => IdentityFor(ManufacturerName, PartNumber);
    public static (string Manufacturer, string Part) IdentityFor(string? manufacturer, string? part)
        => ((manufacturer ?? "").Trim().ToUpperInvariant(), (part ?? "").Trim().ToUpperInvariant());
    public const string NozzleFacePreviewKind = "nozzle-face-v2";
    public string? PreviewKind { get; init; }
    // Content-addressed, nozzle-only preview. Older servers omit these additive fields.
    public string? PreviewVersion { get; init; }
    public bool? PreviewAvailable { get; init; }
    public bool HasNozzleFacePreview => PreviewAvailable == true && PreviewKind == NozzleFacePreviewKind &&
        PreviewVersion is { Length: 64 } version && version.All(Uri.IsHexDigit);
}
public sealed record IrrigationRecognitionNozzleFrame(int Position, string Label, Guid? SourceImagePubId,
    IrrigationRecognitionCrop? Crop, string Color, string Geometry, string PartNumber,
    string Outcome, string OrientationWarning, IReadOnlyList<IrrigationRecognitionCandidate> Candidates)
{
    public string ManufacturerName { get; init; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public (string Manufacturer, string Part) Identity => IrrigationRecognitionCandidate.IdentityFor(ManufacturerName, PartNumber);
    public Guid? NozzleOptionPubId { get; init; }
    // Visible identities from this request, not AI recommendations or installation compatibility.
    public IReadOnlyList<IrrigationRecognitionCandidate> ManualAlternatives { get; init; } = [];
    // Position-specific catalogue identities with no visual recommendation or asserted image.
    public IReadOnlyList<IrrigationRecognitionCandidate> CatalogueChoices { get; init; } = [];
    public IReadOnlyList<IrrigationRecognitionCandidate> Shortlist => Candidates
        .Where(c => c.HasNozzleFacePreview && c.Assessment is "supported" or "uncertain")
        .OrderByDescending(c => c.Assessment == "supported")
        .DistinctBy(c => c.SelectionKey).Take(3).ToList();
}
public sealed record IrrigationRecognitionProgressDto(Guid RunId, int Revision, string Stage,
    IReadOnlyList<IrrigationRecognitionNozzleFrame> Nozzles, Guid? SprinklerPubId = null);
