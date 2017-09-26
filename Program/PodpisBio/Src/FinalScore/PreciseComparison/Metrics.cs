using PodpisBio.Src.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore.PreciseComparison
{
    class Metrics
    {
        public Func<Point, Point, Derivatives, Derivatives, float> METRIC;

        public Metrics() { METRIC = EuclidianDistance; }
        private float EuclidianDistancePoints(Point p1, Point p2, Derivatives d1, Derivatives d2)
        {
            var sum = (p1.X - p2.X) * (p1.X - p2.X);
            sum += (p1.Y - p2.Y) * (p1.Y - p2.Y);

            sum += (p1.Pressure - p2.Pressure) * (p1.Pressure - p2.Pressure);

            sum += (p1.tiltX - p2.tiltX) * (p1.tiltX - p2.tiltX);
            sum += (p1.tiltY - p2.tiltY) * (p1.tiltY - p2.tiltY);

            return (float)Math.Sqrt(sum);
        }

        private float EuclidianDistanceDerivatives(Point p1, Point p2, Derivatives d1, Derivatives d2)
        {
            float sum = 0;
            sum += (d1.Velocity - d2.Velocity) * (d1.Velocity - d2.Velocity);
            sum += (d1.VelocityX - d2.VelocityX) * (d1.VelocityX - d2.VelocityX);
            sum += (d1.VelocityY - d2.VelocityY) * (d1.VelocityY - d2.VelocityY);

            sum += (d1.Acc - d2.Acc) * (d1.Acc - d2.Acc);
            sum += (d1.AccX - d2.AccX) * (d1.AccX - d2.AccX);
            sum += (d1.AccY - d2.AccY) * (d1.AccY - d2.AccY);

            sum += (d1.PressureChange - d2.PressureChange) * (d1.PressureChange - d2.PressureChange);

            // póki tilty nie działają, nie dodaję ich.
            return (float)Math.Sqrt(sum);
        }

        private float EuclidianDistance(Point p1, Point p2, Derivatives d1, Derivatives d2)
        {
            float sum = 0;

            sum += EuclidianDistancePoints(p1, p2, d1, d2) * EuclidianDistancePoints(p1, p2, d1, d2);
            sum += EuclidianDistanceDerivatives(p1, p2, d1, d2) * EuclidianDistanceDerivatives(p1, p2, d1, d2);

            return (float)Math.Sqrt(sum);
        }
    }
}
