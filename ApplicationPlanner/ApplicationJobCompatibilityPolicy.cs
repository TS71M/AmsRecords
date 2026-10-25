using System.Text.Json;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Generic "compatible" wording is never evidence for a specific product pair.</summary>
public static class ApplicationJobCompatibilityPolicy
{
    public sealed record Product(Guid PubId, string Name);
    public sealed record Statement(Guid ProductPubId, string OtherProductName, bool Compatible, string SourceLocation);

    public static IReadOnlyList<Statement> ReadStatements(Guid productPubId, string json)
    {
        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Array) return [];
            return document.RootElement.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Object)
                .Select(x => x.Deserialize<Pair>(new JsonSerializerOptions(JsonSerializerDefaults.Web)))
                .Where(x => x?.Compatible.HasValue == true && !string.IsNullOrWhiteSpace(x.SourceLocation))
                .Select(x => new Statement(productPubId, x!.OtherProductName, x.Compatible!.Value, x.SourceLocation)).ToArray();
        }
        catch (JsonException) { return []; }
    }

    sealed record Pair(string OtherProductName, bool? Compatible, string SourceLocation);

    public static string Merge(string original, string additional)
    {
        try
        {
            using var first = JsonDocument.Parse(original);
            using var second = JsonDocument.Parse(additional);
            if (first.RootElement.ValueKind != JsonValueKind.Array || second.RootElement.ValueKind != JsonValueKind.Array)
                return original;
            return JsonSerializer.Serialize(first.RootElement.EnumerateArray().Select(x => x.Clone())
                .Concat(second.RootElement.EnumerateArray().Select(x => x.Clone())));
        }
        catch (JsonException) { return original; }
    }

    public static IReadOnlyList<string> Check(IReadOnlyList<Product> products, IReadOnlyList<Statement> statements)
    {
        var blockers = new List<string>();
        for (var first = 0; first < products.Count; first++)
        for (var second = first + 1; second < products.Count; second++)
        {
            var a = products[first];
            var b = products[second];
            var evidence = statements.Where(x => !string.IsNullOrWhiteSpace(x.SourceLocation)
                && (x.ProductPubId == a.PubId && SameName(x.OtherProductName, b.Name)
                    || x.ProductPubId == b.PubId && SameName(x.OtherProductName, a.Name))).ToArray();
            if (evidence.Any(x => !x.Compatible))
                blockers.Add($"{a.Name} + {b.Name}: reviewed evidence prohibits this product pair.");
            else if (!evidence.Any(x => x.Compatible))
                blockers.Add($"{a.Name} + {b.Name}: review explicit product-pair compatibility before assignment.");
        }
        return blockers;
    }

    static bool SameName(string a, string b) => !string.IsNullOrWhiteSpace(a)
        && string.Equals(a.Trim(), b.Trim(), StringComparison.OrdinalIgnoreCase);
}
