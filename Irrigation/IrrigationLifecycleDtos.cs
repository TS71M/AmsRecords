using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Irrigation;

public sealed record IrrigationLifecycleResearchRequest(
    [Required, MaxLength(120)] string ManufacturerName,
    [Required, RegularExpression("^(Sprinkler|Nozzle)$")] string TargetKind,
    [Required, MaxLength(160)] string Identifier);

public sealed record IrrigationLifecycleResearchDto(
    Guid PubId, string ManufacturerName, string TargetKind, string Identifier, string State,
    string ProductionStatus, string ProductionSourceUrl,
    string AvailabilityStatus, string AvailabilitySourceUrl,
    string DocumentationStatus, string DocumentationSourceUrl,
    string EvidenceSummary, DateTime CreatedAtUtc, DateTime? CheckedAtUtc, DateTime? ReviewedAtUtc);
