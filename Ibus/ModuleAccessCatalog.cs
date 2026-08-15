namespace AmsRecords.Ibus;

public sealed record ModuleAccessDefinition(
    string Key,
    bool IsReleased = true,
    bool IsPermissionAssignable = true,
    bool IsPilotCapable = true,
    string? LegacyPermissionKey = null);

public static class ModuleAccessCatalog
{
    public const string Fields = "fields";
    public const string Facility = "facility";
    public const string Settings = "settings";
    public const string DailyOperations = "daily-operations";
    public const string ClippingVolume = "clipping-volume";
    public const string GreenSpeed = "green-speed";
    public const string CuttingHeight = "cutting-height";
    public const string Messages = "messages";
    public const string SnagList = "snag-list";
    public const string Diagnostics = "diagnostics";
    public const string TdrMoisture = "tdr-moisture";
    public const string GrassSpeciesAnalysis = "grass-species-analysis";
    public const string Applications = "applications";
    public const string Planning = "planning";
    public const string CurrentWeather = "weather";
    public const string WeatherAnalysis = "weather-analysis";
    public const string Surfaces = "surfaces";
    public const string SurfaceMapping = "surface-mapping";
    public const string SoilTests = "soil-tests";
    public const string WaterTests = "water-tests";
    public const string LeafNitrate = "leaf-nitrate";
    public const string Procural = "procural";
    public const string Statistics = "statistics";
    public const string Infos = "infos";
    public const string ELearning = "e-learning";
    public const string Equipment = "equipment";
    public const string Documents = "documents";
    public const string Reports = "reports";
    public const string Inventory = "inventory";
    public const string Irrigation = "irrigation";

    public static IReadOnlyList<ModuleAccessDefinition> All { get; } =
    [
        new(Fields),
        new(Facility),
        new(Settings),
        new(DailyOperations),
        new(ClippingVolume, LegacyPermissionKey: DailyOperations),
        new(GreenSpeed, LegacyPermissionKey: DailyOperations),
        new(CuttingHeight, LegacyPermissionKey: DailyOperations),
        new(Messages),
        new(SnagList),
        new(Diagnostics),
        new(TdrMoisture, LegacyPermissionKey: Fields),
        new(GrassSpeciesAnalysis),
        new(Applications, IsReleased: false),
        new(Planning, IsReleased: false),
        new(CurrentWeather),
        new(WeatherAnalysis),
        new(Surfaces),
        new(SurfaceMapping, LegacyPermissionKey: Surfaces),
        new(SoilTests),
        new(WaterTests),
        new(LeafNitrate),
        new(Procural),
        new(Statistics, IsReleased: false, IsPermissionAssignable: false),
        new(Infos),
        new(ELearning),
        new(Equipment, IsReleased: false, IsPermissionAssignable: false),
        new(Documents, IsReleased: false, IsPermissionAssignable: false),
        new(Reports, IsReleased: false, IsPermissionAssignable: false),
        new(Inventory, IsReleased: false, IsPermissionAssignable: false),
        new(Irrigation, IsReleased: false, IsPermissionAssignable: false)
    ];
}
