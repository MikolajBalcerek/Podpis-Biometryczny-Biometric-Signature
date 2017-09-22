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
        public List<double> init(Signature sign, List<Signature> signList/*, Wagi*/)
        {
            List<double> ver = new List<double>();
            foreach(Signature first in signList)
            {
                ver.Add(check(first, sign));
            }

            return ver;
        }

        private double check(Signature first, Signature second/*, Wagi*/)
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

            temp = (lengthM + strokesCount + timeSizeRatio + timeSizeRatioForEachStroke + preciseComparison) / 5;
            //temp = lengthM * waga + strokesCount * waga + timeSizeRatio * waga + timeSizeRatioForEachStroke * waga + preciseComparison * waga;

            return temp;
        }

        private double checkLengthM(Signature first, Signature second)
        {
            double temp = 1-(Math.Abs(first.getLentghM()-second.getLentghM()) / first.getLentghM());
            if (temp < 0) { return 0; }

            return temp;
        }

        private double checkStrokesCount(Signature original, Signature testSubject/*, Wagi*/)
        {
            //NIEPRZETESTOWANE!!
            //sprawdzenie ilości stroków dla nowego podpisu wobec każdego z podpisów oryginalnych z osobna
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny

            int originalCount = original.getStrokesOriginal().Count();
            int testSubjectCount = original.getStrokesOriginal().Count();

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

        private double checkTimeSizeRatio(Signature original, Signature testSubject /*, Wagi*/)
        {

            //NIEPRZETESTOWANE!!
            //sprawdzenie checkTimeSizeRatio dla nowego podpisu wobec każdego z podpisów oryginalnych z osobna
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny
            double originalTotalRatio = original.getTimeSizeProbe().getTotalRatioAreaToTime();
            double testSubjectTotalRatio = testSubject.getTimeSizeProbe().getTotalRatioAreaToTime();

            if ((originalTotalRatio/testSubjectTotalRatio) <= 1)
            {
                score = 1 - (1 - originalTotalRatio / testSubjectTotalRatio);
            }
            else
            {
                score = 1 - (((originalTotalRatio / testSubjectTotalRatio)) - 1);
            }

            Debug.WriteLine("Wynik SignVerification dla checkTimeSizeRatio " + score);
            return score;
        }

        private double checkTimeSizeRatioForEachStroke(Signature first, Signature second/*, Wagi*/)
        {
            double temp = 1;

            return temp;
        }

        private double checkPreciseComparison(Signature first, Signature second/*, Wagi*/)
        {
            double temp = 1;

            return temp;
        }
    }
}
