using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FPSAnalizer;

public class FramerateAggregatedPerformance
{
    public FramerateAggregatedPerformance(IList<FramerateCapture> framerateCaptures)
    {
        if (framerateCaptures == null)
            throw new ArgumentNullException(nameof(framerateCaptures));

        if (!framerateCaptures.Any() ||
            framerateCaptures.DistinctBy(c => c.Cpu).Count() > 1 ||
            framerateCaptures.DistinctBy(c => c.Gpu).Count() > 1 ||
            framerateCaptures.DistinctBy(c => c.Memory).Count() > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(framerateCaptures));
        }

        Func<FramerateCapture, string> modelSelector =
            framerateCaptures.DistinctBy(c => c.Cpu).Count() > 1 ? c => c.Cpu : c => c.Gpu;

        FramerateCapture firstCapture = framerateCaptures.First();

        Cpu = firstCapture.Cpu;
        Gpu = firstCapture.Cpu;
        Memory = firstCapture.Cpu;
        FramerateCaptures = framerateCaptures;

        FramerateGeomeanStats = new FrameratePerformanceStats(
            GeometricMean(framerateCaptures.Select(c => c.AverageFramerate)),
            GeometricMean(framerateCaptures.Select(c => c.TenPercentLowFramerate)),
            GeometricMean(framerateCaptures.Select(c => c.OnePercentLowFramerate)),
            GeometricMean(framerateCaptures.Select(c => c.ZeroPointOnePercentLowFramerate)));
    }

    private decimal GeometricMean(IEnumerable<decimal> values)
    {
        return (decimal)Statistics.GeometricMean(values.Select(v => (double)v));
    }

    public FrameratePerformanceStats RelativePerformance(FramerateAggregatedPerformance performanceReference)
    {
        if (performanceReference == null)
            throw new ArgumentNullException(nameof(performanceReference));

        return new FrameratePerformanceStats(
            FramerateGeomeanStats.Average /
                performanceReference.FramerateGeomeanStats.Average * 100m,
            FramerateGeomeanStats.TenPercentLowAverage /
                performanceReference.FramerateGeomeanStats.TenPercentLowAverage * 100m,
            FramerateGeomeanStats.OnePercentLowAverage /
                performanceReference.FramerateGeomeanStats.OnePercentLowAverage * 100m,
            FramerateGeomeanStats.ZeroPointOnePercentLowAverage /
                performanceReference.FramerateGeomeanStats.ZeroPointOnePercentLowAverage * 100m);
    }

    public string Cpu { get; set; }

    public string Gpu { get; set; }

    public string Memory { get; set; }

    public IList<FramerateCapture> FramerateCaptures { get; set; }

    public FrameratePerformanceStats FramerateGeomeanStats { get; set; }
}