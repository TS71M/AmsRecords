using Lib.Planning;
using static AmsRecords.Products.ProductsDtos;

namespace AmsRecords.Products;

public static class ProductImportNutrientReview
{
    public const string Skip = "skip";

    public sealed record Choice(NutrientLookupDto Nutrient, decimal Amount);
    public enum Issue { MissingChoice, Duplicate }
    public sealed record Resolution(List<ProductWebsiteImportNutrientDto> Nutrients, IReadOnlyDictionary<int, Issue> Issues);

    public static bool NeedsReview(ProductWebsiteImportNutrientDto row) =>
        ProductNitrogenBreakdown.FormCode(row.SourceName) is null && (!row.IsMatched || row.NutrientPubId.GetValueOrDefault() == Guid.Empty);

    public static (string Element, decimal Amount)? Equivalent(string? code, decimal amount) => code is null ? null
        : FertilizerNutrientBasis.NormalizeCode(code).Replace('₃', '3') switch
        {
            "CAO" => ("Ca", amount * 0.715m),
            "SO3" => ("S", amount * 0.400m),
            var normalized => FertilizerNutrientBasis.ElementalEquivalent(normalized, amount)
        };

    public static IReadOnlyList<Choice> Choices(ProductWebsiteImportNutrientDto row, IReadOnlyList<NutrientLookupDto> catalogue)
    {
        if (!NeedsReview(row) || row.Amount is < 0 or > 100 || decimal.Round(row.Amount, 4) != row.Amount) return [];
        var declared = ProductNitrogenBreakdown.IsTotalLabel(row.SourceName) ? "N" : row.DeclaredCode;
        if (string.IsNullOrWhiteSpace(declared)) return [];
        var code = Code(declared);
        if (code == "N" && !ProductNitrogenBreakdown.IsTotalLabel(row.SourceName)) return [];
        var equivalent = Equivalent(code, row.Amount);
        return catalogue.Where(n => n.PubId != Guid.Empty && !string.IsNullOrWhiteSpace(n.Code))
            .Select(n => (Nutrient: n, Code: Code(n.Code)))
            .Where(n => n.Code == code || (equivalent.HasValue && n.Code == Code(equivalent.Value.Element)))
            .OrderByDescending(n => n.Code == code).ThenBy(n => n.Nutrient.Name)
            .Select(n => new Choice(n.Nutrient, n.Code == code ? row.Amount : decimal.Round(equivalent!.Value.Amount, 4)))
            .ToArray();
    }

    public static string? ElementKey(string? code) => string.IsNullOrWhiteSpace(code) ? null : Code(Equivalent(code, 0)?.Element ?? code);

    public static Resolution Resolve(IReadOnlyList<ProductWebsiteImportNutrientDto> rows,
        IReadOnlyList<NutrientLookupDto> catalogue, IReadOnlyDictionary<int, string?>? decisions)
    {
        var retained = new List<ProductWebsiteImportNutrientDto>(rows.Count);
        var issues = new Dictionary<int, Issue>();
        var existing = rows.Where(row => row.IsMatched && row.NutrientPubId.HasValue).ToArray();
        var targets = existing.Select(row => row.NutrientPubId!.Value).ToHashSet();
        var elements = existing.Select(row => ElementKey(row.DeclaredCode)).OfType<string>().ToHashSet();
        for (var index = 0; index < rows.Count; index++)
        {
            var row = rows[index];
            if (!NeedsReview(row)) { retained.Add(row); continue; }
            var decision = decisions?.GetValueOrDefault(index);
            if (decision == Skip) continue;
            var choice = Guid.TryParse(decision, out var id) ? Choices(row, catalogue).FirstOrDefault(x => x.Nutrient.PubId == id) : null;
            if (choice is null) { issues.Add(index, Issue.MissingChoice); continue; }
            var element = ElementKey(choice.Nutrient.Code)!;
            if (targets.Contains(id) || elements.Contains(element)) { issues.Add(index, Issue.Duplicate); continue; }
            targets.Add(id);
            elements.Add(element);
            retained.Add(row with { NutrientPubId = id, NutrientName = choice.Nutrient.Name, Amount = choice.Amount,
                DeclaredCode = choice.Nutrient.Code, IsMatched = true });
        }
        return new(retained, issues);
    }

    static string Code(string code) => FertilizerNutrientBasis.NormalizeCode(code).Replace('₃', '3');
}
