namespace AmsRecords.Products;

/// <summary>
/// Classifies a website only from server-maintained domain registrations.
/// User-entered URLs can never assign their own evidence trust.
/// </summary>
public static class ProductEvidenceTrustPolicy
{
    public static ApplicationEvidenceTrustLevel ClassifyWebsite(
        Uri source,
        IEnumerable<string> approvedManufacturerWebsites)
        => approvedManufacturerWebsites.Any(x => MatchesWebsite(source, x))
            ? ApplicationEvidenceTrustLevel.Manufacturer
            : ApplicationEvidenceTrustLevel.UserProvided;

    public static bool MatchesWebsite(Uri source, string? approvedWebsite)
    {
        if (source is null
            || source.Scheme is not ("http" or "https")
            || string.IsNullOrWhiteSpace(approvedWebsite))
            return false;

        var candidate = approvedWebsite.Trim();
        if (!candidate.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            && !candidate.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            candidate = "https://" + candidate;
        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var approved)
            || approved.Scheme is not ("http" or "https")
            || string.IsNullOrWhiteSpace(approved.DnsSafeHost))
            return false;

        var sourceHost = source.IdnHost.TrimEnd('.');
        var approvedHost = approved.IdnHost.TrimEnd('.');
        return sourceHost.Equals(approvedHost, StringComparison.OrdinalIgnoreCase)
            || sourceHost.EndsWith("." + approvedHost, StringComparison.OrdinalIgnoreCase);
    }
}
