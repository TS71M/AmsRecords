using static AmsRecords.ContactDetails.ContactDetailDtos;

namespace AmsRecords.ContactDetails;

public static class ContactDetailsExtensions
{
    const string ProtectedValuePrefix = "enc::v1::";

    public static ContactDetailDto ToDto(this ContactDetail contact)
        => new(
            contact.PubId,
            ReadableOrNull(contact.Phone),
            ReadableOrNull(contact.Email),
            contact.Website,
            contact.Ibu.PubId
        );

    public static ContactDetail ToEntity(this ContactDetailCreateDto dto, Ibu ibu)
        => new()
        {
            Phone = dto.Phone,
            Email = dto.Email,
            Website = dto.Website,
            Ibu = ibu
        };

    public static void UpdateFromDto(this ContactDetail entity, ContactDetailUpdateDto dto)
    {
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Website = dto.Website;
    }

    public static ContactDetailUpdateDto ToUpdateDto(this ContactDetailDto dto)
        => new(
            PubId: dto.PubId,
            Phone: dto.Phone,
            Email: dto.Email,
            Website: dto.Website,
            IbuPubId: dto.IbuPubId
            );

    public static ContactDetailCreateDto ToCreateDto(this ContactDetailUpdateDto dto, ParentEntityType ownerType, Guid ownerPubId)
        => new(
            Phone: dto.Phone,
            Email: dto.Email,
            Website: dto.Website,
            OwnerType: ownerType,
            OwnerPubId: ownerPubId
            );

    static string? ReadableOrNull(string? value)
        => !string.IsNullOrWhiteSpace(value)
           && value.StartsWith(ProtectedValuePrefix, StringComparison.Ordinal)
            ? null
            : value;
}
