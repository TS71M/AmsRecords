namespace AmsRecords.Irrigation;

/// <summary>
/// Stable, vendor-independent codes used by the hydraulic network model.
/// These are strings by design so imports can preserve unknown vendor metadata
/// without coupling the calculation contract to a database enum.
/// </summary>
public static class HydraulicNodeTypes
{
    public const string Source = "SOURCE";
    public const string Junction = "JUNCTION";
    public const string Valve = "VALVE";
    public const string Controller = "CONTROLLER";
    public const string Head = "HEAD";

    public static readonly IReadOnlySet<string> Supported = new HashSet<string>(
        [Source, Junction, Valve, Controller, Head],
        StringComparer.OrdinalIgnoreCase);
}

public static class HydraulicCalculationMethods
{
    public const string DarcyWeisbach = "DARCY_WEISBACH";
}

public static class HydraulicConvergenceStatuses
{
    public const string Converged = "CONVERGED";
    public const string InvalidNetwork = "INVALID_NETWORK";
    public const string NonConvergent = "NON_CONVERGENT";
}

public static class HydraulicPerformanceStatuses
{
    public const string OnTarget = "ON_TARGET";
    public const string BelowTarget = "BELOW_TARGET";
    public const string NoAvailablePressure = "NO_AVAILABLE_PRESSURE";
    public const string NotOperating = "NOT_OPERATING";
}

public sealed record HydraulicHeadDemandInput(
    [property: JsonPropertyName("referencePressureBar")] double ReferencePressureBar,
    [property: JsonPropertyName("referenceFlowM3H")] double ReferenceFlowM3H,
    [property: JsonPropertyName("targetPressureBar")] double? TargetPressureBar = null,
    [property: JsonPropertyName("operating")] bool Operating = true);

public sealed record HydraulicNodeInput(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("nodeTypeCode")] string NodeTypeCode,
    [property: JsonPropertyName("elevationM")] double ElevationM,
    [property: JsonPropertyName("active")] bool Active = true,
    [property: JsonPropertyName("name")] string? Name = null,
    [property: JsonPropertyName("headPubId")] Guid? HeadPubId = null,
    [property: JsonPropertyName("headDemand")] HydraulicHeadDemandInput? HeadDemand = null);

public sealed record HydraulicPipeInput(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("startNodeCode")] string StartNodeCode,
    [property: JsonPropertyName("endNodeCode")] string EndNodeCode,
    [property: JsonPropertyName("lengthM")] double LengthM,
    [property: JsonPropertyName("internalDiameterMm")] double InternalDiameterMm,
    [property: JsonPropertyName("absoluteRoughnessMm")] double AbsoluteRoughnessMm,
    [property: JsonPropertyName("materialCode")] string? MaterialCode = null,
    [property: JsonPropertyName("active")] bool Active = true);

public sealed record HydraulicSourceInput(
    [property: JsonPropertyName("nodeCode")] string NodeCode,
    [property: JsonPropertyName("availablePressureBar")] double AvailablePressureBar);

public sealed record HydraulicDesignSettings(
    [property: JsonPropertyName("designPressureBar")] double? DesignPressureBar = null,
    [property: JsonPropertyName("velocityWarningThresholdMS")] double? VelocityWarningThresholdMS = null,
    [property: JsonPropertyName("pressureToleranceBar")] double PressureToleranceBar = 0.001,
    [property: JsonPropertyName("flowToleranceM3H")] double FlowToleranceM3H = 0.0001,
    [property: JsonPropertyName("maximumIterations")] int MaximumIterations = 100,
    [property: JsonPropertyName("relaxationFactor")] double RelaxationFactor = 0.5,
    [property: JsonPropertyName("waterDensityKgM3")] double WaterDensityKgM3 = 998.2,
    [property: JsonPropertyName("kinematicViscosityM2S")] double KinematicViscosityM2S = 1.004e-6);

public sealed record HydraulicNetworkInput(
    [property: JsonPropertyName("nodes")] IReadOnlyList<HydraulicNodeInput> Nodes,
    [property: JsonPropertyName("pipes")] IReadOnlyList<HydraulicPipeInput> Pipes,
    [property: JsonPropertyName("sources")] IReadOnlyList<HydraulicSourceInput> Sources,
    [property: JsonPropertyName("calculationMethodCode")] string CalculationMethodCode = HydraulicCalculationMethods.DarcyWeisbach,
    [property: JsonPropertyName("settings")] HydraulicDesignSettings? Settings = null);

