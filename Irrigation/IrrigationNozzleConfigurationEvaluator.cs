using Lib.Enums;
using static AmsRecords.Irrigation.IrrigationDtos;

namespace AmsRecords.Irrigation;

public static partial class IrrigationNozzleConfigurationEvaluator
{
    public const string FactoryDocumentedBackflowEvidenceCode = "factory-documented-backflow";

    public static bool HasManufacturerDocumentedBackflowEvidence(SurfaceSprinklerNozzleDto nozzle)
        => nozzle.PositionKind == IrrigationNozzlePositionKind.Rear &&
           nozzle.ApplicationEvidenceCode == FactoryDocumentedBackflowEvidenceCode &&
           nozzle.ApplicationEvidenceLevel is (IrrigationCompatibilityEvidenceLevel.FactoryDocumented or
               IrrigationCompatibilityEvidenceLevel.SharedPlatformDocumented) &&
           !string.IsNullOrWhiteSpace(nozzle.ApplicationEvidenceSourceUrl);

    public static bool HasManufacturerConfigurationEvidence(IrrigationCompatibilityEvidenceLevel evidence)
        => evidence is IrrigationCompatibilityEvidenceLevel.FactoryDocumented or
            IrrigationCompatibilityEvidenceLevel.SharedPlatformDocumented;

    // Main identity can suggest a comparison reference, never prove an installed generation.
    public static IrrigationNozzleConfigurationDto? ReferenceForMainNozzle(
        IEnumerable<IrrigationNozzleConfigurationDto> configurations, Guid? modelPubId, string? mainPartNumber)
    {
        if (!modelPubId.HasValue || string.IsNullOrWhiteSpace(mainPartNumber)) return null;
        var candidates = configurations.Where(x => x.Active && x.IsApprovedReference &&
            x.Scope == IrrigationConfigurationScope.Global && x.SprinklerModel?.PubId == modelPubId &&
            x.Slots.Any(slot => slot.PositionKind == IrrigationNozzlePositionKind.MainFront &&
                CodeMatches(slot, mainPartNumber))).ToList();
        var documented = candidates.Where(x => HasManufacturerConfigurationEvidence(x.EvidenceLevel)).ToList();
        // An approval flag on an old seed/local observation is not manufacturer documentation.
        // Multiple documented editions remain an explicit user choice; never pick by name or insertion order.
        return documented.Count == 1 ? documented[0] : null;
    }

    public sealed record Result(
        IrrigationNozzleConfigurationAssessment Assessment,
        IReadOnlyList<string> Issues);

    public sealed record ReviewGuidance(string Title, string Instructions);

    // Explain the existing assessment; never approve a reference or change a saved review.
    public static ReviewGuidance GuidanceFor(
        IrrigationNozzleConfigurationDto? reference,
        IrrigationNozzleConfigurationAssessment assessment,
        string? reviewDecision, bool needsReview = false)
    {
        if (assessment == IrrigationNozzleConfigurationAssessment.MinorErrors)
            return new("Check minor installation errors",
                "The front nozzle identities match the documented set. Check the additional rear nozzle application or correct the recorded nozzle angle. This category is not approval of hydraulic performance.");
        if (assessment == IrrigationNozzleConfigurationAssessment.Incompatible)
            return new("Review recorded mismatch",
                "Open the review to inspect the saved mismatch and its evidence. Correct an observation only when verified; do not change nozzle identities merely to match the reference. No new AI analysis is required.");
        if (reference is null || !reference.Active)
            return new("Choose a supported reference, or leave unresolved",
                "Open the review and check the model and Expected nozzle set, including its generation and source document. If no documented set can be established, keep the observations and use Needs further review. Do not guess or run AI just to clear this warning.");
        if (!reference.IsApprovedReference || !HasManufacturerConfigurationEvidence(reference.EvidenceLevel))
            return new("Catalogue evidence needed",
                IrrigationSprinklerReviewDecisions.IsCompleted(reviewDecision)
                    ? "Your review is saved. No repeat review or AI run is required just for this warning. The catalogue needs manufacturer documentation for the exact nozzle combination before compatibility can be confirmed. Keep verified observations unchanged; do not approve a set merely to clear the warning."
                    : "Verify only what the photos or readable markings establish. Save Needs further review for uncertain identities. The catalogue needs manufacturer documentation for the exact nozzle combination; another AI run or guessed nozzle numbers will not establish approval.");
        if (assessment == IrrigationNozzleConfigurationAssessment.Compatible)
        {
            if (needsReview && !IrrigationSprinklerReviewDecisions.IsCompleted(reviewDecision))
                return new("Review saved observations",
                    "The catalogue comparison matches, but the recorded identification still needs review. Open the saved photographs and observations in AdminApp, then save a review decision. Another AI run is not required.");
            return new("No catalogue action needed",
                "The recorded configuration matches the selected reference. This catalogue check does not verify physical condition or installation orientation.");
        }
        return new("Check reference generation and recorded parts",
            "Open the review and check the Expected nozzle set and its source first: different generations may share a main nozzle. Correct only independently verified observations. If markings or photographs cannot resolve a part, keep the uncertainty and save Needs further review. Do not rerun AI just to clear this warning.");
    }

