﻿using PodpisBio.Src.Author;
using PodpisBio.Src.FinalScore.PreciseComparison;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore
{
    class DynamicTimeWrapping
    {
        const bool REMOVEOUTLYING = true;
        const bool TIMED_METRIC = false;
        DataPreparation dataPrep = new DataPreparation();
        Metrics metrics = new Metrics();
        public DynamicTimeWrapping() { }

        private float calcSimpleDTW(List<float> ts1, List<float> ts2)
        {
            var n = ts1.Count + 1;
            var m = ts2.Count + 1;

            float[][] dtw = new float[n][];
            //jagged arrays są szybsze od [,]
            for (int i = 0; i < n; i++)
                dtw[i] = new float[m];

            for (int i = 0; i < n; i++)
                dtw[i][0] = float.PositiveInfinity;
            for (int j = 0; j < m; j++)
                dtw[0][j] = float.PositiveInfinity;
            dtw[0][0] = 0;

            for (int i = 1; i < n; i++)
                for (int j = 1; j < m; j++)
                    dtw[i][j] = Math.Abs(ts1[i - 1] - ts2[j - 1]) + min(dtw[i - 1][j], dtw[i][j - 1], dtw[i - 1][j - 1]);

            return dtw[n - 1][m - 1];
        }

        private float calcMetricDTW(List<Point> pts1, List<Point> pts2, List<Derivatives> der1, List<Derivatives> der2,
            Func<Point, Point, Derivatives, Derivatives, float> d)
        {
            var n = pts1.Count + 1;
            var m = pts2.Count + 1;

            float[][] dtw = new float[n][];
            //jagged arrays są szybsze od [,]
            for (int i = 0; i < n; i++)
                dtw[i] = new float[m];

            for (int i = 0; i < n; i++)
                dtw[i][0] = float.PositiveInfinity;
            for (int j = 0; j < m; j++)
                dtw[0][j] = float.PositiveInfinity;
            dtw[0][0] = 0;

            for (int i = 1; i < n; i++)
                for (int j = 1; j < m; j++)
                {
                    var distance = d(pts1[i - 1], pts2[j - 1], der1[i - 1], der2[j - 1]);
                    dtw[i][j] = distance + min(dtw[i - 1][j], dtw[i][j - 1], dtw[i - 1][j - 1]);
                }

            return dtw[n - 1][m - 1];
        }

        private float calcMetricTimeDTW(List<Point> pts1, List<Point> pts2, List<Derivatives> der1, List<Derivatives> der2,
    Func<Point, Point, Derivatives, Derivatives, int, int, float> d)
        {
            var n = pts1.Count + 1;
            var m = pts2.Count + 1;

            float[][] dtw = new float[n][];
            //jagged arrays są szybsze od [,]
            for (int i = 0; i < n; i++)
                dtw[i] = new float[m];

            for (int i = 0; i < n; i++)
                dtw[i][0] = float.PositiveInfinity;
            for (int j = 0; j < m; j++)
                dtw[0][j] = float.PositiveInfinity;
            dtw[0][0] = 0;

            for (int i = 1; i < n; i++)
                for (int j = 1; j < m; j++)
                {
                    var distance = d(pts1[i - 1], pts2[j - 1], der1[i - 1], der2[j - 1], i - 1, j - 1);
                    dtw[i][j] = distance + min(dtw[i - 1][j], dtw[i][j - 1], dtw[i - 1][j - 1]);
                }

            return dtw[n - 1][m - 1];
        }


        public float calcSimilarity(List<float> sgn1Feature, List<float> sgn2Feature)
        {
            Debug.WriteLine("Śr feature " + sgn1Feature.Average());
            var normalisedSgn1 = dataPrep.prepareFeature(sgn1Feature, REMOVEOUTLYING);
            var normalisedSgn2 = dataPrep.prepareFeature(sgn2Feature, REMOVEOUTLYING);


            Debug.WriteLine("Śr feature " + normalisedSgn1.Average());
            return calcSimpleDTW(sgn1Feature, sgn2Feature);
        }

        public float calcSimilarity(List<Point> points1, List<Point> points2, 
            List<Derivatives> derivatives1, List<Derivatives> derivatives2)
        {
            if (REMOVEOUTLYING)
            {
                dataPrep.removeOddsValues(ref points1, ref derivatives1);
                dataPrep.removeOddsValues(ref points2, ref derivatives2);
            }

            points1 = dataPrep.preparePoints(points1);
            points2 = dataPrep.preparePoints(points2);
            derivatives1 = dataPrep.prepareDerivatives(derivatives1);
            derivatives2 = dataPrep.prepareDerivatives(derivatives2);

            //Debug.WriteLine("Śr X " + points1.Average(x => x.X));
            //Debug.WriteLine("Śr Y " + points1.Average(x => x.Y));
            //Debug.WriteLine("Śr Pressure " + points1.Average(x => x.Pressure));
            //Debug.WriteLine("Śr Velocity " + derivatives1.Average(x => x.Velocity));
            //Debug.WriteLine("Śr VelocityX " + derivatives1.Average(x => x.VelocityX));
            //Debug.WriteLine("Śr VelocityY " + derivatives1.Average(x => x.VelocityY));
            //Debug.WriteLine("Śr Acc " + derivatives1.Average(x => x.Acc));
            //Debug.WriteLine("Śr AccX " + derivatives1.Average(x => x.AccX));
            //Debug.WriteLine("Śr AccY " + derivatives1.Average(x => x.AccY));

            if (TIMED_METRIC)
                return calcMetricTimeDTW(points1, points2, derivatives1, derivatives2, metrics.METRIC_TIMED);

            return calcMetricDTW(points1, points2, derivatives1, derivatives2, metrics.METRIC);
        }

        public float calcSimilarity(Signature sgn1, Signature sgn2)
        {
            var points1 = sgn1.getAllOriginalPoints();
            var derivatives1 = sgn1.getOriginalDerivatives();

            var points2 = sgn2.getAllOriginalPoints();
            var derivatives2 = sgn2.getOriginalDerivatives();

            return calcSimilarity(points1, points2, derivatives1, derivatives2);
        }

        private float min(float a, float b, float c)
        {
            return Math.Min(a, Math.Min(b, c));
        }
    }
}
