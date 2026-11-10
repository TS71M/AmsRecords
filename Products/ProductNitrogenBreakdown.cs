using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static AmsRecords.Products.ProductsDtos;

namespace AmsRecords.Products;

public static partial class ProductNitrogenBreakdown
{
    public const int MaximumForms = 7;
    public static IReadOnlyDictionary<string, string> FormLabels { get; } = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Ammonium"] = "Ammonium nitrogen",
            ["Nitrate"] = "Nitrate nitrogen",
            ["Urea"] = "Urea nitrogen",
            ["Organic"] = "Organically bound nitrogen",
            ["MethyleneUrea"] = "Nitrogen from methylene urea",
            ["IBDU"] = "Nitrogen from isobutylidene diurea (IBDU)",
            ["CDU"] = "Nitrogen from crotonylidene diurea (CDU)"
        });

    public sealed record Review(ProductWebsiteImportNutrientDto? Total, IReadOnlyList<ProductNutrientFormDto> Forms, bool HasConflict)
    {
        public bool CanSelect => Total is not null && !HasConflict;
        public bool IsPartial => CanSelect && Forms.Sum(x => x.Amount) < Total!.Amount;
    }

    public static Review Inspect(IReadOnlyList<ProductWebsiteImportNutrientDto> rows)
    {
        var totals = rows.Where(row => row is not null && IsTotal(row)).ToArray();
        var total = totals.Length == 1 ? totals[0] : null;
        var forms = rows.Where(row => row is not null)
            .Select(row => (Row: row, Code: FormCode(row.SourceName)))
            .Where(x => x.Code is not null)
            .Select(x => new ProductNutrientFormDto(x.Code!, x.Row.SourceName.Trim(), x.Row.Amount,
                Minimum().IsMatch(x.Row.SourceName)))
            .Distinct().ToArray();
        var grouped = forms.GroupBy(x => x.FormCode).ToArray();
        var conflict = totals.Length > 1 || forms.Any(f => !IsValid(f))
            || grouped.Any(g => g.Select(f => (f.Amount, f.IsMinimum)).Distinct().Skip(1).Any())
            || total is not null && grouped.Sum(g => g.First().Amount) > total.Amount;
        return new(total, conflict ? forms : grouped.Select(g => g.First()).ToArray(), conflict);
    }

    public static bool IsValid(ProductNutrientFormDto form)
        => form.FormCode is not null && FormLabels.ContainsKey(form.FormCode)
            && form.Label is { Length: > 0 and <= 250 } && FormCode(form.Label) == form.FormCode
            && form.Amount is >= 0m and <= 100m && decimal.Round(form.Amount, 4) == form.Amount;

    static bool IsTotal(ProductWebsiteImportNutrientDto row)
        => row.IsMatched && row.NutrientPubId is { } id && id != Guid.Empty && row.Amount is >= 0m and <= 100m
            && IsTotalLabel(row.SourceName);

    public static bool IsTotalLabel(string? label) => Key(label) is
        "N" or "NITROGEN" or "NITROGENN" or "TOTALNITROGEN" or "TOTALNITROGENN" or "NTOTALNITROGEN"
        or "STICKSTOFF" or "STICKSTOFFN" or "GESAMTSTICKSTOFF" or "GESAMTSTICKSTOFFN" or "NGESAMTSTICKSTOFF"
        or "STICKSTOFFINSGESAMT" or "STICKSTOFFINSGESAMTN" or "NSTICKSTOFFINSGESAMT"
        or "AZOTE" or "AZOTEN" or "AZOTETOTAL" or "AZOTETOTALN"
        or "NITROGENO" or "NITROGENON" or "NITROGENOTOTAL" or "NITROGENOTOTALN"
        or "AZOTO" or "AZOTON" or "AZOTOTOTAL" or "AZOTOTOTALN";

    public static string? FormCode(string? label)
    {
        if (string.IsNullOrWhiteSpace(label)) return null;
        var key = Key(FormulaSuffix().Replace(Qualifiers().Replace(label, ""), ""));
        // Accept a chemical annotation after a recognized form, not a bare ion or ingredient.
        return key switch
        {
            "AMMONIUMSTICKSTOFF" or "AMMONIAKSTICKSTOFF" or "AMMONIUMN" or "AMMONIUMNITROGEN" or "AMMONIACALNITROGEN" => "Ammonium",
            "NITRATSTICKSTOFF" or "NITRATN" or "NITRATENITROGEN" or "NITRICNITROGEN" => "Nitrate",
            "HARNSTOFFSTICKSTOFF" or "UREASTICKSTOFF" or "CARBAMIDSTICKSTOFF" or "CARBAMIDENITROGEN"
                or "HARNSTOFFN" or "CARBAMIDN" or "UREAN" or "UREANITROGEN" or "UREICNITROGEN" => "Urea",
            "ORGANISCHGEBUNDENERSTICKSTOFF" or "ORGANISCHERSTICKSTOFF" or "ORGANISCHGEBUNDENESN" or "ORGANICNITROGEN" or "ORGANICALLYBOUNDNITROGEN" or "ORGANICN" => "Organic",
            "STICKSTOFFAUSMETHYLENHARNSTOFF" or "NITROGENFROMMETHYLENEUREA" or "METHYLENEUREANITROGEN" => "MethyleneUrea",
            "STICKSTOFFAUSISOBUTYLIDENDIHARNSTOFF" or "NITROGENFROMISOBUTYLIDENEDIUREA" or "NITROGENFROMISOBUTYLIDENEDIUREAIBDU" or "IBDUNITROGEN" => "IBDU",
            "STICKSTOFFAUSCROTONYLIDENDIHARNSTOFF" or "NITROGENFROMCROTONYLIDENEDIUREA" or "NITROGENFROMCROTONYLIDENEDIUREACDU" or "CDUNITROGEN" => "CDU",
            _ => null
        };
    }

    static string Key(string? text) => string.Concat((text ?? "").Normalize(NormalizationForm.FormKD)
        .Where(c => char.IsLetterOrDigit(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
        .ToUpperInvariant();

    [GeneratedRegex(@"\b(?:davon|of which|mindestens|mind\.?|minimum|min\.?)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex Qualifiers();

    [GeneratedRegex(@"\b(?:mindestens|mind\.?|minimum|min\.?)\b|≥", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex Minimum();

    [GeneratedRegex(@"\s*\((?:N|NH[4₄](?:\+|[-–]N)?|NO[3₃](?:[-−]|[-–]N)?|Norg)\)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex FormulaSuffix();
}
