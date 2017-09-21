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

        private void init()
        {
            List<double> lengthMList = new List<double>();
            List<double> strokesCountList = new List<double>();
            List<double> totalRatioList = new List<double>();
            List<List<double>> totalRatioForEachStrokeList = new List<List<double>>();

            foreach (Signature s in this.sign)
            {
                lengthMList.Add(s.getLentghM());
                strokesCountList.Add(Convert.ToDouble(s.getStrokesOriginal().Count));
                totalRatioList.Add(s.getTimeSizeProbe().getTotalRatioAreaToTime());
                totalRatioForEachStrokeList.Add(s.getTimeSizeProbe().getRatioAreaToTimeForEachStroke());

            }

            double calcLengthM = calcLengthM_SD(lengthMList);
            double calcStrokesCount = calcStrokesCount_SD(strokesCountList);

            double temp = calcLengthM + calcStrokesCount;

            this.lengthMWeight = calcLengthM / temp;
            this.strokesCountWeight = calcStrokesCount / temp;
        }

        private double calcLengthM_SD(List<double> list)
        {
            double temp = 0;
            float[] lengthM = list.Select(x => (float)x).ToArray();
            float average = lengthM.Average();

            float sumOfSquaresOfDifferences = lengthM.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / lengthM.Length);

            temp = 1 - (sd / Convert.ToDouble(average));

            return temp;
        }

        private double calcStrokesCount_SD(List<double> list)
        {
            double temp = 0;
            float[] strokesCount = list.Select(x => (float)x).ToArray();
            float average = strokesCount.Average();

            float sumOfSquaresOfDifferences = strokesCount.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / strokesCount.Length);

            temp = 1 - (sd / Convert.ToDouble(average));

            return temp;
        }
    }
}
