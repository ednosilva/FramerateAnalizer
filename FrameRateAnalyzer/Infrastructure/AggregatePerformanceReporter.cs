using FramerateAnalyzer.Domain;

namespace FrameRateAnalizer.Infrastructure
{
    public class AggregatePerformanceReporter
    {
        public string ReportPerformance(IList<FramerateCaptureGroup> captureGroups,
            FramerateCaptureGroup referenceCaptureGroup, string delimiter)
        {
            captureGroups = captureGroups
                .OrderByDescending(p => p.Stats.AggregatedFramerate)
                .ToList();

            var resultRows = new List<string>();

            string header = $"Model{delimiter}";

            header += $"Relative Avg{delimiter}";
            header += $"Relative 10% Low Avg{delimiter}";
            header += $"Relative 1% Low Avg{delimiter}";
            header += $"Relative 0.1% Low Avg{delimiter}";

            header += $"Avg Geomean{delimiter}";
            header += $"10% low Avg Geomean{delimiter}";
            header += $"1% Low Avg Geomean{delimiter}";
            header += $"0.1% Low Avg Geomean{delimiter}";

            bool firstModel = true;

            foreach (var group in captureGroups)
            {
                var relativePerformance = group.RelativePerformance(referenceCaptureGroup);

                string row = $"{group.BenchmarkedParts}{delimiter}";

                row += $"{relativePerformance.Average:N3}{delimiter}";
                row += $"{relativePerformance.TenPercentLowAverage:N3}{delimiter}";
                row += $"{relativePerformance.OnePercentLowAverage:N3}{delimiter}";
                row += $"{relativePerformance.ZeroPointOnePercentLowAverage:N3}{delimiter}";

                row += $"{group.Stats.Average:N1}{delimiter}";
                row += $"{group.Stats.TenPercentLowAverage:N1}{delimiter}";
                row += $"{group.Stats.OnePercentLowAverage:N1}{delimiter}";
                row += $"{group.Stats.ZeroPointOnePercentLowAverage:N1}{delimiter}";

                foreach (FramerateCapture capture in group.Captures)
                {
                    if (firstModel)
                    {
                        string gameContext = $"{capture.CaptureDetails} {capture.GameName}";

                        header += $"{gameContext} Avg{delimiter}";
                        header += $"{gameContext} 10% Low Avg{delimiter}";
                        header += $"{gameContext} 1% Low Avg{delimiter}";
                        header += $"{gameContext} 0.1% Low Avg{delimiter}";
                    }

                    row += $"{capture.Stats.Average:N1}{delimiter}";
                    row += $"{capture.Stats.TenPercentLowAverage:N1}{delimiter}";
                    row += $"{capture.Stats.OnePercentLowAverage:N1}{delimiter}";
                    row += $"{capture.Stats.ZeroPointOnePercentLowAverage:N1}{delimiter}";
                }

                firstModel = false;
                resultRows.Add(row);
            }

            resultRows.Insert(0, header);

            return string.Join("\r\n", resultRows);
        }
    }
}
