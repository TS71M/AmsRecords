using System.Linq.Expressions;
using static AmsRecords.Areas.AreaCompositionDtos;
using static AmsRecords.Areas.AreaDtos;

namespace AmsRecords.Areas;

public static class AreaExtensions
{

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
            UseGrowthPotentialTemperatureOverride: input.UseGrowthPotentialTemperatureOverride,
            EtThreshold: input.EtThreshold,
            Irrigated: input.Irrigated,
            Report: input.Report,
            Whc: input.Whc,
            HasSurface: input.HasSurface,
            GreenSpeed: input.GreenSpeed,
            BaseTemp: input.BaseTemp,
            GddDays: input.GddDays,
            DollarSpotMedium: input.DollarSpotMedium,
            DollarSpotHigh: input.DollarSpotHigh,
            DryVwcThreshold: input.DryVwcThreshold,
            WetVwcThreshold: input.WetVwcThreshold
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
            UseGrowthPotentialTemperatureOverride: dto.UseGrowthPotentialTemperatureOverride,
            EtThreshold: dto.EtThreshold,
            Irrigated: dto.Irrigated,
            Report: dto.Report,
            Whc: dto.Whc,
            HasSurface: dto.HasSurface,
            GreenSpeed: dto.GreenSpeed,
            BaseTemp: dto.BaseTemp,
            GddDays: dto.GddDays,
            DollarSpotMedium: dto.DollarSpotMedium,
            DollarSpotHigh: dto.DollarSpotHigh,
            DryVwcThreshold: dto.DryVwcThreshold,
            WetVwcThreshold: dto.WetVwcThreshold
        );
}
