using System.Globalization;
using System.Text;

namespace AmsRecords.GraphAnalysis;

public static class GraphAnalysisSeriesCatalog
{
    static readonly IReadOnlyDictionary<string, GraphAnalysisSeriesDefinition> Definitions =
        new Dictionary<string, GraphAnalysisSeriesDefinition>(StringComparer.Ordinal)
        {
            ["temperature"] = new("Mean temperature", "weather", ["mean temperature", "air temperature", "temperature", "temp"]),
            ["rainfall"] = new("Rainfall", "weather", ["rain events", "rain event", "rainfall", "precipitation", "rain"]),
            ["humidity"] = new("Mean humidity", "weather", ["mean humidity", "relative humidity", "humidity"]),
            ["growthPotential"] = new("Growth potential", "plant", ["growth potential"]),
            ["greenSpeed"] = new("Green speed", "performance", ["green speed", "ball roll", "stimpmeter"]),
            ["clippingVolume"] = new("Clipping daily rate", "performance", ["clipping daily rate", "clipping rate", "clipping volume", "clipping yield", "clippings"]),
            ["leafNitrate"] = new("Leaf nitrate", "plant", ["leaf nitrate", "nitrate"]),
            ["cuttingHeight"] = new("Cutting height", "practice", ["cutting height", "mowing height", "height of cut"]),
            ["riskDollarSpot"] = new("Dollar Spot risk", "risk", ["dollar spot risk", "dollar spot"]),
            ["riskPythium"] = new("Pythium risk", "risk", ["pythium risk", "pythium"]),
            ["riskMicrodochium"] = new("Microdochium risk", "risk", ["microdochium risk", "microdochium"]),
            ["riskDew"] = new("Dew risk", "risk", ["dew risk", "dew"]),
            ["riskFrost"] = new("Frost risk", "risk", ["frost risk", "frost"]),
            ["riskHeatStress"] = new("Heat stress risk", "risk", ["heat stress risk", "heat stress"]),
            ["riskDollarSpotExperimental"] = new("Dollar Spot early warning (experimental)", "risk", ["dollar spot early warning", "experimental dollar spot"])
        };

    public static bool TryGet(string key, out GraphAnalysisSeriesDefinition definition)
        => Definitions.TryGetValue(key, out definition!);

    public static GraphAnalysisSeriesDefinition Get(string key)
        => Definitions.TryGetValue(key, out var definition)
            ? definition
            : new GraphAnalysisSeriesDefinition(key, "other", [key]);
}

public sealed record GraphAnalysisSeriesDefinition(
    string Label,
    string Group,
    IReadOnlyList<string> Aliases);

public static class GraphRelationshipAnalyzer
{
    const int MinimumSampleSize = 8;
    const int MinimumObservationSpanDays = 28;
    const int ClearSampleSize = 10;
    const int VerificationMinimumSampleSize = 20;
    const int VerificationMinimumObservationSpanDays = 90;
    const decimal ClearCoefficient = 0.65m;
    const decimal PossibleCoefficient = 0.50m;

