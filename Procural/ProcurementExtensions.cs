using static AmsRecords.Procural.ProcurementDtos;

namespace AmsRecords.Procural;

public static class ProcurementExtensions
{
    public static FieldProcurementSettingDto ToDto(
        this FieldProcurementSetting setting,
        IReadOnlyList<ProcurementScopeIbuDto>? procurementScopeIbus = null)
        => new(
            PubId: setting.PubId,
            FieldPubId: setting.Field.PubId,
            FieldName: setting.Field.FieldName,
            ProcurementMode: setting.ProcurementMode,
            ProcurementHubIbuPubId: setting.ProcurementHubIbu?.PubId,
            ProcurementHubIbuName: setting.ProcurementHubIbu?.BusinessUnitName ?? string.Empty,
            ProcurementSupplierPubId: setting.ProcurementSupplier?.PubId,
            ProcurementSupplierName: setting.ProcurementSupplier?.SupNam ?? string.Empty,
            ProcurementManagerUserPubId: setting.ProcurementManagerUser?.PubId,
            ProcurementManagerName: setting.ProcurementManagerUser?.FullNameSnapshot ?? setting.ProcurementManagerUser?.Email ?? string.Empty,
            AllowDirectSupplierOrdering: setting.AllowDirectSupplierOrdering,
            ConsolidateRequisitions: setting.ConsolidateRequisitions,
            Notes: setting.Notes,
            ProcurementScopeIbuPubIds: procurementScopeIbus?.Select(x => x.PubId).ToList() ?? [],
            ProcurementScopeIbus: procurementScopeIbus ?? []
        );

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
