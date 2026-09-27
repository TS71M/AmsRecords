namespace AmsRecords.Products;

public static class ProductSupplementRules
{
    // An absent guarantee is allowed; a declared quantity cannot be negative.
    public static string? ValidateAmount(decimal? amount)
        => amount < 0m ? "Supplement amount cannot be negative." : null;
}