    public static GraphAnalysisAskResponseDto Analyze(GraphAnalysisAskRequestDto request)
    {
        var availableSeries = request.Series
            .Where(x => GraphAnalysisSeriesCatalog.TryGet(x.Key, out _))
            .GroupBy(x => x.Key, StringComparer.Ordinal)
            .Select(x => x.First())
            .ToList();
        var isVerification = request.Mode == GraphAnalysisModes.HistoricalVerification;
        var route = isVerification
            ? RouteVerification(request.RequestedSeriesKeys, availableSeries)
            : RouteQuestion(request.Question, availableSeries);
        if (route.Kind == QuestionRouteKind.OutOfScope)
            return OutOfScope(availableSeries);

        var candidates = BuildCandidates(availableSeries, route, isVerification);
        var best = candidates
            .OrderByDescending(x => EvidenceRank(x.EvidenceLevel))
            .ThenByDescending(x => Math.Abs(x.Coefficient))
            .ThenByDescending(x => x.SampleSize)
            .FirstOrDefault();

        var minimumSampleSize = isVerification ? VerificationMinimumSampleSize : MinimumSampleSize;
        var minimumSpanDays = isVerification ? VerificationMinimumObservationSpanDays : MinimumObservationSpanDays;
        if (best is null ||
            best.EvidenceLevel == GraphAnalysisEvidenceLevels.Insufficient ||
            best.SampleSize < minimumSampleSize ||
            best.ObservationSpanDays < minimumSpanDays)
            return Insufficient(availableSeries, best, route, isVerification);

        if (isVerification &&
            best.EvidenceLevel is GraphAnalysisEvidenceLevels.Clear or GraphAnalysisEvidenceLevels.Possible &&
            request.ExpectedDirection is "positive" or "negative" &&
            best.Direction != request.ExpectedDirection)
        {
            return AddVerification(RelationshipResponse(
                GraphAnalysisOutcomes.ConflictingRelationship,
                "Broader history contradicts the selected-period direction",
                $"The broader available history shows a {best.Strength} {DirectionText(best.Direction)} association between {best.SourceSeriesLabel} and {best.TargetSeriesLabel}, opposite to the selected-period result. It is based on {best.SampleSize} aligned days spanning {best.ObservationSpanDays} days. The original pattern is therefore not replicated and may have been period-specific or coincidental. This is not evidence of causation.",
                best,
                availableSeries,
                focusGraph: false), request, best);
        }

        var result = best.EvidenceLevel switch
        {
            GraphAnalysisEvidenceLevels.Clear => RelationshipResponse(
                GraphAnalysisOutcomes.ClearRelationship,
                isVerification ? "Relationship replicated in broader history" : "Clear relationship found",
                BuildClearAnswer(best, isVerification),
                best,
                availableSeries),
            GraphAnalysisEvidenceLevels.Possible => RelationshipResponse(
                GraphAnalysisOutcomes.PossiblePattern,
                isVerification ? "Broader history gives limited support" : "Possible pattern — evidence is limited",
                BuildPossibleAnswer(best, isVerification),
                best,
                availableSeries),
            _ => RelationshipResponse(
                GraphAnalysisOutcomes.NoMeaningfulRelationship,
                isVerification ? "Relationship not replicated in broader history" : "No clear relationship detected",
                BuildNoRelationshipAnswer(best, isVerification),
                best,
                availableSeries,
                focusGraph: false)
        };
        return isVerification ? AddVerification(result, request, best) : result;
    }

    static QuestionRoute RouteVerification(
        IReadOnlyList<string> requestedSeriesKeys,
        IReadOnlyList<GraphAnalysisSeriesDto> series)
    {
        var keys = (requestedSeriesKeys ?? [])
            .Where(key => GraphAnalysisSeriesCatalog.TryGet(key, out _))
            .Where(key => series.Any(item => item.Key == key))
            .Distinct(StringComparer.Ordinal)
            .Take(2)
            .ToList();
        return keys.Count == 2
            ? new QuestionRoute(QuestionRouteKind.SelectedPair, keys, true)
            : QuestionRoute.OutOfScope;
    }

