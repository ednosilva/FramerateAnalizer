using System;
using System.Collections.Generic;

namespace FramerateAnalizer;

public record FramerateStats
{
    public FramerateStats(double average, double tenPercentLowAverage, double onePercentLowAverage,
        double zeroPointOnePercentLowAverage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(average, nameof(average));
        ArgumentOutOfRangeException.ThrowIfNegative(tenPercentLowAverage, nameof(tenPercentLowAverage));
        ArgumentOutOfRangeException.ThrowIfNegative(onePercentLowAverage, nameof(onePercentLowAverage));
        ArgumentOutOfRangeException.ThrowIfNegative(zeroPointOnePercentLowAverage, nameof(zeroPointOnePercentLowAverage));

        Average = average;
        TenPercentLowAverage = tenPercentLowAverage;
        OnePercentLowAverage = onePercentLowAverage;
        ZeroPointOnePercentLowAverage = zeroPointOnePercentLowAverage;
    }

    public double Average { get; }

    public double TenPercentLowAverage { get; }

    public double OnePercentLowAverage { get; }

    public double ZeroPointOnePercentLowAverage { get; }

    public double AggregatedFramerate()
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