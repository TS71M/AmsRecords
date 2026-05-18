using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Accounts;

public class AccountDtos
{
    public record AccountDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("accNum")] int AccNum,
        [property: JsonPropertyName("accDes")] string AccDes,
        [property: JsonPropertyName("accGroupPublicId")] Guid? AccGroupPubId,
        [property: JsonPropertyName("accGroupName")] string? AccGroupName,
        [property: JsonPropertyName("image")] string? Base64Image
        );

    public record AccountUpdateDto(
       [property: JsonPropertyName("publicId")] Guid PubId,
       [property: JsonPropertyName("accNum")] int AccNum,
       [property: JsonPropertyName("accDes")] string AccDes,
       [property: JsonPropertyName("accGroupPublicId")] Guid AccGroupPubId,
       [property: JsonPropertyName("image")] AppImageCreateDto AppImageCreateDto
       );

    public record AccountCreateDto(
        [property: JsonPropertyName("accNum")] int AccNum,
        [property: JsonPropertyName("accDes")] string AccDes,
        [property: JsonPropertyName("accGroupPublicId")] Guid AccGroupPubId,
        [property: JsonPropertyName("appImageCreateDto")] AppImageCreateDto AppImageCreateDto
        );
}