    static QuestionRoute RouteQuestion(string question, IReadOnlyList<GraphAnalysisSeriesDto> series)
    {
        var normalized = Normalize(question);
        if (!ContainsRelationshipIntent(normalized))
            return QuestionRoute.OutOfScope;

        var mentions = new List<(string Key, int Index)>();
        foreach (var item in series)
        {
            var definition = GraphAnalysisSeriesCatalog.Get(item.Key);
            var aliases = definition.Aliases;
            if (item.Key == "riskDollarSpotExperimental" &&
                !normalized.Contains("early warning", StringComparison.Ordinal) &&
                !normalized.Contains("experimental", StringComparison.Ordinal))
            {
                continue;
            }

            var index = aliases
                .Select(alias => PaddedIndexOf(normalized, Normalize(alias)))
                .Where(value => value >= 0)
                .DefaultIfEmpty(-1)
                .Min();
            if (index >= 0)
                mentions.Add((item.Key, index));
        }

        var keys = mentions
            .OrderBy(x => x.Index)
            .Select(x => x.Key)
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var searchLags = ContainsAny(normalized,
            "rain event", "after", "before", "precede", "preceded", "follow", "following", "lag", "delay", "delayed");

        if (keys.Count >= 2)
            return new QuestionRoute(QuestionRouteKind.SelectedPair, keys.Take(2).ToList(), searchLags);

        if (keys.Count == 1 && ContainsAny(normalized,
                "any other", "anything else", "what else", "which other", "which value", "most related", "strongest relation", "strongest relationship"))
        {
            return new QuestionRoute(QuestionRouteKind.OneAgainstAll, keys, searchLags);
        }

        if (keys.Count == 0 && ContainsAny(normalized,
                "any relationship", "any relation", "strongest relationship", "strongest relation", "relationships in the graph", "relations in the graph", "relationships in this graph", "relations in this graph"))
        {
            return new QuestionRoute(QuestionRouteKind.AllPairs, [], searchLags);
        }

        return QuestionRoute.OutOfScope;
    }

    static IReadOnlyList<GraphAnalysisRelationshipDto> BuildCandidates(
        IReadOnlyList<GraphAnalysisSeriesDto> series,
        QuestionRoute route,
        bool historicalVerification)
    {
        var pairs = new List<(GraphAnalysisSeriesDto Source, GraphAnalysisSeriesDto Target)>();
        if (route.Kind == QuestionRouteKind.SelectedPair)
        {
            var first = series.FirstOrDefault(x => x.Key == route.SeriesKeys[0]);
            var second = series.FirstOrDefault(x => x.Key == route.SeriesKeys[1]);
            if (first is not null && second is not null && first.FieldPubId == second.FieldPubId)
                pairs.Add(OrderPair(first, second, route.SearchLags));
        }
        else if (route.Kind == QuestionRouteKind.OneAgainstAll)
        {
            var selected = series.FirstOrDefault(x => x.Key == route.SeriesKeys[0]);
            if (selected is not null)
            {
                pairs.AddRange(series
                    .Where(x => x.FieldPubId == selected.FieldPubId && x.Key != selected.Key)
                    .Select(x => OrderPair(selected, x, route.SearchLags)));
            }
        }
        else
        {
            foreach (var fieldGroup in series.GroupBy(x => x.FieldPubId))
            {
                var items = fieldGroup.ToList();
                for (var i = 0; i < items.Count; i++)
                for (var j = i + 1; j < items.Count; j++)
                    pairs.Add(OrderPair(items[i], items[j], route.SearchLags));
            }
        }

        var broadSearch = route.Kind != QuestionRouteKind.SelectedPair;
        return pairs
            .Select(pair => AnalyzePair(pair.Source, pair.Target, route.SearchLags, broadSearch, historicalVerification))
            .Where(x => x is not null)
            .Cast<GraphAnalysisRelationshipDto>()
            .ToList();
    }

    static (GraphAnalysisSeriesDto Source, GraphAnalysisSeriesDto Target) OrderPair(
        GraphAnalysisSeriesDto first,
        GraphAnalysisSeriesDto second,
        bool searchLags)
    {
        if (!searchLags)
            return (first, second);

        var firstPriority = SourcePriority(first.Group);
        var secondPriority = SourcePriority(second.Group);
        return firstPriority <= secondPriority ? (first, second) : (second, first);
    }

    static int SourcePriority(string group)
        => group switch
        {
            "weather" => 0,
            "practice" => 1,
            "plant" => 2,
            "performance" => 3,
            "risk" => 4,
            _ => 5
        };

