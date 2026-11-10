using static AmsRecords.Products.ProductsDtos;

namespace AmsRecords.Products;

public static class ProductMatchPresentation
{
    public sealed record Choice(ProductWebsiteImportMatchDto Match, string Context)
    {
        public string Label => Join(Match.Name, Context);
    }

    public static IReadOnlyList<Choice> Describe(IEnumerable<ProductWebsiteImportMatchDto> matches, string inactiveLabel)
    {
        var choices = matches.Where(match => match.ProductPubId != Guid.Empty && !string.IsNullOrWhiteSpace(match.Name))
            .DistinctBy(match => match.ProductPubId).Take(10)
            .Select(match => new Choice(match, Context(match, inactiveLabel))).ToArray();
        var duplicates = choices.GroupBy(choice => choice.Label, StringComparer.CurrentCultureIgnoreCase)
            .Where(group => group.Count() > 1).Select(group => group.Key).ToHashSet(StringComparer.CurrentCultureIgnoreCase);
        return choices.Select((choice, index) => duplicates.Contains(choice.Label)
            ? choice with { Context = Join(choice.Context, (index + 1).ToString()) } : choice).ToArray();
    }

    static string Context(ProductWebsiteImportMatchDto match, string inactiveLabel)
    {
        if (match.Catalogue is not { } catalogue) return match.ScopeLabel ?? "";
        var scope = string.Equals(match.ScopeLabel, catalogue.CompanyName, StringComparison.CurrentCultureIgnoreCase)
            ? null : match.ScopeLabel;
        var pack = catalogue.Pack > 0 && !string.IsNullOrWhiteSpace(catalogue.PackUnit)
            ? $"{catalogue.Pack:0.######} {catalogue.PackUnit}" : null;
        return Join(catalogue.CompanyName, scope, pack, catalogue.Active ? null : inactiveLabel);
    }

    static string Join(params string?[] parts) => string.Join(" · ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
}
