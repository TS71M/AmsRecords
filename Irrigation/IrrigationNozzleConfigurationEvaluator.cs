using Lib.Enums;
using static AmsRecords.Irrigation.IrrigationDtos;

namespace AmsRecords.Irrigation;

public static class IrrigationNozzleConfigurationEvaluator
{
    public const string FactoryDocumentedBackflowEvidenceCode = "factory-documented-backflow";

    public sealed record Result(
        IrrigationNozzleConfigurationAssessment Assessment,
        IReadOnlyList<string> Issues);

    public static Result Evaluate(
        IrrigationNozzleConfigurationDto? reference,
        IReadOnlyList<SurfaceSprinklerNozzleDto> installedNozzles,
        string? installedManufacturer,
        string? installedModel)
    {
        var catalogOverrides = installedNozzles
            .Where(x => x.State == IrrigationNozzleState.Installed &&
                        x.CompatibilityOverride &&
                        !HasNonContradictoryApplicationEvidence(x))
            .OrderBy(x => x.Position)
            .Select(x => $"{x.PositionLabel} uses an installed nozzle that is not compatible with the selected sprinkler model.")
            .Distinct()
            .ToList();
        if (catalogOverrides.Count > 0)
            return new(IrrigationNozzleConfigurationAssessment.Incompatible, catalogOverrides);

        if (reference is null)
            return new(IrrigationNozzleConfigurationAssessment.ReviewRequired, ["No reference nozzle configuration is selected."]);
        if (!reference.IsApprovedReference)
        {
            return new(
                IrrigationNozzleConfigurationAssessment.ReviewRequired,
                ["The selected local configuration is recorded for reuse but is not an approved compatibility reference."]);
        }

        var deviations = new List<string>();
        var review = new List<string>();
        CompareModel(reference, installedManufacturer, installedModel, deviations, review);

        var installedByPosition = installedNozzles
            .Where(x => x.Position is >= 1 and <= IrrigationRules.MaximumNozzlesPerSprinkler)
            .GroupBy(x => x.Position)
            .ToDictionary(x => x.Key, x => x.First());
        var unmatchedInstalledPositions = installedByPosition.Keys.ToHashSet();

        foreach (var expected in reference.Slots.OrderBy(x => x.Position))
        {
            var installed = installedByPosition.Values
                .Where(candidate =>
                    unmatchedInstalledPositions.Contains(candidate.Position) &&
                    IrrigationRules.AreNozzlePositionsInterchangeable(candidate.Position, expected.Position))
                .OrderByDescending(candidate => IdentityMatches(expected, candidate))
                .ThenByDescending(candidate => candidate.Position == expected.Position)
                .FirstOrDefault();
            if (installed is null)
            {
                if (!expected.IsOptional)
                    deviations.Add($"The documented set contains a required {expected.PositionLabel} nozzle, but none is recorded.");
                continue;
            }
            unmatchedInstalledPositions.Remove(installed.Position);

            switch (installed.State)
            {
                case IrrigationNozzleState.Empty when !expected.IsOptional:
                    deviations.Add($"The documented set contains a required {expected.PositionLabel} nozzle, but the recorded position is empty.");
                    continue;
                case IrrigationNozzleState.Empty:
                    continue;
                case IrrigationNozzleState.Unknown:
                    review.Add($"Confirm the nozzle installed at {expected.PositionLabel}.");
                    continue;
            }

            if (!IdentityMatches(expected, installed) && HasNonContradictoryApplicationEvidence(installed))
            {
                AddSupplementalEvidenceReview(installed, review);
                continue;
            }

            CompareIdentity(expected, installed, deviations, review);
        }

        foreach (var extra in installedNozzles
                     .Where(x => x.State == IrrigationNozzleState.Installed && unmatchedInstalledPositions.Contains(x.Position))
                     .OrderBy(x => x.Position))
        {
            if (HasNonContradictoryApplicationEvidence(extra))
            {
                AddSupplementalEvidenceReview(extra, review);
                continue;
            }

            deviations.Add($"The installed nozzle at {extra.PositionLabel} is not listed in the selected documented set.");
        }

        if (deviations.Count > 0)
        {
            var issues = deviations.Concat(review).Distinct().ToList();
            if (reference.EvidenceLevel == IrrigationCompatibilityEvidenceLevel.Contradictory)
                return new(IrrigationNozzleConfigurationAssessment.Incompatible, issues);

            issues.Add("This difference is not proof of mechanical or hydraulic incompatibility; verify it against additional manufacturer or field evidence.");
            return new(IrrigationNozzleConfigurationAssessment.ReviewRequired, issues.Distinct().ToList());
        }
        if (review.Count > 0)
            return new(IrrigationNozzleConfigurationAssessment.ReviewRequired, review.Distinct().ToList());
        return new(IrrigationNozzleConfigurationAssessment.Compatible, []);
    }

