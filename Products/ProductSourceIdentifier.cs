namespace AmsRecords.Products;

/// <summary>Normalizes a pasted/scanned web URI or GS1 GTIN without performing network access.</summary>
public static class ProductSourceIdentifier
{
    public static bool TryNormalizeToUrl(string? value, out string normalizedUrl)
    {
        normalizedUrl = string.Empty;
        var input = value?.Trim();
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (Uri.TryCreate(input, UriKind.Absolute, out var uri)
            && uri.Scheme is "http" or "https"
            && string.IsNullOrEmpty(uri.UserInfo)
            && !string.IsNullOrWhiteSpace(uri.DnsSafeHost))
        {
            normalizedUrl = uri.AbsoluteUri;
            return true;
        }

        if (!TryNormalizeGtin(input, out var gtin14))
            return false;

        normalizedUrl = $"https://id.gs1.org/01/{gtin14}";
        return true;
    }

    public static bool TryNormalizeGtin(string? value, out string gtin14)
    {
        gtin14 = string.Empty;
        var input = value?.Trim();
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
        {
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index + 1 < segments.Length; index++)
            {
                if (segments[index] == "01" && TryNormalizeDigits(segments[index + 1], out gtin14))
                    return true;
            }
            return false;
        }

        if (input.StartsWith("]C1", StringComparison.Ordinal)
            || input.StartsWith("]d2", StringComparison.Ordinal))
            input = input[3..];
        if (input.StartsWith("(01)", StringComparison.Ordinal))
            input = input[4..];
        else if (input.Length == 16 && input.StartsWith("01", StringComparison.Ordinal))
            input = input[2..];

        return TryNormalizeDigits(input, out gtin14);
    }

    static bool TryNormalizeDigits(string input, out string gtin14)
    {
        gtin14 = string.Empty;
        if (input.Length is not (8 or 12 or 13 or 14) || !input.All(char.IsAsciiDigit))
            return false;

        var expected = 0;
        var multiplier = 3;
        for (var index = input.Length - 2; index >= 0; index--)
        {
            expected += (input[index] - '0') * multiplier;
            multiplier = multiplier == 3 ? 1 : 3;
        }
        var checkDigit = (10 - expected % 10) % 10;
        if (checkDigit != input[^1] - '0')
            return false;

        gtin14 = input.PadLeft(14, '0');
        return true;
    }
}
