using Lib.Enums;
using static AmsRecords.Irrigation.IrrigationDtos;

namespace AmsRecords.Irrigation;

public static class IrrigationNozzleConfigurationEvaluator
{
    public sealed record Result(
        IrrigationNozzleConfigurationAssessment Assessment,
        IReadOnlyList<string> Issues);

    public static Result Evaluate(
        IrrigationNozzleConfigurationDto? reference,
        IReadOnlyList<SurfaceSprinklerNozzleDto> installedNozzles,
        string? installedManufacturer,
        string? installedModel)
    {
        if (reference is null)
            return new(IrrigationNozzleConfigurationAssessment.ReviewRequired, ["No reference nozzle configuration is selected."]);
        if (!reference.IsApprovedReference)
        {
            return new(
                IrrigationNozzleConfigurationAssessment.ReviewRequired,
                ["The selected local configuration is recorded for reuse but is not an approved compatibility reference."]);
        }

        var incompatible = new List<string>();
        var review = new List<string>();
        CompareModel(reference, installedManufacturer, installedModel, incompatible, review);

        var installedByPosition = installedNozzles
            .Where(x => x.Position is >= 1 and <= IrrigationRules.MaximumNozzlesPerSprinkler)
            .GroupBy(x => x.Position)
            .ToDictionary(x => x.Key, x => x.First());
        var referenceByPosition = reference.Slots
            .GroupBy(x => x.Position)
            .ToDictionary(x => x.Key, x => x.First());

        foreach (var expected in reference.Slots.OrderBy(x => x.Position))
        {
            if (!installedByPosition.TryGetValue(expected.Position, out var installed))
            {
                if (!expected.IsOptional)
                    incompatible.Add($"Missing required {expected.PositionLabel} nozzle.");
                continue;
            }

            if (installed.PositionKind != expected.PositionKind)
            {
                incompatible.Add($"{expected.PositionLabel} is assigned to {installed.PositionKind} instead of {expected.PositionKind}.");
                continue;
            }

            switch (installed.State)
            {
                case IrrigationNozzleState.Empty when !expected.IsOptional:
                    incompatible.Add($"Required {expected.PositionLabel} nozzle is empty.");
                    continue;
                case IrrigationNozzleState.Empty:
                    continue;
                case IrrigationNozzleState.Unknown:
                    review.Add($"Confirm the nozzle installed at {expected.PositionLabel}.");
                    continue;
            }

            CompareIdentity(expected, installed, incompatible, review);
        }

        foreach (var extra in installedNozzles
                     .Where(x => x.State == IrrigationNozzleState.Installed && !referenceByPosition.ContainsKey(x.Position))
                     .OrderBy(x => x.Position))
        {
            incompatible.Add($"Unexpected installed nozzle at {extra.PositionLabel}.");
        }

        if (incompatible.Count > 0)
            return new(IrrigationNozzleConfigurationAssessment.Incompatible, incompatible.Concat(review).Distinct().ToList());
        if (review.Count > 0)
            return new(IrrigationNozzleConfigurationAssessment.ReviewRequired, review.Distinct().ToList());
        return new(IrrigationNozzleConfigurationAssessment.Compatible, []);
    }

    static void CompareModel(
        IrrigationNozzleConfigurationDto reference,
        string? installedManufacturer,
        string? installedModel,
        ICollection<string> incompatible,
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
            incompatible.Add($"Selected configuration is for {expected.ManufacturerName} {expected.ModelName}.");
    }

    static void CompareIdentity(
        IrrigationNozzleConfigurationSlotDto expected,
        SurfaceSprinklerNozzleDto installed,
        ICollection<string> incompatible,
        ICollection<string> review)
    {
        if (!string.IsNullOrWhiteSpace(expected.NozzleCode))
        {
            if (string.IsNullOrWhiteSpace(installed.NozzleCode))
                review.Add($"Confirm nozzle code {expected.NozzleCode} at {expected.PositionLabel}.");
            else if (!Same(installed.NozzleCode, expected.NozzleCode))
                incompatible.Add($"{expected.PositionLabel} uses nozzle {installed.NozzleCode}; expected {expected.NozzleCode}.");
        }
        else if (!string.IsNullOrWhiteSpace(expected.NozzleName))
        {
            if (string.IsNullOrWhiteSpace(installed.NozzleName))
                review.Add($"Confirm {expected.NozzleName} at {expected.PositionLabel}.");
            else if (!Same(installed.NozzleName, expected.NozzleName))
                incompatible.Add($"{expected.PositionLabel} uses {installed.NozzleName}; expected {expected.NozzleName}.");
        }

        if (!string.IsNullOrWhiteSpace(expected.Color) &&
            !string.IsNullOrWhiteSpace(installed.Color) &&
            !Same(installed.Color, expected.Color))
        {
            review.Add($"Color at {expected.PositionLabel} differs from the reference; verify the nozzle code.");
        }
    }

    static bool Same(string? left, string? right)
        => string.Equals(left?.Trim(), right?.Trim(), StringComparison.OrdinalIgnoreCase);
}
