using MathNet.Numerics.Statistics;
using System.Collections.Generic;

namespace FPSAnalizer;

public record class FrameratePerformanceStats(decimal Average, decimal TenPercentLowAverage,
    decimal OnePercentLowAverage, decimal ZeroPointOnePercentLowAverage)
{
    public decimal AggregatedPerformance()
    {
        IList<double> values = [(double)Average, (double)TenPercentLowAverage,
            (double)OnePercentLowAverage, (double)ZeroPointOnePercentLowAverage];

        return (decimal)Statistics.GeometricMean(values);
    }
};