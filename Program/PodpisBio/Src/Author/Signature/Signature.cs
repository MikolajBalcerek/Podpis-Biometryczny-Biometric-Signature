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
        //PRAMETRY AKTUALIZOWANE Z BAZ¥ DANYCH//
        public int AuthorId { get; set; }
        public List<Stroke> Strokes { get; set; }
        public List<Stroke> StrokesModified { get; set; }
        public bool isOriginal { get; set; }
        //KONIEC//

        double lengthO;
        double lengthM;
        double height;
        //private List<InkStroke> richInkStrokes; //stroke z timestampami od microsoftu

        private Author.TimeSize_Probe ownTimeSizeProbe; //klasa badaj¹ca w³asnoœci czasu i rozmiaru podpisu

        public Signature()
        {
            Strokes = new List<Stroke>();

            StrokesModified = new List<Stroke>();
            //richInkStrokes = new List<InkStroke>();
            lengthO = 0;
            lengthM = 0;
            height = 0;
        }
        //G³ówny konstruktor do tworzenia sygnatury
        public Signature(List<Stroke> strokes, int authorId, bool isOriginal) : this()
        {
            this.isOriginal = isOriginal;
            this.AuthorId = authorId;
            this.Strokes = strokes;

            //Docelowo bêdzie wykonywane z tego poziomu, jednak konstruktor nie inicjalizuje wszystkich potrzebnych zmiennych (brak w modelu bazy danych)
            //init();
        }

        //Inicjalizacja obliczeñ
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
            }
            calcLength(false);
            calcHeight();

            //badanie rozmiaru/czas za pomoc¹ TimeSize klasy
            if (object.ReferenceEquals(null, this.ownTimeSizeProbe))
            {
                this.ownTimeSizeProbe = new TimeSize_Probe(this);
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
        


        public double getLentghO()
        {
            return this.lengthO;
        }

        public double getLentghM()
        {
            return this.lengthM;
        }

        public double getHeight()
        {
            return this.height;
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

        public TimeSize_Probe getTimeSizeProbe()
        {
            if(object.ReferenceEquals(null, this.ownTimeSizeProbe))
            {
                this.ownTimeSizeProbe = new TimeSize_Probe(this);
            }
            return this.ownTimeSizeProbe;
        }

        public void calcLength(bool xD)
        {
            double length = 0;
            List<Point> points = new List<Point>();
            if (xD)
            {
                points = this.getAllOriginalPoints();
            }
            else
            {
                points = this.getAllModifiedPoints();
            }
            
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
                //Debug.WriteLine("Dlugosc =" + (max - min));
            }
            else
            {
                //Debug.WriteLine("Dlugosc = 0");
            }

            if (xD)
            {
                this.lengthO = length;
            }
            else
            {
                this.lengthM = length;
            }
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
                //Debug.WriteLine("Wysokosc =" + (max - min));
            }
            else
            {
                //Debug.WriteLine("Wysokosc = 0");
            }

            this.height = height;
        }
    }
}