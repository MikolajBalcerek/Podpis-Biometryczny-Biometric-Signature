using PodpisBio.Src.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src
{
    class Point
    {
        public float X, Y, Pressure, tiltX, tiltY;
        public long Timestamp;

        public Point(){}

        public Point(float x, float y, float pressure, ulong time, float tiltX, float tiltY)
        {
            this.X = x;
            this.Y = y;
            this.Pressure = pressure;
            this.Timestamp = (long)time;
            this.tiltX = tiltX;
            this.tiltY = tiltY;
        }

        public Point(Point point)
        {
            this.X = point.getX();
            this.Y = point.getY();
            this.Pressure = point.getPressure();
            this.Timestamp = point.getTime();
            this.tiltX = point.getTiltX();
            this.tiltY = point.getTiltY();
        }

        public void moveCordinates(float a, float b)
        {
            this.X = this.X + a;
            this.Y = this.Y + b;
        }

        public float getX() { return X; }

        public float getY() { return X; }

        public float getPressure() { return this.Pressure; }

        public float getTiltX() { return this.tiltX; }

        public float getTiltY() { return this.tiltY; }

        public long getTime() { return this.Timestamp; }

    }
}