public sealed record HydraulicValidationIssue(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("entityCode")] string? EntityCode = null);

public sealed record HydraulicWarning(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("entityType")] string EntityType,
    [property: JsonPropertyName("entityCode")] string EntityCode);

public sealed record HydraulicHeadResult(
    [property: JsonPropertyName("nodeCode")] string NodeCode,
    [property: JsonPropertyName("headPubId")] Guid? HeadPubId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("operating")] bool Operating,
    [property: JsonPropertyName("predictedPressureBar")] double PredictedPressureBar,
    [property: JsonPropertyName("predictedFlowM3H")] double PredictedFlowM3H,
    [property: JsonPropertyName("pressureDeficitBar")] double PressureDeficitBar,
    [property: JsonPropertyName("performanceStatus")] string PerformanceStatus);

public sealed record HydraulicPipeResult(
    [property: JsonPropertyName("pipeCode")] string PipeCode,
    [property: JsonPropertyName("fromNodeCode")] string FromNodeCode,
    [property: JsonPropertyName("toNodeCode")] string ToNodeCode,
    [property: JsonPropertyName("flowM3H")] double FlowM3H,
    [property: JsonPropertyName("velocityMS")] double VelocityMS,
    [property: JsonPropertyName("pressureLossBar")] double PressureLossBar,
    [property: JsonPropertyName("elevationPressureChangeBar")] double ElevationPressureChangeBar,
    [property: JsonPropertyName("startPressureBar")] double StartPressureBar,
    [property: JsonPropertyName("endPressureBar")] double EndPressureBar,
    [property: JsonPropertyName("reynoldsNumber")] double ReynoldsNumber,
    [property: JsonPropertyName("darcyFrictionFactor")] double DarcyFrictionFactor);

public sealed record HydraulicSystemResult(
    [property: JsonPropertyName("totalFlowM3H")] double TotalFlowM3H,
    [property: JsonPropertyName("minimumHeadPressureBar")] double? MinimumHeadPressureBar,
    [property: JsonPropertyName("maximumVelocityMS")] double MaximumVelocityMS,
    [property: JsonPropertyName("sourcePressureBar")] double SourcePressureBar,
    [property: JsonPropertyName("sourceStaticHeadM")] double SourceStaticHeadM,
    [property: JsonPropertyName("operatingHeadCount")] int OperatingHeadCount,
    [property: JsonPropertyName("headsBelowTargetCount")] int HeadsBelowTargetCount);

public sealed record HydraulicNetworkResult(
    [property: JsonPropertyName("valid")] bool Valid,
    [property: JsonPropertyName("converged")] bool Converged,
    [property: JsonPropertyName("convergenceStatus")] string ConvergenceStatus,
    [property: JsonPropertyName("calculationMethodCode")] string CalculationMethodCode,
    [property: JsonPropertyName("iterations")] int Iterations,
    [property: JsonPropertyName("system")] HydraulicSystemResult? System,
    [property: JsonPropertyName("heads")] IReadOnlyList<HydraulicHeadResult> Heads,
    [property: JsonPropertyName("pipes")] IReadOnlyList<HydraulicPipeResult> Pipes,
    [property: JsonPropertyName("warnings")] IReadOnlyList<HydraulicWarning> Warnings,
    [property: JsonPropertyName("validationIssues")] IReadOnlyList<HydraulicValidationIssue> ValidationIssues);

/// <summary>
/// Solves a single-source, loop-free irrigation network with Darcy-Weisbach
/// friction loss. Head discharge follows Q = Qref * sqrt(P / Pref), so pressure,
/// downstream flow and friction loss are iterated to a fixed point.
/// </summary>
public static class HydraulicNetworkSolver
{
    const double GravityMS2 = 9.80665;
    const double PascalsPerBar = 100_000d;
    const double SecondsPerHour = 3_600d;

