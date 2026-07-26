using System.Globalization;
using static AmsRecords.Units.UnitDtos;

namespace AmsRecords.Units;

public enum AdaptiveUnitQuantity
{
    Length,
    Surface,
    Volume,
    Mass
}

public sealed record AdaptiveUnitValue(
    decimal CanonicalValue,
    string CanonicalUnitShort,
    decimal DisplayValue,
    UnitDto Unit)
{
    public string UnitShort => Unit.UnitShort;

    public string Text(CultureInfo? culture = null)
        => $"{FormatReadable(DisplayValue, culture)} {UnitShort}".Trim();

    public string ExactText(CultureInfo? culture = null)
        => $"{FormatExact(CanonicalValue, culture)} {CanonicalUnitShort}".Trim();

    public string InputValue
        => DisplayValue.ToString("0.##########", CultureInfo.InvariantCulture);

    static string FormatReadable(decimal value, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentCulture;
        var absolute = Math.Abs(value);
        var precision = absolute switch
        {
            >= 100m => 1,
            >= 10m => 1,
            >= 1m => 2,
            _ => 3
        };
        var rounded = Math.Round(value, precision, MidpointRounding.AwayFromZero);
        var format = rounded == decimal.Truncate(rounded)
            ? "N0"
            : $"N{precision}";
        return rounded.ToString(format, culture);
    }

    static string FormatExact(decimal value, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentCulture;
        return value.ToString("#,0.##########", culture);
    }
}

/// <summary>
/// Selects a readable display scale while preserving a canonical database value.
/// A user's preferred unit determines metric/imperial family; the magnitude then
/// selects the most readable unit within that family.
/// </summary>
public static class AdaptiveUnitFormatter
{
    static readonly string[] MetricLength = ["mm", "cm", "m", "km"];
    static readonly string[] ImperialLength = ["in", "ft", "yd", "mi"];
    static readonly string[] MetricSurface = ["m²", "a", "ha", "km²"];
    static readonly string[] ImperialSurface = ["in²", "ft²", "yd²", "ac"];
    static readonly string[] MetricVolume = ["ml", "l", "m³"];
    static readonly string[] ImperialVolume = ["floz", "cup", "pt", "qt", "gal", "in³", "ft³", "yd³"];
    static readonly string[] MetricMass = ["mg", "g", "kg", "t"];
    static readonly string[] ImperialMass = ["oz", "lb", "st", "ton"];

    public static AdaptiveUnitValue Create(
        decimal canonicalValue,
        AdaptiveUnitQuantity quantity,
        IEnumerable<UnitDto> availableUnits,
        UnitDto? preferredUnit = null)
    {
        var units = availableUnits.Where(x => x.ConversionFactor > 0).ToList();
        var canonicalShort = quantity switch
        {
            AdaptiveUnitQuantity.Length => "m",
            AdaptiveUnitQuantity.Surface => "m²",
            AdaptiveUnitQuantity.Volume => "l",
            AdaptiveUnitQuantity.Mass => "g",
            _ => throw new ArgumentOutOfRangeException(nameof(quantity), quantity, null)
        };
        var canonicalUnit = Find(units, canonicalShort)
            ?? units.FirstOrDefault(x => x.IsBase)
            ?? throw new InvalidOperationException($"No canonical unit is available for {quantity}.");

        var imperial = preferredUnit is not null && IsImperial(quantity, preferredUnit.UnitShort);
        var targetShort = SelectTargetShort(canonicalValue, quantity, imperial);
        var targetUnit = Find(units, targetShort)
            ?? preferredUnit
            ?? canonicalUnit;
        var displayValue = FromCanonical(canonicalValue, targetUnit);

        return new AdaptiveUnitValue(canonicalValue, canonicalUnit.UnitShort, displayValue, targetUnit);
    }

    public static decimal ToCanonical(decimal displayValue, UnitDto unit)
    {
        if (unit.ConversionFactor <= 0)
            throw new InvalidOperationException($"Invalid conversion factor for {unit.UnitShort}.");

        return (displayValue + unit.OffSet) * unit.ConversionFactor;
    }

    public static UnitDto? Find(IEnumerable<UnitDto> units, string? unitShort)
        => string.IsNullOrWhiteSpace(unitShort)
            ? null
            : units.FirstOrDefault(x => string.Equals(x.UnitShort, unitShort, StringComparison.OrdinalIgnoreCase));

    static decimal FromCanonical(decimal canonicalValue, UnitDto unit)
        => (canonicalValue / unit.ConversionFactor) - unit.OffSet;

    static bool IsImperial(AdaptiveUnitQuantity quantity, string unitShort)
        => quantity switch
        {
            AdaptiveUnitQuantity.Length => Contains(ImperialLength, unitShort),
            AdaptiveUnitQuantity.Surface => Contains(ImperialSurface, unitShort),
            AdaptiveUnitQuantity.Volume => Contains(ImperialVolume, unitShort),
            AdaptiveUnitQuantity.Mass => Contains(ImperialMass, unitShort),
            _ => false
        };

    static string SelectTargetShort(decimal canonicalValue, AdaptiveUnitQuantity quantity, bool imperial)
    {
        var absolute = Math.Abs(canonicalValue);
        return (quantity, imperial) switch
        {
            (AdaptiveUnitQuantity.Length, false) => absolute switch
            {
                < 0.01m => "mm",
                >= 1000m => "km",
                _ => "m"
            },
            (AdaptiveUnitQuantity.Length, true) => (absolute / 0.3048m) switch
            {
                < 1m => "in",
                >= 5280m => "mi",
                _ => "ft"
            },
            (AdaptiveUnitQuantity.Surface, false) => absolute switch
            {
                >= 10_000_000m => "km²",
                >= 10_000m => "ha",
                _ => "m²"
            },
            (AdaptiveUnitQuantity.Surface, true) => absolute / 0.09290304m >= 43_560m ? "ac" : "ft²",
            (AdaptiveUnitQuantity.Volume, false) => absolute switch
            {
                < 1m => "ml",
                >= 1000m => "m³",
                _ => "l"
            },
            (AdaptiveUnitQuantity.Volume, true) => absolute / 3.785411784m >= 1m ? "gal" : "floz",
            (AdaptiveUnitQuantity.Mass, false) => absolute switch
            {
                < 1m => "mg",
                >= 1_000_000m => "t",
                >= 1000m => "kg",
                _ => "g"
            },
            (AdaptiveUnitQuantity.Mass, true) => absolute / 453.59237m >= 1m ? "lb" : "oz",
            _ => throw new ArgumentOutOfRangeException(nameof(quantity), quantity, null)
        };
    }

    static bool Contains(IEnumerable<string> candidates, string unitShort)
        => candidates.Any(x => string.Equals(x, unitShort, StringComparison.OrdinalIgnoreCase));
}
