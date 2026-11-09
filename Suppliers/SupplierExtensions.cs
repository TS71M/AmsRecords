using static AmsRecords.Suppliers.SupplierDtos;
using AmsRecords.ContactDetails;

namespace AmsRecords.Suppliers;

public static class SupplierExtensions
{

    public static SupplierUpdateDto ToUpdateDto(this SupplierFormDto dto)
        => new(
            PubId: dto.PubId,
            IbuPubId: dto.IbuPubId,
            FieldPubId: dto.FieldPubId,
            Name: dto.Name,
            Active: dto.Active,
            IsVisible: dto.IsVisible,
            AddressPubId: dto.AddressPubId,
            ContactPubId: dto.ContactPubId,
            AppImagePubId: dto.AppImagePubId,
            PrimaryPhone: dto.PrimaryPhone,
            PrimaryEmail: dto.PrimaryEmail,
            PrimaryWebsite: dto.PrimaryWebsite,
            Address1: dto.Address1,
            Address2: dto.Address2,
            State: dto.State,
            Town: dto.Town,
            Zip: dto.Zip,
            CountryPubId: dto.CountryPubId,
            CountryName: dto.CountryName)
        {
            ImageDataUrl = dto.Image
        };

    public static SupplierCreateDto ToCreateDto(this SupplierUpdateDto dto)
        => new(
            IbuPubId: dto.IbuPubId,
            FieldPubId: dto.FieldPubId,
            Name: dto.Name,
            Active: dto.Active,
            IsVisible: dto.IsVisible,
            PrimaryPhone: dto.PrimaryPhone,
            PrimaryEmail: dto.PrimaryEmail,
            PrimaryWebsite: dto.PrimaryWebsite,
            Address1: dto.Address1,
            Address2: dto.Address2,
            State: dto.State,
            Town: dto.Town,
            Zip: dto.Zip,
            CountryPubId: dto.CountryPubId)
        {
            ImageCreateDto = dto.ImageCreateDto
        };

    public static SupplierContactUpsertDto ToUpsertDto(this SupplierContactFormDto dto)
        => new(
            SupplierPubId: dto.SupplierPubId,
            NamePubId: dto.NamePubId,
            ContactPubId: dto.ContactPubId,
            AppImagePubId: dto.AppImagePubId,
            FirstName: dto.FirstName,
            MiddleName: dto.MiddleName,
            LastName: dto.LastName,
            Position: dto.Position,
            Phone: dto.Phone,
            Email: dto.Email,
            Website: dto.Website,
            IsPrimary: dto.IsPrimary,
            IsActive: dto.IsActive)
        {
            ImageDataUrl = dto.Image
        };

    public static SupplierAddressUpsertDto ToUpsertDto(this SupplierAddressFormDto dto)
        => new(
            SupplierPubId: dto.SupplierPubId,
            AddressPubId: dto.AddressPubId,
            AddressType: dto.AddressType,
            Address1: dto.Address1 ?? "",
            Address2: dto.Address2,
            State: dto.State,
            Town: dto.Town ?? "",
            Zip: dto.Zip ?? "",
            CountryPubId: dto.CountryPubId,
            CountryName: dto.CountryName,
            IsPrimary: dto.IsPrimary,
            IsActive: dto.IsActive
        );

    public sealed record AddressDtoLike(
        string? Address1,
        string? Address2,
        string? State,
        string? Town,
        string? Zip,
        string? CountryName);
}
