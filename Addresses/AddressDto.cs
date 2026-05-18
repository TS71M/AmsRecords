using AmsRecords.Countries;

namespace AmsRecords.Addresses;

public static class AddressDtos
{
    public record AddressDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("address1")] string Address1,
        [property: JsonPropertyName("address2")] string? Address2,
        [property: JsonPropertyName("state")] string? State,
        [property: JsonPropertyName("town")] string Town,
        [property: JsonPropertyName("zip")] string Zip,
        [property: JsonPropertyName("countryId")] Guid CountryPubId,
        [property: JsonPropertyName("country")] string Country
    );

    public record AddressCreateDto(
        [property: JsonPropertyName("address1")][Required][MaxLength(250)] string Address1,
        [property: JsonPropertyName("address2")][MaxLength(250)] string? Address2,
        [property: JsonPropertyName("state")][MaxLength(100)] string? State,
        [property: JsonPropertyName("town")][Required][MaxLength(60)] string Town,
        [property: JsonPropertyName("zip")][Required][MaxLength(60)] string Zip,
        [property: JsonPropertyName("countryId")][Required] Guid CountryPubId,
        [property: JsonPropertyName("addressOwnerType")][Required] ParentEntityType OwnerType,
        [property: JsonPropertyName("ownerPubId")][Required] Guid OwnerPubId
        )
    {
        public AddressCreateDto() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Guid.Empty, ParentEntityType.Ibu, Guid.Empty) { }

        public List<CountryDto> Countries { get; set; } = [];
    }

    public record AddressUpdateDto(
        [property: JsonPropertyName("pubId")][Required] Guid PubId,
        [property: JsonPropertyName("address1")][Required][MaxLength(250)] string Address1,
        [property: JsonPropertyName("address2")][MaxLength(250)] string? Address2,
        [property: JsonPropertyName("state")][MaxLength(100)] string? State,
        [property: JsonPropertyName("town")][Required][MaxLength(60)] string Town,
        [property: JsonPropertyName("zip")][Required][MaxLength(60)] string Zip,
        [property: JsonPropertyName("countryId")][Required] Guid CountryPubId
    )
    {
        public AddressUpdateDto() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Guid.Empty) { }

        [JsonPropertyName("addressOwnerType")]
        public ParentEntityType? OwnerType { get; set; }

        [JsonPropertyName("ownerPubId")]
        public Guid? OwnerPubId { get; set; }

        public List<CountryDto> Countries { get; set; } = [];
    }
}
