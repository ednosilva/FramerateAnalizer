using System.Collections.Generic;

namespace FramerateAnalizer;

public record class FrameratePerformanceStats(double Average, double TenPercentLowAverage,
    double OnePercentLowAverage, double ZeroPointOnePercentLowAverage)
{
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