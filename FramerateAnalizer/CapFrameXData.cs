using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FramerateAnalizer
{
    public class CapFrameXData
    {
        [JsonPropertyName("Hash")]
        public string Hash { get; set; }
        
        [JsonPropertyName("Info")]
        public CaptureInfo Info { get; set; }
        
        [JsonPropertyName("Runs")]
        public List<CaptureRun> Runs { get; set; }
        
        [JsonIgnore]
        public string FilePath { get; set; }
        
        [JsonIgnore]
        public string FileName => System.IO.Path.GetFileName(FilePath);
    }

    public class CaptureInfo
    {
        [JsonPropertyName("AppVersion")]
        public string AppVersion { get; set; }
        
        [JsonPropertyName("Id")]
        public string Id { get; set; }
        
        [JsonPropertyName("Processor")]
        public string Processor { get; set; }
        
        [JsonPropertyName("GameName")]
        public string GameName { get; set; }
        
        [JsonPropertyName("ProcessName")]
        public string ProcessName { get; set; }
        
        [JsonPropertyName("CreationDate")]
        public DateTime CreationDate { get; set; }
        
        [JsonPropertyName("Motherboard")]
        public string Motherboard { get; set; }
        
        [JsonPropertyName("OS")]
        public string OS { get; set; }
        
        [JsonPropertyName("SystemRam")]
        public string SystemRam { get; set; }
        
        [JsonPropertyName("BaseDriverVersion")]
        public string BaseDriverVersion { get; set; }
        
        [JsonPropertyName("GPUDriverVersion")]
        public string GPUDriverVersion { get; set; }
        
        [JsonPropertyName("DriverPackage")]
        public string DriverPackage { get; set; }
        
        [JsonPropertyName("GPU")]
        public string GPU { get; set; }
        
        [JsonPropertyName("GPUCount")]
        public int? GPUCount { get; set; }
        
        [JsonPropertyName("GpuCoreClock")]
        public double? GpuCoreClock { get; set; }
        
        [JsonPropertyName("GpuMemoryClock")]
        public double? GpuMemoryClock { get; set; }
        
        [JsonPropertyName("Comment")]
        public string Comment { get; set; }
        
        [JsonPropertyName("ApiInfo")]
        public string ApiInfo { get; set; }
        
        [JsonPropertyName("ResizableBar")]
        public string ResizableBar { get; set; }
        
        [JsonPropertyName("WinGameMode")]
        public string WinGameMode { get; set; }
        
        [JsonPropertyName("HAGS")]
        public string HAGS { get; set; }
        
        [JsonPropertyName("PresentationMode")]
        public string PresentationMode { get; set; }
        
        [JsonPropertyName("ResolutionInfo")]
        public string ResolutionInfo { get; set; }
    }

    public class CaptureRun
    {
        [JsonPropertyName("Hash")]
        public string Hash { get; set; }
        
        [JsonPropertyName("PresentMonRuntime")]
        public string PresentMonRuntime { get; set; }
        
        [JsonPropertyName("CaptureData")]
        public CaptureData CaptureData { get; set; }
        
        [JsonIgnore]
        public RunStatistics Statistics => new RunStatistics(this);
    }

    public class CaptureData
    {
        [JsonPropertyName("TimeInSeconds")]
        public List<double> TimeInSeconds { get; set; }
        
        [JsonIgnore]
        public double Duration => TimeInSeconds?.Count > 0 ? TimeInSeconds[^1] - TimeInSeconds[0] : 0;
        
        [JsonIgnore]
        public int FrameCount => TimeInSeconds?.Count ?? 0;
        
        [JsonIgnore]
        public double AverageFps => FrameCount > 0 && Duration > 0 ? FrameCount / Duration : 0;
        
        [JsonIgnore]
        public List<double> FrameTimes
        {
            get
            {
                if (TimeInSeconds == null || TimeInSeconds.Count < 2)
                    return new List<double>();
                
                var frameTimes = new List<double>();
                for (int i = 1; i < TimeInSeconds.Count; i++)
                {
                    frameTimes.Add((TimeInSeconds[i] - TimeInSeconds[i - 1]) * 1000);
                }
                return frameTimes;
            }
        }
        
        [JsonIgnore]
        public double AverageFrameTime => FrameTimes.Count > 0 ? FrameTimes.Average() : 0;
    }
}