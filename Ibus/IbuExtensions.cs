using AmsRecords.Addresses;
using AmsRecords.ContactDetails;
using AmsRecords.Coordinates;
using AmsRecords.Fields;
using static AmsRecords.Ibus.IbuDtos;

namespace AmsRecords.Ibus;

public static class IbuExtensions
{
    internal const string IbuMapMarkerName = "Headquarter";
    internal const string IbuMapMarkerColor = "#1f6feb";
    internal const string IbuMapMarkerIconUrl = "/icons/field-office.svg";

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
}
