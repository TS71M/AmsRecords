using System.Text.RegularExpressions;

namespace AmsRecords.Assistant;

public static class AssistantReadingReview
{
    public const string InstructionPattern = @"\s*(?:(?:please|bitte)\s+)?(?:(?:change|correct|set|update)\s+(?:this|the current)\s+(?:reading|measurement)\s+to|actually,?\s+it\s+was|(?:ändere|korrigiere)\s+(?:diesen|den aktuellen)\s+messwert\s+auf|es\s+waren?\s+doch)\s+(?<reading>.+?)\s*";
    static readonly Regex Instruction = new(
        @"\A" + InstructionPattern + @"\z",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(100));

    public static string? ReadInstruction(string question)
        => Instruction.Match(question) is { Success: true } match ? match.Groups["reading"].Value : null;
}
