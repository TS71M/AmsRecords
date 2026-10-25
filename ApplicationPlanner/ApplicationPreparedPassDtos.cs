using Lib.Enums;

namespace AmsRecords.ApplicationPlanner;

public sealed record PrepareApplicationPassRequest(
    Guid AnnualPlanPubId,
    Guid PlanRevisionPubId,
    Guid PlanPassPubId,
    Guid? MachineryPubId = null,
    decimal? CarrierVolumeLitresPerHa = null);

public sealed record AssignApplicationJobRequest(Guid PreparedRevisionPubId, Guid ConcurrencyToken, Guid ExecutorPubId);
public sealed record ReviewApplicationJobRequest(Guid PreparedRevisionPubId, Guid ConcurrencyToken,
    Guid SubmissionEventPubId, bool Approve, string Remarks = "");
public sealed record ApplicationJobPersonDto(Guid PubId, string DisplayName);
public sealed record ApplicationJobInstructionProductDto(Guid SourceProductPubId, decimal Quantity, string Unit);
public sealed record ReviseApplicationJobInstructionsRequest(Guid PreparedRevisionPubId, Guid ConcurrencyToken,
    decimal RemainingWaterLitres, IReadOnlyList<ApplicationJobInstructionProductDto> Products, string Reason,
    bool PreviewOnly = true, string ExpectedRecipeSha256 = "");
public sealed record ApplicationJobInstructionChangeDto(ApplicationPreparedPassRecipeDto Recipe,
    ApplicationPassAssessmentDto Assessment, string RecipeSha256, int CompletedLoads, ApplicationPreparedPassDto? Job = null);
public sealed record ApplicationJobInstructionRevisionDto(Guid RevisionPubId, int RevisionNumber,
    DateTime CreatedAtUtc, string ChangedBy, string Reason, ApplicationPreparedPassRecipeDto Recipe);
public sealed record ApplicationJobSummaryDto(Guid RevisionPubId, Guid FieldPubId, string FieldName,
    string JobName, DateOnly PlannedDate, ApplicationPreparedPassRevisionStatus Status, string ExecutorName);

public sealed record SignApplicationPreparedPassRequest(
    Guid PreparedRevisionPubId,
    Guid ConcurrencyToken,
    bool PpeConfirmed,
    bool MachineConditionConfirmed,
    bool OperatorQualificationConfirmed,
    ApplicationWeatherDecision WeatherDecision,
    DateTime WeatherObservedAtUtc,
    string WeatherSummary,
    string OverrideReason);

public sealed record ApplicationPreparedProductActualDto(
    Guid PlannedApplicationPubId,
    decimal Quantity,
    string Unit,
    Guid? SourceProductPubId = null,
    string ProductName = "");

public sealed record AppendApplicationPreparedPassEventRequest(
    Guid PreparedRevisionPubId,
    Guid ClientEventId,
    ApplicationPreparedPassEventType EventType,
    DateTime OccurredAtUtc,
    int? StepNumber = null,
    decimal? CoverageM2 = null,
    decimal? Latitude = null,
    decimal? Longitude = null,
    string Note = "",
    decimal? ActualCarrierVolumeLitres = null,
    IReadOnlyList<ApplicationPreparedProductActualDto>? ActualProducts = null,
    DateOnly? ExecutedLocalDate = null);

public sealed record ApplicationPreparedPassSafetyConfirmationDto(
    bool PpeConfirmed,
    bool MachineConditionConfirmed,
    bool OperatorQualificationConfirmed,
    ApplicationWeatherDecision WeatherDecision,
    DateTime WeatherObservedAtUtc,
    string WeatherSummary,
    string OverrideReason,
    Guid ConfirmedByUserPubId,
    DateTime ConfirmedAtUtc);