    public static HydraulicNetworkResult Solve(
        HydraulicNetworkInput? input,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var validation = Validate(input);
        if (validation.Issues.Count > 0 || input is null)
        {
            return new HydraulicNetworkResult(
                false,
                false,
                HydraulicConvergenceStatuses.InvalidNetwork,
                Normalize(input?.CalculationMethodCode),
                0,
                null,
                [],
                [],
                validation.Warnings,
                validation.Issues);
        }

        var settings = input.Settings ?? new HydraulicDesignSettings();
        var source = input.Sources.Single();
        var nodes = input.Nodes
            .Where(x => x.Active)
            .ToDictionary(x => Normalize(x.Code), StringComparer.OrdinalIgnoreCase);
        var pipes = input.Pipes.Where(x => x.Active).ToList();
        var topology = BuildTopology(nodes, pipes, Normalize(source.NodeCode));
        var operatingHeads = nodes.Values
            .Where(IsOperatingHead)
            .OrderBy(x => Normalize(x.Code), StringComparer.Ordinal)
            .ToList();
        var demands = nodes.Values.ToDictionary(
            x => Normalize(x.Code),
            x => IsOperatingHead(x) ? x.HeadDemand!.ReferenceFlowM3H : 0d,
            StringComparer.OrdinalIgnoreCase);

        Evaluation? previous = null;
        Evaluation current = Evaluate(nodes, pipes, topology, source, settings, demands);
        var converged = operatingHeads.Count == 0;
        var iterations = 0;

        for (var iteration = 1; !converged && iteration <= settings.MaximumIterations; iteration++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            iterations = iteration;
            current = Evaluate(nodes, pipes, topology, source, settings, demands);
            var nextDemands = new Dictionary<string, double>(demands, StringComparer.OrdinalIgnoreCase);
            var maximumFlowResidual = 0d;

            foreach (var head in operatingHeads)
            {
                var code = Normalize(head.Code);
                var pressureBar = Math.Max(0d, current.NodePressuresBar[code]);
                var demand = head.HeadDemand!;
                var pressureCoupledFlow = demand.ReferenceFlowM3H *
                                          Math.Sqrt(pressureBar / demand.ReferencePressureBar);
                maximumFlowResidual = Math.Max(
                    maximumFlowResidual,
                    Math.Abs(pressureCoupledFlow - demands[code]));
                nextDemands[code] = demands[code] +
                                    settings.RelaxationFactor * (pressureCoupledFlow - demands[code]);
            }

            var maximumPressureChange = previous is null
                ? double.PositiveInfinity
                : operatingHeads.Max(head => Math.Abs(
                    current.NodePressuresBar[Normalize(head.Code)] -
                    previous.NodePressuresBar[Normalize(head.Code)]));

            demands = nextDemands;
            previous = current;
            converged = maximumFlowResidual <= settings.FlowToleranceM3H &&
                        maximumPressureChange <= settings.PressureToleranceBar;
        }

        cancellationToken.ThrowIfCancellationRequested();
        current = Evaluate(nodes, pipes, topology, source, settings, demands);
        var heads = BuildHeadResults(nodes, current, settings);
        var pipeResults = current.Pipes.Values
            .OrderBy(x => x.PipeCode, StringComparer.Ordinal)
            .ToList();
        var warnings = validation.Warnings.ToList();
        AddResultWarnings(heads, pipeResults, settings, warnings);
        if (!converged)
        {
            warnings.Add(new HydraulicWarning(
                "NON_CONVERGENCE",
                $"Pressure-coupled nozzle demand did not converge within {settings.MaximumIterations} iterations.",
                "SYSTEM",
                "SYSTEM"));
        }

        var operatingResults = heads.Where(x => x.Operating).ToList();
        var totalFlow = operatingResults.Sum(x => x.PredictedFlowM3H);
        var system = new HydraulicSystemResult(
            totalFlow,
            operatingResults.Count == 0 ? null : operatingResults.Min(x => x.PredictedPressureBar),
            pipeResults.Count == 0 ? 0d : pipeResults.Max(x => x.VelocityMS),
            source.AvailablePressureBar,
            source.AvailablePressureBar * PascalsPerBar / (settings.WaterDensityKgM3 * GravityMS2),
            operatingResults.Count,
            operatingResults.Count(x => x.PerformanceStatus != HydraulicPerformanceStatuses.OnTarget));

        return new HydraulicNetworkResult(
            true,
            converged,
            converged ? HydraulicConvergenceStatuses.Converged : HydraulicConvergenceStatuses.NonConvergent,
            HydraulicCalculationMethods.DarcyWeisbach,
            iterations,
            system,
            heads,
            pipeResults,
            warnings,
            []);
    }

