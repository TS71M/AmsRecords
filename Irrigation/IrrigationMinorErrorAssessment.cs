using Lib.Enums;
using static AmsRecords.Irrigation.IrrigationDtos;

namespace AmsRecords.Irrigation;

public static partial class IrrigationNozzleConfigurationEvaluator
{
    // Classification is derived from identities, never from issue-message wording or colour alone.
    // Unknown front parts, absent required ports and contradicting evidence cannot be downgraded.
    public static IReadOnlyList<string>? MinorIssues(IrrigationNozzleConfigurationDto? reference,
        IReadOnlyList<SurfaceSprinklerNozzleDto> observed, string? manufacturer, string? model)
    {
        if (reference is not { Active: true, IsApprovedReference: true, SprinklerModel: { Active: true } body } ||
            !HasManufacturerConfigurationEvidence(reference.EvidenceLevel) ||
            !Same(body.ManufacturerName, manufacturer) || !Same(body.ModelName, model) ||
            observed.GroupBy(x => x.Position).Any(g => g.Count() != 1) ||
            observed.Any(x => x.Position < 1 || x.Position > body.MaximumNozzleCount ||
                x.ApplicationEvidenceLevel == IrrigationCompatibilityEvidenceLevel.Contradictory)) return null;
        var front = reference.Slots.Where(x => x.PositionKind != IrrigationNozzlePositionKind.Rear).ToList();
        if (!front.Any(x => x.PositionKind == IrrigationNozzlePositionKind.MainFront)) return null;
        foreach (var expected in reference.Slots)
        {
            var actual = observed.SingleOrDefault(x => x.Position == expected.Position);
            if (actual is null || actual.State is IrrigationNozzleState.Empty or IrrigationNozzleState.NotPresent)
            {
                if (!expected.IsOptional) return null;
                continue;
            }
            if (actual.State != IrrigationNozzleState.Installed || actual.PositionKind != expected.PositionKind ||
                string.IsNullOrWhiteSpace(actual.NozzleCode)) return null;
            if (expected.PositionKind != IrrigationNozzlePositionKind.Rear &&
                (actual.CompatibilityOverride || string.IsNullOrWhiteSpace(expected.NozzleCode) ||
                 !CodeMatches(expected, actual.NozzleCode))) return null;
            // Replacing a required flowing partner is not an additional back nozzle.
            // Keep optional backflow and a spray nozzle replacing a plug as minor
            // application differences; paired-nozzle size conflicts need a real review.
            if (expected.PositionKind == IrrigationNozzlePositionKind.Rear && !expected.IsOptional &&
                !IsPlugIdentity(expected.NozzleCode, expected.NozzleName) && !CodeMatches(expected, actual.NozzleCode)) return null;
        }
        if (observed.Any(x => !reference.Slots.Any(s => s.Position == x.Position) &&
            x.State is not (IrrigationNozzleState.Empty or IrrigationNozzleState.NotPresent) &&
            (x.PositionKind != IrrigationNozzlePositionKind.Rear || x.State != IrrigationNozzleState.Installed ||
             string.IsNullOrWhiteSpace(x.NozzleCode)))) return null;

        var issues = new List<string>();
        foreach (var nozzle in observed.Where(x => x.State == IrrigationNozzleState.Installed))
        {
            if (nozzle.PositionKind == IrrigationNozzlePositionKind.Rear &&
                !reference.Slots.Any(s => s.Position == nozzle.Position && CodeMatches(s, nozzle.NozzleCode)))
            {
                // A substituted plug is not an additional spray nozzle.
                if (IsPlugIdentity(nozzle.NozzleCode, nozzle.NozzleName)) return null;
                issues.Add($"Minor error: additional rear nozzle {nozzle.NozzleCode} at {nozzle.PositionLabel}. " +
                    (HasManufacturerDocumentedBackflowEvidence(nozzle) ? "Manufacturer application documented; verify operating conditions." : "Verify the rear application and its hydraulic effect."));
            }
            if (nozzle.InstallationOrientationStatus == "suspected-misaligned")
                issues.Add($"Minor error: wrong nozzle angle / misalignment at {nozzle.PositionLabel}. {nozzle.InstallationOrientationReason}".Trim());
        }
        return issues;
    }

    static bool IsPlugIdentity(string? code, string? name)
        => code == "102-4335" || (name?.Contains("plug", StringComparison.OrdinalIgnoreCase) ?? false);
}
