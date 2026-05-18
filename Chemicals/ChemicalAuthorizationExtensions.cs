using static AmsRecords.Chemicals.ChemicalAuthorizationDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalAuthorizationExtensions
{
    public static ChemicalAuthorizationDto ToDto(this ChemicalAuthorization x)
        => new(
            x.PubId,
            x.Chemical?.PubId ?? Guid.Empty,
            x.Chemical?.ChemName ?? "",
            x.Jurisdiction?.PubId ?? Guid.Empty,
            x.Jurisdiction?.Name ?? "",
            x.Allowed,
            x.ValidFromUtc,
            x.ValidToUtc,
            x.Active);

    public static ChemicalAuthorization ToEntity(this ChemicalAuthorizationCreateDto dto, int chemId, int jurisdictionId)
        => new()
        {
            ChemId = chemId,
            JurisdictionId = jurisdictionId,
            Allowed = dto.Allowed,
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            Active = true
        };

    public static void UpdateEntity(this ChemicalAuthorization entity, ChemicalAuthorizationUpdateDto dto)
    {
        entity.Allowed = dto.Allowed;
        entity.ValidFromUtc = dto.ValidFromUtc;
        entity.ValidToUtc = dto.ValidToUtc;
        entity.Active = dto.Active;
    }
}