namespace AmsRecords.Surfaces;

public static class SurfaceLabelPolicy
{
    public const int MaximumLength = 80;

    public static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static bool IsValid(string? value)
    {
        var normalized = Normalize(value);
        return normalized is null || normalized.Length <= MaximumLength;
    }

    public static string FormatAreaLabel(string? areaName, string? surfaceLabel, string fallback)
    {
        var area = string.IsNullOrWhiteSpace(areaName) ? fallback : areaName.Trim();
        var label = Normalize(surfaceLabel);
        return label is null ? area : $"{area} · {label}";
    }
}
