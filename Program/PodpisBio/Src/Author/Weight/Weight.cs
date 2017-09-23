using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PodpisBio.Src
{
    class Weight
    {
        private List<Signature> sign;
        
        private double lengthMWeight;
        private double strokesCountWeight;
        private double totalRatioWeight;
        private double totalRatioForEachStrokeWeight;

        public Weight(List<Signature> sign)
        {
            this.sign = sign;
            init();
        }

        public double getLengthMWeight()
        {
            return this.lengthMWeight;
        }

        public double getStrokesCountWeight()
        {
            return this.strokesCountWeight;
        }

        public double getTotalRatioWeight()
        {
            return this.totalRatioWeight;
        }

        public double getTotalRatioForEachStrokeWeight()
        {
            return this.totalRatioForEachStrokeWeight;
        }

        private void init()
        {
            double forWeight = 0.7;
            List<double> lengthMList = new List<double>();
            List<double> strokesCountList = new List<double>();
            List<double> totalRatioList = new List<double>();
            List<List<double>> totalRatioForEachStrokeList = new List<List<double>>();

            foreach (Signature s in this.sign)
            {
                lengthMList.Add(s.getLengthM());
                strokesCountList.Add(Convert.ToDouble(s.getStrokesOriginal().Count));
                totalRatioList.Add(s.getTimeSizeProbe().getTotalRatioAreaToTime());
                totalRatioForEachStrokeList.Add(s.getTimeSizeProbe().getRatioAreaToTimeForEachStroke());

            }

            double calcLengthM = calcLengthM_SD(lengthMList);
            double calcStrokesCount = calcStrokesCount_SD(strokesCountList);
            double calcTotalRatio = calcTotalRatio_SD(totalRatioList) * 2.0;
            double calcTotalRatioForEachStroke = calcTotalRatio / 2.0;

            double temp = calcLengthM + calcStrokesCount + calcTotalRatio + calcTotalRatioForEachStroke;

            this.lengthMWeight = calcLengthM / temp * forWeight;
            this.strokesCountWeight = calcStrokesCount / temp * forWeight;
            this.totalRatioWeight = calcTotalRatio / temp * forWeight;
            this.totalRatioForEachStrokeWeight = calcTotalRatioForEachStroke / temp * forWeight;
            //Debug.WriteLine("Wagi heheszki "+ this.lengthMWeight +" "+ this.strokesCountWeight+" "+ this.totalRatioWeight+" "+ this.totalRatioForEachStrokeWeight);
        }

        private double calc_SD(List<double> list)
        {
            double temp = 0;
            float[] array = list.Select(x => (float)x).ToArray();
            float average = array.Average();

            float sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / array.Length);

            temp = 1 - (sd / Convert.ToDouble(average));
            if (temp < 0) { return 0.0; }

            return temp;
        }

        private double calcLengthM_SD(List<double> list)
        {
            return calc_SD(list);
        }

        private double calcStrokesCount_SD(List<double> list)
        {
            return calc_SD(list);
        }

        private double calcTotalRatio_SD(List<double> list)
        {
            return calc_SD(list);
        }

        //private double calcTotalRatioForEachStroke_SD(List<List<double>> list)
        //{
        //    double temp = 0;
        //    double[][] array = list.Select(a => a.ToArray()).ToArray();
            


        //    return temp;
        //}
        
    }
}
