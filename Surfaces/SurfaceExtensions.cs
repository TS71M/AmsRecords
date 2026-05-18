using static AmsRecords.Surfaces.SurfaceDtos;

namespace AmsRecords.Surfaces;

public static class SurfaceExtensions
{
    public static SurfaceDto ToDto(this Surface s)
        => new(
            PubId: s.PubId,
            FieldPubId: s.Field.PubId,
            AreaPubId: s.Area.PubId,
            HolePubId: s.Hole.PubId,
            AreaName: s.Area.AreaName,
            HoleNumber: s.Hole.HoleNumber,
            SurfaceSizeM2: s.SurfaceSizeM2,
            HoleImgPubId: s.Hole.AppImage?.PubId
        );

    public static SurfaceIndexDto ToIndexDto(this Surface s)
        => new(
            PubId: s.PubId,
            Name: s.Area.AreaName, // <-- stop embedding "Hole X" here
            AreaPubId: s.Area.PubId,
            AreaName: s.Area.AreaName,
            HoleNumber: s.Hole.HoleNumber,
            SurfaceSizeM2: s.SurfaceSizeM2,
            HoleImgPubId: s.Hole.AppImage?.PubId
        );

    public static void UpdateEntity(this Surface s, SurfaceUpdateDto dto)
        => s.SurfaceSizeM2 = dto.SurfaceSizeM2;
}
