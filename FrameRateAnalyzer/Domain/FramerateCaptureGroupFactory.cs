namespace FramerateAnalyzer.Domain
{
    public class FramerateCaptureGroupFactory
    {
        public static IList<FramerateCaptureGroup> Create(IList<FramerateCapture> captures)
        {
            ArgumentNullException.ThrowIfNull(captures, nameof(captures));

            if (!captures.Any())
                throw new ArgumentOutOfRangeException(nameof(captures));

            int distinctCpus = captures.DistinctBy(c => c.Cpu).Count();
            int distinctGpus = captures.DistinctBy(c => c.Gpu).Count();
            int distinctMemory = captures.DistinctBy(c => c.Memory).Count();

            Func<FramerateCaptureGroup, string>? benchmarkedPartSelector = null;

            if (distinctCpus > 1)
            {
                if (distinctMemory > 1)
                {
                    if (distinctGpus > 1)
                        benchmarkedPartSelector = g => $"{g.Cpu} - {g.Gpu} - {g.Memory}";
                    else
                        benchmarkedPartSelector = g => $"{g.Cpu} - {g.Memory}";
                }
                else if (distinctGpus > 1)
                    benchmarkedPartSelector = g => $"{g.Cpu} - {g.Gpu}";
                else
                    benchmarkedPartSelector = g => g.Cpu;
            }
            else if (distinctGpus > 1)
            {
                if (distinctMemory > 1)
                    benchmarkedPartSelector = g => $"{g.Gpu} - {g.Memory}";
                else
                    benchmarkedPartSelector = g => g.Gpu;
            }
            else if (distinctMemory > 1)
            {
                benchmarkedPartSelector = g => g.Memory;
            }
            else
                benchmarkedPartSelector = g => $"{g.Cpu} - {g.Gpu} - {g.Memory}";

            return captures.GroupBy(c => $"{c.Cpu}|{c.Gpu}|{c.Memory}")
                .Select(c => new FramerateCaptureGroup(c.ToList(), benchmarkedPartSelector))
                .ToList();
        }
    }
}
