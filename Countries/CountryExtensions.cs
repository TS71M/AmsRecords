namespace AmsRecords.Countries;

using AmsModels;

public static class CountryExtensions
{

    public static CountryUpdateDto ToUpdateDto(this CountryDto dto)
        => new(
            PubId: dto.PubId,
            CountryCode: dto.CountryCode.ToUpperInvariant().Trim(),
            CountryName: dto.CountryName
            );
}