using System;
using System.Collections.Generic;
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
        }


        public float getX() { return x; }
        public float getY() { return y; }
        public float getPressure() { return pressure; }
    }
}
