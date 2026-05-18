using AmsRecords.Coordinates;
using AmsRecords.Fields;
using static AmsRecords.Addresses.AddressDtos;
using static AmsRecords.AppImages.AppImageDtos;
using static AmsRecords.ContactDetails.ContactDetailDtos;
using static AmsRecords.Jurisdictions.JurisdictionDtos;

namespace AmsRecords.Ibus;

public static class IbuDtos
{
    public record IbuDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("businessUnitName")] string BusinessUnitName,
        [property: JsonPropertyName("status")] IbuStatus Status,
        [property: JsonPropertyName("workspaceKind")] WorkspaceKinds.WorkspaceKind WorkspaceKind,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("address")] AddressDto? Address,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("coordinate")] CoordinateDto? Coordinate,
        [property: JsonPropertyName("jurisdiction")] JurisdictionDto? Jurisdiction,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("registrationDate")] DateTime RegistrationDate,
        [property: JsonPropertyName("updatedDate")] DateTime? UpdatedDate,
        [property: JsonPropertyName("fieldTypePubId")] Guid? FieldTypePubId,
        [property: JsonPropertyName("primaryFieldPubId")] Guid? PrimaryFieldPubId,
        [property: JsonPropertyName("primaryFieldName")] string? PrimaryFieldName,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId,
        [property: JsonPropertyName("visualizationTempUnitShort")] string? VisualizationTempUnitShort,
        [property: JsonPropertyName("visualizationRainUnitShort")] string? VisualizationRainUnitShort,
        [property: JsonPropertyName("visualizationWindUnitShort")] string? VisualizationWindUnitShort,
        [property: JsonPropertyName("fieldCount")] int FieldsCount
        )
    {
        public string? ImageDataUrl { get; set; }
    }

    public record IbuCreateDto(
        [property: JsonPropertyName("businessUnitName")] string BusinessUnitName,
        [property: JsonPropertyName("workspaceKind")] WorkspaceKinds.WorkspaceKind WorkspaceKind,
        [property: JsonPropertyName("addressPublicId")] Guid? AddressPubId,
        [property: JsonPropertyName("contactPublicId")] Guid? ContactPubId,
        [property: JsonPropertyName("coordinatePublicId")] Guid? CoordinatePubId,
        [property: JsonPropertyName("jurisdictionId")] Guid? JurisdictionId,
        [property: JsonPropertyName("registrationDate")] DateTime RegistrationDate,
        [property: JsonPropertyName("fieldTypePubId")] Guid? FieldTypePubId,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId
        )
    {
        public AppImageCreateDto AppImageCreateDto { get; set; } = new();
    };

    public record IbuUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId = default,
        [property: JsonPropertyName("businessUnitName")] string BusinessUnitName = "",
        [property: JsonPropertyName("status")] IbuStatus Status = 0,
        [property: JsonPropertyName("workspaceKind")] WorkspaceKinds.WorkspaceKind WorkspaceKind = 0,
        [property: JsonPropertyName("addressPublicId")] Guid? AddressPubId = default,
        [property: JsonPropertyName("contactPublicId")] Guid? ContactPubId = default,
        [property: JsonPropertyName("coordinatePublicId")] Guid? CoordinatePubId = default,
        [property: JsonPropertyName("jurisdictionId")] Guid? JurisdictionPubId = default,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = default,
        [property: JsonPropertyName("registrationDate")] DateTime? UpdatedDate = default,
        [property: JsonPropertyName("fieldTypePubId")] Guid? FieldTypePubId = default,
        [property: JsonPropertyName("primaryFieldPubId")] Guid? PrimaryFieldPubId = default,
        [property: JsonPropertyName("primaryFieldName")] string? PrimaryFieldName = default,
        [property: JsonPropertyName("visualizationTempUnitPubId")] Guid? VisualizationTempUnitPubId = default,
        [property: JsonPropertyName("visualizationRainUnitPubId")] Guid? VisualizationRainUnitPubId = default,
        [property: JsonPropertyName("visualizationWindUnitPubId")] Guid? VisualizationWindUnitPubId = default
        )
    {
        public string? ImageDataUrl { get; set; }
        public AppImageCreateDto AppImageCreateDto { get; set; } = new();
    };

    public record IbuMapDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("FieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("markerColor")] string? MarkerColor,
        [property: JsonPropertyName("markerIconUrl")] string? MarkerIconUrl,
        [property: JsonPropertyName("ibuLat")] decimal? IbuLat,
        [property: JsonPropertyName("ibuLng")] decimal? IbuLng,
        [property: JsonPropertyName("fields")] List<FieldMapDto> Fields
        );

    public record IbuClaimRequestDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("businessUnitName")] string BusinessUnitName,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("requestedUserPubId")] Guid? RequestedUserPubId,
        [property: JsonPropertyName("status")] IbuClaimRequestStatus Status,
        [property: JsonPropertyName("requestedAtUtc")] DateTime RequestedAtUtc,
        [property: JsonPropertyName("decidedAtUtc")] DateTime? DecidedAtUtc,
        [property: JsonPropertyName("decisionNotes")] string DecisionNotes
    );

    public record IbuClaimDecisionDto(
        [property: JsonPropertyName("claimRequestPubId")] Guid ClaimRequestPubId,
        [property: JsonPropertyName("decisionNotes")] string? DecisionNotes
    );
}
