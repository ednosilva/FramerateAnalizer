using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalyzer.Domain;

public class FramerateCapture
{
    public FramerateCapture(string fileName, string cpu, string gpu, string memory, string gameName,
        string gameSettings, DateTime captureDate, IList<FrameCaptureRun> runs)
    {
        ArgumentNullException.ThrowIfNull(runs, nameof(runs));

        FileName = fileName;
        Cpu = cpu;
        Gpu = gpu;
        Memory = memory;
        GameName = gameName;
        GameSettings = gameSettings;
        CaptureDate = captureDate;
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

    public string FileName { get; }

    public string Cpu { get; }

    public string Gpu { get; }

    public string Memory { get; }

    public string GameName { get; }

    public string GameSettings { get; }

    public DateTime CaptureDate { get; }

    public IList<FrameCaptureRun> Runs { get; }

    public FramerateStats Stats { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is FramerateCapture other && FileName == other.FileName;
    }

    public override int GetHashCode()
    {
        return FileName.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Cpu} - {Gpu} - {Memory} - {GameName} - {GameSettings}";
    }
}