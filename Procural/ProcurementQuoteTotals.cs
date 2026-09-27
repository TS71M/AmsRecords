namespace AmsRecords.Procural;

public static class ProcurementQuoteTotals
{
    public static decimal? Line(decimal quantity, decimal? unitPrice)
    {
        if (unitPrice is null or < 0 || quantity < 0) return null;
        try { return quantity * unitPrice.Value; }
        catch (OverflowException) { return null; }
    }

    public static decimal? Sum(IEnumerable<decimal?> lineTotals)
    {
        decimal total = 0;
        foreach (var line in lineTotals)
        {
            if (!line.HasValue) return null;
            try { total += line.Value; }
            catch (OverflowException) { return null; }
        }
        return total;
    }
}
