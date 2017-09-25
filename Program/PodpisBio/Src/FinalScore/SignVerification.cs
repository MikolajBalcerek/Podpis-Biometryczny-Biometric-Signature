using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore
{
    class SignVerification
    {
        public List<double> init(Signature sign, List<Signature> signList, Weight weights)
        {
            List<double> ver = new List<double>();
            foreach (Signature first in signList)
            {
                ver.Add(check(first, sign, weights));
            }

            return ver;
        }

        private double check(Signature first, Signature second, Weight weights)
        {
            double temp = 0;
            double lengthM = checkLengthM(first, second);
            if (lengthM < 0.5)
            {
                return 0;
            }
            double strokesCount = checkStrokesCount(first, second);
            double timeSizeRatio = checkTimeSizeRatio(first, second);
            double timeSizeRatioForEachStroke = checkTimeSizeRatioForEachStroke(first, second);
            double preciseComparison = checkPreciseComparison(first, second);
            /*
             */

            //temp = preciseComparison;
            temp = lengthM * weights.getLengthMWeight() + strokesCount * weights.getStrokesCountWeight() + timeSizeRatio * weights.getTotalRatioWeight() /*+ timeSizeRatioForEachStroke * weights.getTotalRatioForEachStrokeWeight() + preciseComparison * weights.getPreciseComparisonWeight()*/;
            temp = temp * (1 / (weights.getLengthMWeight() + weights.getStrokesCountWeight()+weights.getTotalRatioWeight()));
            return temp;
        }

        private double checkLengthM(Signature first, Signature second)
        {
            double temp = 1 - (Math.Abs(first.getLengthM() - second.getLengthM()) / first.getLengthM());
            if (temp < 0) { return 0; }

            return temp;
        }

        private double checkStrokesCount(Signature original, Signature testSubject)
        {
            //NIEPRZETESTOWANE!!
            //sprawdzenie ilości stroków dla nowego podpisu wobec każdego z podpisów oryginalnych z osobna
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny

            int originalCount = original.getStrokesOriginal().Count();
            int testSubjectCount = testSubject.getStrokesOriginal().Count();

            Debug.WriteLine((originalCount));
            Debug.WriteLine((testSubjectCount));

            if (originalCount == testSubjectCount)
            {
                score = 1;
            }
            else
            {
                score = 1 - (Math.Abs(originalCount - testSubjectCount) * 0.20);
                if (score <= 0)
                {
                    score = 0;
                }
            }
            Debug.WriteLine("Wynik SignVerification dla checkStrokesCount " + score);

            return score;
        }

        private double checkTimeSizeRatio(Signature original, Signature testSubject)
        {

            //NIEPRZETESTOWANE!!
            //sprawdzenie checkTimeSizeRatio dla nowego podpisu wobec każdego z podpisów oryginalnych z osobna
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny
            double originalTotalRatio = original.getTimeSizeProbe().getTotalRatioAreaToTime();
            double testSubjectTotalRatio = testSubject.getTimeSizeProbe().getTotalRatioAreaToTime();

            if ((originalTotalRatio / testSubjectTotalRatio) <= 1)
            {
                score = 1 - (1 - originalTotalRatio / testSubjectTotalRatio);
            }
            else
            {
                score = 1 - (((originalTotalRatio / testSubjectTotalRatio)) - 1);
            }
            if(score < 0) { score = 0.0; }


            Debug.WriteLine("Wynik SignVerification dla checkTimeSizeRatio " + score);
            return score;
        }

        private double checkTimeSizeRatioForEachStroke(Signature first, Signature second)
        {
            double temp = 1;

            return temp;
        }

        private double checkPreciseComparison(Signature first, Signature second)
        {
            DynamicTimeWrapping dtw = new DynamicTimeWrapping();
            var result = dtw.calcSimilarity(first, second);
            if (result < 1100)
                return 0.95;
            if (result < 1200)
                return 0.8;
            if (result < 1500)
                return 0.7;
            if (result < 1450)
                return 0.6;
            if (result < 1500)
                return 0.2;
            return 0;
        }
    }
}
