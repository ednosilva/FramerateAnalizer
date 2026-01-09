using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FramerateAnalizer;

public class FramerateCapture
{
    public FramerateCapture(string filePath, string cpu, string gpu, string memory, string gameName,
        string gameSettings, DateTime captureDate, IList<RunFrameCapture> runs)
    {
        ArgumentNullException.ThrowIfNull(runs, nameof(runs));

        FilePath = filePath;
        Cpu = cpu;
        Gpu = gpu;
        Memory = memory;
        GameName = gameName;
        GameSettings = gameSettings;
        CaptureDate = captureDate;
        Runs = runs;
        RunStats = runs.Select(c => new FrameratePerformanceStats(c)).ToList();
        AggregatesRunStats = new FrameratePerformanceStats(RunStats);
    }

    public string FilePath { get; }

    public string Cpu { get; }

    public string Gpu { get; }

    public string Memory { get; }

    public string GameName { get; }

    public string GameSettings { get; }

    public DateTime CaptureDate { get; }

    public IList<RunFrameCapture> Runs { get; }

    public IList<FrameratePerformanceStats> RunStats { get; }

    public FrameratePerformanceStats AggregatesRunStats { get; }

    public string FileName { get { return Path.GetFileName(FilePath); } }

    public override bool Equals(object? obj)
    {
        return obj is FramerateCapture other && FilePath == other.FilePath;
    }

    public override int GetHashCode()
    {
        return FilePath.GetHashCode();
    }
}