using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src
{
    class Stroke
    {
        List<Point> points = new List<Point>();

        public Stroke() { }
        public Stroke(List<Point> points)
        {
            this.points = points;
        }

        public List<Point> getPoints() { return points; }
        public void addPoint(Point point) { points.Add(point); }
    }
}
