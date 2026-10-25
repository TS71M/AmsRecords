namespace AmsRecords.ApplicationPlanner;

/// <summary>Conserves planned water/product totals across bounded physical loads.</summary>
public static class ApplicationJobLoadCalculator
{
    public sealed record Product(Guid PubId, string Name, decimal Quantity, string Unit,
        int Order, IReadOnlyList<string> Instructions, decimal DisplacementLitresPerUnit = 0);

    public static IReadOnlyList<ApplicationPreparedLoadDto> Calculate(decimal areaM2, decimal waterLitres,
        decimal capacity, bool liquid, decimal initialWaterFraction, IReadOnlyList<Product> products,
        string initialWaterBasis = "loadWater")
    {
        if (areaM2 <= 0 || capacity <= 0 || waterLitres < 0 || products.Count is < 1 or > 100
            || products.Any(x => x.Quantity < 0 || x.DisplacementLitresPerUnit < 0)
            || liquid && (waterLitres <= 0 || initialWaterFraction is <= 0 or > 1
                || initialWaterBasis is not ("loadWater" or "tankCapacity" or "loadMixture")))
            throw new ArgumentException("Positive area/capacity and reviewed load inputs are required.");
        var total = liquid
            ? waterLitres + products.Sum(x => x.Quantity * x.DisplacementLitresPerUnit)
            : products.Sum(x => x.Quantity);
        if (total <= 0 || decimal.Ceiling(total / capacity) > 200)
            throw new ArgumentException("Split this job into at most 200 loads.");
        var count = (int)decimal.Ceiling(total / capacity);
        var result = new List<ApplicationPreparedLoadDto>(count);
        var remainingProducts = products.ToDictionary(x => x.PubId, x => x.Quantity);
        var remainingWater = waterLitres;
        var remainingArea = areaM2;
        for (var index = 0; index < count; index++)
        {
            var last = index == count - 1;
            var loadVolume = Math.Min(capacity, total - index * capacity);
            // Multiply before dividing: a recurring fraction such as 200/450
            // must not turn an exact 200 kg hopper load into 199.999999 kg.
            var loadWater = last ? remainingWater : decimal.Round(waterLitres * loadVolume / total, 3, MidpointRounding.ToZero);
            var loadArea = last ? remainingArea : decimal.Round(areaM2 * loadVolume / total, 1, MidpointRounding.ToZero);
            var quantities = products.OrderBy(x => x.Order).Select(product =>
            {
                var amount = last ? remainingProducts[product.PubId]
                    : decimal.Round(product.Quantity * loadVolume / total, 6, MidpointRounding.ToZero);
                remainingProducts[product.PubId] -= amount;
                return new ApplicationPreparedLoadProductDto(product.PubId, product.Name, amount,
                    product.Unit, product.Order, product.Instructions);
            }).ToArray();
            var fillBasis = initialWaterBasis switch
            {
                "tankCapacity" => capacity,
                "loadMixture" => loadWater + quantities.Sum(p => p.Quantity
                    * products.Single(x => x.PubId == p.PlannedApplicationPubId).DisplacementLitresPerUnit),
                _ => loadWater
            };
            var initialWater = liquid ? decimal.Round(fillBasis * initialWaterFraction, 3, MidpointRounding.ToZero) : 0;
            if (initialWater > loadWater)
                throw new ArgumentException("The required initial tank fill exceeds this load's water quantity. Review the partial-load instructions.");
            result.Add(new(index + 1, loadArea, loadWater, initialWater, loadWater - initialWater, quantities));
            remainingArea -= loadArea;
            remainingWater -= loadWater;
        }
        if (result.Any(load => (liquid
            ? load.WaterLitres + load.Products.Sum(p => p.Quantity * products.Single(x => x.PubId == p.PlannedApplicationPubId).DisplacementLitresPerUnit)
            : load.Products.Sum(p => p.Quantity)) > capacity))
            throw new ArgumentException("The precision needed for this load exceeds the available capacity. Reduce the planned load size.");
        return result;
    }
}