    public static bool HasNonContradictoryApplicationEvidence(SurfaceSprinklerNozzleDto nozzle)
        => nozzle.ApplicationEvidenceLevel is
            IrrigationCompatibilityEvidenceLevel.FactoryDocumented or
            IrrigationCompatibilityEvidenceLevel.SharedPlatformDocumented or
            IrrigationCompatibilityEvidenceLevel.MechanicallyCompatible or
            IrrigationCompatibilityEvidenceLevel.FieldObserved or
            IrrigationCompatibilityEvidenceLevel.HydraulicallyValidated;

    public static bool HasStrongCompatibleApplicationEvidence(SurfaceSprinklerNozzleDto nozzle)
        => nozzle.ApplicationEvidenceLevel is
            IrrigationCompatibilityEvidenceLevel.FactoryDocumented or
            IrrigationCompatibilityEvidenceLevel.SharedPlatformDocumented or
            IrrigationCompatibilityEvidenceLevel.HydraulicallyValidated;

    static void AddSupplementalEvidenceReview(
        SurfaceSprinklerNozzleDto nozzle,
        ICollection<string> review)
    {
        if (HasStrongCompatibleApplicationEvidence(nozzle))
            return;

        review.Add(string.IsNullOrWhiteSpace(nozzle.ApplicationEvidenceSummary)
            ? $"Verify the supplemental nozzle application at {nozzle.PositionLabel}."
            : nozzle.ApplicationEvidenceSummary);
    }

    static void CompareModel(
        IrrigationNozzleConfigurationDto reference,
        string? installedManufacturer,
        string? installedModel,
        ICollection<string> deviations,
        ICollection<string> review)
    {
        var expected = reference.SprinklerModel;
        if (expected is null)
            return;

        if (string.IsNullOrWhiteSpace(installedManufacturer) || string.IsNullOrWhiteSpace(installedModel))
        {
            review.Add("Confirm the installed sprinkler manufacturer and model.");
            return;
        }

        if (!Same(installedManufacturer, expected.ManufacturerName) || !Same(installedModel, expected.ModelName))
            deviations.Add($"The selected documented set is associated with {expected.ManufacturerName} {expected.ModelName}.");
    }

    static void CompareIdentity(
        IrrigationNozzleConfigurationSlotDto expected,
        SurfaceSprinklerNozzleDto installed,
        ICollection<string> deviations,
        ICollection<string> review)
    {
        if (!string.IsNullOrWhiteSpace(expected.NozzleCode))
        {
            if (string.IsNullOrWhiteSpace(installed.NozzleCode))
                review.Add($"Confirm nozzle code {expected.NozzleCode} at {expected.PositionLabel}.");
            else if (!Same(installed.NozzleCode, expected.NozzleCode))
                deviations.Add($"{expected.PositionLabel} uses nozzle {installed.NozzleCode}; the selected documented set lists {expected.NozzleCode}.");
        }
        else if (!string.IsNullOrWhiteSpace(expected.NozzleName))
        {
            if (string.IsNullOrWhiteSpace(installed.NozzleName))
                review.Add($"Confirm {expected.NozzleName} at {expected.PositionLabel}.");
            else if (!Same(installed.NozzleName, expected.NozzleName))
                deviations.Add($"{expected.PositionLabel} uses {installed.NozzleName}; the selected documented set lists {expected.NozzleName}.");
        }

        if (!string.IsNullOrWhiteSpace(expected.Color) &&
            !string.IsNullOrWhiteSpace(installed.Color) &&
            !Same(installed.Color, expected.Color))
        {
            review.Add($"Color at {expected.PositionLabel} differs from the reference; verify the nozzle code.");
        }
    }

    static bool IdentityMatches(
        IrrigationNozzleConfigurationSlotDto expected,
        SurfaceSprinklerNozzleDto installed)
    {
        if (expected.NozzleOptionPubId.HasValue && installed.NozzleOptionPubId.HasValue)
            return expected.NozzleOptionPubId.Value == installed.NozzleOptionPubId.Value;
        if (!string.IsNullOrWhiteSpace(expected.NozzleCode) && !string.IsNullOrWhiteSpace(installed.NozzleCode))
            return Same(expected.NozzleCode, installed.NozzleCode);
        if (!string.IsNullOrWhiteSpace(expected.NozzleName) && !string.IsNullOrWhiteSpace(installed.NozzleName))
            return Same(expected.NozzleName, installed.NozzleName);
        return !string.IsNullOrWhiteSpace(expected.Color) &&
               !string.IsNullOrWhiteSpace(installed.Color) &&
               Same(expected.Color, installed.Color);
    }

    static bool Same(string? left, string? right)
        => string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
}
