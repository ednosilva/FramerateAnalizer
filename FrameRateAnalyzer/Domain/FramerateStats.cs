namespace FramerateAnalyzer.Domain;

public record FramerateStats
{
    private double? aggregatedFramerate;

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

    public double AggregatedFramerate
    {
        get
        {
            if (!aggregatedFramerate.HasValue)
            {
                List<double> valuesToAggregate =
                [
                    Average,
                    Average,
                    Average,
                    Average,
                    Average,
                    Average,
                    Average,
                    Average,
                    TenPercentLowAverage,
                    TenPercentLowAverage,
                    TenPercentLowAverage,
                    TenPercentLowAverage,
                    OnePercentLowAverage,
                    OnePercentLowAverage,
                    ZeroPointOnePercentLowAverage
                ];

                aggregatedFramerate = Statistics.GeometricMean(valuesToAggregate);
            }

            return aggregatedFramerate.Value;
        }
    }
};