using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalyzer.Domain;

public class FramerateCapture
{
    public FramerateCapture(string cpu, string gpu, string memory, string gameName, string gameSettings,
        IList<FrameCaptureRun> runs, DateTime captureDate)
    {
        ArgumentNullException.ThrowIfNull(runs, nameof(runs));

        Cpu = cpu;
        Gpu = gpu;
        Memory = memory;
        GameName = gameName;
        GameSettings = gameSettings;
        CreationDate = captureDate;
        Runs = runs;

        SetAggregatedStats(runs);
    }

    private void SetAggregatedStats(IList<FrameCaptureRun> runs)
    {
        IList<FramerateStats> runStats = runs.Select(r => r.Stats).ToList();

        double average = runStats.Average(s => s.Average);
        double tenPercentLowAverage = runStats.Average(s => s.TenPercentLowAverage);
        double onePercentLowAverage = runStats.Average(s => s.OnePercentLowAverage);
        double zeroPointOnePercentLowAverage = runStats.Average(s => s.ZeroPointOnePercentLowAverage);

        Stats = new FramerateStats(average, tenPercentLowAverage, onePercentLowAverage,
            zeroPointOnePercentLowAverage);
    }

    public string Cpu { get; }

    public string Gpu { get; }

    public string Memory { get; }

    public string GameName { get; }

    public string GameSettings { get; }

    public DateTime CreationDate { get; }

    public IList<FrameCaptureRun> Runs { get; }

    public FramerateStats Stats { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is FramerateCapture other && new
        {
            Cpu,
            Gpu,
            Memory,
            GameName,
            GameSettings,
            CreationDate
        }
        .Equals(new
        {
            other.Cpu,
            other.Gpu,
            other.Memory,
            other.GameName,
            other.GameSettings,
            other.CreationDate
        });
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Cpu, Gpu, Memory, GameName, GameSettings, CreationDate);
    }

    public override string ToString()
    {
        return $"{Cpu} - {Gpu} - {Memory} - {GameName} - {GameSettings} - {CreationDate}";
    }
}