using System.Text.Json;
using Lib.Enums;

namespace AmsRecords.ApplicationPlanner;

public enum ApplicationPassAssessmentStatus
{
    Ready = 0,
    NeedsReview = 1,
    Blocked = 2
}

public enum ApplicationPassConstraintSeverity
{
    Information = 0,
    Warning = 1,
    Blocker = 2
}

public sealed record ApplicationEvidenceReferenceDto(
    Guid EvidencePubId,
    Guid TargetPubId,
    string TargetType,
    string Title,
    string SourceUri);

public sealed record ApplicationPassConstraintDto(
    string Code,
    ApplicationPassConstraintSeverity Severity,
    string Message,
    string Subject,
    ApplicationEvidenceReferenceDto? Source = null);

public sealed record ApplicationPassAssessmentDto(
    ApplicationPassAssessmentStatus Status,
    int BlockerCount,
    int WarningCount,
    IReadOnlyList<ApplicationPassConstraintDto> Constraints,
    DateTime EvaluatedAtUtc)
{
    public bool CanPrepare => BlockerCount == 0;
}

public sealed record ApplicationPassAssessmentInput(
    DateOnly PlannedDate,
    decimal TreatedAreaM2,
    decimal CarrierVolumeLitresPerHa,
    IReadOnlyList<string> Jurisdictions,
    ApplicationPassMachineAssessmentInput? Machine,
    IReadOnlyList<ApplicationPassProductAssessmentInput> Products);

public sealed record ApplicationPassMachineAssessmentInput(
    string Name,
    bool Active,
    bool Blocked,
    bool Broken,
    decimal? CapacityLitres,
    DateOnly? CalibrationDueOn,
    ApplicationPassMachineProfileInput? Profile,
    decimal? CapacityAmount = null,
    string CapacityUnit = "",
    MachineryApplicationCapability ApplicationCapability = MachineryApplicationCapability.None);

public sealed record ApplicationPassMachineProfileInput(
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    decimal? MinimumOperatingVolumeLitres,
    string SupportedProductFormsJson,
    ApplicationEvidenceReferenceDto Source);

public sealed record ApplicationPassProductAssessmentInput(
    string Name,
    string ProductType,
    string Target,
    decimal Rate,
    string RateUnit,
    decimal PlannedQuantity,
    decimal InventoryQuantity,
    decimal UnitCost,
    IReadOnlyList<ApplicationPassProductProfileInput> Profiles);

public sealed record ApplicationPassProductProfileInput(
    string Jurisdiction,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo,
    string PhysicalForm,
    string HazardsJson,
    string PpeJson,
    string UseRulesJson,
    string WeatherRestrictionsJson,
    string MixingInstructions,
    string CompatibilityJson,
    ApplicationEvidenceReferenceDto Source);

public static class ApplicationPassAssessmentPolicy
{
    public static ApplicationPassAssessmentDto Evaluate(ApplicationPassAssessmentInput input, DateTime evaluatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(input);
        var constraints = new List<ApplicationPassConstraintDto>();
        void Add(string code, ApplicationPassConstraintSeverity severity, string message, string subject,
            ApplicationEvidenceReferenceDto? source = null)
            => constraints.Add(new(code, severity, message, subject, source));

        if (input.Products is not { Count: > 0 })
            Add("products.missing", ApplicationPassConstraintSeverity.Blocker,
                "Add at least one product to this application pass.", "Application pass");

        EvaluateMachine(input, Add);
        foreach (var product in input.Products)
            EvaluateProduct(input, product, Add);

        if (input.Products is { Count: > 1 })
        {
            var applicableProfiles = input.Products
                .Select(x => SelectProductProfile(input, x))
                .Where(x => x is not null)
                .Cast<ApplicationPassProductProfileInput>()
                .ToList();
            var explicitProhibition = applicableProfiles
                .FirstOrDefault(x => ContainsProhibition(x.CompatibilityJson)
                    || ApplicationJobCompatibilityPolicy.ReadStatements(x.Source.TargetPubId, x.CompatibilityJson)
                        .Any(statement => !statement.Compatible && input.Products.Any(product =>
                            string.Equals(product.Name.Trim(), statement.OtherProductName?.Trim(), StringComparison.OrdinalIgnoreCase))));
            if (explicitProhibition is not null)
                Add("compatibility.prohibited", ApplicationPassConstraintSeverity.Blocker,
                    "Reviewed evidence contains an explicit mixing incompatibility. Keep the products in separate passes.",
                    "Tank mix", explicitProhibition.Source);
            else if (applicableProfiles.Count != input.Products.Count
                || ApplicationJobCompatibilityPolicy.Check(input.Products.Select(product =>
                    new ApplicationJobCompatibilityPolicy.Product(SelectProductProfile(input, product)!.Source.TargetPubId, product.Name)).ToArray(),
                    applicableProfiles.SelectMany(profile => ApplicationJobCompatibilityPolicy.ReadStatements(
                        profile.Source.TargetPubId, profile.CompatibilityJson)).ToArray()).Count > 0)
                Add("compatibility.unverified", ApplicationPassConstraintSeverity.Blocker,
                    "Compatibility between every product pair is not explicitly confirmed by structured reviewed evidence.",
                    "Tank mix");
        }

        Add("operator.qualification.pending", ApplicationPassConstraintSeverity.Warning,
            "Assign an operator and confirm the required qualification before preparation.", "Operator");
        Add("weather.current.pending", ApplicationPassConstraintSeverity.Warning,
            "Evaluate current and forecast weather against the reviewed restrictions before preparation.", "Weather");

        var blockers = constraints.Count(x => x.Severity == ApplicationPassConstraintSeverity.Blocker);
        var warnings = constraints.Count(x => x.Severity == ApplicationPassConstraintSeverity.Warning);
        var status = blockers > 0
            ? ApplicationPassAssessmentStatus.Blocked
            : warnings > 0
                ? ApplicationPassAssessmentStatus.NeedsReview
                : ApplicationPassAssessmentStatus.Ready;
        return new(status, blockers, warnings, constraints, evaluatedAtUtc);
    }

