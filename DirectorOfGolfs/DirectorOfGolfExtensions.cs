using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.DirectorOfGolfs.DirectorOfGolfDtos;

namespace AmsRecords.DirectorOfGolfs;

public static class DirectorOfGolfExtensions
{

    public static DirectorOfGolfUpdateDto ToUpdateDto(this DirectorOfGolfDto dto)
        => new(
            PubId: dto.PubId,
            FieldPubId: dto.FieldPubId ?? Guid.Empty,
            ImagePubId: dto.ImagePubId
        )
        {
            ImageDataUrl = dto.ImageDataUrl
        };
}
