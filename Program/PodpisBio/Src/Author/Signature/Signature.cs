using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Input.Inking;
using PodpisBio.Src.Author;

namespace PodpisBio.Src
{
    class Signature
    {
        private List<Stroke> strokesOriginal = new List<Stroke>();
        private List<Stroke> strokesModified = new List<Stroke>();
        double length;
        double height;
        bool isOriginal;

        private List<InkStroke> richInkStrokes; //stroke z timestampami od microsoftu

        public void increaseStrokesCount() { this.strokesCount = this.strokesCount + 1; }
        public int getStrokesCount() { return this.strokesCount; }

        private Author.TimeSize_Probe ownTimeSizeProbe; //klasa badaj¹ca w³asnoœci czasu i rozmiaru podpisu

        private int strokesCount;
        public Signature(List<InkStroke> richInkStrokesGiven, bool isOriginal)
        {
            strokesCount = 0;
            length = 0;
            height = 0;
            this.richInkStrokes = richInkStrokesGiven;
            this.isOriginal = isOriginal;
        }

        public Signature(List<Stroke> strokes, bool isOriginal)
        {
            strokesCount = 0;
            length = 0;
            height = 0;
            this.isOriginal = isOriginal;
            this.strokesOriginal = strokes;
            this.richInkStrokes = new List<InkStroke>();
            //this.init();
            //ownTimeSizeProbe = new Author.TimeSize_Probe(this);

        }

        public Signature(List<Stroke> strokes, List<InkStroke> richInkStrokes, bool isOriginal)
        {
            strokesCount = 0;
            length = 0;
            height = 0;
            this.isOriginal = isOriginal;
            this.strokesOriginal = strokes;
            this.richInkStrokes = richInkStrokes;
            //this.init();
            //ownTimeSizeProbe = new Author.TimeSize_Probe(this);
        }

        public void init()
        {
            List<Stroke> temp = new List<Stroke>();
            foreach (Stroke st in strokesOriginal)
            {
                temp.Add(new Stroke(st));
            }
            this.strokesModified = temp;
            if (getAllModifiedPoints().Count > 0)
            {
                scaleSignature();
                fit();
                calcLength();
                calcHeight();
            }

            //badanie rozmiaru/czas za pomoc¹ TimeSize klasy
            ownTimeSizeProbe = new Author.TimeSize_Probe(this);
        }

        public void addStroke(Stroke stroke)
        {
            strokesOriginal.Add(stroke);
        }

        public void addStrokes(List<Stroke> strokes)
        {
            this.strokesOriginal = strokes;
        }
        


        public double getLentgh()
        {
            return this.length;
        }

        public double getHeight()
        {
            return this.height;
        }


        public List<Stroke> getStrokesOriginal()
        {
            return this.strokesOriginal;
        }

        public List<Stroke> getStrokesModified()
        {
            return this.strokesModified;
        }

        public List<InkStroke> getRichStrokes()
        {
            return this.richInkStrokes;
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

        public List<Derivatives> getOriginalDerivatives()
        {
            List<Derivatives> d = new List<Derivatives>();
            foreach (Stroke stroke in strokesOriginal)
            {
                d.AddRange(stroke.getDerivatives());
            }
            return d;
        }

        public List<Derivatives> getModifiedDerivatives()
        {
            List<Derivatives> d = new List<Derivatives>();
            foreach (Stroke stroke in strokesModified)
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

            foreach (Stroke st in this.strokesModified)
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

            foreach (Stroke st in this.strokesModified)
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

        public void calcHeight()
        {
            //liczy wysokoœæ 
            double height = 0;
            List<Point> points = this.getAllOriginalPoints();
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