    static GraphAnalysisRelationshipDto? AnalyzePair(
        GraphAnalysisSeriesDto source,
        GraphAnalysisSeriesDto target,
        bool searchLags,
        bool broadSearch,
        bool historicalVerification)
    {
        var lags = searchLags ? Enumerable.Range(0, 8) : [0];
        PairStatistics? best = null;
        foreach (var lag in lags)
        {
            var statistics = CalculateStatistics(source, target, lag, broadSearch || searchLags, historicalVerification);
            if (statistics is null)
                continue;

            if (best is null ||
                EvidenceRank(statistics.EvidenceLevel) > EvidenceRank(best.EvidenceLevel) ||
                (EvidenceRank(statistics.EvidenceLevel) == EvidenceRank(best.EvidenceLevel) &&
                 Math.Abs(statistics.Coefficient) > Math.Abs(best.Coefficient)))
            {
                best = statistics;
            }
        }

        if (best is null)
            return null;

        return new GraphAnalysisRelationshipDto(
            source.FieldPubId,
            source.Key,
            GraphAnalysisSeriesCatalog.Get(source.Key).Label,
            target.Key,
            GraphAnalysisSeriesCatalog.Get(target.Key).Label,
            best.LagDays,
            decimal.Round(best.Coefficient, 3),
            best.SampleSize,
            best.ObservationSpanDays,
            decimal.Round(best.Stability, 2),
            best.Coefficient >= 0 ? "positive" : "negative",
            Strength(Math.Abs(best.Coefficient)),
            best.EvidenceLevel,
            EvidenceScore(best, historicalVerification),
            BuildEvidence(source.Key, target.Key, best),
            historicalVerification
                ? BuildComparableOccasions(best)
                : []);
    }

    static PairStatistics? CalculateStatistics(
        GraphAnalysisSeriesDto source,
        GraphAnalysisSeriesDto target,
        int lagDays,
        bool searchPenalty,
        bool historicalVerification)
    {
        var targetByDate = target.Points
            .GroupBy(x => x.Date)
            .ToDictionary(x => x.Key, x => x.Average(y => y.Value));
        var pairs = source.Points
            .GroupBy(x => x.Date)
            .Select(x => new { Date = x.Key, Value = x.Average(y => y.Value) })
            .Select(x => new PairValue(
                x.Date,
                x.Date.AddDays(lagDays),
                x.Value,
                targetByDate.GetValueOrDefault(x.Date.AddDays(lagDays))))
            .Where(x => targetByDate.ContainsKey(x.TargetDate))
            .ToList();

        if (pairs.Count < 3 ||
            pairs.Select(x => x.SourceValue).Distinct().Count() < 4 ||
            pairs.Select(x => x.TargetValue).Distinct().Count() < 4)
        {
            return new PairStatistics(lagDays, 0m, pairs.Count, ObservationSpanDays(pairs), 0m, GraphAnalysisEvidenceLevels.Insufficient, pairs, [], [], []);
        }

        var sourceRanks = Rank(pairs.Select(x => x.SourceValue).ToList());
        var targetRanks = Rank(pairs.Select(x => x.TargetValue).ToList());
        var coefficient = Pearson(sourceRanks, targetRanks);
        var leaveOneOut = new List<decimal>();
        for (var excluded = 0; excluded < pairs.Count; excluded++)
        {
            var sourceSubset = sourceRanks.Where((_, index) => index != excluded).ToList();
            var targetSubset = targetRanks.Where((_, index) => index != excluded).ToList();
            leaveOneOut.Add(Pearson(sourceSubset, targetSubset));
        }

        var sign = Math.Sign(coefficient);
        var stableCount = leaveOneOut.Count(value =>
            Math.Sign(value) == sign && Math.Abs(value) >= 0.40m);
        var stability = leaveOneOut.Count == 0 ? 0m : (decimal)stableCount / leaveOneOut.Count;
        var minimumLeaveOneOut = leaveOneOut.Count == 0 ? 0m : leaveOneOut.Min(value => Math.Abs(value));
        var absolute = Math.Abs(coefficient);
        var clearCoefficient = searchPenalty ? 0.70m : ClearCoefficient;
        var clearSampleSize = searchPenalty ? 12 : ClearSampleSize;
        var possibleCoefficient = searchPenalty ? 0.58m : PossibleCoefficient;
        var minimumSampleSize = historicalVerification ? VerificationMinimumSampleSize : MinimumSampleSize;
        var minimumSpanDays = historicalVerification ? VerificationMinimumObservationSpanDays : MinimumObservationSpanDays;
        var observationSpanDays = ObservationSpanDays(pairs);

        var evidenceLevel = pairs.Count < minimumSampleSize || observationSpanDays < minimumSpanDays
            ? GraphAnalysisEvidenceLevels.Insufficient
            : pairs.Count >= clearSampleSize && absolute >= clearCoefficient && stability >= 0.85m && minimumLeaveOneOut >= 0.40m
                ? GraphAnalysisEvidenceLevels.Clear
                : absolute >= possibleCoefficient && stability >= 0.65m
                    ? GraphAnalysisEvidenceLevels.Possible
                    : GraphAnalysisEvidenceLevels.None;

        var contributions = Contributions(sourceRanks, targetRanks, coefficient);
        var supporting = contributions
            .Where(x => x.Score >= 0)
            .OrderByDescending(x => Math.Abs(x.Score))
            .Take(5)
            .Select(x => x.Index)
            .ToList();
        var contradicting = contributions
            .Where(x => x.Score < 0)
            .OrderByDescending(x => Math.Abs(x.Score))
            .Take(3)
            .Select(x => x.Index)
            .ToList();

        return new PairStatistics(
            lagDays,
            coefficient,
            pairs.Count,
            observationSpanDays,
            stability,
            evidenceLevel,
            pairs,
            supporting,
            contradicting,
            contributions.OrderBy(item => item.Index).Select(item => item.Score).ToList());
    }

