using static AmsRecords.Name.NameDtos;

namespace AmsRecords.Name;

public static class NameExtensions
{
    public static NameDto ToDto(this AmsModels.Name name)
        => new(
            PubId: name.PubId,
            FirstName: name.FirstName,
            MiddleName: name.MiddleName,
            LastName: name.LastName,
            IbuPubId: name.Ibu.PubId
        );

    public static AmsModels.Name ToEntity(this NameCreateDto dto, Ibu ibu)
        => new()
        {
            IbuId = ibu.IbuId,
            Ibu = ibu,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName
        };

    public static void UpdateEntity(this AmsModels.Name entity, NameUpdateDto dto, Ibu ibu)
    {
        entity.FirstName = dto.FirstName;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Ibu = ibu;
        entity.IbuId = ibu.IbuId;
    }

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