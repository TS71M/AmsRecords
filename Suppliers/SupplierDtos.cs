using System.ComponentModel;
using Lib.Constants;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Suppliers;

public static class SupplierDtos
{
    public record SupplierMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name
    );

    public record SupplierListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("fieldName")] string? FieldName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("isFieldScoped")] bool IsFieldScoped,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("primaryContactName")] string? PrimaryContactName,
        [property: JsonPropertyName("primaryContactPosition")] string? PrimaryContactPosition,
        [property: JsonPropertyName("primaryPhone")] string? PrimaryPhone,
        [property: JsonPropertyName("primaryEmail")] string? PrimaryEmail,
        [property: JsonPropertyName("primaryWebsite")] string? PrimaryWebsite,
        [property: JsonPropertyName("primaryAddress")] string? PrimaryAddress,
        [property: JsonPropertyName("contactCount")] int ContactCount,
        [property: JsonPropertyName("addressCount")] int AddressCount
    );

    public record SupplierFormDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("fieldName")] string? FieldName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("appImagePubId")] Guid? AppImagePubId,
        [property: JsonPropertyName("addressPubId")] Guid? AddressPubId,
        [property: JsonPropertyName("contactPubId")] Guid? ContactPubId,
        [property: JsonPropertyName("primaryPhone")] string? PrimaryPhone,
        [property: JsonPropertyName("primaryEmail")] string? PrimaryEmail,
        [property: JsonPropertyName("primaryWebsite")] string? PrimaryWebsite,
        [property: JsonPropertyName("address1")] string? Address1,
        [property: JsonPropertyName("address2")] string? Address2,
        [property: JsonPropertyName("state")] string? State,
        [property: JsonPropertyName("town")] string? Town,
        [property: JsonPropertyName("zip")] string? Zip,
        [property: JsonPropertyName("countryPubId")] Guid? CountryPubId,
        [property: JsonPropertyName("countryName")] string? CountryName
    );

    public sealed record SupplierCreateDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")]
        [property: DisplayName("Supplier Name")]
        [param: Required]
        [param: MaxLength(250)]
        string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("primaryPhone")][param: Phone][param: MaxLength(60)] string? PrimaryPhone,
        [property: JsonPropertyName("primaryEmail")][param: EmailAddress][param: MaxLength(60)] string? PrimaryEmail,
        [property: JsonPropertyName("primaryWebsite")][param: Url][param: MaxLength(200)] string? PrimaryWebsite,
        [property: JsonPropertyName("address1")][param: MaxLength(250)] string? Address1,
        [property: JsonPropertyName("address2")][param: MaxLength(250)] string? Address2,
        [property: JsonPropertyName("state")][param: MaxLength(100)] string? State,
        [property: JsonPropertyName("town")][param: MaxLength(60)] string? Town,
        [property: JsonPropertyName("zip")][param: MaxLength(60)] string? Zip,
        [property: JsonPropertyName("countryPubId")] Guid? CountryPubId)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Suppliers);
    }

    public sealed record SupplierUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId = default,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId = default,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId = null,
        [property: JsonPropertyName("name")]
        [property: DisplayName("Supplier Name")]
        [param: Required]
        [param: MaxLength(250)]
        string Name = "",
        [property: JsonPropertyName("active")] bool Active = true,
        [property: JsonPropertyName("isVisible")] bool IsVisible = true,
        [property: JsonPropertyName("addressPubId")] Guid? AddressPubId = null,
        [property: JsonPropertyName("contactPubId")] Guid? ContactPubId = null,
        [property: JsonPropertyName("appImagePubId")] Guid? AppImagePubId = null,
        [property: JsonPropertyName("primaryPhone")][param: Phone][param: MaxLength(60)] string? PrimaryPhone = null,
        [property: JsonPropertyName("primaryEmail")][param: EmailAddress][param: MaxLength(60)] string? PrimaryEmail = null,
        [property: JsonPropertyName("primaryWebsite")][param: Url][param: MaxLength(200)] string? PrimaryWebsite = null,
        [property: JsonPropertyName("address1")][param: MaxLength(250)] string? Address1 = null,
        [property: JsonPropertyName("address2")][param: MaxLength(250)] string? Address2 = null,
        [property: JsonPropertyName("state")][param: MaxLength(100)] string? State = null,
        [property: JsonPropertyName("town")][param: MaxLength(60)] string? Town = null,
        [property: JsonPropertyName("zip")][param: MaxLength(60)] string? Zip = null,
        [property: JsonPropertyName("countryPubId")] Guid? CountryPubId = null,
        [property: JsonPropertyName("countryName")] string? CountryName = null)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Suppliers);
        public string? ImageDataUrl { get; set; }
    }

    public sealed record SupplierWebsiteImportRequestDto(
        [property: JsonPropertyName("websiteUrl")][param: Required][param: MaxLength(500)] string WebsiteUrl,
        [property: JsonPropertyName("ibuPubId")][Required] Guid IbuPubId
    );

    public sealed record SupplierWebsiteImportPreviewDto(
        [property: JsonPropertyName("websiteUrl")] string WebsiteUrl,
        [property: JsonPropertyName("supplierName")] string? SupplierName,
        [property: JsonPropertyName("primaryPhone")] string? PrimaryPhone,
        [property: JsonPropertyName("primaryEmail")] string? PrimaryEmail,
        [property: JsonPropertyName("primaryWebsite")] string? PrimaryWebsite,
        [property: JsonPropertyName("address1")] string? Address1,
        [property: JsonPropertyName("address2")] string? Address2,
        [property: JsonPropertyName("state")] string? State,
        [property: JsonPropertyName("town")] string? Town,
        [property: JsonPropertyName("zip")] string? Zip,
        [property: JsonPropertyName("countryPubId")] Guid? CountryPubId,
        [property: JsonPropertyName("countryName")] string? CountryName,
        [property: JsonPropertyName("suggestedContacts")] List<SupplierWebsiteImportContactDto> SuggestedContacts,
        [property: JsonPropertyName("sourceUrls")] List<string> SourceUrls,
        [property: JsonPropertyName("notes")] List<string> Notes
    );

    public sealed record SupplierWebsiteImportContactDto(
        [property: JsonPropertyName("fullName")] string FullName,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("middleName")] string? MiddleName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("position")] string? Position,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("website")] string? Website,
        [property: JsonPropertyName("imageUrl")] string? ImageUrl,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl
    );

    public sealed record SupplierContactListDto(
        [property: JsonPropertyName("supplierContactId")] int SupplierContactId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("fullName")] string? FullName,
        [property: JsonPropertyName("position")] string? Position,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("website")] string? Website,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("isActive")] bool IsActive
    );

    public sealed record SupplierContactFormDto(
        [property: JsonPropertyName("supplierContactId")] int SupplierContactId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("namePubId")] Guid? NamePubId,
        [property: JsonPropertyName("contactPubId")] Guid? ContactPubId,
        [property: JsonPropertyName("appImagePubId")] Guid? AppImagePubId,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("middleName")] string? MiddleName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("position")] string? Position,
        [property: JsonPropertyName("phone")] string? Phone,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("website")] string? Website,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("isActive")] bool IsActive
    );

    public sealed record SupplierContactUpsertDto(
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId = default,
        [property: JsonPropertyName("namePubId")] Guid? NamePubId = null,
        [property: JsonPropertyName("contactPubId")] Guid? ContactPubId = null,
        [property: JsonPropertyName("appImagePubId")] Guid? AppImagePubId = null,
        [property: JsonPropertyName("firstName")][param: Required][param: MaxLength(250)] string FirstName = "",
        [property: JsonPropertyName("middleName")][param: MaxLength(250)] string? MiddleName = null,
        [property: JsonPropertyName("lastName")][param: Required][param: MaxLength(250)] string LastName = "",
        [property: JsonPropertyName("position")][param: MaxLength(100)] string? Position = null,
        [property: JsonPropertyName("phone")][param: Phone][param: MaxLength(60)] string? Phone = null,
        [property: JsonPropertyName("email")][param: EmailAddress][param: MaxLength(60)] string? Email = null,
        [property: JsonPropertyName("website")][param: Url][param: MaxLength(200)] string? Website = null,
        [property: JsonPropertyName("imageSourceUrl")][param: Url][param: MaxLength(500)] string? ImageSourceUrl = null,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary = false,
        [property: JsonPropertyName("isActive")] bool IsActive = true)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Suppliers);
        public string? ImageDataUrl { get; set; }
    }

    public sealed record SupplierAddressListDto(
        [property: JsonPropertyName("supplierAddressId")] int SupplierAddressId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("addressType")] string AddressType,
        [property: JsonPropertyName("primaryAddress")] string? PrimaryAddress,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("isActive")] bool IsActive
    );

    public sealed record SupplierAddressFormDto(
        [property: JsonPropertyName("supplierAddressId")] int SupplierAddressId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("addressPubId")] Guid? AddressPubId,
        [property: JsonPropertyName("addressType")] string AddressType,
        [property: JsonPropertyName("address1")] string? Address1,
        [property: JsonPropertyName("address2")] string? Address2,
        [property: JsonPropertyName("state")] string? State,
        [property: JsonPropertyName("town")] string? Town,
        [property: JsonPropertyName("zip")] string? Zip,
        [property: JsonPropertyName("countryPubId")] Guid? CountryPubId,
        [property: JsonPropertyName("countryName")] string? CountryName,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("isActive")] bool IsActive
    );

    public sealed record SupplierAddressUpsertDto(
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId = default,
        [property: JsonPropertyName("addressPubId")] Guid? AddressPubId = null,
        [property: JsonPropertyName("addressType")][param: Required][param: MaxLength(50)] string AddressType = "Main",
        [property: JsonPropertyName("address1")][param: Required][param: MaxLength(250)] string Address1 = "",
        [property: JsonPropertyName("address2")][param: MaxLength(250)] string? Address2 = null,
        [property: JsonPropertyName("state")][param: MaxLength(100)] string? State = null,
        [property: JsonPropertyName("town")][param: Required][param: MaxLength(60)] string Town = "",
        [property: JsonPropertyName("zip")][param: Required][param: MaxLength(60)] string Zip = "",
        [property: JsonPropertyName("countryPubId")][param: Required] Guid? CountryPubId = null,
        [property: JsonPropertyName("countryName")] string? CountryName = null,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary = false,
        [property: JsonPropertyName("isActive")] bool IsActive = true
    );
}