    static int EvidenceScore(PairStatistics statistics, bool historicalVerification)
    {
        var sampleTarget = historicalVerification ? VerificationMinimumSampleSize : ClearSampleSize;
        var spanTarget = historicalVerification ? VerificationMinimumObservationSpanDays : MinimumObservationSpanDays;
        var raw = Math.Abs(statistics.Coefficient) * 45m
                  + statistics.Stability * 30m
                  + Math.Min(1m, (decimal)statistics.SampleSize / sampleTarget) * 15m
                  + Math.Min(1m, (decimal)statistics.ObservationSpanDays / spanTarget) * 10m;
        var rounded = Math.Clamp((int)Math.Round(raw, MidpointRounding.AwayFromZero), 0, 100);
        return statistics.EvidenceLevel switch
        {
            GraphAnalysisEvidenceLevels.Insufficient => Math.Min(39, rounded),
            GraphAnalysisEvidenceLevels.None => Math.Min(49, rounded),
            GraphAnalysisEvidenceLevels.Possible => Math.Clamp(rounded, 50, 74),
            GraphAnalysisEvidenceLevels.Clear => Math.Max(75, rounded),
            _ => rounded
        };
    }

    static IReadOnlyList<GraphAnalysisOccasionDto> BuildComparableOccasions(PairStatistics statistics)
        => statistics.Pairs
            .Select((pair, index) => new GraphAnalysisOccasionDto(
                statistics.ContributionScores.ElementAtOrDefault(index) switch
                {
                    > 0m => GraphAnalysisEvidenceKinds.Supporting,
                    < 0m => GraphAnalysisEvidenceKinds.Contradicting,
                    _ => GraphAnalysisEvidenceKinds.Neutral
                },
                pair.SourceDate,
                pair.TargetDate,
                pair.SourceValue,
                pair.TargetValue))
            .OrderByDescending(item => item.SourceDate)
            .Take(60)
            .ToList();

