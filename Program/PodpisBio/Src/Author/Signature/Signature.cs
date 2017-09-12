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
        double height;

        public void increaseStrokesCount() { this.strokesCount = this.strokesCount + 1; }
        public int getStrokesCount() { return this.strokesCount; }

        private Author.TimeSize_Probe ownTimeSizeProbe; //klasa badaj¹ca w³asnoœci czasu i rozmiaru podpisu


        private int strokesCount;
        public Signature()
        {
            strokesCount = 0;
            length = 0;
            height = 0;
        }
        public Signature(List<Stroke> strokes)
        {
            strokesCount = 0;
            length = 0;
            height = 0;
            this.strokes = strokes;
            ownTimeSizeProbe = new Author.TimeSize_Probe(this);

        }

        public void init()
        {
            calcLength();
            calcHeight();
        }

        public void addStroke(Stroke stroke) { strokes.Add(stroke); }
        public void addStrokes(List<Stroke> strokes) { this.strokes = strokes; }

        public double getLentgh()
        {
            return this.length;
        }

        public double getHeight()
        {
            return this.height;
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

        public void calcHeight()
        {
            //liczy wysokoœæ 
            double height = 0;
            List<Point> points = this.getAllPoints();
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
                    p.Add(points[i].getY());
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
                height = max - min;
                Debug.WriteLine("Wysokosc =" + (max - min));
            }
            else
            {
                Debug.WriteLine("Wysokosc = 0");
            }

            this.height = height;
        }
    }
}