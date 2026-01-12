using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace FramerateAnalyzer.Domain;

public class FramerateCaptureGroup
{
    public FramerateCaptureGroup(IList<FramerateCapture> captures,
        Func<FramerateCaptureGroup, string> benchmarkedPartSelector)
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
        Gpu = firstCapture.Cpu;
        Memory = firstCapture.Memory;
        Captures = captures;

        BenchmarkedParts = benchmarkedPartSelector(this);

        SetStats(captures);
    }

    private void SetStats(IList<FramerateCapture> framerateCaptures)
    {
        IList<FramerateStats> captureStats = framerateCaptures.Select(c => c.Stats).ToList();

        double average = captureStats.Average(s => s.Average);
        double tenPercentLowAverage = captureStats.Average(s => s.TenPercentLowAverage);
        double onePercentLowAverage = captureStats.Average(s => s.OnePercentLowAverage);
        double zeroPointOnePercentLowAverage = captureStats.Average(s => s.ZeroPointOnePercentLowAverage);

        Stats = new FramerateStats(average, tenPercentLowAverage, onePercentLowAverage,
            zeroPointOnePercentLowAverage);
    }

    public FramerateStats RelativePerformance(FramerateCaptureGroup reference)
    {
        ArgumentNullException.ThrowIfNull(reference, nameof(reference));

        double average = Stats.Average / reference.Stats.Average;

        double tenPercentLowAverage = Stats.TenPercentLowAverage / reference.Stats.TenPercentLowAverage;

        double onePercentLowAverage = Stats.OnePercentLowAverage / reference.Stats.OnePercentLowAverage;

        double zeroPointOnePercentLowAverage = Stats.ZeroPointOnePercentLowAverage /
            reference.Stats.ZeroPointOnePercentLowAverage;

        return new FramerateStats(average, tenPercentLowAverage, onePercentLowAverage, zeroPointOnePercentLowAverage);
    }

    public string Cpu { get; }

    public string Gpu { get; }

    public string Memory { get; }

    public string BenchmarkedParts { get; }

    public IList<FramerateCapture> Captures { get; }

    public FramerateStats Stats { get; private set; }

    public override string ToString()
    {
        return $"{Cpu} - {Gpu} - {Memory}";
    }
}