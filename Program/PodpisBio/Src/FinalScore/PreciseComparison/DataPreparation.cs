using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore.PreciseComparison
{
    class DataPreparation
    {
        const int TOLLERANCE_STD = 2;
        private IEnumerable<float> removeOdds(IEnumerable<float> feature)
        {
            var avg = feature.Average();
            var variance = (from x in feature select (x - avg) * (x - avg)).Sum() / (feature.Count() - 1);
            var std = Math.Sqrt(variance);
            return feature.Where(x => (x - avg) < TOLLERANCE_STD * std && (x - avg) > -1 * TOLLERANCE_STD * std).ToList();
        }

        private List<float> normaliseFeatureMinMax(IEnumerable<float> feature)
        {
            var min = feature.Min();
            var max = feature.Max();

            return (from x in feature select (x - min) / (max - min)).ToList();
        }

        private List<float> normaliseFeatureStd(IEnumerable<float> feature)
        {
            var avg = feature.Average();
            var variance = (from x in feature select (x - avg) * (x - avg)).Sum() / (feature.Count() - 1);
            var std = Math.Sqrt(variance);
            return (from x in feature select (float)((x - avg) / std)).ToList();
        }
    }
}
