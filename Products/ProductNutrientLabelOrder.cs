using Lib.Planning;

namespace AmsRecords.Products;

public static class ProductNutrientLabelOrder
{
    // Regulation (EU) 2019/1009, Annex III: primary, secondary, then micronutrients.
    // An oxide is grouped with its element for display only; its code and value are unchanged.
    public static int Rank(string? code)
        => code is null ? int.MaxValue : FertilizerNutrientBasis.NormalizeCode(code).Replace('₃', '3') switch
        {
            "N" => 0,
            "P" or "P2O5" => 1,
            "K" or "K2O" => 2,
            "CA" or "CAO" => 3,
            "MG" or "MGO" => 4,
            "NA" or "NA2O" => 5,
            "S" or "SO3" => 6,
            "B" => 7,
            "CO" => 8,
            "CU" => 9,
            "FE" => 10,
            "MN" => 11,
            "MO" => 12,
            "ZN" => 13,
            _ => int.MaxValue
        };
}
