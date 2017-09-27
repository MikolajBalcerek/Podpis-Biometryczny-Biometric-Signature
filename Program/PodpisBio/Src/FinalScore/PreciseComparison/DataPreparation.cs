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

        // standaryzacja póki co działa bardzo niefajnie :(
        // można dodać lepsze metody samplingu niż sampling liniowy

        const int STANDARD_LENGTH = 550;

        const bool STANDARIZE_LENGTH = false;

        const bool STUPID_LENGTH_STANDARIZATION = false;

        private float calcStd(float average, IEnumerable<float> list)
        {
            var variance = (from x in list select (x - average) * (x - average)).Sum() / (list.Count() - 1);
            return (float)Math.Sqrt(variance);
        }

        private bool outlier(float x, float avg, float std)
        {
            return (x - avg) > TOLLERANCE_STD * std || (x - avg) < -1 * TOLLERANCE_STD * std;
        }

        private IEnumerable<float> upsampleLineary(IEnumerable<float> ts, int scale, int n)
        {
            var result = new float[n * scale];

            int i = 0;
            foreach (var x in ts)
            {
                if (i >= n) break;
                result[i * scale] = x;
                i++;
            }

            for(int j = 0; j < n - 1; j++)
            {
                var left = result[j * scale];
                var right = result[(j + 1) * scale];
                for (int k = j * scale + 1; k < (j + 1) * scale; k++)
                    result[k] = (k * left + (scale - k) * right) / scale;
            }
            return result;
        }

        private IEnumerable<float> downsampleLineary(IEnumerable<float> ts, int scale, int n)
        {
            // zakładam, że scale dzieli ts.Count()
            if (n % scale != 0)
                Debug.WriteLine(" Scale nie dzieli liczby  elementów w downsample!");

            var result = new float[n / scale];
            for (int i = 0; i < n / scale; i++)
            {
                result[i] = ts.Skip(i * scale).Take(scale).Average();
            }
            return result;
        }

         private Tuple<int, int> findShifts(int n, int rangeFeature=5, int rangeStd=50)
        {
            int featureShift = 0;
            int stdShift = 0;
            int minLcm = lcm(n, STANDARD_LENGTH);

            for (int i = 0; i < rangeFeature; i++)
                for (int j = -rangeStd; j < rangeStd + 1; j++)
                    if (lcm(n - i, STANDARD_LENGTH + j) < minLcm)
                    {
                        minLcm = lcm(n - i, STANDARD_LENGTH + j);
                        featureShift = i;
                        stdShift = j;
                    }
            return Tuple.Create(featureShift, stdShift);
        }

        private IEnumerable<float> fitFeatureToSize(IEnumerable<float> feature, int minLcm, int featureShift, int stdShift)
        {
            int n = feature.Count();
            var result = upsampleLineary(feature, minLcm / (n - featureShift), n - featureShift);
            //Debug.WriteLine(result.Count());
            return downsampleLineary(result, minLcm / (STANDARD_LENGTH + stdShift), minLcm);
        }

        private IEnumerable<float> fitFeatureToSize(IEnumerable<float> feature)
        {
            int n = feature.Count();
            var shifts = findShifts(n, 5, 10);
            var minLcm = lcm(n - shifts.Item1, STANDARD_LENGTH + shifts.Item2);
            return fitFeatureToSize(feature, minLcm, shifts.Item1, shifts.Item2);
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

        private IEnumerable<float> stupidFeatureFitToSize(IEnumerable<float> feature)
        {
            if (feature.Count() < 420)
                return upsampleLineary(feature, 2, feature.Count());
            return feature;
        }

        public List<float> prepareFeature(IEnumerable<float> feature, bool removeOutlying)
        {
            if (removeOutlying)
                feature = removeOddsFeature(feature);
            if (STANDARIZE_LENGTH)
                feature = fitFeatureToSize(feature);
            else if (STUPID_LENGTH_STANDARIZATION)
                feature = stupidFeatureFitToSize(feature);
            if (NORMALISATION == "std")
                return normaliseFeatureStd(feature);
            if (NORMALISATION == "minmax")
                return normaliseFeatureMinMax(feature);
            Debug.WriteLine("Wybrano złą opcję normalizacji. Nic nie robię.");
            return feature.ToList();
        }

        private List<float> prepareFeatureGlobal(IEnumerable<float> feature, int minLcm, int featureShift, int stdShift)
        {
            //Debug.WriteLine(feature.Count());
            if (STANDARIZE_LENGTH)
                feature = fitFeatureToSize(feature, minLcm, featureShift, stdShift);
            else if (STUPID_LENGTH_STANDARIZATION)
                feature = stupidFeatureFitToSize(feature);
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
            var shifts = findShifts(points.Count);
            var minLcm = lcm(points.Count - shifts.Item1, STANDARD_LENGTH + shifts.Item2);
            var xs = prepareFeatureGlobal((from x in points select x.X), minLcm, shifts.Item1, shifts.Item2);
            var ys = prepareFeatureGlobal((from x in points select x.Y), minLcm, shifts.Item1, shifts.Item2);
            var pressures = prepareFeatureGlobal((from x in points select x.Pressure), minLcm, shifts.Item1, shifts.Item2);
            //var tiltXs = prepareFeature(from x in points select x.tiltX);
            //var tiltYs = prepareFeature(from x in points select x.tiltY);
            for (int i = 0; i < xs.Count(); i++)
                result.Add(new Point(xs[i], ys[i], pressures[i], 0, 0, 0)); // tiltXs[i], tiltYs[i]));

            return result;
        }

        public List<Derivatives> prepareDerivatives(List<Derivatives> derivatives)
        {
            List<Derivatives> result = new List<Derivatives>();
            var shifts = findShifts(derivatives.Count);
            var minLcm = lcm(derivatives.Count - shifts.Item1, STANDARD_LENGTH + shifts.Item2);
            var v = prepareFeatureGlobal((from x in derivatives select x.Velocity), minLcm, shifts.Item1, shifts.Item2);
            var vx = prepareFeatureGlobal((from x in derivatives select x.VelocityX), minLcm, shifts.Item1, shifts.Item2);
            var vy = prepareFeatureGlobal((from x in derivatives select x.VelocityY), minLcm, shifts.Item1, shifts.Item2);
            var a = prepareFeatureGlobal((from x in derivatives select x.Acc), minLcm, shifts.Item1, shifts.Item2);
            var ax = prepareFeatureGlobal((from x in derivatives select x.AccX), minLcm, shifts.Item1, shifts.Item2);
            var ay = prepareFeatureGlobal((from x in derivatives select x.AccY), minLcm, shifts.Item1, shifts.Item2);
            var pc = prepareFeatureGlobal((from x in derivatives select x.PressureChange), minLcm, shifts.Item1, shifts.Item2);
            //Debug.WriteLine("Max v to " + v.Max() + " ");
            for (int i = 0; i < v.Count(); i++)
                result.Add(new Derivatives(v[i], vx[i], vy[i], a[i], ax[i], ay[i], 0, 0, pc[i]));
            // nie uwzględniam tiltów, póki nie działają.
            //Debug.WriteLine("Max result to " + result.Max(x => x.Velocity) + " ");
            return result;
        }

        private int gcd(int a, int b)
        {
            int tmp;
            while (b != 0)
            {
                tmp = a % b;
                a = b;
                b = tmp;
            }
            return a;
        }

        private int lcm(int a, int b)
        {
            return a * b / gcd(a, b);
        }
    }
}
