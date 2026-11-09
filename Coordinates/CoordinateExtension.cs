namespace AmsRecords.Coordinates;

using AmsModels;

public static class CoordinateExtension
{


    public static CoordinateCreateDto ToCreateDto(this CoordinateUpdateDto dto, ParentEntityType addressOwnerType, Guid ownerId)
        => new(
            Latitude: dto.Latitude,
            Longitude: dto.Longitude,
            OwnerType: addressOwnerType,
            OwnerPubId: ownerId
            );

}