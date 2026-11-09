using AmsRecords.Addresses;
using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.Architects.ArchitectDtos;

namespace AmsRecords.Architects;

public static class ArchitectExtensions
{

    public static ArchitectUpdateDto ToUpdateDto(this ArchitectDto dto)
        => new(
            PubId: dto.PubId,
            FieldPubId: dto.FieldPubId ?? Guid.Empty,
            ImagePubId: dto.ImagePubId
        )
        {
            ImageDataUrl = dto.ImageDataUrl
        };
}
