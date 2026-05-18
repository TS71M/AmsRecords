using static AmsRecords.Holes.HoleDtos;

namespace AmsRecords.Holes;

public static class HoleExtensions
{
    public static HoleDto ToDto(Hole e, Guid fieldPubId) =>
        new(
            e.PubId,
            e.Active,
            fieldPubId,
            e.HolDes,
            e.HolShoNam,
            e.HoleNumber,
            e.AppImage?.PubId,
            string.IsNullOrWhiteSpace(e.AppImage?.RelativePath) ? null : e.AppImage!.RelativePath
        );

    public static HoleMiniDto ToMiniDto(Hole e) =>
        new(
            e.PubId,
            e.HoleNumber,
            e.HolShoNam,
            e.HolDes
        );

    public static Hole ToEntity(this HoleCreateDto dto, int fieldId) =>
        new()
        {
            Active = true,
            FieldId = fieldId,
            HolDes = dto.HolDes.Trim(),
            HolShoNam = dto.HolShoNam.Trim(),
            HoleNumber = dto.HoleNumber,
            // Field and AppImage are assigned by EF via FK / includes
        };

    public static void UpdateEntity(this Hole e, HoleUpdateDto dto)
    {
        e.Active = dto.Active;
        e.HolDes = dto.HolDes.Trim();
        e.HolShoNam = dto.HolShoNam.Trim();
        e.HoleNumber = dto.HoleNumber;
    }
}