    static void EvaluateMachine(
        ApplicationPassAssessmentInput input,
        Action<string, ApplicationPassConstraintSeverity, string, string, ApplicationEvidenceReferenceDto?> add)
    {
        var machine = input.Machine;
        if (machine is null)
        {
            add("machine.missing", ApplicationPassConstraintSeverity.Blocker,
                "Select application machinery for this pass.", "Machinery", null);
            return;
        }
        if (!machine.Active || machine.Blocked || machine.Broken)
            add("machine.unavailable", ApplicationPassConstraintSeverity.Blocker,
                "The selected machine is inactive, blocked, or marked broken.", machine.Name, null);
        if (machine.CalibrationDueOn.HasValue && machine.CalibrationDueOn < input.PlannedDate)
            add("machine.calibration.overdue", ApplicationPassConstraintSeverity.Blocker,
                "The machine calibration is due before the planned application date.", machine.Name, null);
        if (machine.Profile is null)
        {
            add("machine.profile.missing", ApplicationPassConstraintSeverity.Blocker,
                "Create a reviewed application profile from machinery evidence.", machine.Name, null);
            return;
        }

        var profile = machine.Profile;
        if (!AppliesOn(profile.EffectiveFrom, profile.EffectiveTo, input.PlannedDate))
            add("machine.profile.date", ApplicationPassConstraintSeverity.Blocker,
                "The reviewed machinery profile is not effective on the planned date.", machine.Name, profile.Source);
        var granularOnly = machine.ApplicationCapability.HasFlag(MachineryApplicationCapability.GranularApplicator)
            && !machine.ApplicationCapability.HasFlag(MachineryApplicationCapability.LiquidSprayer);
        if (granularOnly)
        {
            if (!machine.CapacityAmount.HasValue || machine.CapacityAmount <= 0 || !IsMassUnit(machine.CapacityUnit))
                add("machine.capacity.unknown", ApplicationPassConstraintSeverity.Blocker,
                    "A positive hopper capacity in a mass unit is required for granular load planning.", machine.Name, profile.Source);
        }
        else if (!machine.CapacityLitres.HasValue || machine.CapacityLitres <= 0)
            add("machine.capacity.unknown", ApplicationPassConstraintSeverity.Blocker,
                "A positive liquid tank capacity is required for load planning.", machine.Name, profile.Source);

        var minimum = profile.MinimumOperatingVolumeLitres;
        if (!granularOnly && minimum.HasValue && machine.CapacityLitres.HasValue && minimum > machine.CapacityLitres)
            add("machine.minimum.exceeds-capacity", ApplicationPassConstraintSeverity.Blocker,
                "The reviewed minimum operating volume exceeds the machine capacity.", machine.Name, profile.Source);
        var totalCarrier = input.CarrierVolumeLitresPerHa * input.TreatedAreaM2 / 10_000m;
        if (!granularOnly && minimum.HasValue && totalCarrier > 0 && totalCarrier < minimum)
            add("machine.minimum.not-met", ApplicationPassConstraintSeverity.Blocker,
                "The planned carrier volume is below the reviewed minimum operating volume.", machine.Name, profile.Source);

        var supportedForms = JsonStrings(profile.SupportedProductFormsJson);
        if (supportedForms.Count == 0)
            add("machine.product-forms.unknown", ApplicationPassConstraintSeverity.Warning,
                "Supported product forms are not documented in the reviewed machinery profile.", machine.Name, profile.Source);
        else
        {
            foreach (var product in input.Products)
            {
                var productProfile = SelectProductProfile(input, product);
                if (productProfile is null)
                    continue;
                if (string.IsNullOrWhiteSpace(productProfile.PhysicalForm))
                    add("product.physical-form.missing", ApplicationPassConstraintSeverity.Blocker,
                        "The reviewed product profile does not identify its physical form.", product.Name, productProfile.Source);
                else if (!MatchesForm(productProfile.PhysicalForm, supportedForms))
                    add("machine.product-form.unsupported", ApplicationPassConstraintSeverity.Blocker,
                        "The reviewed machinery profile does not list this product form as supported.", product.Name, profile.Source);
            }
        }
    }

