namespace AmsRecords.Irrigation;

public static class HydraulicNetworkDtos
{
    public sealed record HydraulicNodeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("nodeTypeCode")] string NodeTypeCode,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("elevationM")] decimal ElevationM,
        [property: JsonPropertyName("headPubId")] Guid? HeadPubId,
        [property: JsonPropertyName("headName")] string? HeadName,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record HydraulicPipeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("startNodePubId")] Guid StartNodePubId,
        [property: JsonPropertyName("startNodeCode")] string StartNodeCode,
        [property: JsonPropertyName("endNodePubId")] Guid EndNodePubId,
        [property: JsonPropertyName("endNodeCode")] string EndNodeCode,
        [property: JsonPropertyName("lengthM")] decimal LengthM,
        [property: JsonPropertyName("internalDiameterMm")] decimal InternalDiameterMm,
        [property: JsonPropertyName("absoluteRoughnessMm")] decimal AbsoluteRoughnessMm,
        [property: JsonPropertyName("materialCode")] string MaterialCode,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record HydraulicSourceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("nodePubId")] Guid NodePubId,
        [property: JsonPropertyName("nodeCode")] string NodeCode,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("availablePressureBar")] decimal AvailablePressureBar,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record HydraulicTopologyDto(
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("calculationMethodCode")] string CalculationMethodCode,
        [property: JsonPropertyName("designPressureBar")] decimal? DesignPressureBar,
        [property: JsonPropertyName("velocityWarningThresholdMS")] decimal? VelocityWarningThresholdMS,
        [property: JsonPropertyName("nodes")] IReadOnlyList<HydraulicNodeDto> Nodes,
        [property: JsonPropertyName("pipes")] IReadOnlyList<HydraulicPipeDto> Pipes,
        [property: JsonPropertyName("sources")] IReadOnlyList<HydraulicSourceDto> Sources);

    public sealed record HydraulicNodeSaveDto(
        [property: JsonPropertyName("code")][param: Required, MaxLength(80)] string Code,
        [property: JsonPropertyName("nodeTypeCode")][param: Required, MaxLength(40)] string NodeTypeCode,
        [property: JsonPropertyName("name")][param: MaxLength(160)] string? Name,
        [property: JsonPropertyName("elevationM")] decimal ElevationM,
        [property: JsonPropertyName("headPubId")] Guid? HeadPubId,
        [property: JsonPropertyName("active")] bool Active = true);

    public sealed record HydraulicPipeSaveDto(
        [property: JsonPropertyName("code")][param: Required, MaxLength(80)] string Code,
        [property: JsonPropertyName("startNodeCode")][param: Required, MaxLength(80)] string StartNodeCode,
        [property: JsonPropertyName("endNodeCode")][param: Required, MaxLength(80)] string EndNodeCode,
        [property: JsonPropertyName("lengthM")][param: Range(typeof(decimal), "0.001", "1000000")] decimal LengthM,
        [property: JsonPropertyName("internalDiameterMm")][param: Range(typeof(decimal), "0.001", "10000")] decimal InternalDiameterMm,
        [property: JsonPropertyName("absoluteRoughnessMm")][param: Range(typeof(decimal), "0", "100")] decimal AbsoluteRoughnessMm,
        [property: JsonPropertyName("materialCode")][param: MaxLength(80)] string? MaterialCode,
        [property: JsonPropertyName("active")] bool Active = true);

    public sealed record HydraulicSourceSaveDto(
        [property: JsonPropertyName("nodeCode")][param: Required, MaxLength(80)] string NodeCode,
        [property: JsonPropertyName("name")][param: MaxLength(160)] string? Name,
        [property: JsonPropertyName("availablePressureBar")][param: Range(typeof(decimal), "0", "100")] decimal AvailablePressureBar,
        [property: JsonPropertyName("active")] bool Active = true);

    public sealed record HydraulicTopologySaveDto(
        [property: JsonPropertyName("calculationMethodCode")][param: Required, MaxLength(40)] string CalculationMethodCode,
        [property: JsonPropertyName("designPressureBar")][param: Range(typeof(decimal), "0", "100")] decimal? DesignPressureBar,
        [property: JsonPropertyName("velocityWarningThresholdMS")][param: Range(typeof(decimal), "0.001", "100")] decimal? VelocityWarningThresholdMS,
        [property: JsonPropertyName("nodes")][param: Required] IReadOnlyList<HydraulicNodeSaveDto> Nodes,
        [property: JsonPropertyName("pipes")][param: Required] IReadOnlyList<HydraulicPipeSaveDto> Pipes,
        [property: JsonPropertyName("sources")][param: Required] IReadOnlyList<HydraulicSourceSaveDto> Sources);

    public sealed record HydraulicAnalysisRequestDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("scenarioPubId")] Guid? ScenarioPubId = null);

    public sealed record HydraulicAreaAnalysisDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("scenarioPubId")] Guid? ScenarioPubId,
        [property: JsonPropertyName("scenarioName")] string? ScenarioName,
        [property: JsonPropertyName("configuredDesignPressureBar")] decimal? ConfiguredDesignPressureBar,
        [property: JsonPropertyName("result")] HydraulicNetworkResult Result);
}
