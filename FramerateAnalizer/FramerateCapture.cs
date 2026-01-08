using System;
using System.IO;

namespace FramerateAnalizer;

public class FramerateCapture
{
    public string FilePath { get; set; }

    public string FileName { get; set; }

    public string FullFileName { get { return Path.Combine(FilePath, FileName); } }

    public string Cpu { get; set; }

    public string Gpu { get; set; }

    public string Memory { get; set; }

    public string GameName { get; set; }

    public string GameSettings { get; set; }

    public DateTime CaptureDate { get; set; }

    public int RunCount { get; set; }

    public double AverageFramerate { get; set; }

    public double TenPercentLowFramerate { get; set; }

    public double OnePercentLowFramerate { get; set; }

    public double ZeroPointOnePercentLowFramerate { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is FramerateCapture other && FullFileName == other.FullFileName;
    }

    public override int GetHashCode()
    {
        return FullFileName.GetHashCode();
    }
}