    static void EvaluateProduct(
        ApplicationPassAssessmentInput input,
        ApplicationPassProductAssessmentInput product,
        Action<string, ApplicationPassConstraintSeverity, string, string, ApplicationEvidenceReferenceDto?> add)
    {
        if (product.PlannedQuantity > product.InventoryQuantity)
            add("inventory.shortfall", ApplicationPassConstraintSeverity.Blocker,
                "The frozen inventory balance is below the planned product quantity.", product.Name, null);
        if (product.UnitCost <= 0)
            add("cost.unknown", ApplicationPassConstraintSeverity.Warning,
                "The frozen unit cost is missing or zero.", product.Name, null);

        var profile = SelectProductProfile(input, product);
        if (profile is null)
        {
            add("product.profile.missing", ApplicationPassConstraintSeverity.Blocker,
                "No reviewed product safety profile matches the field jurisdiction and planned date.", product.Name, null);
            return;
        }

        if (JsonStrings(profile.HazardsJson).Count == 0)
            add("product.hazards.unknown", ApplicationPassConstraintSeverity.Warning,
                "Hazard statements are not documented in the reviewed profile.", product.Name, profile.Source);
        if (JsonStrings(profile.PpeJson).Count == 0)
            add("product.ppe.unknown", ApplicationPassConstraintSeverity.Warning,
                "Required personal protective equipment is not documented in the reviewed profile.", product.Name, profile.Source);
        if (JsonStrings(profile.WeatherRestrictionsJson).Count == 0)
            add("product.weather.unknown", ApplicationPassConstraintSeverity.Warning,
                "Weather restrictions are not documented in the reviewed profile.", product.Name, profile.Source);
        if (string.IsNullOrWhiteSpace(profile.MixingInstructions))
            add("product.mixing.unknown",
                input.Products.Count > 1 ? ApplicationPassConstraintSeverity.Blocker : ApplicationPassConstraintSeverity.Warning,
                "Mixing instructions are not documented in the reviewed profile.", product.Name, profile.Source);

        EvaluateUseRules(product, profile, add);
    }

    static void EvaluateUseRules(
        ApplicationPassProductAssessmentInput product,
        ApplicationPassProductProfileInput profile,
        Action<string, ApplicationPassConstraintSeverity, string, string, ApplicationEvidenceReferenceDto?> add)
    {
        var rules = ReadUseRules(profile.UseRulesJson);
        if (rules.Count == 0)
        {
            add("product.use-rule.missing", ApplicationPassConstraintSeverity.Blocker,
                "No reviewed legal-use rate rule is available for this product.", product.Name, profile.Source);
            return;
        }

        var applicable = rules.Where(x => TargetMatches(product.Target, x.SiteOrTarget)).ToList();
        if (applicable.Count == 0)
        {
            add("product.use-rule.target", ApplicationPassConstraintSeverity.Blocker,
                "No reviewed legal-use rule matches the planned target or site.", product.Name, profile.Source);
            return;
        }

        var comparable = applicable.Where(x => SameUnit(product.RateUnit, x.RateUnit)).ToList();
        if (comparable.Count == 0)
        {
            add("product.use-rule.unit", ApplicationPassConstraintSeverity.Blocker,
                "The reviewed rate rules use a different or unknown unit.", product.Name, profile.Source);
            return;
        }

        if (!comparable.Any(x => (!x.Minimum.HasValue || product.Rate >= x.Minimum)
            && (!x.Maximum.HasValue || product.Rate <= x.Maximum)))
            add("product.use-rule.rate", ApplicationPassConstraintSeverity.Blocker,
                "The planned rate is outside every applicable reviewed rate range.", product.Name, profile.Source);
    }

    static bool AppliesOn(DateOnly? from, DateOnly? to, DateOnly date)
        => (!from.HasValue || from <= date) && (!to.HasValue || to >= date);

    static bool Same(string left, string right)
        => !string.IsNullOrWhiteSpace(left) && !string.IsNullOrWhiteSpace(right)
            && string.Equals(left.Trim(), right.Trim(), StringComparison.OrdinalIgnoreCase);

