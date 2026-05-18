using AmsRecords.Chemicals;
using static AmsRecords.Chemicals.ChemicalTypePolicyOverrideDtos;

namespace API.Extensions;

public static class ChemicalTypePolicyOverrideExtensions
{
    public static void UpdateEntity(this ChemicalTypePolicyOverride x, ChemicalTypePolicyOverrideDtos.ChemicalTypePolicyOverrideUpdateDto dto)
    {
        x.Allowed = dto.Allowed;
        x.ValidFromUtc = dto.ValidFromUtc;
        x.ValidToUtc = dto.ValidToUtc;
        x.Reason = dto.Reason?.Trim();
        x.Active = dto.Active;
    }

    public static ChemicalTypePolicyOverride ToEntity(
        this ChemicalTypePolicyOverrideCreateDto dto,
        ChemPolicyOwnerType ownerType,
        int ownerId,
        int chemTypeId)
        => new()
        {
            OwnerType = ownerType,
            OwnerId = ownerId,
            ChemTypeId = chemTypeId,
            Allowed = dto.Allowed,
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            Reason = dto.Reason?.Trim(),
            Active = true
        };
}
