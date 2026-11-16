using System.Text.RegularExpressions;
using AmsRecords.Assistant;

namespace AmsRecords.ApplicationPlanner;

public static class PlannerAlternativeScheduleCommand
{
    const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    public static bool MatchesEdit(string question) => ReadBasisOption(question) is not null || Regex.IsMatch(question,
        """
        ^\s*(?:(?:please|bitte)\s+)?
        (?:(?:change|switch)(?:\s+(?:this|the|my|our)\s+(?:existing\s+)?(?:gp\s+)?(?:programme|program|schedule))?\s+(?:from|to)\s+
            |(?:wechsle|ändere)\s+(?:von|auf)\s+
            |(?:use|set|apply|schedule|verwende|dünge|make\s+it)\s+)?
        (?: weekly|fortnightly|every\s+|wöchentlich|zweiwöchentlich|vierzehntägig|alle\s+
            |(?:make|create|prepare|draft)\s+(?:(?:an?|the)\s+)?(?:weekly|fortnightly|every\s+[^.!?]+)\s+(?:fertili[sz]er\s+)?alternative\b
            |(?:erstelle|entwirf)\s+(?:eine\s+)?(?:wöchentlich|zweiwöchentlich|vierzehntägig)e\s+(?:dünge)?alternative\b
            |(?:gp\s+|growth\s+potential\s+)?cutoff|gp[ -]grenze
            |(?:skip\s+applications|no\s+nitrogen|keine\s+anwendungen|kein\s+stickstoff)\s+
            |(?:from|von|vom)\s+|(?:the\s+)?date\s+range
            |(?:keep|preserve|behalte)\s+|annual\s+nitrogen|(?:total\s+)?nitrogen\s+per\s+year|jahresstickstoff|four-week|jährlich|vier[- ]wochen
            |stock\s+first|find\s+(?:a\s+cheaper\s+alternative|cheaper\s+purchases)|zuerst\s+den\s+lagerbestand)
        """, Options | RegexOptions.IgnorePatternWhitespace);

    public static string? ReadBasisOption(string question)
    {
        var answer = question.Trim().TrimEnd('.', '!', '?').TrimEnd();
        var option = Regex.Match(answer,
            @"\A(?:(?:(?:the|die)\s+)?(?:(?<annual>first|1st|erste)|(?:second|2nd|zweite))(?:\s+(?:one|option|choice|möglichkeit))?|(?<annual>per year|pro jahr))"
            + @"(?:,?\s+(?:please|bitte))?\z", Options);
        return !option.Success ? null : option.Groups["annual"].Success ? "preserve_annual" : "four_week_reference";
    }

    public static IReadOnlyList<SupportAssistantMessageDto> FollowUpHistory(IReadOnlyList<SupportAssistantMessageDto> history, Guid? field)
        => field.HasValue && history.Count >= 2
            && history[^2] is { Role: "user", ScopeKnown: true } instruction && instruction.FieldPubId == field
            && history[^1] is { Role: "assistant", ScopeKnown: true } reply && reply.FieldPubId == field
            ? [new("user", instruction.Content, field, true), new("assistant", reply.Content, field, true) { AlternativeBasisReview = reply.AlternativeBasisReview }]
            : [];
}
