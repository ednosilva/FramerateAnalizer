using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer;

public class RunStatistics
{
    public int RunIndex { get; set; }
    public string Hash { get; set; }
    public int FrameCount { get; set; }
    public double Duration { get; set; }
    public double AverageFps { get; set; }
    public double AverageFrameTime { get; set; }
    public double TenPercentLowFramerate { get; set; }
    public double OnePercentLowFramerate { get; set; }
    public double ZeroPointOnePercentLowFramerate { get; set; }
    
    public RunStatistics(CaptureRun run)
    {
        if (run?.CaptureData == null) return;
        
        Hash = run.Hash;
        FrameCount = run.CaptureData.FrameCount;
        Duration = run.CaptureData.Duration;
        AverageFrameTime = run.CaptureData.AverageFrameTime;
        AverageFps = run.CaptureData.AverageFps;
        
        var frameTimes = run.CaptureData.FrameTimes;

        if (frameTimes.Count > 0)
        {
            var sortedFrameTimes = new List<double>(frameTimes);
            sortedFrameTimes.Sort((a, b) => b.CompareTo(a));

            TenPercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.1);
            OnePercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.01);
            ZeroPointOnePercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.001);
        }
    }

    private double GetAverageFps(List<double> sortedFrameTimes, double percentage)
    {
        int framesUsed = (int)(sortedFrameTimes.Count * percentage);

        if (framesUsed == 0)
            return sortedFrameTimes[0];

        double averageFrameTime = sortedFrameTimes.Take(framesUsed).Average();
        double averageFps = 1000.0 / averageFrameTime;

        return averageFps;
    }

    public string GetFormattedStats()
    {
        return $"FPS Médio: {AverageFps:F2}\n" +
               $"10% Low: {TenPercentLowFramerate:F2} FPS\n" +
               $"1% Low: {OnePercentLowFramerate:F2} FPS\n" +
               $"0.1% Low: {ZeroPointOnePercentLowFramerate:F2} FPS\n" +
               $"FrameTime Médio: {AverageFrameTime:F2} ms";
    }
}