using static AmsRecords.Addresses.AddressDtos;
using static AmsRecords.AppImages.AppImageDtos;
using static AmsRecords.ContactDetails.ContactDetailDtos;
using static AmsRecords.Name.NameDtos;

namespace AmsRecords.Architects;

public static class ArchitectDtos
{
    public sealed record ArchitectDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] NameDto? Name,
        [property: JsonPropertyName("address")] AddressDto? Address,
        [property: JsonPropertyName("contact")] ContactDetailDto? Contact,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId
    )
    {
        public string? ImageDataUrl { get; set; }
    }

    public sealed record ArchitectCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId
    )
    {
        public AppImageCreateDto AppImageCreateDto { get; set; } = new(null, ImageCategories.Users);
    }

    public sealed record ArchitectUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null
    )
    {
        public ArchitectUpdateDto() : this(Guid.Empty, Guid.Empty, null) { }

        public string? ImageDataUrl { get; set; }
        public AppImageCreateDto AppImageCreateDto { get; set; } = new(null, ImageCategories.Users);
    }
}