    static GraphAnalysisAskResponseDto AddVerification(
        GraphAnalysisAskResponseDto result,
        GraphAnalysisAskRequestDto request,
        GraphAnalysisRelationshipDto relationship)
    {
        var previous = Math.Clamp(request.ExpectedEvidenceScore ?? 0, 0, 100);
        var current = relationship.EvidenceScore;
        var change = current - previous;
        var direction = change switch
        {
            > 2 => "raised",
            < -2 => "lowered",
            _ => "unchanged"
        };
        var occasions = relationship.ComparableOccasions;
        var supporting = occasions.Count(item => item.Kind == GraphAnalysisEvidenceKinds.Supporting);
        var contradicting = occasions.Count(item => item.Kind == GraphAnalysisEvidenceKinds.Contradicting);
        var neutral = occasions.Count - supporting - contradicting;
        var summary = direction == "unchanged"
            ? $"Broader history left field-evidence confidence at {current}/100. It found {supporting} supporting and {contradicting} contradicting comparable occasions."
            : $"Broader history {direction} field-evidence confidence from {previous}/100 to {current}/100. It found {supporting} supporting and {contradicting} contradicting comparable occasions.";
        return result with
        {
            Verification = new GraphAnalysisVerificationDto(
                previous,
                current,
                change,
                direction,
                supporting,
                contradicting,
                neutral,
                summary,
                occasions)
        };
    }

    static int ObservationSpanDays(IReadOnlyList<PairValue> pairs)
        => pairs.Count == 0
            ? 0
            : pairs.Max(pair => pair.TargetDate).DayNumber - pairs.Min(pair => pair.SourceDate).DayNumber + 1;

    static IReadOnlyList<GraphAnalysisEvidenceDto> BuildEvidence(
        string sourceKey,
        string targetKey,
        PairStatistics statistics)
    {
        var evidence = new List<GraphAnalysisEvidenceDto>();
        evidence.AddRange(statistics.SupportingIndices.Select(index => ToEvidence(
            GraphAnalysisEvidenceKinds.Supporting, sourceKey, targetKey, statistics.Pairs[index])));
        evidence.AddRange(statistics.ContradictingIndices.Select(index => ToEvidence(
            GraphAnalysisEvidenceKinds.Contradicting, sourceKey, targetKey, statistics.Pairs[index])));
        return evidence;
    }

    static GraphAnalysisEvidenceDto ToEvidence(string kind, string sourceKey, string targetKey, PairValue pair)
        => new(
            kind,
            sourceKey,
            targetKey,
            pair.SourceDate,
            pair.TargetDate,
            pair.SourceValue,
            pair.TargetValue);

    static IReadOnlyList<(int Index, decimal Score)> Contributions(
        IReadOnlyList<decimal> sourceRanks,
        IReadOnlyList<decimal> targetRanks,
        decimal coefficient)
    {
        var sourceMean = sourceRanks.Average();
        var targetMean = targetRanks.Average();
        var sign = coefficient >= 0 ? 1m : -1m;
        return sourceRanks
            .Select((value, index) => (Index: index, Score: (value - sourceMean) * (targetRanks[index] - targetMean) * sign))
            .ToList();
    }

    static IReadOnlyList<decimal> Rank(IReadOnlyList<decimal> values)
    {
        var ranks = new decimal[values.Count];
        var ordered = values
            .Select((value, index) => (Value: value, Index: index))
            .OrderBy(x => x.Value)
            .ToList();
        var cursor = 0;
        while (cursor < ordered.Count)
        {
            var end = cursor + 1;
            while (end < ordered.Count && ordered[end].Value == ordered[cursor].Value)
                end++;

            var averageRank = ((cursor + 1) + end) / 2m;
            for (var i = cursor; i < end; i++)
                ranks[ordered[i].Index] = averageRank;
            cursor = end;
        }

        return ranks;
    }

    static decimal Pearson(IReadOnlyList<decimal> x, IReadOnlyList<decimal> y)
    {
        if (x.Count != y.Count || x.Count < 2)
            return 0m;
        var xMean = x.Average();
        var yMean = y.Average();
        decimal numerator = 0m;
        decimal xSquares = 0m;
        decimal ySquares = 0m;
        for (var i = 0; i < x.Count; i++)
        {
            var xDelta = x[i] - xMean;
            var yDelta = y[i] - yMean;
            numerator += xDelta * yDelta;
            xSquares += xDelta * xDelta;
            ySquares += yDelta * yDelta;
        }

        if (xSquares == 0m || ySquares == 0m)
            return 0m;
        return numerator / (decimal)Math.Sqrt((double)(xSquares * ySquares));
    }