public sealed record ApplicationPreparedPassProductDto(
    Guid PlannedApplicationPubId,
    Guid? ProductSnapshotPubId,
    Guid? SourceProductPubId,
    string ProductName,
    string Target,
    decimal Rate,
    string RateUnit,
    decimal Quantity,
    string QuantityUnit,
    string PhysicalForm,
    IReadOnlyList<string> Hazards,
    IReadOnlyList<string> Ppe,
    IReadOnlyList<string> WeatherRestrictions,
    IReadOnlyList<string> MixingSequence,
    string CleanupInstructions,
    string DisposalInstructions,
    ApplicationEvidenceReferenceDto Evidence,
    decimal UnitCost = 0,
    string PriceUnit = "",
    decimal? DensityKgPerL = null,
    bool IsAdditional = false);

public sealed record ApplicationPreparedPassStepDto(
    int Number,
    string Kind,
    string Instruction,
    decimal? Amount,
    string Unit,
    ApplicationEvidenceReferenceDto? Evidence = null,
    int? LoadNumber = null);

public sealed record ApplicationPreparedPassRecipeDto(
    int Version,
    string JobName,
    string FieldName,
    string ZoneName,
    DateOnly PlannedLocalDate,
    decimal TreatedAreaM2,
    decimal CarrierVolumeLitresPerHa,
    decimal TotalCarrierVolumeLitres,
    Guid? MachinePubId,
    string MachineName,
    ApplicationEvidenceReferenceDto MachineEvidence,
    IReadOnlyList<string> RequiredPpe,
    IReadOnlyList<string> WeatherRestrictions,
    IReadOnlyList<ApplicationPreparedPassProductDto> Products,
    IReadOnlyList<ApplicationPreparedPassStepDto> Steps,
    IReadOnlyList<ApplicationEvidenceReferenceDto> Sources,
    IReadOnlyList<ApplicationPreparedLoadDto>? Loads = null,
    IReadOnlyList<ApplicationMachineSettingDto>? MachineSettings = null,
    IReadOnlyList<string>? DispatchBlockers = null);

public sealed record ApplicationMachineSettingDto(string Name, string Value, string Unit, string SourceLocation);
public sealed record ApplicationPreparedLoadProductDto(Guid PlannedApplicationPubId, string ProductName,
    decimal Quantity, string Unit, int Order, IReadOnlyList<string> Instructions);
public sealed record ApplicationPreparedLoadDto(int Number, decimal TreatedAreaM2, decimal WaterLitres,
    decimal InitialWaterLitres, decimal TopUpWaterLitres, IReadOnlyList<ApplicationPreparedLoadProductDto> Products,
    IReadOnlyList<ApplicationMachineSettingDto>? MachineSettings = null);

public sealed record ApplicationPreparedPassEventDto(
    Guid PubId,
    Guid ClientEventId,
    ApplicationPreparedPassEventType EventType,
    DateTime OccurredAtUtc,
    int? StepNumber,
    decimal? CoverageM2,
    decimal? Latitude,
    decimal? Longitude,
    string Note,
    decimal? ActualCarrierVolumeLitres,
    IReadOnlyList<ApplicationPreparedProductActualDto> ActualProducts,
    DateTime RecordedAtUtc);

public sealed record ApplicationPreparedPassDto(
    Guid PubId,
    Guid RevisionPubId,
    int RevisionNumber,
    ApplicationPreparedPassRevisionStatus Status,
    bool IsCurrent,
    Guid ConcurrencyToken,
    string RecipeSha256,
    ApplicationPreparedPassRecipeDto Recipe,
    ApplicationPassAssessmentDto Assessment,
    ApplicationPreparedPassSafetyConfirmationDto? SafetyConfirmation,
    DateTime PreparedAtUtc,
    DateTime? SignedAtUtc,
    IReadOnlyList<ApplicationPreparedPassEventDto> Events,
    Guid? AssignedToUserPubId = null, string AssignedToUserName = "",
    bool CanManage = false, bool CanExecute = false, bool CanReview = false,
    Guid? ApprovedExecutionPassPubId = null,
    Guid FieldPubId = default,
    IReadOnlyList<ApplicationJobInstructionRevisionDto>? InstructionHistory = null);
