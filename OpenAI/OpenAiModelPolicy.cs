namespace AmsRecords.OpenAI;

/// <summary>Redirect legacy Sol selections, including stored environment overrides.</summary>
public static class OpenAiModelPolicy
{
    public const string Sol = "gpt-6.1-sol";
    public const string Terra = "gpt-6.1-terra";
    public const string Luna = "gpt-6-luna";

    public static bool IsFamily(string model, string family)
        => model.Equals(family, StringComparison.OrdinalIgnoreCase)
            || model.StartsWith(family + "-", StringComparison.OrdinalIgnoreCase);

    public static bool IsLegacyGeneral(string model)
        => IsFamily(model.Trim(), "gpt-4.1") || IsFamily(model.Trim(), "gpt-4o");

    public static string ResolveVision(string model) => IsLegacyGeneral(model) ? Terra : Resolve(model);
    public static string ResolveDiagnostic(string model) => IsLegacyGeneral(model) ? Sol : Resolve(model);

    public static string Resolve(string model)
    {
        var value = model.Trim();
        if (IsLegacyGeneral(value) || IsFamily(value, "gpt-5.6-luna")) return Luna;
        if (IsFamily(value, "gpt-5.6-terra")) return Terra;
        if (IsFamily(value, "gpt-5.5")) return Sol;
        // The bare GPT-5.6 alias also selects Sol. Preserve other workload tiers.
        if (value.Equals("gpt-5.6", StringComparison.OrdinalIgnoreCase)
            || value.Equals("gpt-5.6-sol", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("gpt-5.6-sol-", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("gpt-5.6-", StringComparison.OrdinalIgnoreCase)
                && value.Length > 8 && char.IsDigit(value[8]))
            return Sol;
        return value;
    }

    public static bool IsCurrentSol(string model)
        => model.Equals(Sol, StringComparison.OrdinalIgnoreCase)
            || model.StartsWith(Sol + "-", StringComparison.OrdinalIgnoreCase);
}
