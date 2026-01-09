using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer;

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

        FramerateGeomeanStats = new FrameratePerformanceStats(framerateCaptures);
    }

    public FrameratePerformanceStats RelativePerformance(FramerateAggregatedPerformance performanceReference)
    {
        if (performanceReference == null)
            throw new ArgumentNullException(nameof(performanceReference));

        return new FrameratePerformanceStats(FramerateGeomeanStats, performanceReference.FramerateGeomeanStats);
    }

    public string Cpu { get; set; }

    public string Gpu { get; set; }

    public string Memory { get; set; }

    public IList<FramerateCapture> FramerateCaptures { get; set; }

    public FrameratePerformanceStats FramerateGeomeanStats { get; set; }
}