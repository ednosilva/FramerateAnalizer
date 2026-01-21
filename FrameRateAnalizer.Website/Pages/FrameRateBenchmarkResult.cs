namespace FramerateAnalyzer.Domain
{
    public record FrameRateBenchmarkResult(IList<string> BenchmarkedParts, FrameRateStats Stats);
}