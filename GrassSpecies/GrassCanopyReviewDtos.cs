namespace AmsRecords.GrassSpecies;

public static class GrassCanopyReviewDtos
{
    public sealed record Point(int Id, int X, int Y, int Sector, int IntroducedAt);
    public sealed record Plan(string Version, string SourceHash, int Width, int Height, IReadOnlyList<Point> Points);
    public sealed record SourceReview(string Format, string DeclaredReviewer, string PlanHash);
    public sealed record Annotation(int PointId, string Label, string Evidence, SourceReview? SourceReview = null);
    public sealed record Share(string Label, int Hits, decimal? GrassPercent);
    public sealed record DensitySummary(int Points, int Reviewed, int GrassHits, int UnassessedHits,
        int WeedHits, int NonVegetationHits, bool Complete, IReadOnlyList<Share> GrassShares);
    public sealed record Snapshot(Guid PhotoPubId, string FieldName, string AreaName, int Revision,
        DateTime? SavedAtUtc, Guid? ReviewerPubId, Plan Plan, string PlanHash,
        IReadOnlyList<Annotation> Annotations, IReadOnlyList<DensitySummary> Summaries);
    public sealed record Save(int ExpectedRevision, string PlanHash, string SourceHash, IReadOnlyList<Annotation> Annotations);
    public sealed record Photo(Guid PhotoPubId, string FieldName, string AreaName, DateTime? CapturedAtUtc, int Revision);
    public sealed record Page(IReadOnlyList<Photo> Photos, int? NextSkip);
    public sealed record Revision(int Number, DateTime SavedAtUtc, Guid ReviewerPubId);
}
