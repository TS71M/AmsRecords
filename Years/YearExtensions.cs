using static AmsRecords.Years.YearDtos;

namespace AmsRecords.Years;

public static class YearExtensions
{

    public static YearUpdateDto ToUpdateDto(this YearDto dto)
        => new(
            PubId: dto.PubId,
            YearNumber: dto.YearNumber,
            IsCurrent: dto.IsCurrent,
            IsVisible: dto.IsVisible,
            IsClosed: dto.IsClosed
            );

    public static YearCreateDto ToCreateDto(this YearUpdateDto dto)
        => new(
            YearNumber: dto.YearNumber,
            IsCurrent: dto.IsCurrent,
            IsVisible: dto.IsVisible,
            IsClosed: dto.IsClosed
        );
}