    static ValidationResult Validate(HydraulicNetworkInput? input)
    {
        var issues = new List<HydraulicValidationIssue>();
        var warnings = new List<HydraulicWarning>();
        if (input is null)
        {
            issues.Add(new("NETWORK_REQUIRED", "A hydraulic network is required."));
            return new(issues, warnings);
        }

        if (!string.Equals(
                Normalize(input.CalculationMethodCode),
                HydraulicCalculationMethods.DarcyWeisbach,
                StringComparison.OrdinalIgnoreCase))
        {
            issues.Add(new(
                "UNSUPPORTED_CALCULATION_METHOD",
                "This solver supports DARCY_WEISBACH only. Hazen-Williams is not applied implicitly."));
        }

        var settings = input.Settings ?? new HydraulicDesignSettings();
        ValidateSettings(settings, issues);
        var nodes = input.Nodes ?? [];
        var pipes = input.Pipes ?? [];
        var sources = input.Sources ?? [];
        if (nodes.Count == 0)
            issues.Add(new("NO_NODES", "Add at least one hydraulic node."));
        if (input.Pipes is null)
            issues.Add(new("PIPES_REQUIRED", "The pipe collection is required."));
        if (sources.Count == 0)
            issues.Add(new("NO_SOURCE", "Exactly one hydraulic source is required."));
        else if (sources.Count != 1)
            issues.Add(new("MULTIPLE_SOURCES_UNSUPPORTED", "This solver requires exactly one source."));
        if (issues.Count > 0 && (input.Nodes is null || input.Pipes is null || input.Sources is null))
            return new(issues, warnings);

        var duplicateNodes = nodes
            .GroupBy(x => Normalize(x.Code), StringComparer.OrdinalIgnoreCase)
            .Where(x => string.IsNullOrWhiteSpace(x.Key) || x.Count() > 1);
        foreach (var duplicate in duplicateNodes)
            issues.Add(new("DUPLICATE_OR_MISSING_NODE_CODE", "Every node needs a unique code.", duplicate.Key));

        var nodeByCode = nodes
            .Where(x => !string.IsNullOrWhiteSpace(x.Code))
            .GroupBy(x => Normalize(x.Code), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
        foreach (var node in nodes.Where(x => x.Active))
        {
            var code = Normalize(node.Code);
            var type = Normalize(node.NodeTypeCode);
            if (!IsFinite(node.ElevationM))
                issues.Add(new("INVALID_NODE_ELEVATION", "Node elevation must be finite.", code));
            if (!HydraulicNodeTypes.Supported.Contains(type))
                issues.Add(new("UNSUPPORTED_NODE_TYPE", $"Node type '{type}' is not supported.", code));
            if (string.Equals(type, HydraulicNodeTypes.Head, StringComparison.OrdinalIgnoreCase) &&
                node.HeadDemand?.Operating == true)
            {
                if (!IsPositiveFinite(node.HeadDemand.ReferencePressureBar))
                    issues.Add(new("MISSING_REFERENCE_PRESSURE", "An operating head needs a positive reference pressure.", code));
                if (!IsNonNegativeFinite(node.HeadDemand.ReferenceFlowM3H) || node.HeadDemand.ReferenceFlowM3H <= 0d)
                    issues.Add(new("MISSING_REFERENCE_FLOW", "An operating head needs a positive reference flow.", code));
                var target = node.HeadDemand.TargetPressureBar ?? settings.DesignPressureBar;
                if (!target.HasValue || !IsNonNegativeFinite(target.Value))
                    issues.Add(new("MISSING_DESIGN_PRESSURE", "An operating head needs a non-negative target or system design pressure.", code));
            }
        }

        var duplicatePipes = pipes
            .GroupBy(x => Normalize(x.Code), StringComparer.OrdinalIgnoreCase)
            .Where(x => string.IsNullOrWhiteSpace(x.Key) || x.Count() > 1);
        foreach (var duplicate in duplicatePipes)
            issues.Add(new("DUPLICATE_OR_MISSING_PIPE_CODE", "Every pipe needs a unique code.", duplicate.Key));

        foreach (var pipe in pipes.Where(x => x.Active))
        {
            var code = Normalize(pipe.Code);
            var start = Normalize(pipe.StartNodeCode);
            var end = Normalize(pipe.EndNodeCode);
            if (!nodeByCode.TryGetValue(start, out var startNode) || !startNode.Active ||
                !nodeByCode.TryGetValue(end, out var endNode) || !endNode.Active)
                issues.Add(new("PIPE_ENDPOINT_MISSING", "An active pipe must connect two active nodes.", code));
            if (string.Equals(start, end, StringComparison.OrdinalIgnoreCase))
                issues.Add(new("PIPE_SELF_LOOP", "A pipe cannot connect a node to itself.", code));
            if (!IsPositiveFinite(pipe.LengthM))
                issues.Add(new("INVALID_PIPE_LENGTH", "Pipe length must be positive and finite.", code));
            if (!IsPositiveFinite(pipe.InternalDiameterMm))
                issues.Add(new("MISSING_PIPE_DIAMETER", "Pipe internal diameter must be positive and finite.", code));
            if (!IsNonNegativeFinite(pipe.AbsoluteRoughnessMm))
                issues.Add(new("INVALID_PIPE_ROUGHNESS", "Pipe absolute roughness must be non-negative and finite.", code));
        }

        if (sources.Count == 1)
        {
            var source = sources[0];
            var sourceCode = Normalize(source.NodeCode);
            if (!nodeByCode.TryGetValue(sourceCode, out var sourceNode) || !sourceNode.Active)
                issues.Add(new("SOURCE_NODE_MISSING", "The source must reference an active node.", sourceCode));
            else if (!string.Equals(Normalize(sourceNode.NodeTypeCode), HydraulicNodeTypes.Source, StringComparison.OrdinalIgnoreCase))
                issues.Add(new("SOURCE_NODE_TYPE_INVALID", "The hydraulic source must reference a SOURCE node.", sourceCode));
            if (!IsNonNegativeFinite(source.AvailablePressureBar))
                issues.Add(new("INVALID_SOURCE_PRESSURE", "Source pressure must be non-negative and finite.", sourceCode));

            foreach (var extraSource in nodes.Where(x => x.Active &&
                         string.Equals(Normalize(x.NodeTypeCode), HydraulicNodeTypes.Source, StringComparison.OrdinalIgnoreCase) &&
                         !string.Equals(Normalize(x.Code), sourceCode, StringComparison.OrdinalIgnoreCase)))
                issues.Add(new("UNCONFIGURED_SOURCE_NODE", "Every SOURCE node must be the configured source.", Normalize(extraSource.Code)));

            if (issues.Count == 0)
                ValidateTopology(input, nodeByCode, sourceCode, issues);
        }

        if (!nodes.Any(IsOperatingHead))
        {
            warnings.Add(new HydraulicWarning(
                "NO_OPERATING_HEADS",
                "The network has no operating heads; calculated demand is zero.",
                "SYSTEM",
                "SYSTEM"));
        }

        return new(issues, warnings);
    }

    static void ValidateSettings(HydraulicDesignSettings settings, List<HydraulicValidationIssue> issues)
    {
        if (settings.DesignPressureBar.HasValue && !IsNonNegativeFinite(settings.DesignPressureBar.Value))
            issues.Add(new("INVALID_DESIGN_PRESSURE", "Design pressure must be non-negative and finite."));
        if (settings.VelocityWarningThresholdMS.HasValue && !IsPositiveFinite(settings.VelocityWarningThresholdMS.Value))
            issues.Add(new("INVALID_VELOCITY_THRESHOLD", "The velocity warning threshold must be positive and finite."));
        if (!IsPositiveFinite(settings.PressureToleranceBar))
            issues.Add(new("INVALID_PRESSURE_TOLERANCE", "Pressure tolerance must be positive and finite."));
        if (!IsPositiveFinite(settings.FlowToleranceM3H))
            issues.Add(new("INVALID_FLOW_TOLERANCE", "Flow tolerance must be positive and finite."));
        if (settings.MaximumIterations is < 1 or > 10_000)
            issues.Add(new("INVALID_MAXIMUM_ITERATIONS", "Maximum iterations must be between 1 and 10000."));
        if (!IsFinite(settings.RelaxationFactor) || settings.RelaxationFactor is <= 0d or > 1d)
            issues.Add(new("INVALID_RELAXATION_FACTOR", "Relaxation factor must be greater than zero and no more than one."));
        if (!IsPositiveFinite(settings.WaterDensityKgM3))
            issues.Add(new("INVALID_WATER_DENSITY", "Water density must be positive and finite."));
        if (!IsPositiveFinite(settings.KinematicViscosityM2S))
            issues.Add(new("INVALID_KINEMATIC_VISCOSITY", "Kinematic viscosity must be positive and finite."));
    }

    static void ValidateTopology(
        HydraulicNetworkInput input,
        IReadOnlyDictionary<string, HydraulicNodeInput> nodeByCode,
        string sourceCode,
        List<HydraulicValidationIssue> issues)
    {
        var activeNodes = input.Nodes.Where(x => x.Active).Select(x => Normalize(x.Code)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var activePipes = input.Pipes.Where(x => x.Active).ToList();
        var adjacency = activeNodes.ToDictionary(x => x, _ => new List<(string Node, string Pipe)>(), StringComparer.OrdinalIgnoreCase);
        foreach (var pipe in activePipes)
        {
            var start = Normalize(pipe.StartNodeCode);
            var end = Normalize(pipe.EndNodeCode);
            adjacency[start].Add((end, Normalize(pipe.Code)));
            adjacency[end].Add((start, Normalize(pipe.Code)));
        }

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { sourceCode };
        var queue = new Queue<(string Node, string? ParentPipe)>();
        queue.Enqueue((sourceCode, null));
        var loopDetected = false;
        while (queue.Count > 0)
        {
            var (node, parentPipe) = queue.Dequeue();
            foreach (var edge in adjacency[node].OrderBy(x => x.Pipe, StringComparer.Ordinal))
            {
                if (string.Equals(edge.Pipe, parentPipe, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!visited.Add(edge.Node))
                {
                    loopDetected = true;
                    continue;
                }
                queue.Enqueue((edge.Node, edge.Pipe));
            }
        }

        if (loopDetected)
            issues.Add(new("LOOPS_UNSUPPORTED", "The first hydraulic solver release supports loop-free networks only."));
        foreach (var disconnected in activeNodes.Where(x => !visited.Contains(x)).OrderBy(x => x, StringComparer.Ordinal))
        {
            var message = string.Equals(
                Normalize(nodeByCode[disconnected].NodeTypeCode),
                HydraulicNodeTypes.Head,
                StringComparison.OrdinalIgnoreCase)
                ? "The active head is disconnected from the source."
                : "The active node is disconnected from the source.";
            issues.Add(new("DISCONNECTED_NODE", message, disconnected));
        }
    }

    static Topology BuildTopology(
        IReadOnlyDictionary<string, HydraulicNodeInput> nodes,
        IReadOnlyList<HydraulicPipeInput> pipes,
        string sourceCode)
    {
        var adjacency = nodes.Keys.ToDictionary(
            x => x,
            _ => new List<(string Node, HydraulicPipeInput Pipe)>(),
            StringComparer.OrdinalIgnoreCase);
        foreach (var pipe in pipes)
        {
            var start = Normalize(pipe.StartNodeCode);
            var end = Normalize(pipe.EndNodeCode);
            adjacency[start].Add((end, pipe));
            adjacency[end].Add((start, pipe));
        }

        var parent = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase) { [sourceCode] = null };
        var parentPipe = new Dictionary<string, HydraulicPipeInput>(StringComparer.OrdinalIgnoreCase);
        var order = new List<string>();
        var queue = new Queue<string>();
        queue.Enqueue(sourceCode);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            order.Add(node);
            foreach (var edge in adjacency[node].OrderBy(x => Normalize(x.Pipe.Code), StringComparer.Ordinal))
            {
                if (parent.ContainsKey(edge.Node))
                    continue;
                parent[edge.Node] = node;
                parentPipe[edge.Node] = edge.Pipe;
                queue.Enqueue(edge.Node);
            }
        }

        return new(parent, parentPipe, order);
    }

    static Evaluation Evaluate(
        IReadOnlyDictionary<string, HydraulicNodeInput> nodes,
        IReadOnlyList<HydraulicPipeInput> pipes,
        Topology topology,
        HydraulicSourceInput source,
        HydraulicDesignSettings settings,
        IReadOnlyDictionary<string, double> demands)
    {
        var subtreeFlows = new Dictionary<string, double>(demands, StringComparer.OrdinalIgnoreCase);
        foreach (var node in topology.Order.AsEnumerable().Reverse())
        {
            var parent = topology.Parent[node];
            if (parent is not null)
                subtreeFlows[parent] += subtreeFlows[node];
        }

        var pressures = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            [Normalize(source.NodeCode)] = source.AvailablePressureBar
        };
        var pipeResults = new Dictionary<string, HydraulicPipeResult>(StringComparer.OrdinalIgnoreCase);
        foreach (var nodeCode in topology.Order.Skip(1))
        {
            var parentCode = topology.Parent[nodeCode]!;
            var pipe = topology.ParentPipe[nodeCode];
            var flowM3H = subtreeFlows[nodeCode];
            var hydraulics = CalculatePipeHydraulics(pipe, flowM3H, settings);
            var elevationChangeM = nodes[nodeCode].ElevationM - nodes[parentCode].ElevationM;
            var elevationPressureChangeBar = settings.WaterDensityKgM3 * GravityMS2 * elevationChangeM / PascalsPerBar;
            var startPressure = pressures[parentCode];
            var endPressure = startPressure - hydraulics.PressureLossBar - elevationPressureChangeBar;
            pressures[nodeCode] = endPressure;
            pipeResults[Normalize(pipe.Code)] = new HydraulicPipeResult(
                Normalize(pipe.Code),
                parentCode,
                nodeCode,
                flowM3H,
                hydraulics.VelocityMS,
                hydraulics.PressureLossBar,
                elevationPressureChangeBar,
                startPressure,
                endPressure,
                hydraulics.ReynoldsNumber,
                hydraulics.FrictionFactor);
        }

        // Valid topology guarantees every active pipe is represented. Keep the
        // parameter to make that invariant explicit at this boundary.
        _ = pipes;
        return new(pressures, pipeResults, subtreeFlows);
    }

