using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PodpisBio.Src.Author
{
    class Signature
    {
        private List<Stroke> strokesOriginal = new List<Stroke>();
        private List<Stroke> strokesModified = new List<Stroke>();
        double length;
        bool isOriginal;


        private int strokesCount;
        public Signature(bool isOriginal)
        {
            strokesCount = 0;
            this.isOriginal = isOriginal;
        }

        public void init()
        {
            fit();
            calcLength();
        }

        public void addStroke(Stroke stroke)
        {
            strokesOriginal.Add(stroke);
        }

        public void addStrokes(List<Stroke> strokes)
        {
            this.strokesOriginal = strokes;
        }
        
        public void increaseStrokesCount()
        {
            this.strokesCount = this.strokesCount + 1;
        }

        public int getStrokesCount()
        {
            return this.strokesCount;
        }

        public double getLentgh()
        {
            return this.length;
        }

        public List<Stroke> getStrokesOriginal()
        {
            return this.strokesOriginal;
        }

        public List<Stroke> getStrokesModified()
        {
            return this.strokesModified;
        }

        public List<Point> getAllOriginalPoints()
        {
            List<Point> points = new List<Point>();
            foreach (Stroke st in strokesOriginal)
            {
                points.AddRange(st.getPoints());
            }
            return points;
        }

        public List<Point> getAllModifiedPoints()
        {
            List<Point> points = new List<Point>();
            foreach (Stroke st in strokesModified)
            {
                points.AddRange(st.getPoints());
            }
            return points;
        }

        public void fit()
        {
            List<Point> points = this.getAllOriginalPoints();

            float x_min = points[0].getX();
            float y_min = points[0].getY();

            foreach (Point x in points)
            {
                if (x.getX() < x_min) { x_min = x.getX(); }
                if (x.getY() < y_min) { y_min = x.getY(); }
            }

            List<Stroke> temp = new List<Stroke>();
            foreach(Stroke st in strokesOriginal)
            {
                temp.Add(new Stroke(st));
            }
            
            foreach (Stroke st in temp)
            {
                foreach(Point x in st.getPoints())
                {
                    x.moveCordinates(x_min, y_min);
                }
            }

            this.strokesModified = temp;
        }

        public void calcLength()
        {
            double len = 0;
            List<Point> points = this.getAllModifiedPoints();

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
            p=p.Distinct().ToList();
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