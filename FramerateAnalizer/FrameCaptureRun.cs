using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer;

public record FrameCaptureRun
{
    public FrameCaptureRun(IList<double> timeInSeconds)
    {
        if (timeInSeconds == null || timeInSeconds.Count < 2)
            throw new ArgumentOutOfRangeException(nameof(timeInSeconds));
        
        SetFrameTimes(timeInSeconds);
        double duration = timeInSeconds[^1] - timeInSeconds[0];
        SetStats(duration);
    }

    public IList<double> FrameTimes { get; private set; }

    public FramerateStats Stats { get; private set; }

    private void SetFrameTimes(IList<double> timeInSeconds)
    {
        FrameTimes = new List<double>();

        for (int i = 1; i < timeInSeconds.Count; i++)
        {
            FrameTimes.Add((timeInSeconds[i] - timeInSeconds[i - 1]) * 1000);
        }
    }

    private void SetStats(double duration)
    {
        var sortedFrameTimes = new List<double>(FrameTimes);
        sortedFrameTimes.Sort((a, b) => b.CompareTo(a));

        double average = Math.Round(FrameTimes.Count / duration, 1);
        double tenPercentLowAverage = Math.Round(GetFramerateLowAverage(sortedFrameTimes, 0.1), 1);
        double onePercentLowAverage = Math.Round(GetFramerateLowAverage(sortedFrameTimes, 0.01), 1);
        double zeroPointOnePercentLowAverage = Math.Round(GetFramerateLowAverage(sortedFrameTimes, 0.001), 1);

        Stats = new FramerateStats(average, tenPercentLowAverage, onePercentLowAverage, zeroPointOnePercentLowAverage);
    }

    private static double GetFramerateLowAverage(List<double> sortedFrameTimes, double frameCountFactor)
    {
        int frameCount = (int)(sortedFrameTimes.Count * frameCountFactor);

        if (frameCount == 0)
            return double.NaN;

        double averageFrameTime = sortedFrameTimes.Take(frameCount).Average();

        return 1000.0 / averageFrameTime;
    }
}