    static PipeHydraulics CalculatePipeHydraulics(
        HydraulicPipeInput pipe,
        double flowM3H,
        HydraulicDesignSettings settings)
    {
        if (flowM3H <= 0d)
            return new(0d, 0d, 0d, 0d);

        var diameterM = pipe.InternalDiameterMm / 1_000d;
        var flowM3S = flowM3H / SecondsPerHour;
        var areaM2 = Math.PI * diameterM * diameterM / 4d;
        var velocity = flowM3S / areaM2;
        var reynolds = velocity * diameterM / settings.KinematicViscosityM2S;
        var relativeRoughness = (pipe.AbsoluteRoughnessMm / 1_000d) / diameterM;
        var frictionFactor = DarcyFrictionFactor(reynolds, relativeRoughness);
        var headLossM = frictionFactor * (pipe.LengthM / diameterM) *
                        velocity * velocity / (2d * GravityMS2);
        var pressureLossBar = settings.WaterDensityKgM3 * GravityMS2 * headLossM / PascalsPerBar;
        return new(velocity, reynolds, frictionFactor, pressureLossBar);
    }

    static double DarcyFrictionFactor(double reynolds, double relativeRoughness)
    {
        if (reynolds <= 0d)
            return 0d;
        var laminar = 64d / reynolds;
        if (reynolds <= 2_300d)
            return laminar;

        // Swamee-Jain is an explicit approximation of Colebrook-White for
        // turbulent Darcy friction. The transition band is interpolated to
        // avoid a numerical discontinuity during demand iteration.
        var turbulent = 0.25d / Math.Pow(
            Math.Log10(relativeRoughness / 3.7d + 5.74d / Math.Pow(reynolds, 0.9d)),
            2d);
        if (reynolds >= 4_000d)
            return turbulent;
        var transition = (reynolds - 2_300d) / (4_000d - 2_300d);
        return laminar + transition * (turbulent - laminar);
    }

