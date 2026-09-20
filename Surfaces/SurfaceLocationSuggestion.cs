using static AmsRecords.Surfaces.SurfaceMappingDtos;

namespace AmsRecords.Surfaces;

public sealed record SurfaceLocationMatch(IReadOnlyList<SurfaceMapLayerItemDto> Candidates, Guid? ClearSurfacePubId);

/// <summary>Advisory only. Never changes a recorded surface or rejects a measurement.</summary>
public static class SurfaceLocationSuggestion
{
    public static SurfaceLocationMatch Evaluate(SurfaceMapLayerDto? map, Guid fieldId, ISet<Guid> turfAreaIds,
        double latitude, double longitude, double? accuracyMetres, DateTimeOffset capturedAt, DateTimeOffset now)
    {
        if (map is null || map.FieldPubId != fieldId || now - capturedAt > TimeSpan.FromSeconds(15)
            || capturedAt - now > TimeSpan.FromSeconds(2) || accuracyMetres is not >= 0 or > 30
            || !double.IsFinite(accuracyMetres.Value)) return new([], null);
        var candidates = new List<SurfaceMapLayerItemDto>();
        Guid? clear = null;
        var margin = Math.Max(3, accuracyMetres.Value);
        foreach (var layer in map.Layers.Where(x => x.LayerType == "surface" && x.ApprovalState == "Approved"
            && x.ParentPubId.HasValue && turfAreaIds.Contains(x.ParentPubId.Value)).DistinctBy(x => x.PubId))
        {
            var distance = SurfaceGeometry.BoundaryDistanceMetres(layer.GeoJson, latitude, longitude);
            if (distance is null) continue;
            var inside = SurfaceGeometry.ContainsPoint(layer.GeoJson, latitude, longitude);
            if (!inside && distance > margin) continue;
            candidates.Add(layer);
            if (inside && distance > margin && accuracyMetres <= 10) clear = layer.PubId;
        }
        return new(candidates, candidates.Count == 1 ? clear : null);
    }
}
