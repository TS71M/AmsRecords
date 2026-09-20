using static AmsRecords.Diagnostics.FieldDiagnosticDtos;
using static AmsRecords.Holes.HoleDtos;
using static AmsRecords.Surfaces.SurfaceDtos;

namespace AmsRecords.Diagnostics;

public static class FieldDiagnosticSurfaceOptions
{
    // Area is already selected: show configured hole short names, preserving the
    // numeric course sequence rather than alphabetically sorting generated labels.
    public static List<FieldDiagnosticSurfaceOptionDto> ForArea(
        IEnumerable<FieldDiagnosticSurfaceOptionDto> eligible,
        IEnumerable<SurfaceMiniDto> surfaces,
        IEnumerable<HoleMiniDto> holes)
    {
        var options = eligible.Where(x => x.PubId != Guid.Empty)
            .DistinctBy(x => x.PubId).ToDictionary(x => x.PubId);
        var holeNames = holes.DistinctBy(x => x.PubId).ToDictionary(x => x.PubId);
        return surfaces.Where(x => options.ContainsKey(x.PubId))
            .DistinctBy(x => x.PubId)
            .OrderBy(x => x.HoleNumber)
            .ThenBy(x => holeNames.GetValueOrDefault(x.HolePubId)?.HolShoNam, StringComparer.CurrentCulture)
            .Select(x => options[x.PubId] with
            {
                DisplayName = holeNames.TryGetValue(x.HolePubId, out var hole)
                    && !string.IsNullOrWhiteSpace(hole.HolShoNam)
                    ? hole.HolShoNam.Trim()
                    : options[x.PubId].DisplayName
            }).ToList();
    }
}
