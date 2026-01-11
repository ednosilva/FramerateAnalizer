using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer
{
    public class FramerateCaptureGroupFactory
    {
        public static IList<FramerateCaptureGroup> Create(IList<FramerateCapture> captures)
        {
            if (!captures.Any() /*||
                captures.DistinctBy(c => c.Cpu).Count() > 1 && captures.DistinctBy(c => c.Gpu).Count() > 1 ||
                captures.DistinctBy(c => c.Cpu).Count() > 1 && captures.DistinctBy(c => c.Memory).Count() > 1 ||
                captures.DistinctBy(c => c.Gpu).Count() > 1 && captures.DistinctBy(c => c.Memory).Count() > 1*/)
            {
                throw new ArgumentOutOfRangeException(nameof(captures));
            }

            Func<FramerateCaptureGroup, string> benchmarkedPartSelector =
                captures.DistinctBy(c => c.Cpu).Count() > 1 ? c => c.Cpu :
                captures.DistinctBy(c => c.Gpu).Count() > 1 ? c => c.Gpu : c => c.Memory;

            return captures.GroupBy(c => $"{c.Cpu}|{c.Gpu}|{c.Memory}")
                .Select(c => new FramerateCaptureGroup(c.ToList(), benchmarkedPartSelector))
                .OrderByDescending(g => g.Stats.AggregatedFramerate())
                .ToList();
        }
    }
}
