using static AmsRecords.Irrigation.SprinklerPerformanceDtos;

namespace AmsRecords.Irrigation;

/// <summary>Published complete standard assemblies for catalogue entries whose numerical rows are not yet indexed.</summary>
public static class IrrigationFactoryPerformanceReferences
{
    public const string RainBird752Source = "https://www.rainbird.com/media/17940";
    // Manufacturer metric table, PDF page 3, standard housing / standard valve; radius m, total assembly flow m³/h.
    // Low-angle radii and the low-flow-valve assemblies (#18–26) are deliberately separate configurations.
    static readonly decimal[] Pressures = [3.4m, 4.1m, 4.8m, 5.5m, 6.2m, 6.9m];
    static readonly IReadOnlyDictionary<string, (decimal[] Radius, decimal[] Flow)> RainBird752 =
        new Dictionary<string, (decimal[], decimal[])>
        {
            ["28"] = ([16.5m,17.1m,17.7m,17.7m,17.4m,18.0m], [3.38m,3.71m,3.99m,4.27m,4.58m,4.86m]),
            ["32"] = ([18.9m,18.9m,19.2m,19.2m,20.4m,20.4m], [3.88m,4.32m,4.62m,4.94m,5.20m,5.44m]),
            ["36"] = ([19.5m,19.8m,20.1m,20.7m,20.7m,21.0m], [4.44m,4.84m,5.27m,5.61m,5.96m,6.18m]),
            ["40"] = ([19.2m,19.8m,20.4m,20.7m,21.0m,21.0m], [5.06m,5.44m,5.98m,6.34m,6.75m,7.06m])
        };

    public static IReadOnlyList<SprinklerPerformanceDataPointDto> ForFactoryAssembly(string manufacturer, string model, string nozzle)
    {
        if (manufacturer != "Rain Bird" || model != "752" || !RainBird752.TryGetValue(nozzle, out var row)) return [];
        return Pressures.Select((pressure,i) => new SprinklerPerformanceDataPointDto(
            // Stable evidence identifiers also survive scenario snapshots and deterministic reruns.
            new Guid($"75200000-0000-0000-{int.Parse(nozzle):D4}-{i + 1:D12}"), pressure,row.Flow[i],row.Radius[i],null,null,
            "Rain Bird 752 standard housing and standard valve · complete Dual-Spreader assembly · performance chart page 3",
            Lib.Constants.SprinklerPerformanceDataQualityCodes.Manufacturer)).ToArray();
    }
}
