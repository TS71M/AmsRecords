using static AmsRecords.Jurisdictions.JurisdictionDtos;

namespace AmsRecords.Jurisdictions;

public static class JurisdictionExtensions
{
    public static JurisdictionDto ToDto(this Jurisdiction j)
        => new(
            j.PubId,
            j.Country?.PubId ?? Guid.Empty,
            j.Country?.CountryCode ?? "",
            j.Country?.CountryName ?? "",
            j.Name,
            j.Active
        );

    public static JurisdictionMiniDto ToMiniDto(this Jurisdiction j)
        => new(j.PubId, j.Name);

    public static void UpdateEntity(this Jurisdiction j, JurisdictionUpdateDto dto)
    {
        j.Name = dto.Name.Trim();
        j.Active = dto.Active;
        // CountryId update is handled in service after lookup by CountryPubId
    }
}
