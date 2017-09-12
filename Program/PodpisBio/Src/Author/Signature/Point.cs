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

            Debug.WriteLine("Point: "+ x+", "+y+", "+ pressure);
        }


        public float getX() { return x; }
        public float getY() { return y; }
        public float getPressure() { return this.pressure; }
        public float getTiltX() { return this.tiltX; }
        public float getTiltY() { return this.tiltY; }
        public ulong getTime() { return this.time; }

    }
}
