using static AmsRecords.Chemicals.ChemicalTypeAuthorizationDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalTypeAuthorizationExtensions
{
    public static ChemicalTypeAuthorizationDto ToDto(this ChemicalTypeAuthorization x)
        => new(
            x.PubId,
            x.ChemType?.PubId ?? Guid.Empty,
            x.ChemType?.ChemTypeName ?? "",
            x.Jurisdiction?.PubId ?? Guid.Empty,
            x.Jurisdiction?.Name ?? "",
            x.Allowed,
            x.ValidFromUtc,
            x.ValidToUtc,
            x.Active);

    public static ChemicalTypeAuthorization ToEntity(
        this ChemicalTypeAuthorizationCreateDto dto,
        int chemTypeId,
        int jurisdictionId)
        => new()
        {
            ChemTypeId = chemTypeId,
            JurisdictionId = jurisdictionId,
            Allowed = dto.Allowed,
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            Active = true
        };

    public static void UpdateEntity(this ChemicalTypeAuthorization entity, ChemicalTypeAuthorizationUpdateDto dto)
    {
        entity.Allowed = dto.Allowed;
        entity.ValidFromUtc = dto.ValidFromUtc;
        entity.ValidToUtc = dto.ValidToUtc;
        entity.Active = dto.Active;
    }
}
