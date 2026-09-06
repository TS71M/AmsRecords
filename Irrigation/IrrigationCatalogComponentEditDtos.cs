using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Irrigation;

public sealed record IrrigationCatalogComponentEditDto(
    Guid PubId, string ManufacturerName, string PartNumber, string ManufacturerNumber,
    string ComponentType, string Name, string Color, string Notes, DateTime UpdatedAtUtc,
    IReadOnlyList<IrrigationCatalogComponentImageDto>? Images = null);

public sealed record IrrigationCatalogComponentImageDto(Guid NozzleOptionPubId, string ModelName,
    string Position, string? ReferenceImageUrl, DateTime UpdatedAtUtc);

public sealed record IrrigationCatalogComponentImageUpdateDto(Guid NozzleOptionPubId,
    [param: MaxLength(500)] string? ReferenceImageUrl, DateTime ExpectedUpdatedAtUtc);

public sealed record IrrigationCatalogComponentUpdateDto(
    [param: MaxLength(80)] string ManufacturerNumber,
    [param: Required, MaxLength(80)] string ComponentType,
    [param: Required, MaxLength(160)] string Name,
    [param: MaxLength(80)] string Color,
    [param: MaxLength(2000)] string Notes,
    DateTime ExpectedUpdatedAtUtc,
    IReadOnlyList<IrrigationCatalogComponentImageUpdateDto>? Images = null);
