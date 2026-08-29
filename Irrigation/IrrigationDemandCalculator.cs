using static AmsRecords.Irrigation.IrrigationDemandDtos;

namespace AmsRecords.Irrigation;

public sealed record IrrigationDemandCalculationInput(
    decimal? Et0Mm,
    decimal? CropCoefficient,
    decimal? RainfallMm = null,
    decimal? EffectiveRainfallMm = null,
    decimal? RootZoneDepthMm = null,
    decimal? AvailableWaterCapacityMmPerMetre = null,
    decimal? AllowableDepletionFraction = null,
    decimal? CurrentVwcPercent = null,
    decimal? TargetVwcPercent = null,
    decimal? ApplicationEfficiencyFraction = null);

public sealed record IrrigationDemandCalculationResult(
    decimal RecommendedNetMm,
    decimal? RecommendedGrossMm,
    string BasisCode,
    decimal? CropEvapotranspirationMm,
    decimal? CreditedRainfallMm,
    string? RainfallBasisCode,
    decimal? ClimaticWaterDeficitMm,
    decimal? TotalAvailableWaterMm,
    decimal? AllowableDepletionMm,
    bool? DepletionTriggerReached,
    decimal? SoilMoistureDeficitMm,
    IReadOnlyList<IrrigationDemandComponentDto> Components,
    IReadOnlyList<string> Warnings);

/// <summary>
/// Pure irrigation demand calculation. It deliberately has no dependency on distribution simulation, hydraulic
/// performance, controller operation, or a device vendor. All depths are millimetres and all fractions are 0..1.
/// </summary>
public static class IrrigationDemandCalculator
{
    public static IrrigationDemandCalculationResult Calculate(IrrigationDemandCalculationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        Validate(input);

        var components = new List<IrrigationDemandComponentDto>();
        var warnings = new List<string>();

        decimal? cropEt = null;
        decimal? creditedRain = null;
        string? rainfallBasis = null;
        decimal? climaticDeficit = null;
        if (HasClimaticBalance(input))
        {
            cropEt = input.Et0Mm!.Value * input.CropCoefficient!.Value;
            creditedRain = input.EffectiveRainfallMm ?? input.RainfallMm!.Value;
            rainfallBasis = input.EffectiveRainfallMm.HasValue
                ? EffectiveRainfallSource
                : MeasuredRainfallSource;
            climaticDeficit = Math.Max(0m, cropEt.Value - creditedRain.Value);
            components.Add(Component("crop_evapotranspiration", "ETc = ET0 × crop coefficient", cropEt.Value));
            components.Add(Component("climatic_water_deficit", "max(0, ETc − credited rainfall)", climaticDeficit.Value));
            if (!input.EffectiveRainfallMm.HasValue)
            {
                warnings.Add(
                    "Effective rainfall was not supplied; measured rainfall is credited one-for-one without an assumed loss factor.");
            }
        }

        decimal? totalAvailableWater = null;
        decimal? allowableDepletion = null;
        bool? depletionTriggerReached = null;
        if (HasRootZoneCapacity(input))
        {
            totalAvailableWater = input.AvailableWaterCapacityMmPerMetre!.Value *
                                  input.RootZoneDepthMm!.Value / 1_000m;
            allowableDepletion = totalAvailableWater.Value * input.AllowableDepletionFraction!.Value;
            components.Add(Component(
                "total_available_water",
                "available water capacity × rootzone depth (m)",
                totalAvailableWater.Value));
            components.Add(Component(
                "allowable_depletion",
                "total available water × allowable depletion fraction",
                allowableDepletion.Value));
            if (climaticDeficit.HasValue)
                depletionTriggerReached = climaticDeficit.Value >= allowableDepletion.Value;
        }

        decimal? soilMoistureDeficit = null;
        if (HasSoilMoistureBalance(input))
        {
            soilMoistureDeficit = Math.Max(
                0m,
                (input.TargetVwcPercent!.Value - input.CurrentVwcPercent!.Value) / 100m *
                input.RootZoneDepthMm!.Value);
            components.Add(Component(
                "soil_moisture_deficit",
                "max(0, target VWC − current VWC) × rootzone depth ÷ 100",
                soilMoistureDeficit.Value));
        }

        decimal net;
        string basis;
        if (soilMoistureDeficit.HasValue)
        {
            net = soilMoistureDeficit.Value;
            basis = SoilMoistureBasis;
            if (climaticDeficit.HasValue)
            {
                warnings.Add(
                    "The current soil-moisture observation is the demand basis; the climatic balance is explanatory and is not added again.");
            }
        }
        else
        {
            net = climaticDeficit!.Value;
            basis = ClimaticBalanceBasis;
            if (allowableDepletion.HasValue && depletionTriggerReached == false)
            {
                net = 0m;
                warnings.Add("The estimated climatic depletion has not reached the supplied allowable-depletion trigger.");
            }
            else if (totalAvailableWater.HasValue && net > totalAvailableWater.Value)
            {
                net = totalAvailableWater.Value;
                warnings.Add("The net depth was capped at the supplied rootzone total available water.");
            }
        }

        decimal? gross = null;
        if (input.ApplicationEfficiencyFraction.HasValue)
        {
            gross = net / input.ApplicationEfficiencyFraction.Value;
            components.Add(Component(
                "gross_application_depth",
                "recommended net depth ÷ explicit application efficiency",
                gross.Value));
        }
        else
        {
            warnings.Add(
                "Recommended gross depth is unavailable because no application-efficiency factor was supplied; distribution uniformity is not inferred by the demand engine.");
        }

        return new IrrigationDemandCalculationResult(
            Round(net),
            RoundNullable(gross),
            basis,
            RoundNullable(cropEt),
            RoundNullable(creditedRain),
            rainfallBasis,
            RoundNullable(climaticDeficit),
            RoundNullable(totalAvailableWater),
            RoundNullable(allowableDepletion),
            depletionTriggerReached,
            RoundNullable(soilMoistureDeficit),
            components,
            warnings);
    }

