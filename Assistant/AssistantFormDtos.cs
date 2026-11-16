using System.Text.Json.Serialization;

namespace AmsRecords.Assistant;

// The originating browser retains the control references and original values.
// A row subject identifies the record to re-read and never grants access.
public sealed record AssistantFormChoiceDto(string Value, string Label);
public sealed record AssistantPlannerProductCaptureDto(Guid SourceProductPubId, string ProductName);
public sealed record AssistantPlannerGrowthAdjustmentDto(Guid ProductSnapshotId, DateOnly Start, DateOnly End);
public sealed record AssistantPlannerFrozenProductDto(Guid ProductId, string Stamp, int? NutrientIndex = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool RateUnitReview = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool PackagingUnitReview = false);
public sealed record AssistantModuleFieldReviewDto(string ModuleKey, string ReviewStamp);
public sealed record AssistantStaffFieldReviewDto(Guid CompanyId, Guid UserId, string ReviewStamp);
public sealed record AssistantLeafNitrateThresholdReviewDto(Guid? SurfaceId);
public sealed record AssistantProductImportNitrogenDependencyDto(Guid FieldId, Guid NutrientId);
public sealed record AssistantFormFieldDto(Guid Id, string Label, string Type, string Value,
    IReadOnlyList<AssistantFormChoiceDto>? Choices = null, string? WaterResultCode = null,
    Guid? SoilAnalytePubId = null, Guid? SoilUnitPubId = null, Guid? SurfacePubId = null,
    string? WaterThresholdBound = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] Guid? RequisitionLineId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? FieldProfileProperty = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] Guid? AreaGrassSpeciesId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] int? AreaGrassBaselinePercent = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProcurementPolicyProperty { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProcurementDetailProperty { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StaffPreference { get; init; }
    public int? StaffRoleId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? StaffManagerId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? StaffReportId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? StaffFieldId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ApplicationExecutionId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ApplicationDoseProperty { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProductImportDeclaredCode { get; init; }
}
public sealed record AssistantFormSnapshotDto(Guid Id, IReadOnlyList<AssistantFormFieldDto> Fields, Guid? PhotoFieldId = null,
    Guid? TdrMeasurementPubId = null, Guid? SoilSurfacePubId = null, Guid? DiagnosticRequestPubId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool RequisitionDraftLines = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] Guid? RequisitionVersion = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] Guid? WorkConversationId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? WorkAudienceToken = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? MachineryReviewStamp = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? HoleReviewStamp = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? SnagReviewStamp = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ContactReviewStamp = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? LocationReviewStamp = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<Guid>? ProductSiteSelection = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<Guid>? ProductSiteBaseline = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool AreaGrassReplacement = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool TdrAnalysisFilters = false,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] AssistantPlannerProductCaptureDto? PlannerProductCapture = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] AssistantModuleFieldReviewDto? ModuleFieldReview = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ProductSiteNameFieldId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantLeafNitrateThresholdReviewDto? LeafNitrateThresholdReview { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? FieldAddressReviewId { get; init; }
    public Guid? OrganizationAddressReviewId { get; init; }
    public Guid? PrimaryFieldContactReviewId { get; init; }
    public Guid? OrganizationContactDetailsReviewId { get; init; }
    public Guid? AccountContactDetailsReviewId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountGroupVersion { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountVersion { get; init; }
    public int? PlannerPlanCreationYear { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantPlannerGrowthAdjustmentDto? PlannerGrowthAdjustment { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? MeasurementReadingUnitId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool PlannerSeasonCopy { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProductImportNutrient { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? ProductImportNutrients { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProductImportNitrogenForm { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? ProductImportNitrogenForms { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantProductImportNitrogenDependencyDto? ProductImportNitrogenDependency { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PlannerAreaCandidatePage { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? PlannerFrozenAreaId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantPlannerFrozenProductDto? PlannerFrozenProduct { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PlannerFrozenSurfaceIndex { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantPlannerZoneReviewDto? PlannerZoneReview { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? IrrigationDesignSurfaceId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<Guid>? ApplicationCorrectionExecutionIds { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantStaffFieldReviewDto? StaffFieldReview { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StaffDirectReportStamp { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? TemporaryReplacementCompanyId { get; init; }
}
public sealed record AssistantPlannerZoneReviewDto(Guid? ZoneId, string Token, IReadOnlyList<Guid> SelectedAreaIds);
public sealed record AssistantFormValueDto(Guid FieldId, string Value);
public sealed record AssistantRequisitionLineDto(Guid ProductId, string Quantity, string Unit, string? Notes);
public sealed record AssistantAreaGrassRowDto(Guid GrassSpeciesId, int Percent);
public sealed record AssistantFormPatchDto(Guid FormId, IReadOnlyList<AssistantFormValueDto> Values, bool ContinueFilling = false,
    Guid? PhotoFieldId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<AssistantRequisitionLineDto>? RequisitionLines = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<Guid>? ProductSiteIds = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<AssistantAreaGrassRowDto>? AreaGrassReplacement = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<Guid>? ModuleFieldIds = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<Guid>? ModuleFieldRemovalIds { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<Guid>? PlannerZoneAreaIds { get; init; }
}

public static class AssistantFormPolicy
{
    public static bool CanContinue(AssistantFormSnapshotDto form, IReadOnlyList<AssistantFormValueDto>? values)
        => ValidPatch(form, values) && values!.Count == 1
            && form.Fields.Any(f => f.Id == values[0].FieldId && (f.Type is "select" or "checkbox") && f.Value != values[0].Value);
    static readonly HashSet<string> Types = ["text", "number", "email", "tel", "url", "date", "time", "datetime-local", "textarea", "select", "checkbox"];

    public static bool Valid(AssistantFormSnapshotDto? form)
    {
        if (form is null) return true;
        if (form.Id == Guid.Empty || form.PhotoFieldId == Guid.Empty || form.TdrMeasurementPubId == Guid.Empty
            || form.PlannerFrozenProduct is { } frozenProduct && (frozenProduct.ProductId == Guid.Empty
                || frozenProduct.Stamp is not { Length: 64 } || !frozenProduct.Stamp.All(Uri.IsHexDigit)
                || form.PhotoFieldId.HasValue || form.PlannerFrozenAreaId.HasValue || form.PlannerProductCapture is not null
                || frozenProduct.NutrientIndex is < 0 or >= 100
                || (frozenProduct.PackagingUnitReview
                    ? frozenProduct.RateUnitReview || frozenProduct.NutrientIndex.HasValue
                        || form.Fields is not [{ Type: "text" }, { Type: "text" }, { Type: "number" }, { Type: "number" },
                            { Type: "number" }, { Type: "text" }, { Type: "number" }, { Type: "number" }, { Type: "checkbox" }]
                    : frozenProduct.RateUnitReview
                    ? frozenProduct.NutrientIndex.HasValue || form.Fields is not [{ Type: "text" }, { Type: "text" }, { Type: "select" },
                        { Type: "number" }, { Type: "number" }, { Type: "number" }, { Type: "number" }, { Type: "number" }, { Type: "checkbox" }]
                    : frozenProduct.NutrientIndex.HasValue
                    ? form.Fields is not [{ Type: "text" }, { Type: "text" }, { Type: "number" }, { Type: "number" },
                        { Type: "number" }, { Type: "number" }, { Type: "number" }, { Type: "checkbox" },
                        { Type: "checkbox" }, { Type: "text" }, { Type: "text" }, { Type: "number" }, { Type: "select" }, { Type: "text" }, { Type: "checkbox" }]
                    : form.Fields is not [{ Type: "text" }, { Type: "text" }, { Type: "number" }, { Type: "number" },
                        { Type: "number" }, { Type: "number" }, { Type: "number" }, { Type: "checkbox" }]))
            || form.PlannerFrozenSurfaceIndex is int surfaceIndex && (form.PlannerFrozenAreaId is null || surfaceIndex is < 0 or >= 1000)
            || form.PlannerFrozenAreaId is { } frozenArea && (frozenArea == Guid.Empty || form.PhotoFieldId.HasValue
                || (form.PlannerFrozenSurfaceIndex.HasValue
                    ? form.Fields is not [{ Type: "text" }, { Type: "checkbox" }, { Type: "number" }, { Type: "checkbox" }, { Type: "text" }, { Type: "number" }]
                    : form.Fields is not [{ Type: "text" }, { Type: "checkbox" }, { Type: "number" }]))
            || form.ModuleFieldReview is { } module && (module.ReviewStamp is not { Length: 64 } || !module.ReviewStamp.All(Uri.IsHexDigit)
                || !AmsRecords.Ibus.ModuleMenuCatalog.Modules.Any(item => item.Key == module.ModuleKey)
                || form.Fields is not [{ Type: "checkbox", Value: "true" or "false" }] || form.PhotoFieldId.HasValue
                || form.PlannerProductCapture is not null || form.TdrAnalysisFilters || form.TdrMeasurementPubId.HasValue
                || form.SoilSurfacePubId.HasValue || form.DiagnosticRequestPubId.HasValue || form.RequisitionDraftLines
                || form.RequisitionVersion.HasValue || form.WorkConversationId.HasValue || form.ProductSiteSelection is not null
                || form.AreaGrassReplacement || form.MachineryReviewStamp is not null || form.HoleReviewStamp is not null
                || form.SnagReviewStamp is not null || form.ContactReviewStamp is not null || form.LocationReviewStamp is not null)
            || form.PlannerProductCapture is { } capture && (capture.SourceProductPubId == Guid.Empty
                || string.IsNullOrWhiteSpace(capture.ProductName) || capture.ProductName.Length > 250
                || form.Fields is not [{ Type: "number" }] || form.PhotoFieldId.HasValue || form.TdrAnalysisFilters
                || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue || form.DiagnosticRequestPubId.HasValue
                || form.RequisitionDraftLines || form.RequisitionVersion.HasValue || form.WorkConversationId.HasValue
                || form.ProductSiteSelection is not null || form.AreaGrassReplacement || form.MachineryReviewStamp is not null
                || form.HoleReviewStamp is not null || form.SnagReviewStamp is not null || form.ContactReviewStamp is not null
                || form.LocationReviewStamp is not null)
            || form.TdrAnalysisFilters && (form.Fields is not ([{ Type: "select" }, { Type: "date" }, { Type: "date" }]
                or [{ Type: "select" }, { Type: "date" }, { Type: "date" }, { Type: "select" }])
                || form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue
                || form.DiagnosticRequestPubId.HasValue || form.RequisitionDraftLines || form.RequisitionVersion.HasValue
                || form.WorkConversationId.HasValue || form.MachineryReviewStamp is not null || form.HoleReviewStamp is not null
                || form.SnagReviewStamp is not null || form.ContactReviewStamp is not null || form.LocationReviewStamp is not null
                || form.ProductSiteSelection is not null || form.AreaGrassReplacement)
            || form.SoilSurfacePubId == Guid.Empty || form.TdrMeasurementPubId.HasValue && form.PhotoFieldId.HasValue
            || form.DiagnosticRequestPubId == Guid.Empty
            || form.RequisitionVersion == Guid.Empty
            || form.WorkConversationId == Guid.Empty
            || new[] { form.MachineryReviewStamp, form.HoleReviewStamp, form.SnagReviewStamp, form.ContactReviewStamp, form.LocationReviewStamp }.Count(stamp => stamp is not null) > 1
            || (form.MachineryReviewStamp ?? form.HoleReviewStamp ?? form.SnagReviewStamp ?? form.ContactReviewStamp ?? form.LocationReviewStamp) is { } recordStamp && (recordStamp.Length != 64 || !recordStamp.All(Uri.IsHexDigit)
                || form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue
                || form.DiagnosticRequestPubId.HasValue || form.RequisitionVersion.HasValue || form.RequisitionDraftLines || form.WorkConversationId.HasValue)
            || form.WorkConversationId.HasValue != (form.WorkAudienceToken is { Length: 64 })
            || form.WorkAudienceToken is not null && (!form.WorkConversationId.HasValue || !form.WorkAudienceToken.All(Uri.IsHexDigit))
            || form.WorkConversationId.HasValue && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue
                || form.SoilSurfacePubId.HasValue || form.DiagnosticRequestPubId.HasValue || form.RequisitionVersion.HasValue
                || form.RequisitionDraftLines)
            || form.RequisitionVersion.HasValue && (form.RequisitionDraftLines || form.PhotoFieldId.HasValue
                || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue || form.DiagnosticRequestPubId.HasValue)
            || form.RequisitionDraftLines && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue
                || form.SoilSurfacePubId.HasValue || form.DiagnosticRequestPubId.HasValue)
            || form.DiagnosticRequestPubId.HasValue && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue)
            || form.SoilSurfacePubId.HasValue && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue)
            || !ValidProductSites(form.ProductSiteSelection) || !ValidProductSites(form.ProductSiteBaseline)
            || form.ProductSiteBaseline is not null && form.ProductSiteSelection is null
            || form.Fields is null || form.Fields.Count is < 1 or > 40
            || form.Fields.Any(f => f is null || f.Id == Guid.Empty || string.IsNullOrWhiteSpace(f.Label)
                || f.Label.Length > 160 || !Types.Contains(f.Type) || f.Value is null || f.Value.Length > 2000
                || f.WaterResultCode is { } code && (string.IsNullOrWhiteSpace(code) || code.Length > 80)
                || f.WaterThresholdBound is not null && (f.WaterThresholdBound is not ("minimum" or "maximum")
                    || f.WaterResultCode is null || f.Type != "number")
                || f.SoilAnalytePubId == Guid.Empty || f.SoilUnitPubId == Guid.Empty
                || f.SoilUnitPubId.HasValue && !f.SoilAnalytePubId.HasValue
                || f.SoilAnalytePubId.HasValue && (f.Type != "number" || f.WaterResultCode is not null || !form.SoilSurfacePubId.HasValue)
                || f.SurfacePubId == Guid.Empty
                || f.AreaGrassSpeciesId == Guid.Empty || f.AreaGrassSpeciesId.HasValue != f.AreaGrassBaselinePercent.HasValue
                || f.AreaGrassSpeciesId.HasValue && (f.Type != "number" || f.AreaGrassBaselinePercent is < 0 or > 100)
                || f.RequisitionLineId == Guid.Empty
                || f.RequisitionLineId.HasValue && (!form.RequisitionVersion.HasValue || f.Type is not ("select" or "text")
                    || f.SurfacePubId.HasValue || f.WaterResultCode is not null || f.SoilAnalytePubId.HasValue)
                || f.SurfacePubId.HasValue && (f.Type is not ("text" or "number") || f.SoilAnalytePubId.HasValue || f.WaterResultCode is not null)
                || f.Choices?.Count > 60 || f.Type != "select" && f.Choices is { Count: > 0 }
                || f.Type == "select" && (f.Choices is not { Count: > 0 }
                    || f.Choices.Select(c => c?.Value).Distinct().Count() != f.Choices.Count)
                || f.ProcurementDetailProperty is not (null or "DefaultDeliveryLocation" or "Notes")
                || f.ProcurementDetailProperty == "DefaultDeliveryLocation" && f.Type != "select"
                || f.ProcurementDetailProperty == "Notes" && f.Type != "textarea"
                || f.Choices?.Any(c => c is null || c.Value is null
                    || c.Value.Length > (f.ProcurementDetailProperty == "DefaultDeliveryLocation" ? 500 : 200)
                    || string.IsNullOrWhiteSpace(c.Label)
                    || c.Label.Length > (f.ProcurementDetailProperty == "DefaultDeliveryLocation" ? 660 : 160)) == true)
            || form.Fields.Select(f => f.Id).Distinct().Count() != form.Fields.Count
            || form.Fields.Any(f => f.Id == form.PhotoFieldId)) return false;
        return form.Fields.Sum(f => f.Label.Length + f.Value.Length
            + (f.Choices?.Sum(c => c.Value.Length + c.Label.Length) ?? 0)) <= 20000;
    }

    static bool ValidProductSites(IReadOnlyList<Guid>? sites)
        => sites is null || sites.Count <= 1000 && !sites.Contains(Guid.Empty) && sites.Distinct().Count() == sites.Count;

    public static bool ValidPatch(AssistantFormSnapshotDto form, IReadOnlyList<AssistantFormValueDto>? values, bool allowApplicationDose = false,
        bool allowTemporaryReplacement = false, bool allowPlannerSeasonCopy = false)
        => Valid(form) && values is { Count: > 0 and <= 40 }
            && values.Select(v => v?.FieldId).Distinct().Count() == values.Count
            && (allowTemporaryReplacement && form.TemporaryReplacementCompanyId.HasValue || allowPlannerSeasonCopy && form.PlannerSeasonCopy
                || form.TdrAnalysisFilters || form.PlannerFrozenAreaId.HasValue || form.PlannerFrozenProduct?.NutrientIndex is not null
                || form.PlannerFrozenProduct?.RateUnitReview == true || values.Count == 1
                || values.All(v => !form.Fields.Any(f => f.Id == v?.FieldId && f.Type is "select" or "checkbox"
                    && !(allowApplicationDose && v?.Value == "true" && f is { Type: "checkbox", ApplicationDoseProperty: "clearCarrier", ApplicationExecutionId: null })
                    && !(allowApplicationDose && f is { Type: "select", ApplicationDoseProperty: "unit", ApplicationExecutionId: { } execution }
                        && form.Fields.Any(dose => dose.ApplicationExecutionId == execution && dose.Type == "number"
                            && (v?.Value == "" ? dose.ApplicationDoseProperty == "rate" : dose.ApplicationDoseProperty == "quantity")
                            && values.Any(amount => amount?.FieldId == dose.Id && !string.IsNullOrEmpty(amount.Value))))
                    && !(allowApplicationDose && v?.Value == "true"
                        && f is { Type: "checkbox", ApplicationDoseProperty: "replaceNotes", ApplicationExecutionId: { } noteExecution }
                        && form.Fields.Any(note => note.ApplicationExecutionId == noteExecution && note.Type == "textarea"
                            && note.ApplicationDoseProperty == "notes" && values.Any(text => text?.FieldId == note.Id))))))
            && values.All(v => v is not null && v.Value is not null && v.Value.Length <= 2000
                && form.Fields.Any(f => f.Id == v.FieldId
                    && (f.Type != "select" || f.Choices?.Any(c => c.Value == v.Value) == true)
                    && (f.Type != "checkbox" || v.Value is "true" or "false")));
}
