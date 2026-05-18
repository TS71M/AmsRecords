using static AmsRecords.Addresses.AddressDtos;

namespace AmsRecords.Addresses;

public static class AddressExtensions
{
    public static AddressDto ToDto(this Address address)
        => new(
            PubId: address.PubId,
            Address1: address.Address1,
            Address2: address.Address2,
            State: address.State,
            Town: address.Town,
            Zip: address.Zip,
            CountryPubId: address.Country!.PubId,
            Country: address.Country.CountryName
        );

    public static Address ToEntity(this AddressCreateDto dto, int ibuId, Guid countryPubId, Ibu ibu, Country country)
        => new()
        {
            IbuId = ibuId,
            CountryId = country.CountryId,
            Ibu = ibu,
            Country = country,
            Address1 = dto.Address1,
            Address2 = dto.Address2,
            Town = dto.Town,
            State = dto.State,
            Zip = dto.Zip
        };

    public static void UpdateEntity(this Address entity, AddressUpdateDto dto, Country country)
    {
        entity.Address1 = dto.Address1;
        entity.Address2 = dto.Address2;
        entity.State = dto.State;
        entity.Town = dto.Town;
        entity.Zip = dto.Zip;
        entity.CountryId = country.CountryId;
        entity.Country = country;
    }

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

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address.Address1))
            parts.Add(address.Address1);

        if (!string.IsNullOrWhiteSpace(address.Address2))
            parts.Add(address.Address2);

        var cityLineParts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address.Zip))
            cityLineParts.Add(address.Zip);

        if (!string.IsNullOrWhiteSpace(address.Town))
            cityLineParts.Add(address.Town);

        if (cityLineParts.Count > 0)
            parts.Add(string.Join(" ", cityLineParts));

        if (!string.IsNullOrWhiteSpace(address.State))
            parts.Add(address.State);

        if (!string.IsNullOrWhiteSpace(address.Country))
            parts.Add(address.Country);

        return parts.Count == 0
            ? null
            : string.Join(", ", parts);
    }
}