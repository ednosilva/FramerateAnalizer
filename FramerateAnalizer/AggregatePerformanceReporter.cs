using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FramerateAnalizer
{
    public class AggregatePerformanceReporter
    {
        public string ReportePerformance(IList<FramerateCaptureGroup> frameratePerformances, string delimiter)
        {
            frameratePerformances = frameratePerformances
                .OrderBy(p => p.Stats.AggregatedFramerate())
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

            foreach (var modelPerformance in frameratePerformances)
            {
                var relativePerformance = modelPerformance.RelativePerformance(frameratePerformances.First());

                string row = $"{modelPerformance.BenchmarkedPart}{delimiter}";

                row += $"{relativePerformance.Average:N1}{delimiter}";
                row += $"{relativePerformance.TenPercentLowAverage:N1}{delimiter}";
                row += $"{relativePerformance.OnePercentLowAverage:N1}{delimiter}";
                row += $"{relativePerformance.ZeroPointOnePercentLowAverage:N1}{delimiter}";

                row += $"{modelPerformance.Stats.Average:N1}{delimiter}";
                row += $"{modelPerformance.Stats.TenPercentLowAverage:N1}{delimiter}";
                row += $"{modelPerformance.Stats.OnePercentLowAverage:N1}{delimiter}";
                row += $"{modelPerformance.Stats.ZeroPointOnePercentLowAverage:N1}{delimiter}";

                foreach (FramerateCapture capture in modelPerformance.Captures)
                {
                    if (firstModel)
                    {
                        string gameContext = $"{capture.GameSettings} {capture.GameName}";

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
