namespace FramerateAnalyzer.Domain;

public class FrameRateCaptureGroup : IFrameRateBenchmarkResult
{
    public FrameRateCaptureGroup(IList<FramerateCapture> captures)
    {
        ArgumentNullException.ThrowIfNull(captures, nameof(captures));

        if (!captures.Any() ||
            captures.DistinctBy(c => c.Cpu).Count() > 1 ||
            captures.DistinctBy(c => c.Gpu).Count() > 1 ||
            captures.DistinctBy(c => c.Memory).Count() > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(captures));
        }

        FramerateCapture firstCapture = captures.First();

        Cpu = firstCapture.Cpu;
        Gpu = firstCapture.Gpu;
        Memory = firstCapture.Memory;
        Captures = captures;

        IList<FrameRateStats> captureStats = captures.Select(c => c.Stats).ToList();

        double averageGeoMean = Statistics.GeometricMean(captureStats.Select(s => s.Average));
        double tenPercentLowAverageGeoMean = Statistics.GeometricMean(captureStats.Select(s => s.TenPercentLowAverage));
        double onePercentLowAverageGeoMean = Statistics.GeometricMean(captureStats.Select(s => s.OnePercentLowAverage));
        double zeroPointOnePercentLowAverageGeoMean = Statistics.GeometricMean(captureStats.Select(s => s.ZeroPointOnePercentLowAverage));

        Stats = new FrameRateStats(averageGeoMean, tenPercentLowAverageGeoMean, onePercentLowAverageGeoMean,
            zeroPointOnePercentLowAverageGeoMean);
    }

    //public FrameRateStats RelativePerformance(FrameRateCaptureGroup reference)
    //{
    //    ArgumentNullException.ThrowIfNull(reference, nameof(reference));

    //    double average = Stats.Average / reference.Stats.Average;

    //    double tenPercentLowAverage = Stats.TenPercentLowAverage / reference.Stats.TenPercentLowAverage;

    //    double onePercentLowAverage = Stats.OnePercentLowAverage / reference.Stats.OnePercentLowAverage;

    //    double zeroPointOnePercentLowAverage = Stats.ZeroPointOnePercentLowAverage /
    //        reference.Stats.ZeroPointOnePercentLowAverage;

    //    return new FrameRateStats(average, tenPercentLowAverage, onePercentLowAverage, zeroPointOnePercentLowAverage);
    //}

    public string Cpu { get; }

    public string Gpu { get; }

    public string Memory { get; }

    public IList<FramerateCapture> Captures { get; }

    public FrameRateStats Stats { get; private set; }

    public override string ToString()
    {
        return $"{Cpu} - {Gpu} - {Memory}";
    }
}