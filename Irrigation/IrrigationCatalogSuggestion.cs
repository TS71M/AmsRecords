using static AmsRecords.Irrigation.IrrigationDtos;

namespace AmsRecords.Irrigation;

public static class IrrigationCatalogSuggestion
{
    public static IrrigationSprinklerModelDto? Resolve(
        string? manufacturerName,
        string? modelName,
        bool needsReview,
        Guid? selectedModelPubId,
        IEnumerable<IrrigationSprinklerModelDto> availableModels)
    {
        _ = needsReview;
        if (selectedModelPubId is not null && selectedModelPubId != Guid.Empty)
            return null;

        var manufacturer = Normalize(manufacturerName);
        var model = Normalize(modelName);
        if (manufacturer.Length == 0 || model.Length == 0)
            return null;

        var matches = availableModels
            .Where(candidate => candidate.Active && Normalize(candidate.ManufacturerName) == manufacturer)
            .Where(candidate => Normalize(candidate.ModelCode) == model || Normalize(candidate.ModelName) == model)
            .Take(2)
            .ToList();

        return matches.Count == 1 ? matches[0] : null;
    }

    static string Normalize(string? value)
        => new((value ?? "").ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());
}
