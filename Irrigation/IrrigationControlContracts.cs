using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace AmsRecords.Irrigation;

public enum IrrigationControlCommandKind
{
    SetStationRuntime = 1,
    StartStation = 2,
    StopStation = 3,
    ApplyProgramAdjustment = 4
}

public enum IrrigationControlTargetKind
{
    Station = 1,
    Program = 2
}

public enum IrrigationControlRecommendationOrigin
{
    DeterministicEngine = 1,
    HumanAuthored = 2,
    AiAssisted = 3
}

public enum IrrigationControlApprovalActorKind
{
    HumanUser = 1,
    AutomatedService = 2,
    AiAgent = 3
}

public enum IrrigationControlIntegrationAuthorizationKind
{
    None = 0,
    OfficialVendorApi = 1,
    VendorAuthorizedIntegration = 2
}

public enum IrrigationControlExecutionSemantics
{
    SingleCommandOnly = 1,
    AtomicTransaction = 2,
    AdapterManagedRollback = 3
}

[Flags]
public enum IrrigationControlEvidenceRequirement
{
    None = 0,
    DeterministicSimulation = 1,
    HydraulicAnalysis = 2,
    AgronomicDemand = 4,
    AdvisoryInterpretation = 8
}

public enum IrrigationControlResultStatus
{
    Pending = 0,
    Succeeded = 1,
    Rejected = 2,
    Failed = 3,
    RolledBack = 4,
    RollbackFailed = 5,
    Unknown = 6
}

public static class IrrigationControlValueCodes
{
    public const string ConfiguredRuntime = "CONFIGURED_RUNTIME";
    public const string Running = "RUNNING";
    public const string Stopped = "STOPPED";
    public const string ProgramAdjustment = "PROGRAM_ADJUSTMENT";
    public const string Seconds = "SECONDS";
    public const string Percent = "PERCENT";
}

public sealed record IrrigationControlTarget(
    Guid IrrigationSystemPubId,
    Guid TargetPubId,
    IrrigationControlTargetKind Kind,
    string DisplayName);

public sealed record IrrigationControlValue(
    string StateCode,
    decimal? NumericValue = null,
    string? UnitCode = null);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "commandType")]
[JsonDerivedType(typeof(SetStationRuntimeCommand), "setStationRuntime")]
[JsonDerivedType(typeof(StartStationCommand), "startStation")]
[JsonDerivedType(typeof(StopStationCommand), "stopStation")]
[JsonDerivedType(typeof(ApplyProgramAdjustmentCommand), "applyProgramAdjustment")]
public abstract record IrrigationControlCommand(
    Guid CommandPubId,
    IrrigationControlTarget Target)
{
    public abstract IrrigationControlCommandKind Kind { get; }
    public abstract IrrigationControlValue RequestedValue { get; }
}

public sealed record SetStationRuntimeCommand(
    Guid CommandPubId,
    IrrigationControlTarget Target,
    int RuntimeSeconds)
    : IrrigationControlCommand(CommandPubId, Target)
{
    public override IrrigationControlCommandKind Kind => IrrigationControlCommandKind.SetStationRuntime;
    public override IrrigationControlValue RequestedValue => new(
        IrrigationControlValueCodes.ConfiguredRuntime,
        RuntimeSeconds,
        IrrigationControlValueCodes.Seconds);
}

public sealed record StartStationCommand(
    Guid CommandPubId,
    IrrigationControlTarget Target,
    int MaximumRuntimeSeconds)
    : IrrigationControlCommand(CommandPubId, Target)
{
    public override IrrigationControlCommandKind Kind => IrrigationControlCommandKind.StartStation;
    public override IrrigationControlValue RequestedValue => new(
        IrrigationControlValueCodes.Running,
        MaximumRuntimeSeconds,
        IrrigationControlValueCodes.Seconds);
}

public sealed record StopStationCommand(
    Guid CommandPubId,
    IrrigationControlTarget Target)
    : IrrigationControlCommand(CommandPubId, Target)
{
    public override IrrigationControlCommandKind Kind => IrrigationControlCommandKind.StopStation;
    public override IrrigationControlValue RequestedValue => new(IrrigationControlValueCodes.Stopped);
}

public sealed record ApplyProgramAdjustmentCommand(
    Guid CommandPubId,
    IrrigationControlTarget Target,
    decimal AdjustmentPercent)
    : IrrigationControlCommand(CommandPubId, Target)
{
    public override IrrigationControlCommandKind Kind => IrrigationControlCommandKind.ApplyProgramAdjustment;
    public override IrrigationControlValue RequestedValue => new(
        IrrigationControlValueCodes.ProgramAdjustment,
        AdjustmentPercent,
        IrrigationControlValueCodes.Percent);
}

public sealed record IrrigationControlEvidence(
    string SimulationFingerprint,
    Guid? ScenarioPubId = null,
    string? HydraulicAnalysisFingerprint = null,
    string? AgronomicDemandFingerprint = null,
    string? AdvisoryInterpretationFingerprint = null,
    bool AdvisoryInterpretationWasAdvisoryOnly = false);

public sealed record IrrigationControlRecommendationSource(
    Guid RecommendationPubId,
    IrrigationControlRecommendationOrigin Origin,
    string SourceReference,
    DateTimeOffset RecommendedAtUtc);

public sealed record IrrigationControlRecommendation(
    Guid IbuPubId,
    IrrigationControlRecommendationSource Source,
    IrrigationControlEvidence Evidence,
    IReadOnlyList<IrrigationControlCommand> Commands);