    static bool SameUnit(string left, string right)
        => NormalizeUnit(left) == NormalizeUnit(right) && NormalizeUnit(left).Length > 0;

    static bool IsMassUnit(string? value)
        => (value ?? string.Empty).Trim().ToLowerInvariant() is "kg" or "kilogram" or "kilograms";

    static string NormalizeUnit(string? value)
    {
        var unit = (value ?? string.Empty).Trim().ToLowerInvariant()
            .Replace("litres", "l", StringComparison.Ordinal)
            .Replace("liters", "l", StringComparison.Ordinal)
            .Replace("kilograms", "kg", StringComparison.Ordinal)
            .Replace("hectares", "ha", StringComparison.Ordinal)
            .Replace("squaremetres", "m2", StringComparison.Ordinal)
            .Replace("squaremeters", "m2", StringComparison.Ordinal)
            .Replace("persquaremetre", "/m2", StringComparison.Ordinal)
            .Replace("persquaremeter", "/m2", StringComparison.Ordinal)
            .Replace("perhectare", "/ha", StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("²", "2", StringComparison.Ordinal);
        return unit switch
        {
            "litresperhectare" => "l/ha",
            "millilitrespersquaremetre" => "ml/m2",
            "kilogramsperhectare" => "kg/ha",
            "gramspermetresquare" => "g/m2",
            _ => unit
        };
    }

    static bool TargetMatches(string planned, string rule)
    {
        var normalizedRule = (rule ?? string.Empty).Trim();
        if (normalizedRule.Length == 0 || normalizedRule is "*" || normalizedRule.Equals("all", StringComparison.OrdinalIgnoreCase))
            return true;
        var normalizedPlanned = (planned ?? string.Empty).Trim();
        return normalizedPlanned.Length > 0
            && (normalizedPlanned.Contains(normalizedRule, StringComparison.OrdinalIgnoreCase)
                || normalizedRule.Contains(normalizedPlanned, StringComparison.OrdinalIgnoreCase));
    }

    static bool MatchesForm(string productType, IReadOnlyList<string> supported)
    {
        var type = (productType ?? string.Empty).Trim();
        return type.Length > 0 && supported.Any(x =>
            type.Contains(x, StringComparison.OrdinalIgnoreCase)
            || x.Contains(type, StringComparison.OrdinalIgnoreCase));
    }

    static bool ContainsProhibition(string json)
        => JsonStrings(json).Any(x => x.Contains("prohibit", StringComparison.OrdinalIgnoreCase)
            || x.Contains("incompatible", StringComparison.OrdinalIgnoreCase)
            || x.Contains("do not mix", StringComparison.OrdinalIgnoreCase));

    static ApplicationPassProductProfileInput? SelectProductProfile(
        ApplicationPassAssessmentInput input,
        ApplicationPassProductAssessmentInput product)
        => product.Profiles
            .Where(x => input.Jurisdictions.Any(j => Same(j, x.Jurisdiction)))
            .Where(x => AppliesOn(x.EffectiveFrom, x.EffectiveTo, input.PlannedDate))
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

    static IReadOnlyList<string> JsonStrings(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];
        try
        {
            using var document = JsonDocument.Parse(json);
            var values = new List<string>();
            CollectStrings(document.RootElement, values);
            return values.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }

    static void CollectStrings(JsonElement value, List<string> values)
    {
        if (value.ValueKind == JsonValueKind.String)
        {
            values.Add(value.GetString() ?? string.Empty);
            return;
        }
        if (value.ValueKind == JsonValueKind.Array)
            foreach (var item in value.EnumerateArray()) CollectStrings(item, values);
        else if (value.ValueKind == JsonValueKind.Object)
            foreach (var property in value.EnumerateObject()) CollectStrings(property.Value, values);
    }

    static IReadOnlyList<UseRule> ReadUseRules(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return [];
        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Array) return [];
            return document.RootElement.EnumerateArray()
                .Where(x => x.ValueKind == JsonValueKind.Object)
                .Select(x => new UseRule(
                    String(x, "siteOrTarget"),
                    Decimal(x, "minimumRate"),
                    Decimal(x, "maximumRate"),
                    String(x, "rateUnit")))
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }

    static string String(JsonElement value, string name)
        => Property(value, name) is { ValueKind: JsonValueKind.String } property ? property.GetString() ?? string.Empty : string.Empty;

    static decimal? Decimal(JsonElement value, string name)
        => Property(value, name) is { ValueKind: JsonValueKind.Number } property && property.TryGetDecimal(out var number) ? number : null;

    static JsonElement? Property(JsonElement value, string name)
    {
        foreach (var property in value.EnumerateObject())
            if (property.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return property.Value;
        return null;
    }

    sealed record UseRule(string SiteOrTarget, decimal? Minimum, decimal? Maximum, string RateUnit);
}
