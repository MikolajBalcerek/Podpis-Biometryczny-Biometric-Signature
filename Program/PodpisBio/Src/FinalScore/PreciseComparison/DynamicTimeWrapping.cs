using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore
{
    class DynamicTimeWrapping
    {
        public DynamicTimeWrapping() { }

        public float calcDTW(List<float> ts1, List<float> ts2)
        {
            var n = ts1.Count + 1;
            var m = ts2.Count + 1;

            float[][] dtw = new float[n][];
            //jagged arrays są szybsze od [,]
            for (int i = 0; i < m; i++)
                dtw[i] = new float[m];

            for (int i = 0; i < n; i++)
                dtw[i][0] = float.PositiveInfinity;
            for (int i = 0; i < m; i++)
                dtw[0][i] = float.PositiveInfinity;
            dtw[0][0] = 0;

            for (int i = 1; i < n; i++)
                for (int j = 1; j < m; j++)
                    dtw[i][j] = Math.Abs(ts1[i - 1] - ts2[j - 1]) + min(dtw[i - 1][j], dtw[i][j - 1], dtw[i - 1][j - 1]);

            return dtw[n - 1][m - 1];
        }

        private float min(float a, float b, float c)
        {
            return Math.Min(a, Math.Min(b, c));
        }
    }
}
