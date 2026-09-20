using System.Text.Json;
using static AmsRecords.Surfaces.SurfaceMappingDtos;

namespace AmsRecords.Surfaces;

/// <summary>Dependency-free offline/server geometry primitives. Areas use a local equirectangular projection.</summary>
public static class SurfaceGeometry
{
    const double EarthRadius = 6371008.8;

    /// <summary>Chooses a target on the largest mapped component, never a course-wide fallback coordinate.</summary>
    public static CoordinateDto? GuidanceTarget(string? geoJson)
    {
        if (string.IsNullOrWhiteSpace(geoJson)) return null;
        try
        {
            using var document = JsonDocument.Parse(geoJson);
            var root = document.RootElement;
            if (!root.TryGetProperty("type", out var type) || !root.TryGetProperty("coordinates", out var coordinates))
                return null;
            var polygons = type.GetString() switch
            {
                "Polygon" => new[] { coordinates },
                "MultiPolygon" => coordinates.EnumerateArray().ToArray(),
                _ => []
            };
            var exteriors = polygons.Where(p => p.GetArrayLength() > 0).Select(p => p[0].EnumerateArray()
                .Select(v => new CoordinateDto(v[1].GetDouble(), v[0].GetDouble())).ToArray())
                .Where(points => points.Length >= 4 && points.All(p => double.IsFinite(p.Latitude)
                    && p.Latitude is >= -90 and <= 90 && double.IsFinite(p.Longitude) && p.Longitude is >= -180 and <= 180)
                    && points[0] == points[^1] && RingAreaM2(points) > 0)
                .OrderByDescending(RingAreaM2).ToArray();
            if (exteriors.Length == 0) return null;
            var exterior = exteriors[0];
            var center = new CoordinateDto(exterior.SkipLast(1).Average(p => p.Latitude),
                exterior.SkipLast(1).Average(p => p.Longitude));
            // For concave outlines or holes, an exterior boundary vertex is safer than an outside centroid.
            return ContainsPoint(geoJson, center.Latitude, center.Longitude) ? center : exterior[0];
        }
        catch (Exception ex) when (ex is JsonException or InvalidOperationException or IndexOutOfRangeException or FormatException)
        {
            return null;
        }
    }
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
        return ContainsPoints(geoJson, [new CoordinateDto(latitude, longitude)])[0];
    }

    /// <summary>Distance to the nearest exterior or exclusion edge, using a local metre projection.</summary>
    public static double? BoundaryDistanceMetres(string? geoJson, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(geoJson) || !double.IsFinite(latitude) || !double.IsFinite(longitude)
            || latitude is < -89 or > 89 || longitude is < -180 or > 180) return null;
        try
        {
            using var document = JsonDocument.Parse(geoJson);
            var root = document.RootElement;
            var coordinates = root.GetProperty("coordinates");
            var polygons = root.GetProperty("type").GetString() switch
            {
                "Polygon" => new[] { coordinates },
                "MultiPolygon" => coordinates.EnumerateArray().ToArray(),
                _ => []
            };
            var lat0 = latitude * Math.PI / 180d;
            var origin = Project(new(latitude, longitude), lat0);
            var minimum = double.PositiveInfinity;
            foreach (var polygon in polygons)
            foreach (var ring in polygon.EnumerateArray())
            {
                var points = ring.EnumerateArray().Select(p => new CoordinateDto(p[1].GetDouble(), p[0].GetDouble())).ToArray();
                if (points.Length < 4 || points[0] != points[^1] || points.Any(p => !double.IsFinite(p.Latitude)
                    || !double.IsFinite(p.Longitude) || p.Latitude is < -90 or > 90 || p.Longitude is < -180 or > 180)) return null;
                for (var i = 1; i < points.Length; i++)
                {
                    var a = Project(points[i - 1], lat0);
                    var b = Project(points[i], lat0);
                    var dx = b.X - a.X; var dy = b.Y - a.Y;
                    var lengthSquared = dx * dx + dy * dy;
                    var t = lengthSquared == 0 ? 0 : Math.Clamp(((origin.X - a.X) * dx + (origin.Y - a.Y) * dy) / lengthSquared, 0, 1);
                    minimum = Math.Min(minimum, Math.Sqrt(Math.Pow(origin.X - a.X - t * dx, 2) + Math.Pow(origin.Y - a.Y - t * dy, 2)));
                }
            }
            return double.IsFinite(minimum) ? minimum : null;
        }
        catch (Exception ex) when (ex is JsonException or InvalidOperationException or KeyNotFoundException or IndexOutOfRangeException or FormatException)
        {
            return null;
        }
    }

    /// <summary>Tests many coordinates against one parsed Polygon or MultiPolygon GeoJSON value.</summary>
    public static IReadOnlyList<bool> ContainsPoints(string? geoJson, IReadOnlyList<CoordinateDto> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        var result = new bool[points.Count];
        if (string.IsNullOrWhiteSpace(geoJson) || points.Count == 0)
            return result;
        try
        {
            using var document = JsonDocument.Parse(geoJson);
            var root = document.RootElement;
            if (!root.TryGetProperty("type", out var type) || !root.TryGetProperty("coordinates", out var coordinates))
                return result;
            var geometryType = type.GetString();
            for (var index = 0; index < points.Count; index++)
            {
                var point = points[index];
                if (!double.IsFinite(point.Latitude) || point.Latitude is < -90 or > 90 ||
                    !double.IsFinite(point.Longitude) || point.Longitude is < -180 or > 180)
                    continue;
                result[index] = geometryType switch
                {
                    "Polygon" => PolygonContains(coordinates, point.Latitude, point.Longitude),
                    "MultiPolygon" => coordinates.EnumerateArray()
                        .Any(x => PolygonContains(x, point.Latitude, point.Longitude)),
                    _ => false
                };
            }
            return result;
        }
        catch (JsonException) { return result; }
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