public sealed record IrrigationControlCommandReview(
    IrrigationControlCommand Command,
    IrrigationControlValue OldValue,
    IrrigationControlValue NewValue,
    string ExpectedStateToken);

public sealed record IrrigationControlReview(
    Guid ReviewPubId,
    Guid IbuPubId,
    IrrigationControlRecommendationSource Source,
    IrrigationControlEvidence Evidence,
    string AdapterKey,
    string AdapterVersion,
    DateTimeOffset PreparedAtUtc,
    DateTimeOffset ExpiresAtUtc,
    IReadOnlyList<IrrigationControlCommandReview> Commands);

public sealed record IrrigationControlApproval(
    Guid ApprovalPubId,
    Guid ReviewPubId,
    string ReviewFingerprint,
    IrrigationControlApprovalActorKind ActorKind,
    Guid ApprovedByUserPubId,
    DateTimeOffset ApprovedAtUtc,
    DateTimeOffset ExpiresAtUtc,
    bool ExplicitApproval);

public sealed record IrrigationControlIntegrationAuthorization(
    IrrigationControlIntegrationAuthorizationKind Kind,
    string EvidenceReference,
    DateTimeOffset VerifiedAtUtc);

public sealed record IrrigationControlCommandCapability(
    IrrigationControlCommandKind CommandKind,
    IrrigationControlEvidenceRequirement RequiredEvidence,
    int? MaximumRuntimeSeconds = null,
    decimal? MinimumAdjustmentPercent = null,
    decimal? MaximumAdjustmentPercent = null);

public sealed record IrrigationControlAdapterCapabilities(
    string AdapterKey,
    string AdapterVersion,
    IrrigationControlIntegrationAuthorization Authorization,
    IrrigationControlExecutionSemantics ExecutionSemantics,
    bool EnforcesExpectedStateToken,
    bool AutomaticWriteRetriesEnabled,
    IReadOnlyList<IrrigationControlCommandCapability> Commands);

/// <summary>
/// Produces the immutable digest that a human approval must bind. The digest includes the recommendation origin,
/// deterministic evidence, adapter version, every visible old/new value, and every external-state precondition.
/// </summary>
public static class IrrigationControlReviewFingerprint
{
    public static string Compute(IrrigationControlReview review)
    {
        ArgumentNullException.ThrowIfNull(review);
        var canonical = new StringBuilder();
        Add(canonical, review.ReviewPubId);
        Add(canonical, review.IbuPubId);
        Add(canonical, review.Source.RecommendationPubId);
        Add(canonical, (int)review.Source.Origin);
        Add(canonical, review.Source.SourceReference);
        Add(canonical, review.Source.RecommendedAtUtc);
        Add(canonical, review.Evidence.SimulationFingerprint);
        Add(canonical, review.Evidence.ScenarioPubId);
        Add(canonical, review.Evidence.HydraulicAnalysisFingerprint);
        Add(canonical, review.Evidence.AgronomicDemandFingerprint);
        Add(canonical, review.Evidence.AdvisoryInterpretationFingerprint);
        Add(canonical, review.Evidence.AdvisoryInterpretationWasAdvisoryOnly);
        Add(canonical, review.AdapterKey);
        Add(canonical, review.AdapterVersion);
        Add(canonical, review.PreparedAtUtc);
        Add(canonical, review.ExpiresAtUtc);
        Add(canonical, review.Commands.Count);
        foreach (var item in review.Commands)
        {
            Add(canonical, item.Command.CommandPubId);
            Add(canonical, (int)item.Command.Kind);
            Add(canonical, item.Command.Target.IrrigationSystemPubId);
            Add(canonical, item.Command.Target.TargetPubId);
            Add(canonical, (int)item.Command.Target.Kind);
            Add(canonical, item.Command.Target.DisplayName);
            Add(canonical, item.OldValue);
            Add(canonical, item.NewValue);
            Add(canonical, item.ExpectedStateToken);
        }

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical.ToString())));
    }

    public static bool Matches(IrrigationControlReview review, string? fingerprint)
    {
        if (string.IsNullOrWhiteSpace(fingerprint))
            return false;
        try
        {
            var expected = Convert.FromHexString(Compute(review));
            var supplied = Convert.FromHexString(fingerprint);
            return expected.Length == supplied.Length && CryptographicOperations.FixedTimeEquals(expected, supplied);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    static void Add(StringBuilder value, IrrigationControlValue item)
    {
        Add(value, item.StateCode);
        Add(value, item.NumericValue);
        Add(value, item.UnitCode);
    }

    static void Add(StringBuilder value, Guid item) => Add(value, item.ToString("N"));
    static void Add(StringBuilder value, Guid? item) => Add(value, item?.ToString("N"));
    static void Add(StringBuilder value, DateTimeOffset item) => Add(value, item.ToUniversalTime().Ticks);
    static void Add(StringBuilder value, bool item) => Add(value, item ? "1" : "0");
    static void Add(StringBuilder value, int item) => Add(value, item.ToString(CultureInfo.InvariantCulture));
    static void Add(StringBuilder value, long item) => Add(value, item.ToString(CultureInfo.InvariantCulture));
    static void Add(StringBuilder value, decimal? item) => Add(value, item?.ToString("G29", CultureInfo.InvariantCulture));

    static void Add(StringBuilder value, string? item)
    {
        var normalized = item ?? "";
        value.Append(normalized.Length.ToString(CultureInfo.InvariantCulture));
        value.Append(':');
        value.Append(normalized);
        value.Append('|');
    }
}
