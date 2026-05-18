using static AmsRecords.Years.YearDtos;

namespace AmsRecords.Years;

public static class YearExtensions
{
    public static YearDto ToDto(this Year year)
         => new(
             PubId: year.PubId,
             YearNumber: year.YearNumber,
             IsCurrent: year.IsCurrent,
             IsVisible: year.IsVisible,
             IsClosed: year.IsClosed
          );

    public static Year ToEntity(this YearCreateDto dto)
        => new()
        {
            YearNumber = dto.YearNumber,
            IsCurrent = dto.IsCurrent,
            IsVisible = dto.IsVisible,
            IsClosed = dto.IsClosed
        };

    public static void UpdateEntity(this Year entity, YearUpdateDto dto)
    {
        entity.YearNumber = dto.YearNumber;
        entity.IsCurrent = dto.IsCurrent;
        entity.IsVisible = dto.IsVisible;
        entity.IsClosed = dto.IsClosed;
    }

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