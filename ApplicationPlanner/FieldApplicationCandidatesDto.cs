using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

public sealed record FieldApplicationCandidatesDto(Guid FieldPubId, string Kind, int Page, int PageSize,
    int TotalCount, IReadOnlyList<FieldApplicationCandidateDto> Rows);

public sealed record FieldApplicationCandidateDto(Guid PubId, string Name, decimal? AreaM2 = null,
    string? NumeratorUnit = null, string? DenominatorUnit = null, decimal? DensityKgPerL = null,
    FertilizerApplicationRateUnit? RateUnit = null);
