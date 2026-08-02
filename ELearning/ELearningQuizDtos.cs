namespace AmsRecords.ELearning;

public static class ELearningQuizDtos
{
    public sealed record QuizSectionDto(
        string Slug,
        string Title,
        string Description,
        int QuestionPoolSize,
        int QuestionsPerQuiz,
        int PassingScore,
        bool Eligible,
        DateTime? EligibleFromUtc,
        QuizResultDto? LatestResult);

    public sealed record QuizQuestionDto(
        string Id,
        string Prompt,
        IReadOnlyList<string> Options,
        string? ImageUrl,
        string? ImageAlt);

    public sealed record QuizStartDto(
        Guid AttemptPubId,
        string SectionSlug,
        string SectionTitle,
        int PassingScore,
        IReadOnlyList<QuizQuestionDto> Questions);

    public sealed record QuizAnswerDto(string QuestionId, int SelectedOptionIndex);

    public sealed record QuizSubmitDto(Guid AttemptPubId, IReadOnlyList<QuizAnswerDto> Answers);

    public sealed record QuizResultDto(
        Guid AttemptPubId,
        string SectionSlug,
        string SectionTitle,
        int Score,
        bool Passed,
        string Medal,
        int CorrectAnswers,
        int QuestionCount,
        DateTime CompletedUtc,
        DateTime? EligibleFromUtc);

    public sealed record QuizSectionAchievementDto(
        string SectionSlug,
        string SectionTitle,
        int AttemptCount,
        int? BestScore,
        string Medal,
        bool Passed,
        DateTime? PassedUtc,
        bool Eligible,
        DateTime? EligibleFromUtc);

    public sealed record LearningTimelineItemDto(
        Guid ActivityPubId,
        string ActivityType,
        string SectionSlug,
        string Title,
        int? Score,
        bool? Passed,
        string? Medal,
        int? EngagedSeconds,
        int? MaxScrollPercent,
        DateTime OccurredUtc);

    public sealed record ReadingActivityCompleteDto(
        string PagePath,
        string PageTitle,
        int EngagedSeconds,
        int ReadingThresholdSeconds,
        int MaxScrollPercent);

    public sealed record ReadingActivityDto(
        Guid PubId,
        string PagePath,
        string PageTitle,
        int EngagedSeconds,
        int MaxScrollPercent,
        DateTime CompletedUtc);

    public sealed record QuizRecommendationDto(
        string SectionSlug,
        string SectionTitle,
        string Message);

    public sealed record QuizAchievementsDto(
        int TotalAttempts,
        int MeaningfulReadingSessions,
        int PassedSectionCount,
        int TotalSectionCount,
        int GoldMedals,
        int SilverMedals,
        int BronzeMedals,
        int? BestScore,
        IReadOnlyList<QuizSectionAchievementDto> Sections,
        IReadOnlyList<LearningTimelineItemDto> Timeline,
        QuizRecommendationDto? Recommendation);
}
