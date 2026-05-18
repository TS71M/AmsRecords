using static AmsRecords.AppImages.AppImageDtos;
using static AmsRecords.ContactDetails.ContactDetailDtos;
using static AmsRecords.Name.NameDtos;

namespace AmsRecords.DirectorOfGolfs;

public static class DirectorOfGolfDtos
{
    public sealed record DirectorOfGolfDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("name")] NameDto? Name,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId
    )
    {
        public string? ImageDataUrl { get; set; }
    }

    public sealed record DirectorOfGolfCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId
    )
    {
        public AppImageCreateDto AppImageCreateDto { get; set; } = new(null, ImageCategories.Users);
    }

    public sealed record DirectorOfGolfUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null
    )
    {
        public DirectorOfGolfUpdateDto() : this(Guid.Empty, Guid.Empty, null) { }

        public string? ImageDataUrl { get; set; }
        public AppImageCreateDto AppImageCreateDto { get; set; } = new(null, ImageCategories.Users);
    }
}
