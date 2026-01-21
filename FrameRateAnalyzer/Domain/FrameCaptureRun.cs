namespace FramerateAnalyzer.Domain;

public record FrameCaptureRun
{
    public FrameCaptureRun(string hash, IList<double> frameTimes)
    {
        ArgumentOutOfRangeException.ThrowIfNullOrWhiteSpace(hash, nameof(hash));

        if (frameTimes == null || !frameTimes.Any())
            throw new ArgumentOutOfRangeException(nameof(frameTimes));

        Hash = hash;
        FrameTimes = frameTimes;
        SetStats();
    }

    public string Hash { get; }

    public IList<double> FrameTimes { get; private set; }

    public FrameRateStats Stats { get; private set; }

    private void SetStats(/*double duration*/)
    {
        var sortedFrameTimes = new List<double>(FrameTimes);
        sortedFrameTimes.Sort((a, b) => b.CompareTo(a));

        double average = GetFramerateLowAverage(sortedFrameTimes, 1.0);
        double tenPercentLowAverage = GetFramerateLowAverage(sortedFrameTimes, 0.1);
        double onePercentLowAverage = GetFramerateLowAverage(sortedFrameTimes, 0.01);
        double zeroPointOnePercentLowAverage = GetFramerateLowAverage(sortedFrameTimes, 0.001);

        Stats = new FrameRateStats(average, tenPercentLowAverage, onePercentLowAverage, zeroPointOnePercentLowAverage);
    }

    private static double GetFramerateLowAverage(List<double> sortedFrameTimes, double frameCountFactor)
    {
        int frameCount = (int)Math.Round(sortedFrameTimes.Count * frameCountFactor);

        if (frameCount == 0)
            return double.NaN;

        double averageFrameTime = sortedFrameTimes.Take(frameCount).Average();

        return 1000.0 / averageFrameTime;
    }
}
