namespace AmsRecords.Diagnostics;

// Presentation only: retain the complete stored assessment for supervisor review.
public static class DiagnosticUserPresentation
{
    public static string WithoutGeneralGuidance(string? text)
    {
        var value = text ?? string.Empty;
        foreach (var phrase in GeneralGuidance)
            value = value.Replace(phrase, string.Empty, StringComparison.Ordinal);
        value = System.Text.RegularExpressions.Regex.Replace(value,
            @"The uploaded image set does not support a confident AI diagnosis\. No disease or stress label reached the [0-9.,]+% confidence threshold, so any named conditions should be treated only as possibilities until a supervisor or agronomist reviews close-up symptoms and field context\.", string.Empty);
        value = System.Text.RegularExpressions.Regex.Replace(value,
            @"Die Fotos reichen für eine sichere Diagnose nicht aus\. Keine Ursache erreichte die Konfidenzschwelle von [0-9.,]+ %\. Die genannten Möglichkeiten müssen anhand von Nahaufnahmen und Beobachtungen vor Ort geprüft werden\.", string.Empty);
        return value.Trim();
    }

    public static bool HasUncertainGuidance(string? text)
        => (text ?? string.Empty).Contains("The uploaded image set does not support a confident AI diagnosis.", StringComparison.Ordinal)
            || (text ?? string.Empty).Contains("Die Fotos reichen für eine sichere Diagnose nicht aus.", StringComparison.Ordinal)
            || (text ?? string.Empty).Contains(GeneralGuidance[0], StringComparison.Ordinal)
            || (text ?? string.Empty).Contains(GeneralGuidance[3], StringComparison.Ordinal);

    static readonly string[] GeneralGuidance =
    [
        "Do not use this AI result as a confirmed diagnosis or treatment trigger. Capture better diagnostic photos if needed, inspect the turf in person, and use supervisor review before changing disease or stress management.",
        "Review this case with a supervisor or agronomist before treatment decisions.",
        "Answer these questions in the field note and submit a new analysis with any requested photos.",
        "Keine gesicherte Diagnose. Symptome vor Ort prüfen und vor einer Behandlung die Aufsicht hinzuziehen.",
        "Vor einer Behandlung mit der Aufsicht oder einer agronomischen Fachperson besprechen.",
        "Fragen in der Feldnotiz beantworten und mit den angeforderten Fotos erneut analysieren."
    ];
}
