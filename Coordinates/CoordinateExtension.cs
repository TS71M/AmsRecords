namespace AmsRecords.Coordinates;

using AmsModels;

public static class CoordinateExtension
{
    public static CoordinateDto ToDto(this Coordinate coord)
        => new(coord.PubId, coord.Latitude, coord.Longitude);

    public static Coordinate ToEntity(this CoordinateCreateDto dto, User user)
        => new()
        {
            Latitude = (decimal)dto.Latitude,
            Longitude = (decimal)dto.Longitude,
            IbuId = user.IbuId,
            Ibu = user.Ibu!
        };

    public static void UpdateEntity(this Coordinate entity, CoordinateUpdateDto dto)
    {
        entity.Latitude = (decimal)dto.Latitude;
        entity.Longitude = (decimal)dto.Longitude;
    }

    public static void UpdateFromDto(this Coordinate entity, CoordinateUpdateDto dto)
    {
        entity.Latitude = (decimal)dto.Latitude;
        entity.Longitude = (decimal)dto.Longitude;
    }


    public static CoordinateCreateDto ToCreateDto(this CoordinateUpdateDto dto, ParentEntityType addressOwnerType, Guid ownerId)
        => new(
            Latitude: dto.Latitude,
            Longitude: dto.Longitude,
            OwnerType: addressOwnerType,
            OwnerPubId: ownerId
            );

}