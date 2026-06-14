using static AmsRecords.Procural.ProcurementDtos;

namespace AmsRecords.Procural;

public static class ProcurementExtensions
{
    public static ProcurementHubSettingDto ToDto(this ProcurementHubSetting setting)
        => new(
            PubId: setting.PubId,
            HubIbuPubId: setting.HubIbu.PubId,
            HubIbuName: setting.HubIbu.BusinessUnitName,
            Active: setting.Active,
            AllowDirectSupplierOrderingDefault: setting.AllowDirectSupplierOrderingDefault,
            ConsolidateRequisitionsDefault: setting.ConsolidateRequisitionsDefault,
            Notes: setting.Notes,
            Members: setting.Members
                .OrderByDescending(x => x.Active)
                .ThenByDescending(x => x.IsPrimary)
                .ThenBy(x => x.User.FullNameSnapshot)
                .ThenBy(x => x.User.Email)
                .Select(x => new ProcurementHubMemberDto(
                    PubId: x.PubId,
                    UserPubId: x.User.PubId,
                    UserName: x.User.FullNameSnapshot,
                    UserEmail: x.User.Email ?? string.Empty,
                    Role: x.Role,
                    Active: x.Active,
                    IsPrimary: x.IsPrimary))
                .ToList()
        );

    public static FieldProcurementSettingDto ToDto(
        this FieldProcurementSetting setting,
        IReadOnlyList<ProcurementScopeIbuDto>? procurementScopeIbus = null)
    {
        var isLocalMode = setting.ProcurementMode == ProcurementMode.Local;

        return new(
            PubId: setting.PubId,
            FieldPubId: setting.Field.PubId,
            FieldName: setting.Field.FieldName,
            ProcurementMode: setting.ProcurementMode,
            ProcurementHubIbuPubId: isLocalMode ? null : setting.ProcurementHubIbu?.PubId,
            ProcurementHubIbuName: isLocalMode ? string.Empty : setting.ProcurementHubIbu?.BusinessUnitName ?? string.Empty,
            ProcurementSupplierPubId: null,
            ProcurementSupplierName: string.Empty,
            ProcurementManagerUserPubId: null,
            ProcurementManagerName: string.Empty,
            AllowDirectSupplierOrdering: isLocalMode || setting.AllowDirectSupplierOrdering,
            ConsolidateRequisitions: !isLocalMode && setting.ConsolidateRequisitions,
            Notes: setting.Notes,
            ProcurementScopeIbuPubIds: procurementScopeIbus?.Select(x => x.PubId).ToList() ?? [],
            ProcurementScopeIbus: procurementScopeIbus ?? []
        );
    }

    public static PurchaseRequisitionLineDto ToDto(this PurchaseRequisitionLine line)
        => new(
            PubId: line.PubId,
            ProductPubId: line.Product.PubId,
            ProductName: line.Product.ProNam,
            PreferredProductSupplierPubId: line.PreferredProductSupplier?.PubId,
            PreferredSupplierName: line.PreferredProductSupplier?.Supplier?.SupNam ?? string.Empty,
            Quantity: line.Quantity,
            QuantityLabel: $"{line.Quantity:N2} {(line.Product.UniWeiPack?.UnitShort ?? string.Empty)}",
            Notes: line.Notes
        );

    public static PurchaseRequisitionListDto ToListDto(this PurchaseRequisition requisition)
        => new(
            PubId: requisition.PubId,
            IbuPubId: requisition.Ibu.PubId,
            FieldPubId: requisition.Field.PubId,
            FieldName: requisition.Field.FieldName,
            Title: requisition.Title,
            ReferenceNo: requisition.ReferenceNo,
            Status: requisition.Status,
            RequestedDt: requisition.RequestedDt,
            NeedByDate: requisition.NeedByDate,
            RequestedByUserPubId: requisition.RequestedByUser.PubId,
            RequestedByName: requisition.RequestedByUser.FullNameSnapshot,
            ProcurementHubIbuPubId: requisition.ProcurementHubIbu?.PubId,
            ProcurementHubIbuName: requisition.ProcurementHubIbu?.BusinessUnitName ?? string.Empty,
            ProcurementManagerUserPubId: requisition.ProcurementManagerUser?.PubId,
            ProcurementManagerName: requisition.ProcurementManagerUser?.FullNameSnapshot ?? requisition.ProcurementManagerUser?.Email ?? string.Empty,
            LineCount: requisition.Lines.Count,
            TotalQuantity: requisition.Lines.Sum(x => x.Quantity)
        );

    public static PurchaseRequisitionDto ToDto(this PurchaseRequisition requisition)
        => new(
            PubId: requisition.PubId,
            IbuPubId: requisition.Ibu.PubId,
            FieldPubId: requisition.Field.PubId,
            FieldName: requisition.Field.FieldName,
            Title: requisition.Title,
            ReferenceNo: requisition.ReferenceNo,
            Status: requisition.Status,
            RequestedDt: requisition.RequestedDt,
            NeedByDate: requisition.NeedByDate,
            ReviewedDt: requisition.ReviewedDt,
            RequestedByUserPubId: requisition.RequestedByUser.PubId,
            RequestedByName: requisition.RequestedByUser.FullNameSnapshot,
            ProcurementHubIbuPubId: requisition.ProcurementHubIbu?.PubId,
            ProcurementHubIbuName: requisition.ProcurementHubIbu?.BusinessUnitName ?? string.Empty,
            ProcurementManagerUserPubId: requisition.ProcurementManagerUser?.PubId,
            ProcurementManagerName: requisition.ProcurementManagerUser?.FullNameSnapshot ?? requisition.ProcurementManagerUser?.Email ?? string.Empty,
            Notes: requisition.Notes,
            DecisionNotes: requisition.DecisionNotes,
            Lines: requisition.Lines
                .OrderBy(x => x.Product.ProNam)
                .Select(x => x.ToDto())
                .ToList()
        );
}
