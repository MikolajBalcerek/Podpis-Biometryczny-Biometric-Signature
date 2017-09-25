using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore.PreciseComparison
{
    // TO DZIAŁA ŹLE. TRZEBA SKALIBROWAĆ PARAMETRY
    class ProbabilisticDTW
    {
        const int NUM_SEGMENTS = 12;

        float a = 120;
        float b = 180;
        // ai, bi powinny być związane z rozmiarem segmentu

        DynamicTimeWrapping dtw = new DynamicTimeWrapping();
        public ProbabilisticDTW() { }

        private float calcSegmentProbability(float distance)
        {
            if (distance <= a)
                return 1 - distance / (distance + a);
            if (distance > b)
                return 0;
            return (b - distance) / (b - distance + b - a);
        }

        public float calcSimilarity(Signature sgn1, Signature sgn2)
        {
            List<float> probabilities = new List<float>();

            var pts1 = sgn1.getAllOriginalPoints();
            var der1 = sgn1.getOriginalDerivatives();
            var pts2 = sgn2.getAllOriginalPoints();
            var der2 = sgn2.getOriginalDerivatives();

            var len1 = (int)pts1.Count / NUM_SEGMENTS;
            var len2 = (int)pts2.Count / NUM_SEGMENTS ;
            //Debug.WriteLine("len1 = " + len1 + ", len2 = " + len2 + ", pts1 = " + pts1.Count + ", pts2 = " + pts2.Count);

            for(int i = 1; i <= NUM_SEGMENTS; i++)
            {
                var seg1Pts = pts1.Skip((i - 1) * len1).Take(len1).ToList();
                var seg1Der = der1.Skip((i - 1) * len1).Take(len1).ToList();

                var seg2Pts = pts2.Skip((i - 1) * len2).Take(len2).ToList();
                var seg2Der = der2.Skip((i - 1) * len2).Take(len2).ToList();

                //var xs1 = (from x in seg1Pts select x.X).ToList();
                //var xs2 = (from x in seg2Pts select x.X).ToList();
                //var distX = dtw.calcSimilarity(xs1, xs2);

                var dist = dtw.calcSimilarity(seg1Pts, seg2Pts, seg1Der, seg2Der);
                //Debug.WriteLine(dist);
                probabilities.Add(calcSegmentProbability(dist));
            }

            var numerator = probabilities.Aggregate(1.0, (prod, next) => prod * next);
            var denominator = probabilities.Aggregate(1.0, (prod, next) => prod * (1.0 - next));
            return (float)(1.0/(1.0 + (denominator/numerator)));
        }
    }
}
