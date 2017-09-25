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

        private float calcStd(float average, IEnumerable<float> list)
        {
            var variance = (from x in list select (x - average) * (x - average)).Sum() / (list.Count() - 1);
            return (float)Math.Sqrt(variance);
        }

        private bool outlier(float x, float avg, float std)
        {
            return (x - avg) > TOLLERANCE_STD * std || (x - avg) < -1 * TOLLERANCE_STD * std;
        }

        private IEnumerable<float> removeOddsFeature(IEnumerable<float> feature)
        {
            var avg = feature.Average();
            var std = calcStd(avg, feature);
            return feature.Where(x => !outlier(x, avg, std)).ToList();
        }

        private IEnumerable<int> findOutlierIndices(IEnumerable<float> feature)
            // zwraca indeksy odstających danych, uporządkowane malejąco
        {
            var avg = feature.Average();
            var std = calcStd(avg, feature);
            return  feature.Select((v, i) => new { v, i }).Where(x => outlier(x.v, avg, std)).Select(x => x.i)
                .OrderByDescending(v => v);
        }

        private void removeAtIndices(ref List<Point> pts, ref List<Derivatives> ders, List<int> indices)
        {
            foreach(var i in indices)
            {
                pts.RemoveAt(i);
                ders.RemoveAt(i);
            }
        }
        public void removeOddsValues(ref List<Point> pts, ref List<Derivatives> ders)
        {
            var indices = findOutlierIndices(from x in pts select x.X).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in pts select x.Y).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in pts select x.Pressure).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.Velocity).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.VelocityX).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.VelocityY).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.Acc).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.AccX).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            indices = findOutlierIndices(from x in ders select x.AccY).ToList();
            removeAtIndices(ref pts, ref ders, indices);
            //indices = findOutlierIndices(from x in ders select x.PressureChange);
            //removeAtIndices(ref pts, ref ders, indices);
            //ta lista nie ma sensu - 0, -1, 1
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
            var std = calcStd(avg, feature);
            return (from x in feature select (float)((x - avg) / std)).ToList();
        }

        public List<float> prepareFeature(IEnumerable<float> feature, bool removeOutlying)
        {
            if (removeOutlying)
                feature = removeOddsFeature(feature);
            if (NORMALISATION == "std")
                return normaliseFeatureStd(feature);
            if (NORMALISATION == "minmax")
                return normaliseFeatureMinMax(feature);
            Debug.WriteLine("Wybrano złą opcję normalizacji. Nic nie robię.");
            return feature.ToList();
        }

        private List<float> prepareFeatureGlobal(IEnumerable<float> feature)
        {
            if (NORMALISATION == "std")
                return normaliseFeatureStd(feature);
            if (NORMALISATION == "minmax")
                return normaliseFeatureMinMax(feature);
            Debug.WriteLine("Wybrano złą opcję normalizacji. Nic nie robię.");
            return feature.ToList();
        }

        public List<Point> preparePoints(List<Point> points)
        {
            List<Point> result = new List<Point>();
            var xs = prepareFeatureGlobal(from x in points select x.X);
            var ys = prepareFeatureGlobal(from x in points select x.Y);
            var pressures = prepareFeatureGlobal(from x in points select x.Pressure);
            //var tiltXs = prepareFeature(from x in points select x.tiltX);
            //var tiltYs = prepareFeature(from x in points select x.tiltY);
            for (int i = 0; i < points.Count(); i++)
                result.Add(new Point(xs[i], ys[i], pressures[i], (ulong)points[i].Timestamp, 0, 0)); // tiltXs[i], tiltYs[i]));

            return result;
        }

        public List<Derivatives> prepareDerivatives(List<Derivatives> derivatives)
        {
            List<Derivatives> result = new List<Derivatives>();
            var v = prepareFeatureGlobal(from x in derivatives select x.Velocity);
            var vx = prepareFeatureGlobal(from x in derivatives select x.VelocityX);
            var vy = prepareFeatureGlobal(from x in derivatives select x.VelocityY);
            var a = prepareFeatureGlobal(from x in derivatives select x.Acc);
            var ax = prepareFeatureGlobal(from x in derivatives select x.AccX);
            var ay = prepareFeatureGlobal(from x in derivatives select x.AccY);
            var pc = prepareFeatureGlobal(from x in derivatives select x.PressureChange);
            //Debug.WriteLine("Max v to " + v.Max() + " ");
            for (int i = 0; i < derivatives.Count(); i++)
                result.Add(new Derivatives(v[i], vx[i], vy[i], a[i], ax[i], ay[i], 0, 0, pc[i]));
            // nie uwzględniam tiltów, póki nie działają.
            //Debug.WriteLine("Max result to " + result.Max(x => x.Velocity) + " ");
            return result;
        }
    }
}
