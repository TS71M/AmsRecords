using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.DirectorOfGolfs.DirectorOfGolfDtos;

namespace AmsRecords.DirectorOfGolfs;

public static class DirectorOfGolfExtensions
{
    public static DirectorOfGolfDto ToDto(this DirectorOfGolf entity)
        => new(
            PubId: entity.PubId,
            FieldPubId: entity.Fields.FirstOrDefault()?.PubId,
            IbuPubId: entity.Ibu.PubId,
            Name: entity.Name?.ToDto(),
            Contact: entity.Contact?.ToDto(),
            ImagePubId: entity.AppImage?.PubId
        );

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
