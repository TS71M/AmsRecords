using System.Text.Json;
using System.Text.Json.Serialization;
using static AmsRecords.Irrigation.SprinklerPerformanceDtos;

namespace AmsRecords.Irrigation;

public static class DistributionMeasurementParser
{
    public const int MaximumCharacters = 64000;
    public const int MaximumPoints = 500;
    static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 8
    };

    public static DistributionImportPreview? Parse(string? json, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(json) || json.Length > MaximumCharacters)
        { error = "Supply a measurement JSON document of at most 64,000 characters."; return null; }
        DistributionMeasurementImport? data;
        try { data = JsonSerializer.Deserialize<DistributionMeasurementImport>(json, Options); }
        catch (JsonException) { error = "Invalid measurement JSON or unsupported fields. Use the version 1 template; raw PRF/DTA/DTM files are not accepted."; return null; }
        if (data is null) { error = "Measurement data are required."; return null; }
        error = Validate(data);
        if (error is not null) return null;
        var distanceFactor = data.DistanceUnit == "ft" ? 0.3048m : 1m;
        var radius = data.Points[^1].Distance * distanceFactor;
        if (radius is < 0.001m or > 200m)
        { error = "Measured radius must be between 0.001 and 200 metres."; return null; }
        var maximum = data.Points.Max(p => p.Application);
        var points = data.Points.Select(p => new SprinklerDistributionPointSaveDto(
            decimal.Round(p.Distance * distanceFactor / radius, 6, MidpointRounding.AwayFromZero),
            decimal.Round(p.Application / maximum, 6, MidpointRounding.AwayFromZero))).ToList();
        if (points.Select(p => p.NormalizedDistance).Distinct().Count() != points.Count)
        { error = "Distances collide at the catalogue's six-decimal precision."; return null; }
        var integratedFlow = IntegrateFlow(data, distanceFactor);
        var recovery = integratedFlow / (double)data.FlowM3H * 100;
        var volumeCheck = FormattableString.Invariant($" Piecewise-linear integration of the absolute radial samples gives {integratedFlow:F4} m³/h, or {recovery:F1}% of the stated measured flow. Review units, test duration, collection losses and sampling before activation. This diagnostic does not certify the test or rescale its measurements.");
        return new(data, radius, points,
            "Imported tests are saved inactive. These are measurements for the stated nozzle option and test conditions, not manufacturer set approval. Activate only after verifying applicability and matching pressure-performance data. No interpolation or extrapolation of measured profiles is performed." + volumeCheck,
            integratedFlow, recovery);
    }

    static double IntegrateFlow(DistributionMeasurementImport data, decimal distanceFactor)
    {
        var rateFactor = data.ApplicationUnit == "in/h" ? 25.4m : 1m;
        decimal radialIntegral = 0;
        for (var i = 1; i < data.Points.Count; i++)
        {
            var a = data.Points[i - 1]; var b = data.Points[i];
            var r0 = a.Distance * distanceFactor; var r1 = b.Distance * distanceFactor;
            var p0 = a.Application * rateFactor; var p1 = b.Application * rateFactor;
            // Exact integral of r*p(r) for a linear segment. Integrating depth alone
            // would miss the increasing area of successive radial rings.
            radialIntegral += (r1 - r0) / 6 * ((2 * r0 + r1) * p0 + (r0 + 2 * r1) * p1);
        }
        // Current import contract is full-circle; mm*m² is litres, not m³.
        return 2 * Math.PI * (double)radialIntegral / 1000;
    }

    static string? Validate(DistributionMeasurementImport d)
    {
        if (d.SchemaVersion != 1) return "Only measurement schema version 1 is supported.";
        if (d.Pattern != "Radial" || d.ApplicationScope != "NozzleOption" || d.ArcDegrees != 360)
            return "This importer accepts full-circle radial tests of the exact nozzle option only. Whole-assembly, partial-circle and 2D tests require a context-aware format; they must not be flattened or borrowed.";
        if (!Text(d.Manufacturer, 120) || !Text(d.ModelCode, 80) || !Text(d.NozzleCode, 80) ||
            !Text(d.Position, 40) || !Text(d.Generation, 120))
            return "Manufacturer, model code, nozzle code, position and generation are required within the template limits.";
        if (!Text(d.Source, 500) || !Text(d.TestId, 120) || !Text(d.ReusePermission, 1000) || !Text(d.TestConditions, 2000))
            return "Supply the source (500 characters), test ID (120), reuse permission evidence (1,000), and test conditions (2,000).";
        if (d.SourceSha256 is null || d.SourceSha256.Length != 64 || d.SourceSha256.Any(c => !Uri.IsHexDigit(c)))
            return "Supply the SHA-256 fingerprint of the original source file (64 hexadecimal characters).";
        if (d.TestDate == default || d.TestDate > DateOnly.FromDateTime(DateTime.UtcNow)) return "Supply a valid test date that is not in the future.";
        if (d.PressureBar is < 0.001m or > 100m || decimal.Round(d.PressureBar, 3) != d.PressureBar)
            return "Pressure must be 0.001–100 bar with at most three decimal places.";
        if (d.HeightM is < 0m or > 20m || d.FlowM3H is <= 0m or > 1000m) return "Supply test height (0–20 m) and measured flow (greater than zero, at most 1,000 m³/h).";
        if (d.DistanceUnit is not ("m" or "ft") || d.ApplicationUnit is not ("mm/h" or "in/h"))
            return "Supported units are m or ft, and mm/h or in/h. Convert catch depth to a rate using the actual test duration first.";
        if (d.Points is null || d.Points.Count is < 3 or > MaximumPoints || d.Points.Any(p => p is null))
            return "Supply 3–500 measured points, including the origin and the end of the pattern.";
        if (d.Points.Any(p => p.Distance is < 0 or > 1000 || p.Application is < 0 or >= 9999))
            return "Measurements must be nonnegative and within limits. Missing-value sentinels such as 9999 must be resolved against the original source.";
        if (d.Points[0].Distance != 0 || d.Points[^1].Application != 0 || d.Points.Max(p => p.Application) <= 0)
            return "A complete measured profile must start at distance zero, include positive application, and end at measured zero application. Do not invent endpoints.";
        if (d.Points.Zip(d.Points.Skip(1)).Any(p => p.First.Distance >= p.Second.Distance))
            return "Measurement distances must be unique and strictly increasing.";
        return null;
    }

    static bool Text(string? value, int maximum) => !string.IsNullOrWhiteSpace(value) && value.Length <= maximum &&
        !value.StartsWith("REPLACE:", StringComparison.OrdinalIgnoreCase);
}
