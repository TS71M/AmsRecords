using System.Text.RegularExpressions;
using static AmsRecords.Products.ProductsDtos;

namespace AmsRecords.Products;

public sealed record ProductListQueueCommand(string Name, bool Skipped)
{
    public static ProductListQueueCommand? Parse(string question)
    {
        if (question.Length > 4000) return null;
        var match = Regex.Match(question,
            """
            ^\s*(?:(?:please|bitte|por\s+favor|s’il\s+vous\s+plaît|s'il\s+vous\s+plaît)\s+)?
            (?:
                (?:(?<skip>skip|überspringe|omite|ignore|ignora)|restore|restaura|rétablis)\s+
                (?<name>.+?)\s+(?:(?:in|to)\s+the\s+list|in\s+der\s+liste|en\s+la\s+lista|dans\s+la\s+liste|na\s+lista)
                | stelle\s+(?<name>.+?)\s+in\s+der\s+liste\s+wieder\s+her
            )\s*[.!?]?\s*$
            """,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);
        var name = match.Groups["name"].Value.Trim().Trim('"', '\'', '„', '“', '”', '«', '»').Trim();
        return match.Success && name.Length > 0 ? new(name, match.Groups["skip"].Success) : null;
    }

    public static ProductListImportItemDto? FindUniqueRow(ProductListImportPreviewDto list, string name)
    {
        var matches = list.Items.Where(row => string.Equals(row.Preview.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase)
            || string.Equals(row.CreatedName?.Trim(), name, StringComparison.OrdinalIgnoreCase)).Take(2).ToArray();
        return matches is [var row] ? row : null;
    }
}