    public static Result Evaluate(
        IrrigationNozzleConfigurationDto? reference,
        IReadOnlyList<SurfaceSprinklerNozzleDto> installedNozzles,
        string? installedManufacturer,
        string? installedModel)
    {
        if (MinorIssues(reference, installedNozzles, installedManufacturer, installedModel) is { Count: > 0 } minor)
            return new(IrrigationNozzleConfigurationAssessment.MinorErrors, minor);
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
                [reference.Scope == IrrigationConfigurationScope.Global
                    ? "The selected catalogue configuration is not an approved compatibility reference. This catalogue-evidence gap is not proof that the recorded installation is wrong."
                    : "The selected local configuration is recorded for reuse but is not an approved compatibility reference."]);
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
                case IrrigationNozzleState.NotPresent when !expected.IsOptional:
                    deviations.Add($"The documented set requires {expected.PositionLabel}, but the observation says this position does not exist. Check the sprinkler model and reference.");
                    continue;
                case IrrigationNozzleState.NotPresent:
                    continue;
                case IrrigationNozzleState.Empty when !expected.IsOptional:
                    deviations.Add($"The documented set contains a required {expected.PositionLabel} nozzle, but the recorded position is empty.");
                    continue;
                case IrrigationNozzleState.Empty:
                    continue;
                case IrrigationNozzleState.Unknown:
                    review.Add($"Confirm the nozzle installed at {expected.PositionLabel}.");
                    continue;
            }

            if (!IdentityMatches(expected, installed) && HasNonContradictoryApplicationEvidence(installed) &&
                (expected.IsOptional || IsPlugIdentity(expected.NozzleCode, expected.NozzleName)))
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
        => nozzle.PositionKind == IrrigationNozzlePositionKind.Rear &&
           nozzle.ApplicationEvidenceCode == FactoryDocumentedBackflowEvidenceCode &&
           nozzle.ApplicationEvidenceLevel is
            IrrigationCompatibilityEvidenceLevel.FactoryDocumented or
            IrrigationCompatibilityEvidenceLevel.SharedPlatformDocumented or
            IrrigationCompatibilityEvidenceLevel.MechanicallyCompatible or
            IrrigationCompatibilityEvidenceLevel.FieldObserved or
            IrrigationCompatibilityEvidenceLevel.HydraulicallyValidated;

    public static bool HasStrongCompatibleApplicationEvidence(SurfaceSprinklerNozzleDto nozzle)
        => HasManufacturerDocumentedBackflowEvidence(nozzle);

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
            else if (!CodeMatches(expected, installed.NozzleCode))
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
            return CodeMatches(expected, installed.NozzleCode);
        if (!string.IsNullOrWhiteSpace(expected.NozzleName) && !string.IsNullOrWhiteSpace(installed.NozzleName))
            return Same(expected.NozzleName, installed.NozzleName);
        return !string.IsNullOrWhiteSpace(expected.Color) &&
               !string.IsNullOrWhiteSpace(installed.Color) &&
               Same(expected.Color, installed.Color);
    }

    static bool CodeMatches(IrrigationNozzleConfigurationSlotDto expected, string observedCode)
        => Same(expected.NozzleCode, observedCode) ||
           (!string.IsNullOrWhiteSpace(expected.OrderingPartNumber) && Same(expected.OrderingPartNumber, observedCode));

    static bool Same(string? left, string? right)
        => string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
}
