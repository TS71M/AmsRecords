using System.ComponentModel.DataAnnotations;
using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Field-first capture. Product IDs refer to the catalogue, never to a plan
/// snapshot. The server must authorize references and freeze inputs atomically.</summary>
public sealed record RecordFieldApplicationDto(
    Guid FieldPubId,
    Guid SubmissionId,
    DateOnly ExecutedLocalDate,
    IReadOnlyList<Guid> SurfacePubIds,
    decimal ActualTreatedAreaM2,
    IReadOnlyList<RecordFieldApplicationProductDto> Products,
    Guid? MachineryPubId = null,
    decimal? ActualCarrierVolumeLitres = null,
    FieldApplicationPlanLinkDto? PlanLink = null) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FieldPubId == Guid.Empty) yield return Error("Select a field.", nameof(FieldPubId));
        if (SubmissionId == Guid.Empty) yield return Error("A submission identifier is required.", nameof(SubmissionId));
        if (ExecutedLocalDate.Year is < 1900 or > 2100) yield return Error("Enter a date between 1900 and 2100.", nameof(ExecutedLocalDate));
        if (SurfacePubIds is null || SurfacePubIds.Count is < 1 or > 1000
            || SurfacePubIds.Any(id => id == Guid.Empty) || SurfacePubIds.Distinct().Count() != SurfacePubIds.Count)
            yield return Error("Select distinct treated surfaces.", nameof(SurfacePubIds));
        if (!Fits(ActualTreatedAreaM2, 100000000000m, 1) || ActualTreatedAreaM2 == 0)
            yield return Error("Enter a positive treated area with at most one decimal place.", nameof(ActualTreatedAreaM2));
        if (MachineryPubId == Guid.Empty) yield return Error("Select valid machinery or leave it unset.", nameof(MachineryPubId));
        if (ActualCarrierVolumeLitres.HasValue && !Fits(ActualCarrierVolumeLitres.Value, 1000000000m, 3))
            yield return Error("Enter a supported carrier volume.", nameof(ActualCarrierVolumeLitres));
        if (PlanLink is not null && (PlanLink.PlanPubId == Guid.Empty || PlanLink.RevisionPubId == Guid.Empty || PlanLink.PassPubId == Guid.Empty))
            yield return Error("Provide a complete plan link or leave it unset.", nameof(PlanLink));
        if (Products is null || Products.Count is < 1 or > 100)
        {
            yield return Error("Select between one and 100 products.", nameof(Products));
            yield break;
        }
        if (Products.Any(p => p is null) || Products.Where(p => p is not null).Select(p => p.ProductPubId).Distinct().Count() != Products.Count)
        {
            yield return Error("Select each product once within this pass.", nameof(Products));
            yield break;
        }
        for (var i = 0; i < Products.Count; i++)
        {
            var p = Products[i];
            var path = $"{nameof(Products)}[{i}]";
            if (p.ProductPubId == Guid.Empty) yield return Error("Select a product.", path + ".ProductPubId");
            if (p.PlannedApplicationPubId == Guid.Empty || (PlanLink is null && p.PlannedApplicationPubId.HasValue))
                yield return Error("A planned application requires plan context.", path + ".PlannedApplicationPubId");
            if (p.CompletesPlannedApplication && !p.PlannedApplicationPubId.HasValue)
                yield return Error("Independent work cannot complete a planned application.", path + ".CompletesPlannedApplication");
            var rate = p.ActualRate.HasValue;
            var quantity = p.ActualProductQuantity.HasValue;
            if (rate == quantity)
                yield return Error("Enter a rate or a measured quantity, not both.", path);
            if (rate && (!Fits(p.ActualRate!.Value, 10000000m, 3) || p.ActualRate <= 0 || !p.RateUnit.HasValue || !Enum.IsDefined(p.RateUnit.Value)))
                yield return Error("Enter a positive rate and supported rate unit.", path + ".ActualRate");
            if (quantity && (!Fits(p.ActualProductQuantity!.Value, 1000000000m, 3) || p.ActualProductQuantity <= 0
                || !p.QuantityUnit.HasValue || !Enum.IsDefined(p.QuantityUnit.Value)))
                yield return Error("Enter a positive quantity and supported quantity unit.", path + ".ActualProductQuantity");
            if ((!quantity && p.QuantityUnit.HasValue) || (!rate && p.RateUnit.HasValue))
                yield return Error("Units must match the selected entry mode.", path);
            if (p.ActualCost.HasValue && !Fits(p.ActualCost.Value, 10000000000m, 2))
                yield return Error("Enter a supported cost or leave it unknown.", path + ".ActualCost");
            if (p.Notes?.Length > 2000) yield return Error("Notes may contain at most 2000 characters.", path + ".Notes");
        }
    }

    private static bool Fits(decimal value, decimal limit, int precision)
        => value >= 0 && value < limit && decimal.Round(value, precision) == value;
    private static ValidationResult Error(string message, string member) => new(message, [member]);
}

public sealed record FieldApplicationPlanLinkDto(Guid PlanPubId, Guid RevisionPubId, Guid? PassPubId = null);

public sealed record RecordFieldApplicationProductDto(Guid ProductPubId,
    decimal? ActualRate = null, FertilizerApplicationRateUnit? RateUnit = null,
    decimal? ActualProductQuantity = null, FertilizerProductQuantityUnit? QuantityUnit = null,
    decimal? ActualCost = null, string? Notes = null,
    Guid? PlannedApplicationPubId = null, bool CompletesPlannedApplication = false);