    static GraphAnalysisAskResponseDto OutOfScope(IReadOnlyList<GraphAnalysisSeriesDto> series)
        => new(
            GraphAnalysisOutcomes.OutOfScope,
            "This question is outside the graph",
            "I can only answer questions about relationships between values available in this graph. Try naming two displayed values, or ask which value has the strongest relationship with another displayed value.",
            [],
            [],
            null,
            SuggestedQuestions(series));

    static GraphAnalysisAskResponseDto Insufficient(
        IReadOnlyList<GraphAnalysisSeriesDto> series,
        GraphAnalysisRelationshipDto? relationship,
        QuestionRoute route,
        bool historicalVerification)
    {
        var labels = ResolveRequestedLabels(series, relationship, route);
        var countText = relationship is null
            ? "There are too few varied, overlapping observations."
            : $"The best comparison has {relationship.SampleSize} aligned days spanning {relationship.ObservationSpanDays} days, but there are too few varied observations or too little stable overlap.";
        var minimumSampleSize = historicalVerification ? VerificationMinimumSampleSize : MinimumSampleSize;
        var minimumSpanDays = historicalVerification ? VerificationMinimumObservationSpanDays : MinimumObservationSpanDays;
        return new GraphAnalysisAskResponseDto(
            GraphAnalysisOutcomes.InsufficientData,
            historicalVerification ? "Broader history is still insufficient" : "Not enough evidence",
            $"There is not enough data to assess a clear relationship between {labels}. {countText} At least {minimumSampleSize} varied, aligned observations spanning {minimumSpanDays} days are required. This does not prove that no relationship exists.",
            [],
            [],
            relationship,
            SuggestedQuestions(series));
    }

    static string ResolveRequestedLabels(
        IReadOnlyList<GraphAnalysisSeriesDto> series,
        GraphAnalysisRelationshipDto? relationship,
        QuestionRoute route)
    {
        if (relationship is not null)
            return $"{relationship.SourceSeriesLabel} and {relationship.TargetSeriesLabel}";
        var labels = route.SeriesKeys
            .Select(key => GraphAnalysisSeriesCatalog.Get(key).Label)
            .ToList();
        return labels.Count switch
        {
            0 => "the requested graph values",
            1 => $"{labels[0]} and the other graph values",
            _ => string.Join(" and ", labels.Take(2))
        };
    }

    static GraphAnalysisAskResponseDto RelationshipResponse(
        string outcome,
        string title,
        string answer,
        GraphAnalysisRelationshipDto relationship,
        IReadOnlyList<GraphAnalysisSeriesDto> series,
        bool focusGraph = true)
        => new(
            outcome,
            title,
            answer,
            focusGraph ? [relationship.FieldPubId] : [],
            focusGraph ? [relationship.SourceSeriesKey, relationship.TargetSeriesKey] : [],
            relationship,
            SuggestedQuestions(series));

    static string BuildClearAnswer(GraphAnalysisRelationshipDto relationship, bool historicalVerification)
    {
        var timing = Timing(relationship);
        var scope = historicalVerification ? "The broader available history" : "The selected data";
        return $"{scope} show a {relationship.Strength} {DirectionText(relationship.Direction)} association between {relationship.SourceSeriesLabel} and {relationship.TargetSeriesLabel}{timing}. It is based on {relationship.SampleSize} aligned days spanning {relationship.ObservationSpanDays} days and remained stable when individual observations were removed. This is an association, not evidence that one value caused the other.";
    }

    static string BuildPossibleAnswer(GraphAnalysisRelationshipDto relationship, bool historicalVerification)
    {
        var timing = Timing(relationship);
        var scope = historicalVerification ? "in the broader available history" : "in the selected period";
        return $"A possible {DirectionText(relationship.Direction)} pattern appears between {relationship.SourceSeriesLabel} and {relationship.TargetSeriesLabel}{timing} {scope}, based on {relationship.SampleSize} aligned days spanning {relationship.ObservationSpanDays} days. The pattern is not strong or stable enough to call a clear relationship. Treat it as uncertain, not as evidence of cause.";
    }

