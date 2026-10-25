using static AmsRecords.Areas.SurfaceCompositionTransmissionDtos;
using static AmsRecords.Surfaces.SurfaceDtos;

namespace AmsRecords.Areas;

public static class AreaCompositionAggregatePolicy
{
    public sealed record Species(string Name, decimal Percent, decimal Confidence);
    public sealed record Result(int Total, int Analyzed, int Required, decimal WeedCover, IReadOnlyList<Species> Species)
    {
        public bool Available => Total > 0 && Analyzed >= Required;
    }

    public static int RequiredSurfaceCount(int total) => total <= 0 ? 0 : total / 2 + 1;
    public static bool CanRecord(SurfaceCompositionTransmissionDto? latest) => latest?.BlocksNewAnalysis != true;

    public static Result Calculate(IEnumerable<SurfaceMiniDto> surfaces, IReadOnlyDictionary<Guid, SurfaceCompositionTransmissionDto> latest)
    {
        var all = surfaces.Where(x => x.PubId != Guid.Empty).DistinctBy(x => x.PubId).ToArray();
        var analyzed = all.Where(x => latest.TryGetValue(x.PubId, out var record)
            && record.SurfacePubId == x.PubId && record.BlocksNewAnalysis).ToArray();
        var weighted = new Dictionary<string, (string Name, decimal Percent, decimal Confidence, decimal Weight)>(StringComparer.CurrentCultureIgnoreCase);
        decimal totalWeight = 0, weeds = 0;
        foreach (var surface in analyzed)
        {
            var record = latest[surface.PubId];
            var weight = surface.SurfaceSize > 0 ? surface.SurfaceSize : 1m;
            totalWeight += weight;
            weeds += record.WeedCoveragePercent * weight;
            foreach (var species in record.Species)
            {
                var key = species.GrassSpeciesPubId?.ToString("D") ?? species.Name.Trim();
                weighted.TryGetValue(key, out var previous);
                weighted[key] = (species.Name, previous.Percent + species.Percent * weight,
                    previous.Confidence + species.Confidence * weight, previous.Weight + weight);
            }
        }
        Species[] rows = totalWeight == 0 ? [] : weighted.Values
            .Select(x => new Species(x.Name, x.Percent / totalWeight, x.Weight > 0 ? x.Confidence / x.Weight : 0))
            .Where(x => x.Percent > 0).OrderByDescending(x => x.Percent).ThenBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase).ToArray();
        return new(all.Length, analyzed.Length, RequiredSurfaceCount(all.Length), totalWeight > 0 ? weeds / totalWeight : 0, rows);
    }
}
