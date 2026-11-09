using static AmsRecords.Addresses.AddressDtos;

namespace AmsRecords.Addresses;

public static class AddressExtensions
{

    public static AddressCreateDto ToCreateDto(this AddressUpdateDto dto, ParentEntityType addressOwnerType, Guid ownerId)
        => new(
            Address1: dto.Address1,
            Address2: dto.Address2,
            State: dto.State,
            Town: dto.Town,
            Zip: dto.Zip,
            CountryPubId: dto.CountryPubId,
            OwnerType: addressOwnerType,
            OwnerPubId: ownerId
            )
        {
            Countries = dto.Countries
        };

    public static AddressUpdateDto ToUpdateDto(this AddressDto dto)
        => new(
            PubId: dto.PubId,
            Address1: dto.Address1,
            Address2: dto.Address2,
            State: dto.State,
            Town: dto.Town,
            Zip: dto.Zip,
            CountryPubId: dto.CountryPubId
        );


    public static string? ToSingleLine(this AddressDto? address)
    {
        if (address is null)
            return null;

        return FormatSingleLine(
            address.Address1,
            address.Address2,
            address.State,
            address.Town,
            address.Zip,
            address.Country);
    }

    public static string? FormatSingleLine(
        string? address1,
        string? address2,
        string? state,
        string? town,
        string? zip,
        string? country)
    {

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address1))
            parts.Add(address1.Trim());

        if (!string.IsNullOrWhiteSpace(address2))
            parts.Add(address2.Trim());

        var cityLineParts = new List<string>();

        if (!string.IsNullOrWhiteSpace(zip))
            cityLineParts.Add(zip.Trim());

        if (!string.IsNullOrWhiteSpace(town))
            cityLineParts.Add(town.Trim());

        if (cityLineParts.Count > 0)
            parts.Add(string.Join(" ", cityLineParts));

        if (!string.IsNullOrWhiteSpace(state))
            parts.Add(state.Trim());

        if (!string.IsNullOrWhiteSpace(country))
            parts.Add(country.Trim());

        return parts.Count == 0
            ? null
            : string.Join(", ", parts);
    }
}
