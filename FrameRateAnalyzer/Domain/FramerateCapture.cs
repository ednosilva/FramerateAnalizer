namespace FramerateAnalyzer.Domain;

public class FramerateCapture : IFrameRateBenchmarkResult
{
    public FramerateCapture(string cpu, string gpu, string memory, string gameName, string captureDetails,
        IList<FrameCaptureRun> runs, DateTime captureDate)
    {
        ArgumentNullException.ThrowIfNull(runs, nameof(runs));

        Cpu = cpu;
        Gpu = gpu;
        Memory = memory;
        GameName = gameName;
        CaptureDetails = captureDetails;
        CreationDate = captureDate;
        Runs = runs;

        SetRunStats(runs);
    }

    private void SetRunStats(IList<FrameCaptureRun> runs)
    {
        IList<FrameRateStats> runStats = runs.Select(r => r.Stats).ToList();

        double average = runStats.Average(s => s.Average);
        double tenPercentLowAverage = runStats.Average(s => s.TenPercentLowAverage);
        double onePercentLowAverage = runStats.Average(s => s.OnePercentLowAverage);
        double zeroPointOnePercentLowAverage = runStats.Average(s => s.ZeroPointOnePercentLowAverage);

        Stats = new FrameRateStats(average, tenPercentLowAverage, onePercentLowAverage,
            zeroPointOnePercentLowAverage);
    }

    public string Cpu { get; protected set; }

    public string Gpu { get; protected set; }

    public string Memory { get; protected set; }

    public FrameRateStats Stats { get; protected set; }

    public string GameName { get; }

    public string CaptureDetails { get; }

    public DateTime CreationDate { get; }

    public IList<FrameCaptureRun> Runs { get; }

    public override bool Equals(object? obj)
    {
        return obj is FramerateCapture other && new
        {
            Cpu,
            Gpu,
            Memory,
            GameName,
            CaptureDetails,
            CreationDate
        }
        .Equals(new
        {
            other.Cpu,
            other.Gpu,
            other.Memory,
            other.GameName,
            other.CaptureDetails,
            other.CreationDate
        });
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Cpu, Gpu, Memory, GameName, CaptureDetails, CreationDate);
    }

    public override string ToString()
    {
        return $"{GameName} - {CaptureDetails} - {Cpu} - {Gpu} - {Memory} - {CreationDate}";
    }
}