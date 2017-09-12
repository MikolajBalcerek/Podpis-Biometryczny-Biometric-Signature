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
        private float x, y, pressure;

        public Point(float x, float y, float pressure)
        {
            this.x = x;
            this.y = y;
            this.pressure = pressure;

            Debug.WriteLine("Point: "+ x+", "+y+", "+ pressure);
        }

        public void moveCordinates(float a, float b)
        {
            this.x = this.x - a;
            this.y = this.y - b;
        }

        public float getX() { return x; }
        public float getY() { return y; }
        public float getPressure() { return pressure; }
    }
}
