using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer;

public record FrameratePerformanceStats
{
    public FrameratePerformanceStats(RunFrameCapture runFrameCapture)
    {
        ArgumentNullException.ThrowIfNull(runFrameCapture, nameof(runFrameCapture));

        Average = runFrameCapture.GetAverageFps(100);
        TenPercentLowAverage = runFrameCapture.GetAverageFps(10);
        OnePercentLowAverage = runFrameCapture.GetAverageFps(1);
        ZeroPointOnePercentLowAverage = runFrameCapture.GetAverageFps(0.1);
    }

    public FrameratePerformanceStats(IList<FrameratePerformanceStats> performanceStats)
    {
        ArgumentNullException.ThrowIfNull(performanceStats, nameof(performanceStats));

        Average = performanceStats.Average(s => s.Average);
        TenPercentLowAverage = performanceStats.Average(s => s.TenPercentLowAverage);
        OnePercentLowAverage = performanceStats.Average(s => s.OnePercentLowAverage);
        ZeroPointOnePercentLowAverage = performanceStats.Average(s => s.ZeroPointOnePercentLowAverage);
    }

    public FrameratePerformanceStats(IList<FramerateCapture> framerateCaptures)
    {
        ArgumentNullException.ThrowIfNull(framerateCaptures, nameof(framerateCaptures));

        IList<FrameratePerformanceStats> aggregatedStats = framerateCaptures.Select(c => c.AggregatesRunStats).ToList();

        Average = Statistics.GeometricMean(aggregatedStats.Select(c => c.Average));
        TenPercentLowAverage = Statistics.GeometricMean(aggregatedStats.Select(c => c.TenPercentLowAverage));
        OnePercentLowAverage = Statistics.GeometricMean(aggregatedStats.Select(c => c.OnePercentLowAverage));
        ZeroPointOnePercentLowAverage = Statistics.GeometricMean(aggregatedStats.Select(c => c.ZeroPointOnePercentLowAverage));
    }

    public FrameratePerformanceStats(FrameratePerformanceStats performanceStats, FrameratePerformanceStats performanceStatsReference)
    {
        Average = performanceStats.Average /
            performanceStatsReference.Average * 100.0;

        TenPercentLowAverage = performanceStats.TenPercentLowAverage /
            performanceStatsReference.TenPercentLowAverage * 100.0;

        OnePercentLowAverage = performanceStats.OnePercentLowAverage /
            performanceStatsReference.OnePercentLowAverage * 100.0;

        ZeroPointOnePercentLowAverage = performanceStats.ZeroPointOnePercentLowAverage /
            performanceStatsReference.ZeroPointOnePercentLowAverage * 100.0;
    }

    public double Average { get; }

    public double TenPercentLowAverage { get; }

    public double OnePercentLowAverage { get; }

    public double ZeroPointOnePercentLowAverage { get; }

    public double AggregatedPerformance()
    {
        List<double> values = 
        [
            Average,
            Average,
            Average,
            Average,
            TenPercentLowAverage,
            TenPercentLowAverage,
            TenPercentLowAverage,
            OnePercentLowAverage,
            OnePercentLowAverage,
            ZeroPointOnePercentLowAverage
        ];

        return Statistics.GeometricMean(values);
    }
};