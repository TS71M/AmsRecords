namespace AmsRecords.Irrigation;

/// <summary>Shared wording for chart baselines and historical orientation assessments.</summary>
public static class NozzleInstallationOrientation
{
    public const string UnmarkedAngle = "No additional rotation marked; compare directional geometry with the upright chart image (0°).";
    public const string ChartLegend = "Angles describe installation rotation, not watering arcs. For directional nozzles, an unmarked rotation uses the upright chart image (0°) as the comparison baseline. A rotationally symmetric opening has no clock direction; a round outer body alone does not establish symmetry. Missing images cannot establish orientation.";

    // Older analyses treated absent numeric metadata as absent directional evidence.
    // Do not present that historical input assumption as a fact about today's chart.
    // This changes presentation only; it neither reanalyses nor approves an installation.
    public static string ReviewReason(string? status, string? reason)
    {
        var text = reason?.Trim() ?? "";
        if (!string.Equals(status, "unknown", StringComparison.OrdinalIgnoreCase) ||
            !text.Contains("reference", StringComparison.OrdinalIgnoreCase)) return text;

        string[] absenceClaims = ["no exact", "no position-specific", "no installation", "no directional",
            "without", "not supplied", "not provided", "not available", "lack of", "missing"];
        return absenceClaims.Any(x => text.Contains(x, StringComparison.OrdinalIgnoreCase))
            ? "The saved analysis did not verify orientation against the current reference. Compare the directional opening/restrictor with the upright chart image and any marked rotation."
            : text;
    }
}
