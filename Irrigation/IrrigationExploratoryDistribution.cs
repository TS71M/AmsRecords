using static AmsRecords.Irrigation.IrrigationAdvisorDesignDtos;
using static AmsRecords.Irrigation.SprinklerPerformanceDtos;

namespace AmsRecords.Irrigation;

/// <summary>Explicit engineering assumptions, not recovered or measured nozzle characteristics.</summary>
public static class IrrigationExploratoryDistribution
{
    public const string Long = "Long";
    public const string Middle = "Middle";
    public const string Short = "Short";
    public const string Basis = "Exploratory role estimate v1. Overlapping triangular radial bands and per-port flow weights are assumptions, not Toro measurements. Long/middle/short weights are 5/2/1, normalized over the included flowing ports. Total flow and throw come from the complete assembly's catalogue curve. Individual component flows and uniformity rankings are not validated; equal-role parts are not distinguished by this model.";
    public sealed record Estimate(IReadOnlyList<NozzlePart> Parts, IReadOnlyList<SprinklerDistributionPointDto> Points);
    sealed record Band(double Start, double Peak, double End, double Weight);
    static Band? ForRole(string? role) => role switch
    {
        Long => new(.35, .8, 1, 5),
        Middle => new(.15, .45, .75, 2),
        Short => new(0, 0, .45, 1),
        _ => null
    };

    public static Estimate? Create(IReadOnlyList<NozzlePart> parts)
    {
        ArgumentNullException.ThrowIfNull(parts);
        if (parts.Count > 20) throw new ArgumentException("Too many parts for a role estimate.", nameof(parts));
        if (parts.Any(p => p is null || p.EstimatedRangeRole is not null && ForRole(p.EstimatedRangeRole) is null))
            throw new ArgumentException("Unknown exploratory range role.", nameof(parts));
        var bands = parts.Select(p => ForRole(p.EstimatedRangeRole)).ToArray();
        var totalWeight = bands.Sum(b => b?.Weight ?? 0);
        if (totalWeight == 0) return null;
        var annotated = parts.Select((p, i) => bands[i] is { } b
            ? p with { AssumedFlowSharePercent = 100 * b.Weight / totalWeight,
                AssumedStartRadiusFraction = b.Start, AssumedPeakRadiusFraction = b.Peak, AssumedEndRadiusFraction = b.End }
            : p with { AssumedFlowSharePercent = null, AssumedStartRadiusFraction = null,
                AssumedPeakRadiusFraction = null, AssumedEndRadiusFraction = null }).ToArray();
        // Each component is normalized by its radial area integral before combining:
        // equal curve heights do not represent equal flow at different distances.
        var rates = Enumerable.Range(0, 101).Select(i => bands.Where(b => b is not null)
            .Sum(b => b!.Weight / totalWeight * Height(b, i / 100d) / Integral(b))).ToArray();
        var maximum = rates.Max();
        var points = rates.Select((rate, i) => new SprinklerDistributionPointDto(i / 100m,
            decimal.Round((decimal)(rate / maximum), 8))).ToArray();
        return new(annotated, points);
    }

    static double Height(Band b, double radius)
    {
        if (radius < b.Start || radius >= b.End) return 0;
        return radius < b.Peak ? (radius - b.Start) / (b.Peak - b.Start)
            : (b.End - radius) / (b.End - b.Peak);
    }

    static double Integral(Band b) => (b.Peak - b.Start) * (b.Start + 2 * b.Peak) / 6
        + (b.End - b.Peak) * (2 * b.Peak + b.End) / 6;
}
