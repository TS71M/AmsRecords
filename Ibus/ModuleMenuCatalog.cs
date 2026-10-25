using Ams.Shared.Icons;

namespace AmsRecords.Ibus;

// Presentation only: release, pilot and permission policy remains in ModuleAccessCatalog
// and the persisted API settings. Client adapters retain their own supported routes.
public sealed record ModuleMenuDefinition(string Key, string Title, string Description, string IconCssClass);
public sealed record ModuleMenuGroup(string Title, IReadOnlyList<ModuleMenuDefinition> Modules);

public static class ModuleMenuCatalog
{
    public static IReadOnlyList<ModuleMenuGroup> Groups { get; } =
    [
        new("Core", [
            new("fields", "Fields", "Course, pitch, or site records and their field hubs.", AppIcons.Fields.CssClass),
            new("facility", "Facility", "Organisation details, users, fields, suppliers, machines, and supporting setup.", AppIcons.ModuleFacility.CssClass),
            new("settings", "Settings", "Workspace setup and module preferences.", AppIcons.Settings.CssClass)
        ]),
        new("Operations", [
            new("daily-operations", "Daily Operations", "Job boards, daily logs, and operational tracking.", AppIcons.Operations.CssClass),
            new("clipping-volume", "Clipping Volume", "Record mower catch-box volume and review growth trends.", AppIcons.ClippingVolume.CssClass),
            new("leaf-nitrate", "Leaf Nitrate", "Record fresh-sap nitrate readings and evaluate field trends against local rails.", AppIcons.LeafNitrate.CssClass),
            new("green-speed", "Green Speed", "Record Stimpmeter roll distance and review putting-surface consistency.", AppIcons.GreenSpeed.CssClass),
            new("cutting-height", "Cutting Height", "Record verified heights of cut and review trends.", AppIcons.CuttingHeight.CssClass),
            new("messages", "Messages", "Field-based work conversations and image updates.", AppIcons.Messages.CssClass),
            new("snag-list", "Snag List", "Issues, defects, and course repair follow-up.", AppIcons.SnagList.CssClass),
            new("diagnostics", "Diagnostics", "AI-assisted turf image diagnostics and review queues.", AppIcons.Diagnostic.CssClass),
            new("tdr-moisture", "TDR Moisture", "Capture, synchronize, correct positions, map, and review surface moisture measurements.", AppIcons.TdrMoisture.CssClass),
            new("grass-species-analysis", "Grass Species Analysis", "Identify grass species and weed coverage from representative surface photos.", AppIcons.GrassSpecies.CssClass),
            new("equipment", "Equipment", "Machine servicing, equipment history, and workshop tasks.", AppIcons.ModuleEquipment.CssClass),
            new("inventory", "Inventory", "Multi-location stock counts and movements.", AppIcons.ModuleInventory.CssClass)
        ]),
        new("Agronomy", [
            new("applications", "Applications", "Application recording, execution and corrections.", AppIcons.ModuleApplications.CssClass),
            new("planning", "Planning", "Calendars, weekly planning, projects, and shifts.", AppIcons.Calendar.CssClass),
            new("weather", "Current Weather", "Current field conditions for daily work.", AppIcons.Weather.CssClass),
            new("weather-analysis", "Weather Analysis", "Weather overview, hourly and daily history, risk maps, GDD, GP, and GTS models.", AppIcons.Analysis.CssClass),
            new("surfaces", "Surfaces", "Surface setup and hole area totals.", AppIcons.ModuleSurfaces.CssClass),
            new("surface-mapping", "Surface Mapping", "Capture, refine, approve, and maintain surface boundary geometry.", AppIcons.ModuleSurfaceMapping.CssClass),
            new("soil-tests", "Soil Tests", "Soil test records, imports, and timelines.", AppIcons.ModuleSoilTests.CssClass),
            new("water-tests", "Water Tests", "Water analysis reports and imports.", AppIcons.ModuleWaterTests.CssClass),
            new("irrigation", "Irrigation", "Field-wide simulation, hydraulics, calibration, scenarios, and controller synchronization.", AppIcons.ModuleIrrigation.CssClass)
        ]),
        new("Business", [
            new("infos", "Infos", "Information pages, unit tools, releases, and support references.", AppIcons.Info.CssClass),
            new("e-learning", "E-Learning", "Learning content for diseases and cultivar references.", AppIcons.ELearning.CssClass),
            new("procural", "Procural", "Procurement requests, approvals, quotations, supplier orders, and receiving.", AppIcons.Procural.CssClass),
            new("statistics", "Statistics", "Consumption, nutrient, cost, time, stock, and weather analysis.", AppIcons.ModuleStatistics.CssClass),
            new("documents", "Documents", "Labels, SDS files, reports, certificates, and attached documents.", AppIcons.ModuleDocuments.CssClass),
            new("reports", "Reports", "Configurable PDF and management reports.", AppIcons.ModuleReports.CssClass),
            new("remote", "Remote Access", "Securely access explicitly authorized organisation computers.", AppIcons.ModuleRemote.CssClass)
        ]),
    ];

    public static IReadOnlyList<ModuleMenuDefinition> Modules { get; } = Groups.SelectMany(x => x.Modules).ToArray();
    private static readonly IReadOnlyDictionary<string, ModuleMenuDefinition> ByKey =
        Modules.ToDictionary(x => x.Key, StringComparer.OrdinalIgnoreCase);

    public static ModuleMenuDefinition? Find(string? key) =>
        string.IsNullOrWhiteSpace(key) ? null : ByKey.GetValueOrDefault(key.Trim());
}