    static string BuildNoRelationshipAnswer(GraphAnalysisRelationshipDto relationship, bool historicalVerification)
        => $"No clear relationship was detected between {relationship.SourceSeriesLabel} and {relationship.TargetSeriesLabel} in {(historicalVerification ? "the broader available history" : "the selected period")}. {relationship.SampleSize} aligned days spanning {relationship.ObservationSpanDays} days were testable, but the association was too weak or unstable. This does not prove that no relationship exists.";

    static string Timing(GraphAnalysisRelationshipDto relationship)
        => relationship.LagDays == 0
            ? string.Empty
            : $", with {relationship.TargetSeriesLabel} compared {relationship.LagDays} {(relationship.LagDays == 1 ? "day" : "days")} after {relationship.SourceSeriesLabel}";

    static string DirectionText(string direction)
        => direction == "positive" ? "same-direction" : "opposite-direction";

    static string Strength(decimal absoluteCoefficient)
        => absoluteCoefficient >= 0.80m ? "strong" : absoluteCoefficient >= 0.65m ? "moderate" : "limited";

    static IReadOnlyList<string> SuggestedQuestions(IReadOnlyList<GraphAnalysisSeriesDto> series)
    {
        var keys = series.Select(x => x.Key).ToHashSet(StringComparer.Ordinal);
        var questions = new List<string>();
        if (keys.Contains("leafNitrate"))
            questions.Add("Is Leaf nitrate related to any other value?");
        if (keys.Contains("rainfall") && keys.Contains("riskDollarSpot"))
            questions.Add("Did rainfall precede changes in Dollar Spot risk?");
        questions.Add("What is the strongest relationship in this graph?");
        return questions.Take(3).ToList();
    }

    static bool ContainsRelationshipIntent(string value)
        => ContainsAny(value,
            "relation", "relationship", "related", "correlat", "association", "associated", "link", "linked", "connection", "connected", "compare", "precede", "follow", "lag", "pattern", "trend", "move together", "opposite");

    static bool ContainsAny(string value, params string[] terms)
        => terms.Any(term => value.Contains(term, StringComparison.Ordinal));

    static int PaddedIndexOf(string value, string term)
        => $" {value} ".IndexOf($" {term} ", StringComparison.Ordinal);

    static string Normalize(string value)
    {
        var decomposed = (value ?? string.Empty).Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        var previousWasSpace = true;
        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                continue;
            var normalized = char.IsLetterOrDigit(character) ? char.ToLowerInvariant(character) : ' ';
            if (normalized == ' ' && previousWasSpace)
                continue;
            builder.Append(normalized);
            previousWasSpace = normalized == ' ';
        }

        return builder.ToString().Trim();
    }

    static int EvidenceRank(string level)
        => level switch
        {
            GraphAnalysisEvidenceLevels.Clear => 3,
            GraphAnalysisEvidenceLevels.Possible => 2,
            GraphAnalysisEvidenceLevels.None => 1,
            _ => 0
        };

    sealed record QuestionRoute(QuestionRouteKind Kind, IReadOnlyList<string> SeriesKeys, bool SearchLags)
    {
        public static QuestionRoute OutOfScope { get; } = new(QuestionRouteKind.OutOfScope, [], false);
    }

    enum QuestionRouteKind
    {
        OutOfScope,
        SelectedPair,
        OneAgainstAll,
        AllPairs
    }

    sealed record PairValue(
        DateOnly SourceDate,
        DateOnly TargetDate,
        decimal SourceValue,
        decimal TargetValue);

    sealed record PairStatistics(
        int LagDays,
        decimal Coefficient,
        int SampleSize,
        int ObservationSpanDays,
        decimal Stability,
        string EvidenceLevel,
        IReadOnlyList<PairValue> Pairs,
        IReadOnlyList<int> SupportingIndices,
        IReadOnlyList<int> ContradictingIndices,
        IReadOnlyList<decimal> ContributionScores);
}
