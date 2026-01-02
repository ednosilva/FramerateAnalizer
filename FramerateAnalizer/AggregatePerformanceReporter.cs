using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FPSAnalizer
{
    public class AggregatePerformanceReporter
    {
        public string ReportePerformance(IList<FramerateAggregatedPerformance> frameratePerformances)
        {
            frameratePerformances = frameratePerformances
                .OrderBy(p => p.FramerateGeomeanStats.AggregatedPerformance())
                .ToList();

            Func<FramerateAggregatedPerformance, string> modelSelector =
                frameratePerformances.DistinctBy(c => c.Cpu).Count() > 1 ? c => c.Cpu : c => c.Gpu;

            string delimiter = "\t";

            var resultRows = new List<string>();

            string header = $"Model{delimiter}";

            bool firstModel = true;

            foreach (var modelPerformance in frameratePerformances)
            {
                header += $"Relative Avg{delimiter}";
                //header += $"Relative 10% Low Avg{delimiter}";
                header += $"Relative 1% Low Avg{delimiter}";
                header += $"Relative 0.1% Low Avg{delimiter}";

                var relativePerformance = modelPerformance.RelativePerformance(frameratePerformances.First());

                string row = $"{modelSelector(modelPerformance)}{delimiter}";

                row += $"{relativePerformance.Average:N1}{delimiter}";
                //row += $"{relativePerformance.TenPercentLowAverage}{delimiter}";
                row += $"{relativePerformance.OnePercentLowAverage:N1}{delimiter}";
                row += $"{relativePerformance.ZeroPointOnePercentLowAverage:N1}{delimiter}";

                header += $"Avg Geomean{delimiter}";
                //header += $"10% low Avg Geomean{delimiter}";
                header += $"1% Low Avg Geomean{delimiter}";
                header += $"0.1% Low Avg Geomean{delimiter}";

                row += $"{modelPerformance.FramerateGeomeanStats.Average:N1}{delimiter}";
                //row += $"{modelPerformance.FramerateGeomeanStats.TenPercentLowAverage}{delimiter}";
                row += $"{modelPerformance.FramerateGeomeanStats.OnePercentLowAverage:N1}{delimiter}";
                row += $"{modelPerformance.FramerateGeomeanStats.ZeroPointOnePercentLowAverage:N1}{delimiter}";

                foreach (FramerateCapture capture in modelPerformance.FramerateCaptures)
                {
                    if (firstModel)
                    {
                        string gameContext = $"{capture.GameSettings} {capture.GameName}";

                        header += $"{gameContext} Avg{delimiter}";
                        //header += $"{gameContext} 10% Low Avg{delimiter}";
                        header += $"{gameContext} 1% Low Avg{delimiter}";
                        header += $"{gameContext} 0.1% Low Avg{delimiter}";
                    }

                    row += $"{capture.AverageFramerate:N1}{delimiter}";
                    //row += $"{capture.TenPercentLowFramerate:N1}{delimiter}";
                    row += $"{capture.OnePercentLowFramerate:N1}{delimiter}";
                    row += $"{capture.ZeroPointOnePercentLowFramerate:N1}{delimiter}";
                }

                firstModel = false;
                resultRows.Add(row);
            }

            resultRows.Insert(0, header);

            return string.Join("\r\n", resultRows);
        }

        //public string ReportePerformance(IList<FramerateCapture> framerateCaptures)
        //{
        //    Func<FramerateCapture, string> modelSelector = framerateCaptures.DistinctBy(c => c.Cpu).Count() > 1 ?
        //        c => c.Cpu : c => c.Gpu;

        //    framerateCaptures = framerateCaptures.OrderBy(modelSelector).ToList();

        //    string delimiter = "\t";

        //    var resultRows = new List<string>();

        //    string header = $"Model{delimiter}";

        //    bool firstModel = true;

        //    foreach (var modelCaptures in framerateCaptures.GroupBy(modelSelector))
        //    {
        //        string row = $"{modelCaptures.Key}{delimiter}";

        //        foreach (var gameSettingsCaptures in modelCaptures.GroupBy(c => c.GameSettings))
        //        {
        //            foreach (var gameCaptures in gameSettingsCaptures.GroupBy(c => c.GameName))
        //            {
        //                if (firstModel)
        //                {
        //                    header += $"{gameSettingsCaptures.Key} {gameCaptures.Key} Avg{delimiter}";
        //                    header += $"{gameSettingsCaptures.Key} {gameCaptures.Key} 1% Low{delimiter}";
        //                    header += $"{gameSettingsCaptures.Key} {gameCaptures.Key} 0.1% Low{delimiter}";
        //                }

        //                var capture = gameCaptures.Single();
        //                row += $"{capture.AverageFramerate:N1}{delimiter}";
        //                row += $"{capture.OnePercentLowFramerate:N1}{delimiter}";
        //                row += $"{capture.ZeroPointOnePercentLowFramerate:N1}{delimiter}";
        //            }
        //        }

        //        firstModel = false;
        //        resultRows.Add(row);
        //    }

        //    resultRows.Insert(0, header);

        //    return string.Join("\r\n", resultRows);
        //}
    }
}
