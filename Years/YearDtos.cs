namespace AmsRecords.Years;

public static class YearDtos
{
    public record YearDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("yearNumber")] int YearNumber,
        [property: JsonPropertyName("isCurrent")] bool IsCurrent,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("isClosed")] bool IsClosed
    );

    public record YearMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("yearNumber")] string YearNumber
    );

    public partial record YearCreateDto(
        [property: JsonPropertyName("yearNumber")] int YearNumber,
        [property: JsonPropertyName("isCurrent")] bool IsCurrent,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("isClosed")] bool IsClosed
    ) : IYearNumber
    {
        public Guid PubId => Guid.Empty;
    }


    public record YearUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("yearNumber")] int YearNumber,
        [property: JsonPropertyName("isCurrent")] bool IsCurrent,
        [property: JsonPropertyName("isVisible")] bool IsVisible,
        [property: JsonPropertyName("isClosed")] bool IsClosed
    ) : IYearNumber
    {
        public YearUpdateDto() : this(Guid.Empty, 2020, false, false, false) { }


    }

    public interface IYearNumber
    {
        int YearNumber { get; }
        Guid PubId { get; }
    }
}
