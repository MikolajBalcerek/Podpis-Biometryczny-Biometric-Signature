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
        //private float velocity;
        //private long time; // czas, zarejestrowania punktu w ms
        //private int pressurechange; // -1/0/+1 w zależności od poprzedniego nacisku punktu

        public Point(float x, float y, float pressure)//, float velocity, long time, int pressChange)
        {
            this.x = x;
            this.y = y;
            this.pressure = pressure;
            //this.velocity = velocity;
            //this.time = time;
            //this.pressureChange = pressChange;
        }


        public float getX() { return x; }
        public float getY() { return y; }
        public float getPressure() { return pressure; }
        //public float getVelocity() { return velocity; }
        //public long getTime() { return time; }
        //public int getPressureChange() { return pressureChange; }
        //public void setVelocity(float v) { this.velocity = v; }
        //public void setPressureChange(int p) { this.pressureChange = p; }
    }
}
