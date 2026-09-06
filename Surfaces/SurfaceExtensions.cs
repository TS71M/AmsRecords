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
            UseForClippingMeasurements: SurfaceClippingMeasurementPolicy.IsEnabled(
                s.Area.ClippingsRemoved,
                s.UseForClippingMeasurements),
            HoleImgPubId: s.Hole.AppImage?.PubId,
            DryVwcThreshold: s.DryVwcThreshold,
            WetVwcThreshold: s.WetVwcThreshold,
            AreaDryVwcThreshold: s.Area.DryVwcThreshold,
            AreaWetVwcThreshold: s.Area.WetVwcThreshold,
            SurfaceLabel: s.SurfaceLabel
        )
        {
            HoleAssignments = ToHoleAssignments(s)
        };

    public static SurfaceIndexDto ToIndexDto(this Surface s)
        => new(
            PubId: s.PubId,
            Name: SurfaceLabelPolicy.FormatAreaLabel(s.Area.AreaName, s.SurfaceLabel, "Surface"),
            AreaPubId: s.Area.PubId,
            AreaName: s.Area.AreaName,
            HoleNumber: s.Hole.HoleNumber,
            SurfaceSizeM2: s.SurfaceSizeM2,
            UseForClippingMeasurements: SurfaceClippingMeasurementPolicy.IsEnabled(
                s.Area.ClippingsRemoved,
                s.UseForClippingMeasurements),
            HoleImgPubId: s.Hole.AppImage?.PubId,
            SurfaceLabel: s.SurfaceLabel
        )
        {
            HoleAssignments = ToHoleAssignments(s)
        };

    public static void UpdateEntity(this Surface s, SurfaceUpdateDto dto)
    {
        s.SurfaceSizeM2 = dto.SurfaceSizeM2;
        s.UseForClippingMeasurements = dto.UseForClippingMeasurements;
        s.DryVwcThreshold = dto.DryVwcThreshold;
        s.WetVwcThreshold = dto.WetVwcThreshold;
        s.SurfaceLabel = SurfaceLabelPolicy.Normalize(dto.SurfaceLabel);
    }

    static List<SurfaceHoleAssignmentDto> ToHoleAssignments(Surface surface)
    {
        var assignments = surface.HoleAssignments.Count == 0
            ? [new SurfaceHoleAssignmentDto(
                surface.Hole.PubId,
                surface.Hole.HoleNumber,
                surface.Hole.HolDes,
                true)]
            : surface.HoleAssignments
                .OrderBy(x => x.Hole.HoleNumber)
                .Select(x => new SurfaceHoleAssignmentDto(
                    x.Hole.PubId,
                    x.Hole.HoleNumber,
                    x.Hole.HolDes,
                    x.HoleId == surface.HoleId))
                .ToList();

        return assignments;
    }
}
