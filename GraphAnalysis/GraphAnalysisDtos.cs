namespace AmsRecords.GraphAnalysis;

public static class GraphAnalysisScopeTypes
{
    public const string Field = "field";
    public const string Ibu = "ibu";
}

public static class GraphAnalysisModes
{
    public const string SelectedPeriod = "selectedPeriod";
    public const string HistoricalVerification = "historicalVerification";
}

public static class GraphAnalysisOutcomes
{
    public const string OutOfScope = "outOfScope";
    public const string InsufficientData = "insufficientData";
    public const string NoMeaningfulRelationship = "noMeaningfulRelationship";
    public const string PossiblePattern = "possiblePattern";
    public const string ClearRelationship = "clearRelationship";
    public const string ConflictingRelationship = "conflictingRelationship";
}

public static class GraphAnalysisEvidenceKinds
{
    public const string Supporting = "supporting";
    public const string Contradicting = "contradicting";
    public const string Neutral = "neutral";
}

public static class GraphAnalysisEvidenceLevels
{
    public const string Insufficient = "insufficient";
    public const string None = "none";
    public const string Possible = "possible";
    public const string Clear = "clear";
}

public static class GraphAnalysisScienceAssessments
{
    public const string Supporting = "supporting";
    public const string Contradicting = "contradicting";
    public const string Mixed = "mixed";
    public const string NotFound = "notFound";
    public const string Unavailable = "unavailable";
}

public static class GraphAnalysisConfidencePolicy
{
    public static int CombinedEvidenceScore(string outcome, int fieldEvidenceScore, int scientificAdjustment)
    {
        var fieldScore = Math.Clamp(fieldEvidenceScore, 0, 100);
        var scienceCanInfluence = outcome is GraphAnalysisOutcomes.ClearRelationship
            or GraphAnalysisOutcomes.PossiblePattern
            or GraphAnalysisOutcomes.ConflictingRelationship;
        return scienceCanInfluence
            ? Math.Clamp(fieldScore + Math.Clamp(scientificAdjustment, -10, 5), 0, 100)
            : fieldScore;
    }
}

public sealed record GraphAnalysisFieldDto(
    Guid FieldPubId,
    [param: MaxLength(180)] string FieldName);

public sealed record GraphAnalysisScopeDto(
    [param: Required, MaxLength(24)] string ScopeType,
    [param: MinLength(1), MaxLength(20)] IReadOnlyList<GraphAnalysisFieldDto> Fields);

public sealed record GraphAnalysisPointDto(
    DateOnly Date,
    decimal Value,
    int Count = 1);

public sealed record GraphAnalysisSeriesDto(
    Guid FieldPubId,
    [param: Required, MaxLength(64)] string Key,
    [param: Required, MaxLength(120)] string Label,
    [param: MaxLength(32)] string Unit,
    [param: Required, MaxLength(32)] string Group,
    [param: MaxLength(1200)] IReadOnlyList<GraphAnalysisPointDto> Points);

public sealed record GraphAnalysisAskRequestDto(
    [param: Required, MaxLength(300)] string Question,
    [param: Required, MaxLength(32)] string Mode,
    GraphAnalysisScopeDto Scope,
    DateOnly From,
    DateOnly To,
    Guid? AreaPubId,
    [param: MaxLength(2)] IReadOnlyList<string> RequestedSeriesKeys,
    [param: MaxLength(16)] string? ExpectedDirection,
    [param: MinLength(2), MaxLength(320)] IReadOnlyList<GraphAnalysisSeriesDto> Series,
    Guid? ParentAnalysisPubId = null,
    [param: Range(0, 100)] int? ExpectedEvidenceScore = null);

public sealed record GraphAnalysisEvidenceDto(
    string Kind,
    string SourceSeriesKey,
    string TargetSeriesKey,
    DateOnly SourceDate,
    DateOnly TargetDate,
    decimal SourceValue,
    decimal TargetValue);

public sealed record GraphAnalysisRelationshipDto(
    Guid FieldPubId,
    string SourceSeriesKey,
    string SourceSeriesLabel,
    string TargetSeriesKey,
    string TargetSeriesLabel,
    int LagDays,
    decimal Coefficient,
    int SampleSize,
    int ObservationSpanDays,
    decimal Stability,
    string Direction,
    string Strength,
    string EvidenceLevel,
    int EvidenceScore,
    IReadOnlyList<GraphAnalysisEvidenceDto> Evidence,
    IReadOnlyList<GraphAnalysisOccasionDto> ComparableOccasions);

public sealed record GraphAnalysisOccasionDto(
    string Kind,
    DateOnly SourceDate,
    DateOnly TargetDate,
    decimal SourceValue,
    decimal TargetValue);

public sealed record GraphAnalysisScientificSourceDto(
    [property: MaxLength(240)] string Title,
    [property: MaxLength(1000)] string Url,
    [property: MaxLength(500)] string Relevance);

public sealed record GraphAnalysisScientificEvidenceDto(
    string Assessment,
    string Summary,
    int ScoreAdjustment,
    IReadOnlyList<GraphAnalysisScientificSourceDto> Sources);

public sealed record GraphAnalysisVerificationDto(
    int PreviousEvidenceScore,
    int NewEvidenceScore,
    int Change,
    string ConfidenceDirection,
    int SupportingOccasionCount,
    int ContradictingOccasionCount,
    int NeutralOccasionCount,
    string Summary,
    IReadOnlyList<GraphAnalysisOccasionDto> OtherOccasions);

public sealed record GraphAnalysisAskResponseDto(
    string Outcome,
    string Title,
    string Answer,
    IReadOnlyList<Guid> FieldsToShow,
    IReadOnlyList<string> SeriesToShow,
    GraphAnalysisRelationshipDto? Relationship,
    IReadOnlyList<string> SuggestedQuestions,
    Guid? AnalysisPubId = null,
    DateTime? CreatedAtUtc = null,
    int? CombinedEvidenceScore = null,
    GraphAnalysisScientificEvidenceDto? ScientificEvidence = null,
    GraphAnalysisVerificationDto? Verification = null);

public sealed record GraphAnalysisHistoryItemDto(
    Guid AnalysisPubId,
    Guid? ParentAnalysisPubId,
    string Mode,
    DateTime CreatedAtUtc,
    string Question,
    DateOnly From,
    DateOnly To,
    Guid? AreaPubId,
    string Outcome,
    string Title,
    string Answer,
    string? SourceSeriesLabel,
    string? TargetSeriesLabel,
    int? FieldEvidenceScore,
    int? CombinedEvidenceScore,
    GraphAnalysisScientificEvidenceDto? ScientificEvidence,
    GraphAnalysisVerificationDto? Verification);
