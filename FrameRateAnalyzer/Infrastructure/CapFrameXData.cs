using System;
using System.Collections.Generic;

namespace FramerateAnalyzer.Infrastructure
{
    public class CapFrameXData
    {
        public string Hash { get; set; }
        
        public CaptureInfo Info { get; set; }
        
        public List<CaptureRun> Runs { get; set; }
        
        public string FileName { get; set; }
    }

    public class CaptureInfo
    {
        public string AppVersion { get; set; }
        
        public string Id { get; set; }
        
        public string Processor { get; set; }
        
        public string GameName { get; set; }
        
        public string ProcessName { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public string Motherboard { get; set; }
        
        public string OS { get; set; }
        
        public string SystemRam { get; set; }
        
        public string BaseDriverVersion { get; set; }
        
        public string GPUDriverVersion { get; set; }
        
        public string DriverPackage { get; set; }
        
        public string GPU { get; set; }
        
        public int? GPUCount { get; set; }
        
        public double? GpuCoreClock { get; set; }
        
        public double? GpuMemoryClock { get; set; }
        
        public string Comment { get; set; }
        
        public string ApiInfo { get; set; }
        
        public string ResizableBar { get; set; }
        
        public string WinGameMode { get; set; }
        
        public string HAGS { get; set; }
        
        public string PresentationMode { get; set; }
        
        public string ResolutionInfo { get; set; }
    }

    public class CaptureRun
    {
        public string Hash { get; set; }
        
        public string PresentMonRuntime { get; set; }
        
        public CaptureData CaptureData { get; set; }
    }

    public class CaptureData
    {
        public List<double> MsBetweenPresents { get; set; }
    }
}