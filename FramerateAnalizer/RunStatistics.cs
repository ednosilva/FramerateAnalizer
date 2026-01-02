using System.Collections.Generic;
using System.Linq;

namespace FPSAnalizer;

public class RunStatistics
{
    public int RunIndex { get; set; }
    public string Hash { get; set; }
    public int FrameCount { get; set; }
    public decimal Duration { get; set; }
    public decimal AverageFps { get; set; }
    public decimal AverageFrameTime { get; set; }
    public decimal TenPercentLowFramerate { get; set; }
    public decimal OnePercentLowFramerate { get; set; }
    public decimal ZeroPointOnePercentLowFramerate { get; set; }
    
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
            var sortedFrameTimes = new List<decimal>(frameTimes);
            sortedFrameTimes.Sort((a, b) => b.CompareTo(a));

            TenPercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.1m);
            OnePercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.01m);
            ZeroPointOnePercentLowFramerate = GetAverageFps(sortedFrameTimes, 0.001m);
        }
    }

    private decimal GetAverageFps(List<decimal> sortedFrameTimes, decimal percentage)
    {
        int framesUsed = (int)(sortedFrameTimes.Count * percentage);

        if (framesUsed == 0)
            return sortedFrameTimes[0];

        decimal averageFrameTime = sortedFrameTimes.Take(framesUsed).Average();
        decimal averageFps = 1000m / averageFrameTime;

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