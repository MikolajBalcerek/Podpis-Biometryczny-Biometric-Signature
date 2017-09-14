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
        private float x, y, pressure, tiltX, tiltY;
        private ulong time;

        public Point(float x, float y, float pressure, ulong time, float tiltX, float tiltY)
        {
            this.x = x;
            this.y = y;
            this.pressure = pressure;
            this.time = time;
            this.tiltX = tiltX;
            this.tiltY = tiltY;
        }

        public Point(Point point)
        {
            this.x = point.getX();
            this.y = point.getY();
            this.pressure = point.getPressure();
            this.time = point.getTime();
            this.tiltX = point.getTiltX();
            this.tiltY = point.getTiltY();
        }

        public void moveCordinates(float a, float b)
        {
            this.x = this.x + a;
            this.y = this.y + b;
        }

        public float getX() { return x; }

        public float getY() { return y; }

        public float getPressure() { return this.pressure; }

        public float getTiltX() { return this.tiltX; }

        public float getTiltY() { return this.tiltY; }

        public ulong getTime() { return this.time; }

    }
}
