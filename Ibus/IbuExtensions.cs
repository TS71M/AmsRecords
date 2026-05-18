using AmsRecords.Addresses;
using AmsRecords.ContactDetails;
using AmsRecords.Coordinates;
using AmsRecords.Fields;
using static AmsRecords.Ibus.IbuDtos;
using static AmsRecords.Jurisdictions.JurisdictionExtensions;

namespace AmsRecords.Ibus;

public static class IbuExtensions
{
    const string IbuMapMarkerName = "Headquarter";
    const string IbuMapMarkerColor = "#1f6feb";
    const string IbuMapMarkerIconUrl = "/icons/field-office.svg";

    public static IbuDto ToDto(this Ibu ibu, int fieldsCount = 0)
        => new(
            PubId: ibu.PubId,
            BusinessUnitName: ibu.BusinessUnitName,
            Status: ibu.Status,
            WorkspaceKind: ibu.WorkspaceKind,
            FieldTypeName: ibu.FieldTypeName,
            Address: ibu.MainAddress?.ToDto(),
            Contact: ibu.Contact?.ToDto(),
            Coordinate: ibu.Coordinate?.ToDto(),
            Jurisdiction: ibu.Jurisdiction?.ToDto(),
            ImagePubId: ibu.AppImage?.PubId,
            RegistrationDate: ibu.RegistrationDate,
            UpdatedDate: ibu.UpdatedDate,
            FieldTypePubId: ibu.FieldType?.PubId,
            PrimaryFieldPubId: ibu.PrimaryField?.PubId,
            PrimaryFieldName: ibu.PrimaryField?.FieldName,
            VisualizationTempUnitPubId: ibu.VisualizationTempUnit?.PubId,
            VisualizationRainUnitPubId: ibu.VisualizationRainUnit?.PubId,
            VisualizationWindUnitPubId: ibu.VisualizationWindUnit?.PubId,
            VisualizationTempUnitShort: ibu.VisualizationTempUnit?.UnitShort,
            VisualizationRainUnitShort: ibu.VisualizationRainUnit?.UnitShort,
            VisualizationWindUnitShort: ibu.VisualizationWindUnit?.UnitShort,
            FieldsCount: fieldsCount
        );

    public static IbuMapDto ToMapDto(this Ibu ibu)
       => new(
           ibu.PubId,
           ibu.BusinessUnitName,
           IbuMapMarkerName,
           IbuMapMarkerColor,
           IbuMapMarkerIconUrl,
           ibu.Coordinate?.Latitude,
           ibu.Coordinate?.Longitude,
           [.. ibu.Fields.Select(f => f.ToMapDto())]
           );

    public static Ibu ToEntity(this IbuCreateDto createDto, int? fieldTypeId, int jurisdictionId)
        => new()
        {
            BusinessUnitName = createDto.BusinessUnitName,
            WorkspaceKind = createDto.WorkspaceKind,
            FieldTypeId = fieldTypeId,
            JurisdictionId = jurisdictionId,
            RegistrationDate = createDto.RegistrationDate
        };

    public static IbuCreateDto ToCreateDto(this IbuDto dto, FieldType fieldType)
        => new(
            dto.BusinessUnitName,
            dto.WorkspaceKind,
            dto.Address?.PubId,
            dto.Contact?.PubId,
            dto.Coordinate?.PubId,
            dto.Jurisdiction?.PubId,
            dto.RegistrationDate,
            fieldType.PubId,
            dto.VisualizationTempUnitPubId,
            dto.VisualizationRainUnitPubId,
            dto.VisualizationWindUnitPubId
            );

    public static IbuUpdateDto ToUpdateDto(this IbuDto dto)
       => new(
           dto.PubId,
           dto.BusinessUnitName,
           dto.Status,
           dto.WorkspaceKind,
           dto.Address?.PubId,
           dto.Contact?.PubId,
           dto.Coordinate?.PubId,
           dto.Jurisdiction?.PubId,
           dto.ImagePubId,
           dto.RegistrationDate,
           dto.FieldTypePubId,
           dto.PrimaryFieldPubId,
           dto.PrimaryFieldName,
           dto.VisualizationTempUnitPubId,
           dto.VisualizationRainUnitPubId,
           dto.VisualizationWindUnitPubId
           );

    public static void UpdateEntity(
        this Ibu entity,
        IbuUpdateDto updateDto,
        int jurisId,
        int? fieldTypeId,
        int? visualizationTempUnitId,
        int? visualizationRainUnitId,
        int? visualizationWindUnitId)
    {
        entity.BusinessUnitName = updateDto.BusinessUnitName;
        if (updateDto.Status != 0)
            entity.Status = updateDto.Status;
        if (updateDto.WorkspaceKind != 0)
            entity.WorkspaceKind = updateDto.WorkspaceKind;
        entity.FieldTypeId = fieldTypeId;
        entity.UpdatedDate = updateDto.UpdatedDate;
        entity.JurisdictionId = jurisId;
        entity.VisualizationTempUnitId = visualizationTempUnitId;
        entity.VisualizationRainUnitId = visualizationRainUnitId;
        entity.VisualizationWindUnitId = visualizationWindUnitId;
        entity.UpdatedDate = DateTime.UtcNow;
    }
}
