namespace AmsRecords.Irrigation;

public static class IrrigationReferenceImageUrl
{
    public static bool IsValid(string? value, bool allowHttp = false)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        var candidate = value.Trim();
        if (candidate.Length > 500 || candidate.Any(char.IsControl) || candidate.Contains('\\')) return false;
        if (candidate.StartsWith('/') && !candidate.StartsWith("//", StringComparison.Ordinal))
            return true;
        return Uri.TryCreate(candidate, UriKind.Absolute, out var uri) &&
            (uri.Scheme == "https" || allowHttp && uri.Scheme == "http") && string.IsNullOrEmpty(uri.UserInfo);
    }
}
