using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Holes;

public static class HoleDtos
{
    public sealed record HoleDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("holDes")] string HolDes,
        [property: JsonPropertyName("holShoNam")] string HolShoNam,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("imagePath")] string? ImagePath
    )
    {
        public string? ImageThumbDataUrl { get; set; }
    };

    public sealed record HoleMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("holShoNam")] string HolShoNam,
        [property: JsonPropertyName("holDes")] string HolDes
    );

    public sealed record HoleCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("holDes")] string HolDes,
        [property: JsonPropertyName("holShoNam")] string HolShoNam,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("image")] AppImageCreateDto? AppImageCreateDto

    );

    public sealed record HoleUpdateDto(
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("holDes")] string HolDes,
        [property: JsonPropertyName("holShoNam")] string HolShoNam,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("image")] AppImageCreateDto? AppImageCreateDto
    );
}