    static void Validate(IrrigationDemandCalculationInput input)
    {
        ValidateNonNegative(input.Et0Mm, nameof(input.Et0Mm));
        ValidatePositive(input.CropCoefficient, nameof(input.CropCoefficient));
        ValidateNonNegative(input.RainfallMm, nameof(input.RainfallMm));
        ValidateNonNegative(input.EffectiveRainfallMm, nameof(input.EffectiveRainfallMm));
        ValidatePositive(input.RootZoneDepthMm, nameof(input.RootZoneDepthMm));
        ValidatePositive(input.AvailableWaterCapacityMmPerMetre, nameof(input.AvailableWaterCapacityMmPerMetre));
        ValidateFraction(input.AllowableDepletionFraction, nameof(input.AllowableDepletionFraction), allowZero: false);
        ValidatePercent(input.CurrentVwcPercent, nameof(input.CurrentVwcPercent));
        ValidatePercent(input.TargetVwcPercent, nameof(input.TargetVwcPercent));
        ValidateFraction(input.ApplicationEfficiencyFraction, nameof(input.ApplicationEfficiencyFraction), allowZero: false);

        var climaticValuesSupplied = input.Et0Mm.HasValue || input.CropCoefficient.HasValue ||
                                    input.RainfallMm.HasValue || input.EffectiveRainfallMm.HasValue;
        if (climaticValuesSupplied && !HasClimaticBalance(input))
        {
            throw new ArgumentException(
                "A climatic demand calculation requires ET0, crop coefficient, and rainfall or effective rainfall.");
        }

        var soilMoistureValuesSupplied = input.CurrentVwcPercent.HasValue || input.TargetVwcPercent.HasValue;
        if (soilMoistureValuesSupplied && !HasSoilMoistureBalance(input))
        {
            throw new ArgumentException(
                "A soil-moisture demand calculation requires current VWC, target VWC, and rootzone depth.");
        }

        var rootZoneCapacityValuesSupplied = input.AvailableWaterCapacityMmPerMetre.HasValue ||
                                             input.AllowableDepletionFraction.HasValue;
        if (rootZoneCapacityValuesSupplied && !HasRootZoneCapacity(input))
        {
            throw new ArgumentException(
                "An allowable-depletion calculation requires rootzone depth, available water capacity, and allowable depletion fraction.");
        }

        if (!HasClimaticBalance(input) && !HasSoilMoistureBalance(input))
        {
            throw new ArgumentException(
                "Provide either a complete climatic balance or a complete soil-moisture balance; unavailable agronomic parameters are not guessed.");
        }
    }

    static bool HasClimaticBalance(IrrigationDemandCalculationInput input)
        => input.Et0Mm.HasValue && input.CropCoefficient.HasValue &&
           (input.EffectiveRainfallMm.HasValue || input.RainfallMm.HasValue);

    static bool HasSoilMoistureBalance(IrrigationDemandCalculationInput input)
        => input.CurrentVwcPercent.HasValue && input.TargetVwcPercent.HasValue && input.RootZoneDepthMm.HasValue;

    static bool HasRootZoneCapacity(IrrigationDemandCalculationInput input)
        => input.RootZoneDepthMm.HasValue && input.AvailableWaterCapacityMmPerMetre.HasValue &&
           input.AllowableDepletionFraction.HasValue;

    static void ValidateNonNegative(decimal? value, string name)
    {
        if (value is < 0m)
            throw new ArgumentOutOfRangeException(name, "The value cannot be negative.");
    }

    static void ValidatePositive(decimal? value, string name)
    {
        if (value.HasValue && value.Value <= 0m)
            throw new ArgumentOutOfRangeException(name, "The value must be greater than zero.");
    }

    static void ValidatePercent(decimal? value, string name)
    {
        if (value is < 0m or > 100m)
            throw new ArgumentOutOfRangeException(name, "The value must be between 0 and 100.");
    }

    static void ValidateFraction(decimal? value, string name, bool allowZero)
    {
        if (!value.HasValue)
            return;
        var minimumValid = allowZero ? value.Value >= 0m : value.Value > 0m;
        if (!minimumValid || value.Value > 1m)
            throw new ArgumentOutOfRangeException(name, "The value must be a fraction greater than zero and no more than one.");
    }

    static IrrigationDemandComponentDto Component(string code, string formula, decimal value)
        => new(code, formula, Round(value));

    static decimal Round(decimal value)
        => Math.Round(value, 3, MidpointRounding.AwayFromZero);

    static decimal? RoundNullable(decimal? value)
        => value.HasValue ? Round(value.Value) : null;
}
