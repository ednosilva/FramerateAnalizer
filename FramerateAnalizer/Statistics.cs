using System;
using System.Collections.Generic;
using System.Linq;

namespace FramerateAnalizer
{
    public static class Statistics
    {
        public static double GeometricMean(IReadOnlyCollection<double> values)
        {
            if (!values.Any())
                return double.NaN;

            return Math.Exp(values.Average(Math.Log));
        }
    }
}
