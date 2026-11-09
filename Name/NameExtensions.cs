using static AmsRecords.Name.NameDtos;

namespace AmsRecords.Name;

public static class NameExtensions
{

    public static NameUpdateDto ToUpdateDto(this NameDto dto)
        => new(
            PubId: dto.PubId,
            FirstName: dto.FirstName,
            MiddleName: dto.MiddleName,
            LastName: dto.LastName,
            IbuPubId: dto.IbuPubId
        );

    public static NameCreateDto ToCreateDto(this NameUpdateDto updateDto, ParentEntityType ownerType, Guid ownerPubId)
       => new(
           FirstName: updateDto.FirstName,
           MiddleName: updateDto.MiddleName,
           LastName: updateDto.LastName,
           IbuPubId: updateDto.IbuPubId,
           OwnerType: ownerType,
           OwnerPubId: ownerPubId
       );
}