using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PodpisBio.Src
{
    class Signature
    {
        private List<Stroke> strokes = new List<Stroke>();
        double length;
        bool isOriginal;

        public void increaseStrokesCount() { this.strokesCount = this.strokesCount + 1; }
        public int getStrokesCount() { return this.strokesCount; }


        private int strokesCount;
        public Signature(bool isOriginal)
        {
            strokesCount = 0;
            this.isOriginal = isOriginal;
        }

        public void init()
        {
            calcLength();
        }

        public void addStroke(Stroke stroke) { strokes.Add(stroke); }
        public void addStrokes(List<Stroke> strokes) { this.strokes = strokes; }

        public double getLentgh()
        {
            return this.length;
        }

        public List<Stroke> getStrokes()
        {
            return this.strokes;
        }

        public List<Point> getAllPoints()
        {
            List<Point> points = new List<Point>();
            foreach (Stroke st in strokes)
            {
                points.AddRange(st.getPoints());
            }
            return points;
        }



        public void calcLength()
        {
            double len = 0;
            List<Point> points = this.getAllPoints();

            Debug.WriteLine("Adam liczy");

            List<double> p = new List<double>();

            for (int i = 0; i < points.Count(); i++)
            {

                int temp = 0;
                foreach (Point s in points)
                {
                    if ((s.getX() - 1 < points[i].getX() && s.getX() + 1 > points[i].getX()) && (s.getY() > points[i].getY() + 2 || s.getY() < points[i].getY() - 2))
                    {
                        temp++;
                    }
                }
                if (temp > 0)
                {
                    p.Add(points[i].getX());
                }
            }
            p.Distinct();
            if (p.Count() > 0)
            {
                double min = p[0];
                double max = p[0];
                foreach (double elem in p)
                {
                    if (elem < min) { min = elem; }
                    if (elem > max) { max = elem; }
                }
                length = max - min;
                Debug.WriteLine("Dlugosc =" + (max - min));
            }
            else
            {
                Debug.WriteLine("Dlugosc = 0");
            }

            this.length = len;
        }
    }
}