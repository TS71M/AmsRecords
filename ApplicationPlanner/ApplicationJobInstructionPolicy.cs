using Lib.Enums;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Replanning is only safe before mixing a load, never halfway through it.</summary>
public static class ApplicationJobInstructionPolicy
{
    public sealed record Boundary(int CompletedLoads, int LastCompletedStep, decimal RemainingAreaM2);

    public static Boundary? GetBoundary(ApplicationPreparedPassDto job)
    {
        if (!job.IsCurrent || job.Status is not (ApplicationPreparedPassRevisionStatus.Assigned or ApplicationPreparedPassRevisionStatus.Signed)
            || job.Recipe.Loads is not { Count: > 0 }) return null;
        var acknowledged = job.Events.Where(x => x.EventType == ApplicationPreparedPassEventType.StepAcknowledged)
            .Select(x => x.StepNumber).OfType<int>().ToHashSet();
        var completed = 0;
        var last = 0;
        foreach (var load in job.Recipe.Loads)
        {
            var apply = job.Recipe.Steps.Single(x => x.Kind == "Apply" && x.LoadNumber == load.Number);
            if (!acknowledged.Contains(apply.Number)) break;
            completed++; last = apply.Number;
        }
        if (job.Recipe.Steps.Any(x => acknowledged.Contains(x.Number) && x.Number > last && x.Kind != "PreStart")) return null;
        var remaining = job.Recipe.TreatedAreaM2 - job.Recipe.Loads.Take(completed).Sum(x => x.TreatedAreaM2);
        return remaining > 0 ? new(completed, last, remaining) : null;
    }

    public static decimal CompletedQuantity(ApplicationPreparedPassRecipeDto recipe, Boundary boundary, Guid productKey)
        => recipe.Loads!.Take(boundary.CompletedLoads).Sum(x => x.Products.Where(p => p.PlannedApplicationPubId == productKey).Sum(p => p.Quantity));

    public static ApplicationPreparedPassRecipeDto Combine(ApplicationPreparedPassRecipeDto old,
        ApplicationPreparedPassRecipeDto remaining, Boundary boundary)
    {
        var prefix = old.Steps.Where(x => x.Number <= boundary.LastCompletedStep || boundary.LastCompletedStep == 0 && x.Kind == "PreStart").ToArray();
        var loads = old.Loads!.Take(boundary.CompletedLoads)
            .Concat(remaining.Loads!.Select(x => x with { Number = x.Number + boundary.CompletedLoads })).ToArray();
        if (loads.Length > 200) throw new ArgumentException("Split this job into at most 200 loads.");
        var steps = prefix.Concat(remaining.Steps.Where(x => x.Kind != "PreStart").Select(x => x with
            { LoadNumber = x.LoadNumber.HasValue ? x.LoadNumber + boundary.CompletedLoads : null }))
            .Select((step, index) => step with { Number = index + 1 }).ToArray();
        return remaining with {
            PlannedLocalDate = old.PlannedLocalDate,
            TreatedAreaM2 = old.TreatedAreaM2,
            TotalCarrierVolumeLitres = old.Loads!.Take(boundary.CompletedLoads).Sum(x => x.WaterLitres) + remaining.TotalCarrierVolumeLitres,
            Products = remaining.Products.Select(x => x with { Quantity = x.Quantity + CompletedQuantity(old, boundary, x.PlannedApplicationPubId) }).ToArray(),
            Loads = loads, Steps = steps,
            Sources = old.Sources.Concat(remaining.Sources).DistinctBy(x => x.EvidencePubId).ToArray()
        };
    }
}
