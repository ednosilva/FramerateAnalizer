using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer;

public record RunFrameCapture
{
    public RunFrameCapture(IList<double> timeInSeconds)
    {
        if (timeInSeconds == null || timeInSeconds.Count < 2)
            throw new ArgumentOutOfRangeException(nameof(timeInSeconds));

        TimeInSeconds = timeInSeconds;
    }

    public IList<double> TimeInSeconds { get; }

    public double Duration
    {
        get
        {
            return TimeInSeconds[^1] - TimeInSeconds[0];
        }
    }

    public int FrameCount
    {
        get
        {
            return TimeInSeconds.Count - 1;
        }
    }

    public IList<double> FrameTimes
    {
        get
        {
            if (TimeInSeconds == null || TimeInSeconds.Count < 2)
                return [];

            var frameTimes = new List<double>();

            for (int i = 1; i < TimeInSeconds.Count; i++)
            {
                frameTimes.Add((TimeInSeconds[i] - TimeInSeconds[i - 1]) * 1000);
            }

            return frameTimes;
        }
    }

    public double GetAverageFps(double percentage)
    {
        if (percentage == 100.0)
            return FrameCount / Duration;

        var sortedFrameTimes = new List<double>(FrameTimes);
        sortedFrameTimes.Sort((a, b) => b.CompareTo(a));

        int frameCount = (int)(sortedFrameTimes.Count * percentage / 100.0);

        if (frameCount == 0)
            return double.NaN;

        double averageFrameTime = sortedFrameTimes.Take(frameCount).Average();
        double averageFps = 1000.0 / averageFrameTime;

        return averageFps;
    }
}
