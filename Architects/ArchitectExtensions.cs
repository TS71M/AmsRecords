using AmsRecords.Addresses;
using AmsRecords.ContactDetails;
using AmsRecords.Name;
using static AmsRecords.Architects.ArchitectDtos;

namespace AmsRecords.Architects;

public static class ArchitectExtensions
{
    public static ArchitectDto ToDto(this Architect architect)
        => new(
            PubId: architect.PubId,
            FieldPubId: architect.Fields.FirstOrDefault()?.PubId,
            Name: architect.Name?.ToDto(),
            Address: architect.Address?.ToDto(),
            Contact: architect.Contact?.ToDto(),
            ImagePubId: architect.AppImage?.PubId
        );

    public static ArchitectUpdateDto ToUpdateDto(this ArchitectDto dto)
        => new(
            PubId: dto.PubId,
            FieldPubId: dto.FieldPubId ?? Guid.Empty,
            ImagePubId: dto.ImagePubId
        )
        {
            ImageDataUrl = dto.ImageDataUrl
        };
}
