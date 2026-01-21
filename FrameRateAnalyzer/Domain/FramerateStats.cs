namespace FramerateAnalyzer.Domain;

public record FrameRateStats
{
    private double? aggregatedFramerate;

    public FrameRateStats(double average, double tenPercentLowAverage, double onePercentLowAverage,
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

    public double AggregatedFrameRate
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

    public FrameRateStats RelativePerformance(FrameRateStats reference)
    {
        ArgumentNullException.ThrowIfNull(reference, nameof(reference));

        double average = Average / reference.Average;
        double tenPercentLowAverage = TenPercentLowAverage / reference.TenPercentLowAverage;
        double onePercentLowAverage = OnePercentLowAverage / reference.OnePercentLowAverage;
        double zeroPointOnePercentLowAverage = ZeroPointOnePercentLowAverage / reference.ZeroPointOnePercentLowAverage;

        return new FrameRateStats(average, tenPercentLowAverage, onePercentLowAverage, zeroPointOnePercentLowAverage);
    }
};