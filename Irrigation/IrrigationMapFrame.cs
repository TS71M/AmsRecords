using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AmsRecords.Irrigation;

public sealed record IrrigationGpsPoint(double Lat, double Lng);
public sealed record IrrigationMapAnchor(double X, double Y, double Lat, double Lng);
public sealed record IrrigationMapBasis(IrrigationGpsPoint Origin, IrrigationGpsPoint East100M, IrrigationGpsPoint North100M);
public sealed record IrrigationBoundaryMapDto(string? FrameVersion, string? SetupMessage,
    IReadOnlyList<IrrigationGpsPoint> Points, IReadOnlyList<IrrigationGpsPoint> Sprinklers, IrrigationMapBasis? Basis = null);
public sealed record IrrigationBoundaryMapSaveDto(string FrameVersion, IReadOnlyList<IrrigationGpsPoint> Points);

// A rigid fit preserves the metre distances used by the precipitation engine. Reject a
// drawing grid with a different scale instead of silently changing nozzle throw distances.
public sealed class IrrigationMapFrame
{
    const double EarthRadius = 6371008.8;
    const double Radians = Math.PI / 180;
    readonly double lat, lng, x, y, east, north, cosine, sine;
    public string Version { get; }
    public static IrrigationMapFrame AtGpsOrigin(IrrigationGpsPoint origin)
    {
        if (!Valid(origin)) throw new ArgumentException("The surface has an invalid map location.");
        return new(origin);
    }
    IrrigationMapFrame(IrrigationGpsPoint origin)
    {
        lat = origin.Lat; lng = origin.Lng; cosine = 1;
        Version = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(origin))));
    }
    IrrigationMapFrame(IrrigationMapAnchor[] anchors)
    {
        lat = anchors.Average(p => p.Lat); lng = anchors.Average(p => p.Lng);
        x = anchors.Average(p => p.X); y = anchors.Average(p => p.Y);
        var geographic = anchors.Select(p => Project(new(p.Lat, p.Lng))).ToArray();
        east = geographic.Average(p => p.X); north = geographic.Average(p => p.Y);
        double dot = 0, cross = 0, spread = 0;
        for (var i = 0; i < anchors.Length; i++)
        {
            var dx = anchors[i].X - x; var dy = anchors[i].Y - y;
            var de = geographic[i].X - east; var dn = geographic[i].Y - north;
            dot += dx * de + dy * dn; cross += dx * dn - dy * de;
            spread += dx * dx + dy * dy;
        }
        var magnitude = Math.Sqrt(dot * dot + cross * cross);
        if (spread / anchors.Length < 100 || magnitude <= 0 || Math.Abs(magnitude / spread - 1) > 0.02)
            throw new ArgumentException("The linked sprinkler locations do not match the imported drawing scale. Check their map locations and Lynx links before drawing a watering area.");
        cosine = dot / magnitude; sine = cross / magnitude;
        if (anchors.Any(p => { var q = ToPlanar(new(p.Lat, p.Lng)); return Math.Sqrt((q.X - p.X) * (q.X - p.X) + (q.Y - p.Y) * (q.Y - p.Y)) > 3; }))
            throw new ArgumentException("The linked sprinkler locations do not line up with the imported drawing. Check their map locations and Lynx links before drawing a watering area.");
        Version = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(anchors))));
    }
    public static IrrigationMapFrame Create(IEnumerable<IrrigationMapAnchor> source)
    {
        var anchors = source.OrderBy(p => p.X).ThenBy(p => p.Y).ThenBy(p => p.Lat).ThenBy(p => p.Lng).ToArray();
        if (anchors.Length < 3)
            throw new ArgumentException("First locate and link at least three well-spaced sprinklers on the course map. This lets the app align the imported sprinkler drawing with Google Maps.");
        if (anchors.Any(p => !double.IsFinite(p.X) || !double.IsFinite(p.Y) || !Valid(new(p.Lat,p.Lng))))
            throw new ArgumentException("Some linked sprinkler locations are invalid. Correct them on the course map first.");
        return new(anchors);
    }
    public static bool Valid(IrrigationGpsPoint p) => double.IsFinite(p.Lat) && double.IsFinite(p.Lng) && Math.Abs(p.Lat) < 85 && Math.Abs(p.Lng) <= 180;
    IrrigationPlanarPoint Project(IrrigationGpsPoint p)
    {
        if (!Valid(p)) throw new ArgumentException("The map boundary contains an invalid location.");
        var delta = (p.Lng - lng + 540) % 360 - 180;
        return new(delta * Radians * EarthRadius * Math.Cos(lat * Radians), (p.Lat - lat) * Radians * EarthRadius);
    }
    public IrrigationPlanarPoint ToPlanar(IrrigationGpsPoint p)
    {
        var q = Project(p); var de = q.X - east; var dn = q.Y - north;
        return new(x + cosine * de + sine * dn, y - sine * de + cosine * dn);
    }
    public IrrigationGpsPoint ToGps(IrrigationPlanarPoint p)
    {
        var de = east + cosine * (p.X - x) - sine * (p.Y - y);
        var dn = north + sine * (p.X - x) + cosine * (p.Y - y);
        return new(lat + dn / (Radians * EarthRadius), (lng + de / (Radians * EarthRadius * Math.Cos(lat * Radians)) + 540) % 360 - 180);
    }
}

