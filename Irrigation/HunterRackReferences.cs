namespace AmsRecords.Irrigation;

// Manufacturer-labelled studio pixels, not global part numbers or installed-angle evidence.
public static class HunterRackReferences
{
    public const string Attribution = "Photo courtesy Hunter Industries";
    public const string SourceUrl = "https://www.hunterirrigation.com/node/154841";
    public const string SourceSha256 = "7F7172D22CF960E1C751767CA50F95399474808DEB08450C1553721CA0DF51DF";
    public sealed record Face(int X, int Y, int Width, int Height, bool Inverted, string SourceKey = "Rack3");
    public sealed record Source(string Key, int Width, int Height, string Sha256, string Url);
    public static Source SourceFor(Face face) => face.SourceKey switch
    {
        "MPR25" => new("MPR25", 800, 800, "B3B46314DF6D363C7BB6D71B34B85EA3739F22C0BC06595E21D6900F77F5F996", "https://www.hunterirrigation.com/en-metric/irrigation-product/mpr-25"),
        "MPR30" => new("MPR30", 800, 800, "2B5420BD762414C542B90866FD18F77C2EFE249E0329A9E8C5E0781F20B6BBED", "https://www.hunterirrigation.com/en-metric/irrigation-product/mpr-30"),
        "MPR35" => new("MPR35", 800, 800, "D33107973DA59AE2C3D3BF129992A91C6B99237E949AB9748FFC7DD34C27DC89", "https://www.hunterirrigation.com/en-metric/irrigation-product/mpr-35"),
        "BlackSR" => new("BlackSR", 500, 283, "1D408BA6461450F074F82A0B9D1922AD94CFB72961ED10E0632C051E94FFCDD7", "https://www.hunterirrigation.com/sites/default/files/pgp-black-nozzles-path.png"),
        "Rack3" => new("Rack3", 1090, 437, SourceSha256, SourceUrl),
        _ => throw new ArgumentOutOfRangeException(nameof(face))
    };
    const string AssetPrefix = "/_content/WebClient.Shared/images/nozzles/hunter/rack/hunter-rack-";

    public static string? ImageUrl(string? manufacturer, string? modelCode, string? designation)
        => Find(manufacturer, modelCode, designation) is null ? null :
            AssetPrefix + designation!.Trim().ToLowerInvariant().Replace('.', '-').Replace(' ', '-') + ".webp";

    public static string ImageAttribution(string? imageUrl)
    {
        if (imageUrl is null || !imageUrl.StartsWith(AssetPrefix, StringComparison.Ordinal) ||
            !imageUrl.EndsWith(".webp", StringComparison.Ordinal)) return "";
        var key = imageUrl[AssetPrefix.Length..^5];
        var code = key.StartsWith("mpr-", StringComparison.Ordinal) ? key.ToUpperInvariant()
            : key.EndsWith("-la", StringComparison.Ordinal) ? key[..^3].Replace('-', '.') + " LA"
            : key.EndsWith("-sr", StringComparison.Ordinal) ? key[..^3].Replace('-', '.') + " SR"
            : key.Replace('-', '.');
        return ImageUrl("Hunter", "I-20", code) == imageUrl ? Attribution : "";
    }

    public static IrrigationDtos.IrrigationSprinklerNozzleOptionDto FillMissing(IrrigationDtos.IrrigationSprinklerNozzleOptionDto option)
        => option.Active && string.IsNullOrWhiteSpace(option.ReferenceImageUrl)
            ? option with { ReferenceImageUrl = ImageUrl(option.ManufacturerName, option.SprinklerModelCode, option.NozzleCode) }
            : option;

    public static Face? Find(string? manufacturer, string? modelCode, string? designation)
    {
        if (!string.Equals(manufacturer?.Trim(), "Hunter", StringComparison.OrdinalIgnoreCase) ||
            modelCode?.Trim().ToUpperInvariant() is not ("PGP-ULTRA" or "I-20")) return null;
        var code = designation?.Trim().ToUpperInvariant();
        if (code is not null && code.StartsWith("MPR-", StringComparison.Ordinal))
        {
            var parts = code.Split('-');
            if (parts.Length != 3 || parts[1] is not ("25" or "30" or "35")) return null;
            var key = "MPR" + parts[1];
            return parts[2] switch
            {
                "90" => new(115, 205, 180, 225, false, key),
                "120" => new(350, 110, 185, 240, false, key),
                "180" => new(220, 460, 175, 175, true, key),
                "360" => new(475, 375, 165, 185, true, key),
                _ => null
            };
        }
        return code switch
        {
            "1.0 SR" => new(30, 49, 65, 70, false, "BlackSR"),
            "2.0 SR" => new(128, 48, 65, 72, false, "BlackSR"),
            "0.50 SR" => new(402, 48, 65, 72, false, "BlackSR"),
            "1.5 SR" => new(32, 169, 65, 70, true, "BlackSR"),
            "3.0 SR" => new(304, 167, 65, 72, true, "BlackSR"),
            "0.75 SR" => new(400, 169, 65, 70, true, "BlackSR"),
            "1.5" => new(25, 45, 150, 105, false),
            "2.5" => new(185, 45, 145, 105, false),
            "4.0" => new(340, 45, 155, 105, false),
            "6.0" => new(505, 40, 150, 110, false),
            "2.0" => new(30, 295, 145, 95, true),
            "3.0" => new(190, 295, 140, 95, true),
            "5.0" => new(350, 295, 145, 95, true),
            "8.0" => new(510, 290, 145, 95, true),
            "2.0 LA" => new(785, 40, 125, 115, false),
            "3.5 LA" => new(945, 40, 125, 115, false),
            "2.5 LA" => new(785, 295, 125, 95, true),
            "4.5 LA" => new(945, 290, 125, 95, true),
            _ => null
        };
    }
}
