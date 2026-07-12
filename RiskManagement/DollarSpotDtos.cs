using AmsRecords.Messages;
using static Lib.Enums.RiskLevels;

namespace AmsRecords.RiskManagement;

public static class DollarSpotDtos
{
    public record DollarSpotNDayRiskDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] IReadOnlyList<DollarSpotDayRiskDto> Days,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories,
        [property: JsonPropertyName("hoursHave")] int? HoursHave,
        [property: JsonPropertyName("hoursNeed")] int? HoursNeed,
        [property: JsonPropertyName("hoursCoveragePct")] int? HoursCoveragePct,
        [property: JsonPropertyName("consecutiveDaysHave")] int? ConsecutiveDaysHave,
        [property: JsonPropertyName("consecutiveDaysNeed")] int? ConsecutiveDaysNeed,
        [property: JsonPropertyName("consecutiveDaysCoveragePct")] int? ConsecutiveDaysCoveragePct,
        [property: JsonPropertyName("missingReason")] AppMessageDto? MissingReason
    )
    {
        [JsonPropertyName("earlyWarning")]
        public DollarSpotEarlyWarningDto? EarlyWarning { get; init; }
    }

    public record DollarSpotDayRiskDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("probabilityPct")] decimal? ProbabilityPct,
        [property: JsonPropertyName("meanAtC")] decimal? MeanAtC,
        [property: JsonPropertyName("meanRhPct")] decimal? MeanRhPct,
        [property: JsonPropertyName("riskLevel")] RiskLevel Level,
        [property: JsonPropertyName("isAction")] bool IsAction,
        [property: JsonPropertyName("messages")] AppMessageDto[] Messages
    );

    public sealed record DollarSpotEarlyWarningDto(
        [property: JsonPropertyName("modelId")] string ModelId,
        [property: JsonPropertyName("modelRevision")] string ModelRevision,
        [property: JsonPropertyName("isExperimental")] bool IsExperimental,
        [property: JsonPropertyName("days")] IReadOnlyList<DollarSpotEarlyWarningDayDto> Days,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories
    );

    public sealed record DollarSpotEarlyWarningDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasWeatherData")] bool HasWeatherData,
        [property: JsonPropertyName("weatherSuitabilityIndex")] decimal? WeatherSuitabilityIndex,
        [property: JsonPropertyName("estimatedCanopyWetnessIndex")] decimal? EstimatedCanopyWetnessIndex,
        [property: JsonPropertyName("smithKernsProbabilityPct")] decimal? SmithKernsProbabilityPct,
        [property: JsonPropertyName("nearDewHours")] int? NearDewHours,
        [property: JsonPropertyName("estimatedWetnessHoursAfterDewRemoval")] decimal? EstimatedWetnessHoursAfterDewRemoval,
        [property: JsonPropertyName("nightHumidityTemperatureIndex")] decimal? NightHumidityTemperatureIndex,
        [property: JsonPropertyName("fieldEvidenceLevel")] string FieldEvidenceLevel,
        [property: JsonPropertyName("recommendation")] string Recommendation,
        [property: JsonPropertyName("dataCompleteness")] string DataCompleteness,
        [property: JsonPropertyName("observation")] DollarSpotObservationDto? Observation,
        [property: JsonPropertyName("missingInputs")] string[] MissingInputs,
        [property: JsonPropertyName("reasonCodes")] string[] ReasonCodes
    );

    public sealed record DollarSpotObservationDto(
        [property: JsonPropertyName("pubId")] Guid? PubId,
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("morningMyceliumObserved")] bool? MorningMyceliumObserved,
        [property: JsonPropertyName("leafLesionsObserved")] bool? LeafLesionsObserved,
        [property: JsonPropertyName("activeExpansionObserved")] bool? ActiveExpansionObserved,
        [property: JsonPropertyName("ldsOrHydrophobicityPresent")] bool? LdsOrHydrophobicityPresent,
        [property: JsonPropertyName("dewManuallyRemoved")] bool? DewManuallyRemoved,
        [property: JsonPropertyName("dewRemovedAtLocal")] TimeOnly? DewRemovedAtLocal,
        [property: JsonPropertyName("submittedUtc")] DateTime? SubmittedUtc,
        [property: JsonPropertyName("dewRemovalStartedAtLocal")] TimeOnly? DewRemovalStartedAtLocal = null,
        [property: JsonPropertyName("dewRemovalCompletedAtLocal")] TimeOnly? DewRemovalCompletedAtLocal = null,
        [property: JsonPropertyName("dewRemovalCoveragePct")] decimal? DewRemovalCoveragePct = null
    );

    public sealed record DollarSpotObservationUpsertDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("morningMyceliumObserved")] bool? MorningMyceliumObserved,
        [property: JsonPropertyName("leafLesionsObserved")] bool? LeafLesionsObserved,
        [property: JsonPropertyName("activeExpansionObserved")] bool? ActiveExpansionObserved,
        [property: JsonPropertyName("ldsOrHydrophobicityPresent")] bool? LdsOrHydrophobicityPresent,
        [property: JsonPropertyName("dewManuallyRemoved")] bool? DewManuallyRemoved,
        [property: JsonPropertyName("dewRemovedAtLocal")] TimeOnly? DewRemovedAtLocal,
        [property: JsonPropertyName("dewRemovalStartedAtLocal")] TimeOnly? DewRemovalStartedAtLocal = null,
        [property: JsonPropertyName("dewRemovalCompletedAtLocal")] TimeOnly? DewRemovalCompletedAtLocal = null,
        [property: JsonPropertyName("dewRemovalCoveragePct")] decimal? DewRemovalCoveragePct = null
    );

    public sealed record DollarSpotEarlyWarningSettingsUpdateDto(
        [property: JsonPropertyName("fieldEnabled")] bool FieldEnabled,
        [property: JsonPropertyName("dollarSpotHistoryKnown")] bool? DollarSpotHistoryKnown,
        [property: JsonPropertyName("responsibleUserPubId")] Guid? ResponsibleUserPubId,
        [property: JsonPropertyName("reminderEnabled")] bool ReminderEnabled,
        [property: JsonPropertyName("seasonMode")] string SeasonMode,
        [property: JsonPropertyName("reminderLocalTime")] TimeOnly ReminderLocalTime,
        [property: JsonPropertyName("acknowledgeOnboarding")] bool AcknowledgeOnboarding
    );

    public sealed record DollarSpotEarlyWarningStatusDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldEnabled")] bool FieldEnabled,
        [property: JsonPropertyName("modelId")] string ModelId,
        [property: JsonPropertyName("modelRevision")] string ModelRevision,
        [property: JsonPropertyName("isExperimental")] bool IsExperimental,
        [property: JsonPropertyName("dollarSpotHistoryKnown")] bool? DollarSpotHistoryKnown,
        [property: JsonPropertyName("responsibleUserPubId")] Guid? ResponsibleUserPubId,
        [property: JsonPropertyName("responsibleUserName")] string? ResponsibleUserName,
        [property: JsonPropertyName("effectiveReminderRecipientPubId")] Guid? EffectiveReminderRecipientPubId,
        [property: JsonPropertyName("effectiveReminderRecipientName")] string? EffectiveReminderRecipientName,
        [property: JsonPropertyName("responsibleUserOptions")] IReadOnlyList<DollarSpotResponsibleUserOptionDto> ResponsibleUserOptions,
        [property: JsonPropertyName("needsOnboarding")] bool NeedsOnboarding,
        [property: JsonPropertyName("reminderEnabled")] bool ReminderEnabled,
        [property: JsonPropertyName("seasonMode")] string SeasonMode,
        [property: JsonPropertyName("reminderLocalTime")] TimeOnly ReminderLocalTime,
        [property: JsonPropertyName("seasonState")] string SeasonState,
        [property: JsonPropertyName("seasonReason")] string SeasonReason,
        [property: JsonPropertyName("fiveDayMeanTempC")] decimal? FiveDayMeanTempC,
        [property: JsonPropertyName("todayLocal")] DateOnly TodayLocal,
        [property: JsonPropertyName("answeredToday")] bool AnsweredToday,
        [property: JsonPropertyName("todayObservation")] DollarSpotObservationDto? TodayObservation
    );

    public sealed record DollarSpotResponsibleUserOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("isAvailable")] bool IsAvailable
    );

    public record DollarSpotMetricsDto(
        [property: JsonPropertyName("favorableHours")] int FavorableHours,
        [property: JsonPropertyName("avgTempC")] decimal AvgTempC,
        [property: JsonPropertyName("avgRh")] decimal AvgRh,
        [property: JsonPropertyName("windowHours")] int WindowHours
        );

    public record RiskIndicatorDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("level")] RiskLevel Level,
        [property: JsonPropertyName("score")] decimal Score,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("metrics")] DollarSpotMetricsDto Metrics,
        [property: JsonPropertyName("notes")] string[] Notes
    );
}
