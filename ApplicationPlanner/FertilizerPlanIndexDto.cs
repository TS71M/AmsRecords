namespace AmsRecords.ApplicationPlanner;

public sealed record FertilizerPlanIndexDto(Guid FieldPubId, string FieldName, IReadOnlyList<FertilizerPlanIndexEntryDto> Plans);
public sealed record FertilizerPlanIndexEntryDto(Guid PubId, int Year, string Name, string? Status = null);
