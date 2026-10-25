using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Actuals are an audit report, never a mutation of the signed recipe.</summary>
public static class ApplicationJobReportPolicy
{
    public sealed record ProductDifference(string ProductName, decimal? PlannedQuantity,
        string PlannedUnit, decimal ActualQuantity, string ActualUnit, bool Changed);

    public static IReadOnlyList<ProductDifference> Products(ApplicationPreparedPassRecipeDto recipe,
        IReadOnlyList<ApplicationPreparedProductActualDto> actuals)
        => actuals.Select(actual =>
        {
            var planned = recipe.Products.FirstOrDefault(x => x.IsAdditional
                ? actual.PlannedApplicationPubId == Guid.Empty && x.SourceProductPubId == actual.SourceProductPubId
                : x.PlannedApplicationPubId == actual.PlannedApplicationPubId);
            return new ProductDifference(planned?.ProductName ?? actual.ProductName, planned?.Quantity,
                planned?.QuantityUnit ?? "", actual.Quantity, actual.Unit,
                planned is null || actual.Quantity != planned.Quantity
                    || !string.Equals(actual.Unit, planned.QuantityUnit, StringComparison.OrdinalIgnoreCase));
        }).ToArray();

    public static bool HasChanges(ApplicationPreparedPassRecipeDto recipe, decimal? area, decimal? water,
        IReadOnlyList<ApplicationPreparedProductActualDto> actuals)
        => area != recipe.TreatedAreaM2 || water.GetValueOrDefault() != recipe.TotalCarrierVolumeLitres
            || Products(recipe, actuals).Any(x => x.Changed);

    public static string? Validate(ApplicationPreparedPassRecipeDto recipe, decimal? area, decimal? water,
        IReadOnlyList<ApplicationPreparedProductActualDto>? actuals, string? reason)
    {
        if (area is not > 0 || area > recipe.TreatedAreaM2 || decimal.Round(area.Value, 1) != area
            || water is < 0 or >= 1_000_000_000m || water.HasValue && decimal.Round(water.Value, 3) != water)
            return "Enter valid actual area and water quantities.";
        if (actuals is null || actuals.Count is < 1 or > 100 || actuals.Any(x => x is null
            || x.Quantity <= 0 || x.Quantity >= 1_000_000_000m || decimal.Round(x.Quantity, 3) != x.Quantity
            || !Enum.TryParse<FertilizerProductQuantityUnit>(x.Unit, true, out var unit) || !Enum.IsDefined(unit)
            || (x.PlannedApplicationPubId == Guid.Empty
                ? x.SourceProductPubId is null || x.SourceProductPubId == Guid.Empty : x.SourceProductPubId.HasValue)))
            return "Enter positive product quantities with up to three decimals and kilograms or litres.";
        var assigned = actuals.Where(x => x.PlannedApplicationPubId != Guid.Empty).Select(x => x.PlannedApplicationPubId).ToArray();
        if (!assigned.OrderBy(x => x).SequenceEqual(recipe.Products.Where(x => !x.IsAdditional).Select(x => x.PlannedApplicationPubId).OrderBy(x => x))
            || recipe.Products.Where(x => x.IsAdditional).Any(p => !actuals.Any(a => a.PlannedApplicationPubId == Guid.Empty && a.SourceProductPubId == p.SourceProductPubId))
            || actuals.Where(x => x.PlannedApplicationPubId == Guid.Empty).Select(x => x.SourceProductPubId).Distinct().Count()
                != actuals.Count(x => x.PlannedApplicationPubId == Guid.Empty))
            return "Report each assigned product once and select each additional product once.";
        if (HasChanges(recipe, area, water, actuals) && string.IsNullOrWhiteSpace(reason))
            return "Explain changes to the assigned quantities, water, area or products.";
        return null;
    }
}
