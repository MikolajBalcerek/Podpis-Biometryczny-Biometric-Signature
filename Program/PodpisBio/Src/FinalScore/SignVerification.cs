using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore
{
    //Sprawdza podany podpis ze wszystkimi z podanej listy (return lista podobienstwa dla kazdego z podanej listy (0-1))
    class SignVerification
    {
        public List<double> init(Signature sign, List<Signature> signList, Weight weights)
        {
            //Debug.WriteLine("Next");

            List<double> forDTW = new List<double>();
            for (int i = 0; i < signList.Count; i++)
            {
                for (int j = i+1; j < signList.Count; j++)
                {
                    if (i != j)
                    {
                        DynamicTimeWrapping dtw = new DynamicTimeWrapping();
                        forDTW.Add(dtw.calcSimilarity(signList[j], signList[i]));
                    }
                }
            }
            //StringBuilder result = new StringBuilder();
            //result.Append(forDTW.Count + " <> ");
            //foreach (var d in forDTW)
            //{
            //    result.Append(" " + d);
            //}
            //Debug.WriteLine(result);

            double averDTW = forDTW.Average();

            List<double> ver = new List<double>();
            foreach (Signature first in signList)
            {
                ver.Add(check(first, sign, weights,averDTW));
            }

            return ver;
        }
        
        //Podobienstwo 2 podpisow (return 1 - identyczne)
        private double check(Signature first, Signature second, Weight weights, double forDTW)
        {
            double temp = 0;
            double lengthM = checkLengthM(first, second);
            if (lengthM < 0.5)
            {
                return 0;
            }
            double strokesCount = checkStrokesCount(first, second);
            double timeSizeRatio = checkTimeSizeRatio(first, second);
            double timeSizeRatioAverageForEachStroke = checkAverageTimeSizeRatioForEachStroke(first, second);
            //double preciseComparison = checkPreciseComparison(first, second);
            double preciseComparison = checkPreciseComparisonByJA(first, second, forDTW);
            /*
             */

            //temp = preciseComparison;
            temp = lengthM * weights.getLengthMWeight() + strokesCount * weights.getStrokesCountWeight() + timeSizeRatio * weights.getTotalRatioWeight() + timeSizeRatioAverageForEachStroke * weights.getAverageTotalRatioForEachStrokeWeight()  + preciseComparison * weights.getPreciseComparisonWeight();
            //temp = temp * (1 / (weights.getLengthMWeight() + weights.getStrokesCountWeight()+weights.getTotalRatioWeight() + weights.getAverageTotalRatioForEachStrokeWeight()));
            return temp;
        }

        //Podobienstwo dlugosci sygnatur (return 1 - identyczne)
        private double checkLengthM(Signature first, Signature second)
        {
            double temp = 1 - (Math.Abs(first.getLengthM() - second.getLengthM()) / first.getLengthM());
            if (temp < 0) { return 0; }

            return temp;
        }
        
        //Podobienstwo ilosci nacisniec (return 1 - identyczne)
        private double checkStrokesCount(Signature original, Signature testSubject)
        {
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny

            int originalCount = original.getStrokesOriginal().Count();
            int testSubjectCount = testSubject.getStrokesOriginal().Count();

            //Debug.WriteLine((originalCount));
            //Debug.WriteLine((testSubjectCount));

            if (originalCount == testSubjectCount)
            {
                score = 1;
            }
            else
            {
                score = 1 - (Math.Abs(originalCount - testSubjectCount) * 0.15);
                if (score <= 0)
                {
                    score = 0;
                }
            }
            //Debug.WriteLine("Wynik SignVerification dla checkStrokesCount " + score);

            return score;
        }
        
        private double checkTimeSizeRatio(Signature original, Signature testSubject)
        {
            //NIEPRZETESTOWANE
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

        private double checkAverageTimeSizeRatioForEachStroke(Signature original, Signature testSubject)
        {

            //NIEPRZETESTOWANE 
            //sprawdza dla każdego podpisu z oryginalnych z osobna
            //porównuje średnie z TimeSize pociągnięć
            double score = 1; // Zmienna zwracająca jak dobrze metoda uważa podpis jest wiarygodny
            List<Double> originalTimeSizeRatioForEachStroke = original.getTimeSizeProbe().getRatioAreaToTimeForEachStroke();
            List<Double> testSubjectTimeSizeRatioForEachStroke = testSubject.getTimeSizeProbe().getRatioAreaToTimeForEachStroke();

            //double __stroke__weight__ = 1 / originalTimeSizeRatioForEachStroke.Count(); // wartość wagi maksymalna dla jednego porównania stroków to 1 (maksymalny wynik dla wszystkich) przez ilość stroków w oryginalne

            double averageOriginal = originalTimeSizeRatioForEachStroke.Average();
            double averageTestSubject = testSubjectTimeSizeRatioForEachStroke.Average();

            score = 1 - ((Math.Pow((Math.Abs(averageOriginal - averageTestSubject) / averageOriginal), 2)) * 0.8);
            if (score <= 0)
            {
                score = 0;
            }



            Debug.WriteLine("Wynik z Średni timeSizeStroke " + score);

            return score;
        }

        private double checkPreciseComparison(Signature first, Signature second)
        {
            DynamicTimeWrapping dtw = new DynamicTimeWrapping();
            var result = dtw.calcSimilarity(first, second);
            Debug.WriteLine(result);
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


        private double checkPreciseComparisonByJA(Signature first, Signature second, double forDTW)
        {
            DynamicTimeWrapping dtw = new DynamicTimeWrapping();
            var result = dtw.calcSimilarity(first, second);
            //Debug.WriteLine(result);

            double temp = 0.0;

            if(result <= forDTW)
            {
                temp = 1.0;
            }
            else
            {
                temp = 1.0 - (Math.Abs((forDTW - result) / forDTW));
            }
            if (temp < 0) { return 0.0; }

            return temp;
        }
    }
}
