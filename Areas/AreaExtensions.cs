using System.Linq.Expressions;
using static AmsRecords.Areas.AreaCompositionDtos;
using static AmsRecords.Areas.AreaDtos;

namespace AmsRecords.Areas;

public static class AreaExtensions
{
    public static AreaDto ToDto(this Area area)
    {
        var current = area.CurrentComposition();

        var grass = (current?.GrassSpecies ?? Enumerable.Empty<AreaGrassSpecies>())
            .OrderByDescending(x => x.Percent)
            .Select(x => new AreaGrassSpeciesRowDto(
                x.PubId,
                x.GrassSpecies!.PubId,
                x.Percent
            ))
            .ToList();

        var soils = (current?.SoilTypes ?? Enumerable.Empty<AreaSoilType>())
            .OrderByDescending(x => x.Percent)
            .Select(x => new AreaSoilTypeRowDto(
                x.PubId,
                x.SoilType!.PubId,
                x.Percent
            ))
            .ToList();

        return new AreaDto(
            area.PubId,
            area.Field?.PubId,
            area.Active,
            area.AreaName,
            area.Planning,
            area.ClippingsRemoved,
            area.Color,
            area.NMaxMonth,
            area.TempOpt,
            area.EtThreshold,
            area.Irrigated,
            area.Report,
            area.Whc,
            area.HasSurface,
            area.GreenSpeed,
            area.BaseTemp,
            area.GddDays,
            area.DollarSpotMedium,
            area.DollarSpotHigh,
            area.AreaGroup?.PubId ?? Guid.Empty,
            area.AreaGroup?.Name ?? string.Empty,
            area.AreaGroup?.Code ?? string.Empty,
            grass,
            soils
        );
    }

    public static Area ToEntity(
        this AreaCreateDto dto,
        Field field,
        AreaGroup group)
        => new()
        {
            FieldId = field.FieldId,
            AreaGroupId = group.AreaGroupId,

            Active = dto.Active,
            AreaName = dto.AreaName,

            Planning = dto.Planning,
            ClippingsRemoved = dto.ClippingsRemoved,
            Color = dto.Color,

            NMaxMonth = dto.NMax,
            TempOpt = dto.TempOpt,
            EtThreshold = dto.EtThreshold,

            Irrigated = dto.Irrigated,
            Report = dto.Report,
            Whc = dto.Whc,

            HasSurface = dto.HasSurface,
            GreenSpeed = dto.GreenSpeed,

            BaseTemp = dto.BaseTemp,
            GddDays = dto.GddDays,

            DollarSpotMedium = dto.DollarSpotMedium,
            DollarSpotHigh = dto.DollarSpotHigh,

            AreaGroup = group,
            Field = field
        };

    public static void UpdateEntity(
        this AreaUpdateDto dto,
        Area area,
        AreaGroup group)
    {
        area.Active = dto.Active;
        area.AreaName = dto.AreaName;

        area.Planning = dto.Planning;
        area.ClippingsRemoved = dto.ClippingsRemoved;
        area.Color = dto.Color;

        area.NMaxMonth = dto.NMax;
        area.TempOpt = dto.TempOpt;
        area.EtThreshold = dto.EtThreshold;

        area.Irrigated = dto.Irrigated;
        area.Report = dto.Report;
        area.Whc = dto.Whc;

        area.HasSurface = dto.HasSurface;
        area.GreenSpeed = dto.GreenSpeed;

        area.BaseTemp = dto.BaseTemp;
        area.GddDays = dto.GddDays;

        area.DollarSpotMedium = dto.DollarSpotMedium;
        area.DollarSpotHigh = dto.DollarSpotHigh;

        area.AreaGroupId = group.AreaGroupId;
    }

    public static readonly Expression<Func<Area, AreaIndexDto>> ToIndexDto
       = a => new AreaIndexDto(
           a.PubId,
           a.Field!.FieldName,
           a.AreaName,
           a.Active,
           a.HasSurface,
           a.Planning,
           a.Irrigated,
           a.Color,
           a.AreaGroup!.PubId,
           a.AreaGroup!.Name,
           a.AreaGroup!.Code
       );

    public static readonly Expression<Func<Area, AreaDtoBase>> ToDtoBase
        = a => new AreaDtoBase(
            a.PubId,
            a.Field != null ? a.Field.PubId : null,
            a.AreaGroup != null ? a.AreaGroup.PubId : null,
            a.Active,
            a.AreaName,
            a.Planning,
            a.ClippingsRemoved,
            a.Color,
            a.NMaxMonth,
            a.TempOpt,
            a.EtThreshold,
            a.Irrigated,
            a.Report,
            a.Whc,
            a.HasSurface,
            a.GreenSpeed,
            a.BaseTemp,
            a.GddDays,
            a.DollarSpotMedium,
            a.DollarSpotHigh
        );

    public sealed record AreaDtoBase(
        Guid PubId,
        Guid? FieldPubId,
        Guid? AreaGroupPubId,
        bool Active,
        string AreaName,
        bool Planning,
        bool ClippingsRemoved,
        int Color,
        decimal NMax,
        decimal TempOpt,
        decimal EtThreshold,
        bool Irrigated,
        bool Report,
        decimal Whc,
        bool HasSurface,
        bool GreenSpeed,
        int BaseTemp,
        int GddDays,
        int DollarSpotMedium,
        int DollarSpotHigh
    );

    public static AreaUpdateDto ToUpdateDto(this AreaCreateDto input)
        => new(
            AreaGroupPubId: input.AreaGroupPubId,
            Active: input.Active,
            AreaName: input.AreaName,
            Planning: input.Planning,
            ClippingsRemoved: input.ClippingsRemoved,
            Color: input.Color,
            NMax: input.NMax,
            TempOpt: input.TempOpt,
            EtThreshold: input.EtThreshold,
            Irrigated: input.Irrigated,
            Report: input.Report,
            Whc: input.Whc,
            HasSurface: input.HasSurface,
            GreenSpeed: input.GreenSpeed,
            BaseTemp: input.BaseTemp,
            GddDays: input.GddDays,
            DollarSpotMedium: input.DollarSpotMedium,
            DollarSpotHigh: input.DollarSpotHigh
        );

    public static AreaCreateDto ToCreateDto(this AreaDto dto)
        => new(
            FieldPubId: dto.FieldPubId ?? Guid.Empty,
            AreaGroupPubId: dto.AreaGroupPubId,
            AreaName: dto.AreaName,
            Active: dto.Active,
            Planning: dto.Planning,
            ClippingsRemoved: dto.ClippingsRemoved,
            Color: dto.Color,
            NMax: dto.NMax,
            TempOpt: dto.TempOpt,
            EtThreshold: dto.EtThreshold,
            Irrigated: dto.Irrigated,
            Report: dto.Report,
            Whc: dto.Whc,
            HasSurface: dto.HasSurface,
            GreenSpeed: dto.GreenSpeed,
            BaseTemp: dto.BaseTemp,
            GddDays: dto.GddDays,
            DollarSpotMedium: dto.DollarSpotMedium,
            DollarSpotHigh: dto.DollarSpotHigh
        );

    static AreaComposition? CurrentComposition(this Area area)
    => area.AreaCompositions?
        .Where(x => x.ValidToUtc == null)
        .OrderByDescending(x => x.ValidFromUtc)
        .FirstOrDefault();
}
