namespace AmsRecords.Irrigation;

/// <summary>User-approved visual legend; does not change catalogue compatibility or saved observations.</summary>
public static class NozzleDisplayIdentity
{
    public sealed record ProductPhoto(string Part, string SourceUrl, string Description);

    // Original official-store photographs, reviewed 2026-09-20. Exact ordering IDs
    // only: size, colour and similar product names must not select another part's image.
    // Oblique assembly/kit views are useful comparisons, not calibrated face evidence.
    public static ProductPhoto? RainBirdProductPhoto(string? manufacturer, string? partNumber)
    {
        if (!string.Equals(manufacturer?.Trim(), "Rain Bird", StringComparison.OrdinalIgnoreCase)) return null;
        var part = partNumber?.Trim();
        var item = part switch
        {
            "213701" => ("rain-bird-700-751-spreader-nozzle-midrange-green.html",
                "Oblique green midrange spreader photograph; the slanted mouth and internal feature are visible, but this is not an installed-face or clock-angle baseline."),
            "213702" => ("rain-bird-700-751-spreader-nozzle-midrange-orange.html",
                "Oblique orange midrange spreader photograph; the slanted mouth and internal feature are visible, but this is not an installed-face or clock-angle baseline."),
            "211593" => ("rain-bird-700-series-spreader-nozzle-plug.html",
                "Oblique gray spreader-plug product photograph with a closed end; not a discharging nozzle or proof of installed orientation. The photographed colour does not overwrite recorded colour evidence."),
            "211417" => ("739390046106.html",
                "Oblique black midrange spreader photograph; the official page has a numeric title, but SKU 211417 and its description identify the component. Not a calibrated installed-face or clock-angle reference."),
            "21370032" => ("rain-bird-700-series-range-nozzle-assembly-32-blue-nozzle.html",
                "Oblique blue range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21370040" => ("rain-bird-700-series-range-nozzle-assembly-40-orange-nozzle.html",
                "Oblique orange range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21370044" => ("rain-bird-700-series-range-nozzle-assembly-44-green-nozzle.html",
                "Oblique green range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21375036" => ("rain-bird-700-751-series-penta-nozzle-assembly-36-yellow-nozzle.html",
                "Oblique yellow Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21375040" => ("rain-bird-700-751-series-penta-nozzle-assembly-40-orange-nozzle.html",
                "Oblique orange Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21375044" => ("rain-bird-700-751-series-penta-nozzle-assembly-44-green-nozzle.html",
                "Oblique green Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21375048" => ("rain-bird-700-751-series-penta-nozzle-assembly-48-black-nozzle.html",
                "Oblique black Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21375122" => ("rain-bird-751-series-penta-nozzle-assembly-22-red-nozzle.html",
                "Product-kit photograph: the red Penta nozzle is on the right; the separate pale component on the left is not an additional observed nozzle or a sprinkler-cap reference."),
            "21370028" => ("rain-bird-700-series-range-nozzle-assembly-28-white-nozzle.html",
                "Oblique white range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21370036" => ("rain-bird-700-series-range-nozzle-assembly-36-yellow-nozzle.html",
                "Oblique yellow range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21375028" => ("rain-bird-700-751-series-penta-nozzle-assembly-28-white-nozzle.html",
                "Oblique white Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21370048" => ("rain-bird-700-series-range-nozzle-assembly-48-black.html",
                "Oblique black range-nozzle assembly photograph; aperture and body are visible, but this view does not establish exact size or installed clock angle."),
            "21375032" => ("rain-bird-700-751-series-penta-nozzle-assembly-32-blue-nozzle.html",
                "Oblique blue Penta assembly photograph; multiple discharge features belong to one replaceable assembly, not independently identified nozzle positions."),
            "21375120" => ("rain-bird-751-series-penta-nozzle-assembly-20-gray-nozzle.html",
                "Product-kit photograph: the gray Penta nozzle is on the right; the separate black component on the left is not an additional observed nozzle or a sprinkler-cap reference."),
            "213705" => ("rain-bird-700-751-spreader-nozzle-midrange-blue-with-diffuser.html",
                "Oblique blue midrange spreader-with-diffuser photograph; not an installed face or a documented clock-angle baseline."),
            _ => ((string?)null, (string?)null)
        };
        return item.Item1 is null ? null : new(part!, "https://store.rainbird.com/" + item.Item1, item.Item2!);
    }

    // Individually labelled auxiliary schematics, Hunter Landscape Vol41 EM p35.
    // Exact parts only: neither size aliases nor primary colour circles are face references.
    public static string? HunterSchematicPart(string? manufacturer, string? partNumber)
        => string.Equals(manufacturer?.Trim(), "Hunter", StringComparison.OrdinalIgnoreCase) &&
           partNumber?.Trim() is "803603" or "315313" or "803611" or "315311"
            ? partNumber.Trim() : null;

    // Rain Bird international turf catalogue 2022, page 41. Size aliases are model-local,
    // never ordering identities shared by other Rain Bird rotors or other manufacturers.
    public static string? RainBirdMaxiPawPart(string? manufacturer, string? modelCode, string? nozzleCode)
    {
        if (!string.Equals(manufacturer?.Trim(), "Rain Bird", StringComparison.OrdinalIgnoreCase)) return null;
        var code = nozzleCode?.Trim().ToUpperInvariant();
        if (code is "206592-06" or "206592-07" or "206592-08" or "206592-10" or "206592-12" or "115902-07" or "115902-10")
            return code;
        if (!string.Equals(modelCode?.Trim(), "2045A", StringComparison.OrdinalIgnoreCase)) return null;
        return code switch
        {
            "06" => "206592-06", "07" => "206592-07", "08" => "206592-08",
            "10" => "206592-10", "12" => "206592-12",
            "07 LA" => "115902-07", "10 LA" => "115902-10", _ => null
        };
    }

    // Presentation only: preserve recorded colour evidence and part identities in storage.
    public static string ColorLabel(string? color)
    {
        var value = color?.Trim() ?? "";
        return value.StartsWith('#') ? value
            : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
    }

    public static string? ApprovedColor(string? manufacturer, string? partNumber) =>
        !string.Equals(manufacturer?.Trim(), "Toro", StringComparison.OrdinalIgnoreCase) ? null : partNumber?.Trim() switch
        {
            "102-2925" => "Gray",
            "102-2910" => "Blue",
            _ => null
        };
}
