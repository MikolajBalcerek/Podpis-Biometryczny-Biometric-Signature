using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PodpisBio.Src.Author
{
    class Stroke
    {
        List<Point> points = new List<Point>();
        List<Derivatives> derivatives = new List<Derivatives>();

        public Stroke() { }

        public Stroke(List<Point> points)
        {
            this.points = points;
        }

        public Stroke(Stroke stroke)
        {
            foreach(Point p in stroke.getPoints())
            {
                this.points.Add(new Point(p));
            }
            this.derivatives = stroke.getDerivatives();
        }

        public List<Point> getPoints() { return points; }

        public List<Derivatives> getDerivatives() { return this.derivatives; }

        public void addPoint(Point point)
        {
            if (!this.points.Any())
            {
                this.points.Add(point);
                this.derivatives.Add(new Derivatives());
            }
            else
            {
                Dynamics calculator = new Dynamics();
                var prevPoint = this.points[this.points.Count - 1];
                var prevDerivatives = this.derivatives[this.derivatives.Count() - 1];
                var derivatives = calculator.calcDerivatives(prevPoint, point, prevDerivatives);
                this.points.Add(point);
                this.derivatives.Add(derivatives);
                Debug.WriteLine("Adam oblicza pochodne v = " + derivatives.Velocity + " i acc = " + derivatives.Acc);
            }

        }

        public float getWidth() { return points.Max(pt => pt.getX()) - points.Min(pt => pt.getX()); }

        public float getHeight() { return points.Max(pt => pt.getY()) - points.Min(pt => pt.getY()); }

        public ulong getTime() { return points[points.Count - 1].getTime() - points[0].getTime(); }

        // TODO: MK dodaj obliczanie sługości i średniej szybkości
    }
}
