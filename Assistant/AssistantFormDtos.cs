namespace AmsRecords.Assistant;

// The originating browser retains the control references and original values.
// A row subject identifies the record to re-read and never grants access.
public sealed record AssistantFormChoiceDto(string Value, string Label);
public sealed record AssistantFormFieldDto(Guid Id, string Label, string Type, string Value,
    IReadOnlyList<AssistantFormChoiceDto>? Choices = null, string? WaterResultCode = null,
    Guid? SoilAnalytePubId = null, Guid? SoilUnitPubId = null, Guid? SurfacePubId = null);
public sealed record AssistantFormSnapshotDto(Guid Id, IReadOnlyList<AssistantFormFieldDto> Fields, Guid? PhotoFieldId = null,
    Guid? TdrMeasurementPubId = null, Guid? SoilSurfacePubId = null, Guid? DiagnosticRequestPubId = null);
public sealed record AssistantFormValueDto(Guid FieldId, string Value);
public sealed record AssistantFormPatchDto(Guid FormId, IReadOnlyList<AssistantFormValueDto> Values, bool ContinueFilling = false,
    Guid? PhotoFieldId = null);

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
            || form.SoilSurfacePubId == Guid.Empty || form.TdrMeasurementPubId.HasValue && form.PhotoFieldId.HasValue
            || form.DiagnosticRequestPubId == Guid.Empty
            || form.DiagnosticRequestPubId.HasValue && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue || form.SoilSurfacePubId.HasValue)
            || form.SoilSurfacePubId.HasValue && (form.PhotoFieldId.HasValue || form.TdrMeasurementPubId.HasValue)
            || form.Fields is null || form.Fields.Count is < 1 or > 40
            || form.Fields.Any(f => f is null || f.Id == Guid.Empty || string.IsNullOrWhiteSpace(f.Label)
                || f.Label.Length > 160 || !Types.Contains(f.Type) || f.Value is null || f.Value.Length > 2000
                || f.WaterResultCode is { } code && (string.IsNullOrWhiteSpace(code) || code.Length > 80)
                || f.SoilAnalytePubId == Guid.Empty || f.SoilUnitPubId == Guid.Empty
                || f.SoilUnitPubId.HasValue && !f.SoilAnalytePubId.HasValue
                || f.SoilAnalytePubId.HasValue && (f.Type != "number" || f.WaterResultCode is not null || !form.SoilSurfacePubId.HasValue)
                || f.SurfacePubId == Guid.Empty
                || f.SurfacePubId.HasValue && (f.Type is not ("text" or "number") || f.SoilAnalytePubId.HasValue || f.WaterResultCode is not null)
                || f.Choices?.Count > 60 || f.Type != "select" && f.Choices is { Count: > 0 }
                || f.Type == "select" && (f.Choices is not { Count: > 0 }
                    || f.Choices.Select(c => c?.Value).Distinct().Count() != f.Choices.Count)
                || f.Choices?.Any(c => c is null || c.Value is null || c.Value.Length > 200
                    || string.IsNullOrWhiteSpace(c.Label) || c.Label.Length > 160) == true)
            || form.Fields.Select(f => f.Id).Distinct().Count() != form.Fields.Count
            || form.Fields.Any(f => f.Id == form.PhotoFieldId)) return false;
        return form.Fields.Sum(f => f.Label.Length + f.Value.Length
            + (f.Choices?.Sum(c => c.Value.Length + c.Label.Length) ?? 0)) <= 20000;
    }

    public static bool ValidPatch(AssistantFormSnapshotDto form, IReadOnlyList<AssistantFormValueDto>? values)
        => Valid(form) && values is { Count: > 0 and <= 40 }
            && values.Select(v => v?.FieldId).Distinct().Count() == values.Count
            && (values.Count == 1 || values.All(v => !form.Fields.Any(f => f.Id == v?.FieldId && f.Type is "select" or "checkbox")))
            && values.All(v => v is not null && v.Value is not null && v.Value.Length <= 2000
                && form.Fields.Any(f => f.Id == v.FieldId
                    && (f.Type != "select" || f.Choices?.Any(c => c.Value == v.Value) == true)
                    && (f.Type != "checkbox" || v.Value is "true" or "false")));
}
