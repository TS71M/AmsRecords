using static AmsRecords.ContactDetails.ContactDetailDtos;

namespace AmsRecords.ContactDetails;

public static class ContactDetailsExtensions
{
    const string ProtectedValuePrefix = "enc::v1::";

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

    public static string? ReadableOrNull(string? value)
        => !string.IsNullOrWhiteSpace(value)
           && value.StartsWith(ProtectedValuePrefix, StringComparison.Ordinal)
            ? null
            : value;
}
