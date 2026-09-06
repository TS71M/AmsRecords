using static AmsRecords.Surfaces.SurfaceDtos;

namespace AmsRecords.Surfaces;

public static class SurfaceIndexExtensions
{
    /// <summary>
    /// Returns each physical Surface once even though the field index intentionally
    /// presents a shared Surface beneath every assigned hole.
    /// </summary>
    public static IReadOnlyList<SurfaceIndexDto> PhysicalSurfaces(this SurfacesIndexDto index)
    {
        var occurrences = index.HoleCards
            .SelectMany(card => card.Surfaces.Select(surface => new SurfaceOccurrence(surface, card)))
            .Where(x => x.Surface.PubId != Guid.Empty)
            .ToList();

        return occurrences
            .GroupBy(x => x.Surface.PubId)
            .Select(group =>
            {
                var surface = group.First().Surface;
                if (surface.HoleAssignments.Count == 0)
                    surface.HoleAssignments = BuildFallbackAssignments(group, surface.HoleNumber);

                return surface;
            })
            .ToList();
    }

    static List<SurfaceHoleAssignmentDto> BuildFallbackAssignments(
        IEnumerable<SurfaceOccurrence> occurrences,
        int primaryHoleNumber)
    {
        var holes = occurrences
            .Select(x => x.Hole)
            .GroupBy(x => x.HolePubId)
            .Select(x => x.First())
            .OrderBy(x => x.HoleNumber)
            .ToList();
        var primaryHolePubId = holes.FirstOrDefault(x => x.HoleNumber == primaryHoleNumber)?.HolePubId
            ?? holes.First().HolePubId;

        return holes
            .Select(x => new SurfaceHoleAssignmentDto(
                x.HolePubId,
                x.HoleNumber,
                x.HoleName,
                x.HolePubId == primaryHolePubId))
            .ToList();
    }

    sealed record SurfaceOccurrence(SurfaceIndexDto Surface, HoleSurfaceCardDto Hole);
}
