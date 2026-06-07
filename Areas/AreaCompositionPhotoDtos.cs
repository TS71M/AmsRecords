namespace AmsRecords.Areas;

public static class AreaCompositionPhotoDtos
{
    public sealed record AreaCompositionPhotoDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("updatedAtUtc")] DateTime UpdatedAtUtc,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("relativePath")] string? RelativePath
    )
    {
        [JsonPropertyName("captureMetadata")]
        public AmsRecords.AppImages.AppImageDtos.AppImageCaptureMetadataDto? CaptureMetadata { get; init; }
    }

    public sealed record AreaCompositionPhotoCreateDto(
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId
    );
}
