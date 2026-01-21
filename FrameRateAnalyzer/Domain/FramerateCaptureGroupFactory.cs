using System.Text.RegularExpressions;

namespace FramerateAnalyzer.Domain
{
    public class FramerateCaptureGroupFactory
    {
        public static IList<FrameRateCaptureGroup> Create(IList<FramerateCapture> captures)
        {
            ArgumentNullException.ThrowIfNull(captures, nameof(captures));

            if (!captures.Any())
                throw new ArgumentOutOfRangeException(nameof(captures));

            return captures.GroupBy(c => $"{c.Cpu}|{c.Gpu}|{c.Memory}")
                .Select(c => new FrameRateCaptureGroup(c.ToList()))
                .ToList();
        }

        public static Func<IFrameRateBenchmarkResult, IList<string>> BenchmarkedPartsSelector(IList<FramerateCapture> captures)
        {
            ArgumentNullException.ThrowIfNull(captures, nameof(captures));

            if (!captures.Any())
                throw new ArgumentOutOfRangeException(nameof(captures));

            int distinctCpus = captures.DistinctBy(c => c.Cpu).Count();
            int distinctGpus = captures.DistinctBy(c => c.Gpu).Count();
            int distinctMemory = captures.DistinctBy(c => c.Memory).Count();

            if (distinctCpus > 1)
            {
                if (distinctMemory > 1)
                {
                    if (distinctGpus > 1)
                        return g => [g.Cpu, g.Gpu, g.Memory];
                    else
                        return g => [g.Cpu, g.Memory];
                }
                else if (distinctGpus > 1)
                    return g => [g.Cpu, g.Gpu];
                else
                    return g => [g.Cpu];
            }
            else if (distinctGpus > 1)
            {
                if (distinctMemory > 1)
                    return g => [g.Gpu, g.Memory];
                else
                    return g => [g.Gpu];
            }
            else if (distinctMemory > 1)
            {
                return g => [g.Memory];
            }
            else
                return g => [g.Cpu, g.Gpu, g.Memory];

            throw new ArgumentOutOfRangeException(nameof(captures));
        }
    }
}
