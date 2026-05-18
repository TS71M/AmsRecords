namespace AmsRecords.SnagLists;

public static class SnagListPriority
{
    public const int Low = 1;
    public const int Normal = 2;
    public const int High = 3;
    public const int Urgent = 4;
    public const int Critical = 5;

    public sealed record Option(int Value, string Name, string BadgeCssClass, string HexColor);

    public static readonly IReadOnlyList<Option> Options =
    [
        new(Low, "Low", "bg-secondary", "#6C757D"),
        new(Normal, "Normal", "bg-info text-dark", "#5BC0DE"),
        new(High, "High", "bg-warning text-dark", "#FFC107"),
        new(Urgent, "Urgent", "bg-orange text-dark", "#F59E0B"),
        new(Critical, "Critical", "bg-danger", "#DC3545")
    ];

    public static int Normalize(int priority)
        => priority switch
        {
            <= Low => Low,
            >= Critical => Critical,
            _ => priority
        };

    public static Option Get(int priority)
    {
        var normalized = Normalize(priority);
        return Options.First(x => x.Value == normalized);
    }
}
