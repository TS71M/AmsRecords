using static AmsRecords.FieldTypes.FieldTypeDtos;

namespace AmsRecords.FieldTypes;

public static class FieldTypeExtensions
{
    // -------------------- Entity -> DTO --------------------

    public static FieldTypeDto ToDto(this FieldType fieldType)
        => new(
            fieldType.PubId,
            fieldType.FieldTypeName,
            fieldType.MarkerColor,
            fieldType.MarkerIconUrl
        );

    // -------------------- DTO -> Entity --------------------

    // For CreateDto
    public static FieldType ToEntity(this FieldTypeCreateDto dto)
        => new()
        {
            // PubId is DB-generated (newsequentialid) -> do NOT set here
            FieldTypeName = (dto.FieldTypeName ?? "").Trim(),
            MarkerColor = string.IsNullOrWhiteSpace(dto.MarkerColor) ? null : dto.MarkerColor.Trim(),
            MarkerIconUrl = string.IsNullOrWhiteSpace(dto.MarkerIconUrl) ? null : dto.MarkerIconUrl.Trim(),
        };

    // Optional: For full DTO (generally avoid setting PubId from client unless you really want it)
    public static FieldType ToEntity(this FieldTypeDto dto)
        => new()
        {
            PubId = dto.PubId,
            FieldTypeName = (dto.FieldTypeName ?? "").Trim(),
            MarkerColor = string.IsNullOrWhiteSpace(dto.MarkerColor) ? null : dto.MarkerColor.Trim(),
            MarkerIconUrl = string.IsNullOrWhiteSpace(dto.MarkerIconUrl) ? null : dto.MarkerIconUrl.Trim(),
        };

    // -------------------- Update existing entity --------------------

    public static void UpdateEntity(this FieldTypeUpdateDto dto, FieldType entity)
    {
        // PubId is validated in service; don't touch it here.

        entity.FieldTypeName = (dto.FieldTypeName ?? "").Trim();
        entity.MarkerColor = string.IsNullOrWhiteSpace(dto.MarkerColor) ? null : dto.MarkerColor.Trim();
        entity.MarkerIconUrl = string.IsNullOrWhiteSpace(dto.MarkerIconUrl) ? null : dto.MarkerIconUrl.Trim();
    }
}