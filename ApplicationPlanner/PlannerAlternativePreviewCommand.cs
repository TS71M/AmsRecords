using System.Text.RegularExpressions;

namespace AmsRecords.ApplicationPlanner;

public enum PlannerAlternativePreviewKind { None, Comparison, CostComparison }

public static class PlannerAlternativePreviewCommand
{
    public const string Instruction = @"^(?:(?:please|bitte)\s+)?(?:preview (?:this|the) alternative"
        + @"|show (?:me )?(?:the )?(?:alternative preview|(?:cost )?comparison)|compare (?:this|the) alternative with (?:the )?original"
        + @"|zeig(?:e)? (?:mir )?(?:den )?(?:kostenvergleich|vergleich)|(?:diese|die) alternative anzeigen)[.!?]?$";
    public const string Cost = @"\bcost comparison\b|kostenvergleich";

    public static PlannerAlternativePreviewKind Parse(string? question)
    {
        const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
        var text = question?.Trim() ?? "";
        if (!Regex.IsMatch(text, Instruction, options)) return PlannerAlternativePreviewKind.None;
        return Regex.IsMatch(text, Cost, options) ? PlannerAlternativePreviewKind.CostComparison : PlannerAlternativePreviewKind.Comparison;
    }
}
