using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Accounts;

public class AccountGroupDtos
{
    public record AccountGroupDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("orderNumber")] int OrderNumber,
        [property: JsonPropertyName("accGroupName")][MaxLength(250)] string AccGroupName,
        [property: JsonPropertyName("image")] string? Base64Image = null
        );

    public record AccountGroupCreateDto(
        [property: JsonPropertyName("orderNumber")] int OrderNumber,
        [property: JsonPropertyName("accGroupName")][MaxLength(250)] string AccGroupName,
        [property: JsonPropertyName("appImageCreateDto")] AppImageCreateDto AppImageCreateDto
        );

    public record AccountGroupUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("orderNumber")] int OrderNumber,
        [property: JsonPropertyName("accGroupName")][MaxLength(250)] string AccGroupName,
        [property: JsonPropertyName("appImageCreateDto")] AppImageCreateDto AppImageCreateDto
        );
}