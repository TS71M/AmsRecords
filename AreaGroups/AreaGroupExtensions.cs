using static AmsRecords.AreaGroups.AreaGroupDtos;

namespace AmsRecords.AreaGroups;

public static class AreaGroupExtensions
{
    public static AreaGroupDto ToDto(this AreaGroup x)
        => new(
            x.PubId,
            x.Code,
            x.Name,
            x.Active,
            x.SupportsSurfaces,
            x.SupportsTurfDefaults,
            x.SupportsComposition,
            x.SortOrder
        );

    public static AreaGroupMiniDto ToMiniDto(this AreaGroup x)
        => new(
            x.PubId,
            x.Name
        );

    public static AreaGroupCreateDto ToCreateDto(this AreaGroupDto input)
        => new(
            Code: input.Code,
            Name: input.Name,
            SupportsSurfaces: input.SupportsSurfaces,
            SupportsTurfDefaults: input.SupportsTurfDefaults,
            SupportsComposition: input.SupportsComposition,
            SortOrder: input.SortOrder
        );

    public static AreaGroupUpdateDto ToUpdateDto(this AreaGroupDto input)
        => new(
            PubId: input.PubId,
            Code: input.Code,
            Name: input.Name,
            Active: input.Active,
            SupportsSurfaces: input.SupportsSurfaces,
            SupportsTurfDefaults: input.SupportsTurfDefaults,
            SupportsComposition: input.SupportsComposition,
            SortOrder: input.SortOrder
        );
    public static AreaGroup ToEntity(this AreaGroupCreateDto dto)
        => new()
        {
            Code = dto.Code.Trim(),
            Name = dto.Name.Trim(),
            Active = true,
            SupportsSurfaces = dto.SupportsSurfaces,
            SupportsTurfDefaults = dto.SupportsTurfDefaults,
            SupportsComposition = dto.SupportsComposition,
            SortOrder = dto.SortOrder
        };

    public static void UpdateEntity(this AreaGroupUpdateDto dto, AreaGroup entity)
    {
        entity.Code = dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.Active = dto.Active;
        entity.SupportsSurfaces = dto.SupportsSurfaces;
        entity.SupportsTurfDefaults = dto.SupportsTurfDefaults;
        entity.SupportsComposition = dto.SupportsComposition;
        entity.SortOrder = dto.SortOrder;
    }
}
