using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Assistant;

public sealed record CourseAssistantAskDto(
    Guid FieldPubId,
    [property: Required, StringLength(2000)] string Question,
    [property: StringLength(20)] string? Culture = null,
    IReadOnlyList<SupportAssistantMessageDto>? History = null);

public sealed record CourseAssistantMeasurementDto(decimal Value, string Unit, DateTime MeasuredAtUtc);

public sealed record CourseAssistantSurfaceDto(
    Guid SurfacePubId, Guid AreaPubId, string Name, int HoleNumber,
    decimal? AreaM2,
    CourseAssistantMeasurementDto? LatestMoisture,
    CourseAssistantMeasurementDto? LatestGreenSpeed);

public sealed record CourseAssistantAnswerDto(
    string Answer,
    IReadOnlyList<CourseAssistantSurfaceDto> Sources,
    bool CanGuideToSurfaces);
