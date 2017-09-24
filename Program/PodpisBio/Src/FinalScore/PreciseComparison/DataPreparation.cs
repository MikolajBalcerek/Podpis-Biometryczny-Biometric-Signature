using PodpisBio.Src.Author;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore.PreciseComparison
{
    class DataPreparation
    {
        const int TOLLERANCE_STD = 2;

        const string NORMALISATION = "std"; // druga opcja to "minmax"

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

        public List<float> prepareFeature(IEnumerable<float> feature)
        {
            if (NORMALISATION == "std")
                return normaliseFeatureStd(removeOdds(feature));
            if (NORMALISATION == "minmax")
                return normaliseFeatureMinMax(removeOdds(feature));
            Debug.WriteLine("Wybrano złą opcję normalizacji. Nic nie robię.");
            return feature.ToList();
        }

        public List<Point> preparePoints(List<Point> points)
        {
            List<Point> result = new List<Point>();
            var xs = prepareFeature(from x in points select x.X);
            var ys = prepareFeature(from x in points select x.Y);
            var pressures = prepareFeature(from x in points select x.Pressure);
            var tiltXs = prepareFeature(from x in points select x.tiltX);
            var tiltYs = prepareFeature(from x in points select x.tiltY);
            for (int i = 0; i < points.Count(); i++)
                result.Add(new Point(xs[i], ys[i], pressures[i], (ulong)points[i].Timestamp, tiltXs[i], tiltYs[i]));

            return result;
        }

        public List<Derivatives> prepareDerivatives(List<Derivatives> derivatives)
        {
            List<Derivatives> result = new List<Derivatives>();
            var v = prepareFeature(from x in derivatives select x.Velocity);
            var vx = prepareFeature(from x in derivatives select x.VelocityX);
            var vy = prepareFeature(from x in derivatives select x.VelocityY);
            var a = prepareFeature(from x in derivatives select x.Acc);
            var ax = prepareFeature(from x in derivatives select x.AccX);
            var ay = prepareFeature(from x in derivatives select x.AccY);
            var pc = prepareFeature(from x in derivatives select x.PressureChange);

            for (int i = 0; i < derivatives.Count(); i++)
                result.Add(new Derivatives(v[i], vx[i], vy[i], a[i], ax[i], ay[i], 0, 0, pc[i]));
            // nie uwzględniam tiltów, póki nie działają.

            return result;
        }
    }
}
