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
        public int AuthorId { get; set; }
        public List<Stroke> Strokes { get; set; }
        public List<Stroke> StrokesModified { get; set; }
        public bool isOriginal { get; set; }

        public double length;
        private int strokesCount;

        public Signature()
        {
            Strokes = new List<Stroke>();
            StrokesModified = new List<Stroke>();
        }
        public Signature(bool isOriginal) : this()
        {
            strokesCount = 0;
            this.isOriginal = isOriginal;
        }

        public void init()
        {
            List<Stroke> temp = new List<Stroke>();
            foreach (Stroke st in Strokes)
            {
                temp.Add(new Stroke(st));
            }
            this.StrokesModified = temp;
            if (getAllModifiedPoints().Count > 0)
            {
                scaleSignature();
                fit();
                calcLength();
            }
        }

        public void addStroke(Stroke stroke)
        {
            Strokes.Add(stroke);
        }

        public void addStrokes(List<Stroke> strokes)
        {
            this.Strokes = strokes;
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
            return this.Strokes;
        }

        public List<Stroke> getStrokesModified()
        {
            return this.StrokesModified;
        }

        public List<Point> getAllOriginalPoints()
        {
            List<Point> points = new List<Point>();
            foreach (Stroke st in Strokes)
            {
                points.AddRange(st.getPoints());
            }
            return points;
        }

        public List<Point> getAllModifiedPoints()
        {
            List<Point> points = new List<Point>();
            foreach (Stroke st in StrokesModified)
            {
                points.AddRange(st.getPoints());
            }
            return points;
        }

        public List<Derivatives> getOriginalDerivatives()
        {
            List<Derivatives> d = new List<Derivatives>();
            foreach (Stroke stroke in Strokes)
            {
                d.AddRange(stroke.getDerivatives());
            }
            return d;
        }

        public List<Derivatives> getModifiedDerivatives()
        {
            List<Derivatives> d = new List<Derivatives>();
            foreach (Stroke stroke in StrokesModified)
            {
                d.AddRange(stroke.getDerivatives());
            }
            return d;
        }

        public void fit()
        {
            List<Point> points = this.getAllModifiedPoints();

            float x_min = points[0].getX();
            float y_min = points[0].getY();

            foreach (Point x in points)
            {
                if (x.getX() < x_min) { x_min = x.getX(); }
                if (x.getY() < y_min) { y_min = x.getY(); }
            }

            //List<Stroke> temp = new List<Stroke>();
            //foreach(Stroke st in strokesModified)
            //{
            //    temp.Add(new Stroke(st));
            //}

            foreach (Stroke st in this.StrokesModified)
            {
                foreach (Point x in st.getPoints())
                {
                    x.moveCordinates(-x_min, -y_min);
                }
            }

            //this.strokesModified = temp;
        }

        public void scaleSignature()
        {
            List<Point> points = this.getAllModifiedPoints();
            float[] p_y = points.Select(x => x.getY()).ToArray();
            
            float average = p_y.Average();

            float sumOfSquaresOfDifferences = p_y.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / p_y.Length);

            double  dpi = Windows.Graphics.Display.DisplayProperties.LogicalDpi;

            double mm = dpi / Convert.ToDouble(25.4);

            double hight = mm * 10;

            foreach (Stroke st in this.StrokesModified)
            {
                foreach (Point p in st.getPoints())
                {
                    double temp = p.getX() * (0.5 * hight / sd);
                    float x = Convert.ToSingle(temp);
                    temp = ((Convert.ToDouble(average)- p.getY()) * (0.5 * hight / sd)) + Convert.ToDouble(average);
                    float y = Convert.ToSingle(temp) - p.getY();

                    p.moveCordinates(x, -y);
                }
            }


        }

        public void calcLength()
        {
            double len = 0;
            List<Point> points = this.getAllModifiedPoints();
            
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