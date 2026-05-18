namespace AmsRecords.Surfaces;

public static class SurfaceDtos
{
    public sealed record SurfaceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("surfaceSize")] decimal SurfaceSizeM2,
        [property: JsonPropertyName("holeImgPubId")] Guid? HoleImgPubId
    )
    {
        public string? ImageThumbDataUrl { get; set; }
    }

    public sealed record SurfaceMiniDto(
       [property: JsonPropertyName("pubId")] Guid PubId,
       [property: JsonPropertyName("surfaceName")] string SurfaceName,
       [property: JsonPropertyName("holePubId")] Guid HolePubId,
       [property: JsonPropertyName("holeName")] string HoleName,
       [property: JsonPropertyName("holeNumber")] int HoleNumber,
       [property: JsonPropertyName("surfaceSize")] decimal SurfaceSize
    );

    public sealed record SurfaceCreateDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("surfaceSize")] decimal SurfaceSizeM2
    );

    public sealed record SurfaceUpdateDto(
        [property: JsonPropertyName("surfaceSize")] decimal SurfaceSizeM2
    );

    public sealed record SurfacesIndexDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("fieldImagePubId")] Guid? FieldImagePubId,
        [property: JsonPropertyName("areasCount")] int AreasCount,
        [property: JsonPropertyName("holesCount")] int HolesCount,
        [property: JsonPropertyName("holeCards")] List<HoleSurfaceCardDto> HoleCards
    )
    {
        public string? ImageThumbDataUrl { get; set; }
    }

    public sealed record HoleSurfaceCardDto(
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("holShoNam")] string HolShoNam,
        [property: JsonPropertyName("holeName")] string HoleName,
        [property: JsonPropertyName("holeImgPubId")] Guid? HoleImgPubId,
        [property: JsonPropertyName("surfaces")] List<SurfaceIndexDto> Surfaces
    )
    {
        public string? ImageThumbDataUrl { get; set; } // optional if you still embed thumbs
    }

    // Useful for UI lists like: "Green Hole 1"
    public sealed record SurfaceIndexDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("surfaceSize")] decimal SurfaceSizeM2,
        [property: JsonPropertyName("holeImgPubId")] Guid? HoleImgPubId
    )
    {
        public string? ImageThumbDataUrl { get; set; }
    }
}
