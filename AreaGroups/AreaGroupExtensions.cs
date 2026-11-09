using static AmsRecords.AreaGroups.AreaGroupDtos;

namespace AmsRecords.AreaGroups;

public static class AreaGroupExtensions
{

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
}