    static IReadOnlyList<HydraulicHeadResult> BuildHeadResults(
        IReadOnlyDictionary<string, HydraulicNodeInput> nodes,
        Evaluation evaluation,
        HydraulicDesignSettings settings)
    {
        return nodes.Values
            .Where(x => string.Equals(Normalize(x.NodeTypeCode), HydraulicNodeTypes.Head, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => Normalize(x.Code), StringComparer.Ordinal)
            .Select(node =>
            {
                var code = Normalize(node.Code);
                var operating = IsOperatingHead(node);
                var rawPressure = evaluation.NodePressuresBar[code];
                var availablePressure = Math.Max(0d, rawPressure);
                var target = node.HeadDemand?.TargetPressureBar ?? settings.DesignPressureBar ?? 0d;
                var flow = operating ? evaluation.SubtreeFlowsM3H[code] : 0d;
                var deficit = operating ? Math.Max(0d, target - availablePressure) : 0d;
                var status = !operating
                    ? HydraulicPerformanceStatuses.NotOperating
                    : availablePressure <= 0d
                        ? HydraulicPerformanceStatuses.NoAvailablePressure
                        : deficit > settings.PressureToleranceBar
                            ? HydraulicPerformanceStatuses.BelowTarget
                            : HydraulicPerformanceStatuses.OnTarget;
                return new HydraulicHeadResult(
                    code,
                    node.HeadPubId,
                    string.IsNullOrWhiteSpace(node.Name) ? code : node.Name.Trim(),
                    operating,
                    availablePressure,
                    flow,
                    deficit,
                    status);
            })
            .ToList();
    }

    static void AddResultWarnings(
        IReadOnlyList<HydraulicHeadResult> heads,
        IReadOnlyList<HydraulicPipeResult> pipes,
        HydraulicDesignSettings settings,
        List<HydraulicWarning> warnings)
    {
        foreach (var head in heads.Where(x => x.Operating &&
                     x.PerformanceStatus != HydraulicPerformanceStatuses.OnTarget))
        {
            warnings.Add(new HydraulicWarning(
                "HEAD_BELOW_TARGET_PRESSURE",
                $"Head '{head.Name}' is {head.PressureDeficitBar:0.###} bar below its configured pressure target.",
                "HEAD",
                head.NodeCode));
        }

        if (!settings.VelocityWarningThresholdMS.HasValue)
            return;
        foreach (var pipe in pipes.Where(x => x.VelocityMS > settings.VelocityWarningThresholdMS.Value))
        {
            warnings.Add(new HydraulicWarning(
                "PIPE_VELOCITY_THRESHOLD_EXCEEDED",
                $"Pipe '{pipe.PipeCode}' velocity {pipe.VelocityMS:0.###} m/s exceeds the configured {settings.VelocityWarningThresholdMS.Value:0.###} m/s design threshold.",
                "PIPE",
                pipe.PipeCode));
        }
    }

    static bool IsOperatingHead(HydraulicNodeInput node)
        => node.Active &&
           string.Equals(Normalize(node.NodeTypeCode), HydraulicNodeTypes.Head, StringComparison.OrdinalIgnoreCase) &&
           node.HeadDemand?.Operating == true;

    static string Normalize(string? value) => (value ?? string.Empty).Trim().ToUpperInvariant();
    static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    static bool IsPositiveFinite(double value) => value > 0d && IsFinite(value);
    static bool IsNonNegativeFinite(double value) => value >= 0d && IsFinite(value);

    sealed record ValidationResult(
        IReadOnlyList<HydraulicValidationIssue> Issues,
        IReadOnlyList<HydraulicWarning> Warnings);

    sealed record Topology(
        IReadOnlyDictionary<string, string?> Parent,
        IReadOnlyDictionary<string, HydraulicPipeInput> ParentPipe,
        IReadOnlyList<string> Order);

    sealed record Evaluation(
        IReadOnlyDictionary<string, double> NodePressuresBar,
        IReadOnlyDictionary<string, HydraulicPipeResult> Pipes,
        IReadOnlyDictionary<string, double> SubtreeFlowsM3H);

    readonly record struct PipeHydraulics(
        double VelocityMS,
        double ReynoldsNumber,
        double FrictionFactor,
        double PressureLossBar);
}
