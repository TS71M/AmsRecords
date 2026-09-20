namespace AmsRecords.Irrigation;

/// <summary>Non-negative per-head runtime fit to a uniform depth, using per-minute contribution grids.</summary>
public static class IrrigationRuntimeBalancer
{
    public static double BestRuntime(IReadOnlyList<double> rate, IReadOnlyList<double> otherDepth,
        IReadOnlyList<bool> mask, double target = 1)
    {
        double numerator = 0, denominator = 0;
        for (var i = 0; i < mask.Count; i++)
        {
            var weight = mask[i] ? 1d : .05d;
            numerator += weight * rate[i] * ((mask[i] ? target : 0) - otherDepth[i]);
            denominator += weight * rate[i] * rate[i];
        }
        return denominator <= 1e-15 ? 0 : Math.Max(0, numerator / denominator);
    }

    public static (double[] Runtimes, double[] Depth) Fit(IReadOnlyList<double[]> rates,
        IReadOnlyList<bool> mask, double target, CancellationToken ct = default)
    {
        if (!double.IsFinite(target) || target <= 0 || mask.Count == 0 || !mask.Any(x => x) || rates.Count == 0 ||
            rates.Any(r => r.Length != mask.Count || r.Any(v => !double.IsFinite(v) || v < 0)))
            throw new ArgumentException("Provide valid per-minute grids, a surface mask and a positive target depth.");
        var depth = new double[mask.Count]; var runtimes = new double[rates.Count];
        for (var pass = 0; pass < 60; pass++)
        {
            var change = 0d;
            for (var h = 0; h < rates.Count; h++)
            {
                ct.ThrowIfCancellationRequested();
                var old = runtimes[h];
                for (var i = 0; i < depth.Length; i++) depth[i] -= rates[h][i] * old;
                runtimes[h] = BestRuntime(rates[h], depth, mask, target);
                for (var i = 0; i < depth.Length; i++) depth[i] += rates[h][i] * runtimes[h];
                change = Math.Max(change, Math.Abs(runtimes[h] - old));
            }
            if (change < 1e-8) break;
        }
        Normalize(runtimes, depth, mask, target);
        return (runtimes, depth);
    }

    public static void Normalize(double[] runtimes, double[] depth, IReadOnlyList<bool> mask, double target)
    {
        var mean = Enumerable.Range(0, mask.Count).Where(i => mask[i]).Average(i => Math.Max(0, depth[i]));
        if (mean <= 1e-12) throw new ArgumentException("These sprinklers do not reach the selected surface.");
        var scale = target / mean;
        for (var h = 0; h < runtimes.Length; h++) runtimes[h] *= scale;
        for (var i = 0; i < depth.Length; i++) depth[i] = Math.Max(0, depth[i]) * scale;
    }
}
