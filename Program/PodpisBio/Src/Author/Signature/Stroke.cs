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
        public void addPoint(Point point)
        {
            //var velocity = this.calcVelocity(point.getX(), point.getY(), point.getTime());
            //var pressChange = calcPressureChange(point.getPressure());
            //point.setVelocity(velocity);
            //point.setPressureChange(pressChange);
            points.Add(point);
        }
        //private float calcVelocity(float x, float y, long time)
        //{
        //    if (!this.points.Any())
        //        return 0;
        //    var prevPoint = this.points[this.points.Count() - 1];
        //    var deltaTime = time - prevPoint.getTime();
        //    var deltaX = x - prevPoint.getX();
        //    var deltaY = y - prevPoint.getY();
        //    return (deltaX * deltaX + deltaY * deltaY) / deltaTime;
        //}

        //private int calcPressureChange(float pressure)
        //{
        //    if (!this.points.Any())
        //        return 0;
        //    var previousPress = this.points[this.points.Count() - 1].getPressure();
        //    var difference = pressure - previousPress;
        //    return (difference == 0) ? 0 : (difference > 0 ? 1 : -1);
        //}
    }
}
