using System.Text.Json;
using static AmsRecords.Surfaces.SurfaceMappingDtos;

namespace AmsRecords.Surfaces;

/// <summary>Dependency-free offline/server geometry primitives. Areas use a local equirectangular projection.</summary>
public static class SurfaceGeometry
{
    const double EarthRadius = 6371008.8;
    /// <summary>Boundary mode accepts every physical READ but persists its impossible moisture sentinel as -1 VWC.</summary>
    public static decimal NormalizeBoundaryVwc(decimal? measuredVwcPercent) => -1m;
    public static double RingAreaM2(IReadOnlyList<CoordinateDto> points)
    {
        if (points.Count < 3) return 0;
        var lat0 = points.Average(p => p.Latitude) * Math.PI / 180d;
        double sum = 0;
        for (var i = 0; i < points.Count; i++)
        {
            var a = Project(points[i], lat0); var b = Project(points[(i + 1) % points.Count], lat0);
            sum += a.X * b.Y - b.X * a.Y;
        }
        return Math.Abs(sum) / 2d;
    }

    public static double GeometryAreaM2(IEnumerable<SubsectionGeometryDto> sections) => sections.Sum(s =>
        s.Rings.Sum(r => (r.IsExclusion ? -1d : 1d) * RingAreaM2(r.Points)));

    public static bool IsNearStart(IReadOnlyList<CoordinateDto> points, double thresholdMetres = 2.5) =>
        points.Count >= 3 && DistanceM(points[0], points[^1]) <= thresholdMetres;

    public static IReadOnlyList<string> Validate(IEnumerable<SubsectionGeometryDto> sections)
    {
        var warnings = new List<string>();
        foreach (var s in sections)
        foreach (var r in s.Rings)
        {
            if (r.Points.Count < 3) warnings.Add($"{s.Name}: ring {r.RingId} needs at least three points.");
            if (r.Points.Any(p => p.Latitude is < -90 or > 90 || p.Longitude is < -180 or > 180))
                warnings.Add($"{s.Name}: ring {r.RingId} contains invalid coordinates.");
            if (r.Points.Zip(r.Points.Skip(1), DistanceM).Any(d => d > 250))
                warnings.Add($"{s.Name}: ring {r.RingId} contains a likely GNSS outlier.");
        }
        return warnings;
    }

    public static string ToGeoJson(IEnumerable<SubsectionGeometryDto> sections)
    {
        var polygons = sections.SelectMany(s => s.Rings.Where(r => !r.IsExclusion).Select(exterior =>
        {
            var rings = new List<double[][]> { Close(exterior.Points) };
            rings.AddRange(s.Rings.Where(r => r.IsExclusion && r.Points.Count >= 3).Select(r => Close(r.Points)));
            return rings.ToArray();
        })).ToArray();
        return JsonSerializer.Serialize(new { type = "MultiPolygon", coordinates = polygons });
    }

    public static bool ContainsPoint(string? geoJson, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(geoJson) || latitude is < -90 or > 90 || longitude is < -180 or > 180)
            return false;
        try
        {
            using var document = JsonDocument.Parse(geoJson);
            var root = document.RootElement;
            if (!root.TryGetProperty("type", out var type) || !root.TryGetProperty("coordinates", out var coordinates))
                return false;
            return type.GetString() switch
            {
                "Polygon" => PolygonContains(coordinates, latitude, longitude),
                "MultiPolygon" => coordinates.EnumerateArray().Any(x => PolygonContains(x, latitude, longitude)),
                _ => false
            };
        }
        catch (JsonException) { return false; }
    }

    static bool PolygonContains(JsonElement polygon, double latitude, double longitude)
    {
        var rings = polygon.EnumerateArray().ToList();
        if (rings.Count == 0 || !RingContains(rings[0], latitude, longitude)) return false;
        return !rings.Skip(1).Any(x => RingContains(x, latitude, longitude));
    }

    static bool RingContains(JsonElement ring, double latitude, double longitude)
    {
        var points = ring.EnumerateArray().Select(x =>
        {
            var values = x.EnumerateArray().ToArray();
            return values.Length >= 2
                ? (Longitude: values[0].GetDouble(), Latitude: values[1].GetDouble())
                : (Longitude: double.NaN, Latitude: double.NaN);
        }).Where(x => double.IsFinite(x.Longitude) && double.IsFinite(x.Latitude)).ToList();
        if (points.Count < 3) return false;
        var inside = false;
        for (var i = 0; i < points.Count; i++)
        {
            var a = points[i];
            var b = points[(i + points.Count - 1) % points.Count];
            if ((a.Latitude > latitude) != (b.Latitude > latitude) &&
                longitude < (b.Longitude - a.Longitude) * (latitude - a.Latitude) /
                (b.Latitude - a.Latitude) + a.Longitude)
                inside = !inside;
        }
        return inside;
    }

    static double[][] Close(IReadOnlyList<CoordinateDto> points)
    {
        if (points.Count == 0) return [];
        var result = points.Select(p => new[] { p.Longitude, p.Latitude }).ToList();
        if (points[0] != points[^1]) result.Add([points[0].Longitude, points[0].Latitude]);
        return [.. result];
    }
    static (double X, double Y) Project(CoordinateDto p, double lat0) =>
        (EarthRadius * p.Longitude * Math.PI / 180d * Math.Cos(lat0), EarthRadius * p.Latitude * Math.PI / 180d);
    static double DistanceM(CoordinateDto a, CoordinateDto b)
    {
        var dLat = (b.Latitude - a.Latitude) * Math.PI / 180d;
        var dLon = (b.Longitude - a.Longitude) * Math.PI / 180d;
        var lat1 = a.Latitude * Math.PI / 180d; var lat2 = b.Latitude * Math.PI / 180d;
        var h = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
        return 2 * EarthRadius * Math.Asin(Math.Min(1, Math.Sqrt(h)));
    }
}
