using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author.Weight
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

            double calcLengthM = calc_SD(lengthMList);
            double calcStrokesCount = calc_SD(strokesCountList);
            double calcTotalRatio = calc_SD(totalRatioList);
            double calcTotalRatioForEachStroke = calcTotalRatioForEachStroke_SD(totalRatioForEachStrokeList);

            double temp = calcLengthM + calcStrokesCount;

            this.lengthMWeight = calcLengthM / temp;
            this.strokesCountWeight = calcStrokesCount / temp;
        }

        private double calc_SD(List<double> list)
        {
            double temp = 0;
            float[] array = list.Select(x => (float)x).ToArray();
            float average = array.Average();

            float sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / array.Length);

            temp = 1 - (sd / Convert.ToDouble(average));

            return temp;
        }

        private double calcTotalRatioForEachStroke_SD(List<List<double>> list)
        {
            double temp = 0;
            double[][] array = list.Select(a => a.ToArray()).ToArray();
            


            return temp;
        }
        
    }
}
