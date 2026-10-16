using System.Globalization;
using System.Text;

namespace AmsRecords.GraphAnalysis;

public sealed record RainfallDay(DateOnly Date, decimal? Millimeters);
public sealed record DryPeriodResult(DateOnly? Start, DateOnly? End, int Days, int KnownDays, int MissingDays);

public static class DryPeriodAnalysis
{
    public static bool IsQuestion(string question)
    {
        var text = new string(question.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()).ToLowerInvariant();
        return (text.Contains("longest") || text.Contains("langste") || text.Contains("plus longue")
                || text.Contains("mas largo") || text.Contains("mais longo"))
            && (text.Contains("without rain") || text.Contains("no rain") || text.Contains("dry spell") || text.Contains("dry period")
                || text.Contains("ohne regen") || text.Contains("ohne niederschlag") || text.Contains("trockenperiode")
                || text.Contains("sans pluie") || text.Contains("sin lluvia") || text.Contains("sem chuva"));
    }

    public static DryPeriodResult Calculate(IEnumerable<RainfallDay> source, DateOnly from, DateOnly to)
    {
        if (to < from) throw new ArgumentException("Invalid rainfall period.");
        var days = source.Where(x => x.Date >= from && x.Date <= to).GroupBy(x => x.Date)
            .ToDictionary(g => g.Key, g => g.Count() == 1 ? g.First().Millimeters : null);
        DateOnly? bestStart = null, bestEnd = null, runStart = null;
        int best = 0, run = 0, known = 0;
        for (var number = from.DayNumber; number <= to.DayNumber; number++)
        {
            var date = DateOnly.FromDayNumber(number);
            var value = days.GetValueOrDefault(date);
            if (value is >= 0) known++;
            if (value != 0) { run = 0; runStart = null; continue; }
            runStart ??= date;
            run++;
            if (run > best) { best = run; bestStart = runStart; bestEnd = date; }
        }
        return new(bestStart, bestEnd, best, known, to.DayNumber - from.DayNumber + 1 - known);
    }

    public static GraphAnalysisAskResponseDto? AnalyzeGraph(GraphAnalysisAskRequestDto request)
    {
        if (request.Mode != GraphAnalysisModes.SelectedPeriod || !IsQuestion(request.Question)) return null;
        var rain = request.Series.FirstOrDefault(s => s.Key == "rainfall" && s.FieldPubId == request.Scope.Fields[0].FieldPubId);
        if (rain is null) return new(GraphAnalysisOutcomes.InsufficientData, "Rainfall data required",
            "Rainfall values are unavailable for this graph period.", [], [], null, []);
        var result = Calculate(rain.Points.Select(p => new RainfallDay(p.Date, p.Value)), request.From, request.To);
        if (result.KnownDays == 0) return new(GraphAnalysisOutcomes.InsufficientData, "Rainfall data required",
            "No valid rainfall values are available for this graph period.", [], [], null, [], DryPeriod: result);
        var german = request.Question.Contains("regen", StringComparison.OrdinalIgnoreCase)
            || request.Question.Contains("trocken", StringComparison.OrdinalIgnoreCase)
            || request.Question.Contains("niederschlag", StringComparison.OrdinalIgnoreCase);
        var answer = result.Days == 0
            ? german ? "Kein Tag mit 0 mm Niederschlag im verfügbaren Zeitraum." : "No day with 0 mm rainfall in the available period."
            : german
                ? $"Längste bekannte Periode ohne Regen: {result.Days} Tage, {result.Start:dd.MM.yyyy} bis {result.End:dd.MM.yyyy} (0 mm pro Tag)."
                : $"Longest known period without rain: {result.Days} days, {result.Start:yyyy-MM-dd} to {result.End:yyyy-MM-dd} (0 mm per day).";
        if (result.MissingDays > 0) answer += german
            ? $" {result.MissingDays} Tage ohne gültige Niederschlagswerte; Datenlücken unterbrechen die Periode."
            : $" {result.MissingDays} days lack valid rainfall values; missing days break the run.";
        return new(GraphAnalysisOutcomes.DescriptiveStatistic, german ? "Periode ohne Regen" : "Period without rain",
            answer, [rain.FieldPubId], ["rainfall"], null, [], DryPeriod: result);
    }
}
