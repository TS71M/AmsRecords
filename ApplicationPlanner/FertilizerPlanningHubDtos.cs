namespace AmsRecords.ApplicationPlanner;

public static class FertilizerPlanningHubDtos
{
    public const string Route = "fertilizer-planning-hub";
    public const string ApproveAction = "approve-fertilizer-plan";
    public const int MaximumFields = 500;
    public sealed record Hub(int Year, IReadOnlyList<Site> Sites);
    public sealed record Site(Guid FieldPubId, string FieldName, string OwnerName, bool CanEdit, IReadOnlyList<Plan> Plans);
    public sealed record Plan(Guid PubId, string Name, string Status)
    {
        public Guid? RevisionPubId { get; init; }
        public int? RevisionNumber { get; init; }
    }
    public sealed record Comparison(IReadOnlyList<ComparisonColumn> Plans);
    public sealed record ComparisonColumn(Guid FieldPubId, string FieldName, string OwnerName, Plan Plan,
        int ProductCount, decimal? ReferenceAreaM2, IReadOnlyList<Lib.Planning.ApplicationPlanSeasonSummary.Nutrient> Nutrients);
}
