namespace FramerateAnalyzer.Domain
{
    public interface IFrameRateBenchmarkResult
    {
        string Cpu { get; }

        string Gpu { get; }

        string Memory { get; }

        FrameRateStats Stats { get; }
    }
}