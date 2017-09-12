using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    // TODO: MK dodaj obliczanie pochodnych tiltów
    class Derivatives
    {
        private float velocityX = 0;
        private float velocityY = 0;
        private float velocity = 0;
        private float accX = 0;
        private float accY = 0;
        private float acc = 0;
        private float pressureChange = 0;
        /*
         * lista zmian sił nacisku
         * -1 -- nacisk maleje
         *  0 -- nacisk się nie zmienia
         * +1 -- nacisk rośnie
         */
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Velocity { get; set; }
        public float AccX { get; set; }
        public float AccY { get; set; }
        public float Acc { get; set; }
        public float PressureChange { get; set; }

    }
    class Dynamics
    {
        private float calcVelocity(Point prevPoint, Point currentPoint)
        {
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            var deltaX = currentPoint.getX() - prevPoint.getX();
            var deltaY = currentPoint.getY() - prevPoint.getY();
            return (deltaX * deltaX + deltaY * deltaY) / deltaTime;
        }

        private float calcVelocityX(Point prevPoint, Point currentPoint)
        {
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            var deltaX = currentPoint.getX() - prevPoint.getX();
            return deltaX / deltaTime;
        }

        private float calcVelocityY(Point prevPoint, Point currentPoint)
        {
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            var deltaY = currentPoint.getY() - prevPoint.getY();
            return deltaY / deltaTime;
        }

        private int calcPressureChange(Point prevPoint, Point currentPoint)
        {
            var difference = currentPoint.getPressure() - prevPoint.getPressure();
            return (difference == 0) ? 0 : (difference > 0 ? 1 : -1);
        }

        private float calcAccX(Derivatives current, Derivatives previous, Point currentPoint, Point prevPoint)
        {
            var deltaV = current.VelocityX - previous.VelocityX;
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            return deltaV / deltaTime;
        }

        private float calcAccY(Derivatives current, Derivatives previous, Point currentPoint, Point prevPoint)
        {
            var deltaV = current.VelocityY - previous.VelocityY;
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            return deltaV / deltaTime;
        }

        private float calcAcc(Derivatives current, Derivatives previous, Point currentPoint, Point prevPoint)
        {
            var deltaV = current.Velocity - previous.Velocity;
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            return deltaV / deltaTime;
        }

        public Derivatives calcDerivatives(Point prev, Point current, Derivatives prevDerivative)
        {
            Derivatives derivative = new Derivatives();
            derivative.VelocityX = this.calcVelocityX(prev, current);
            derivative.VelocityY = this.calcVelocityY(prev, current);
            derivative.Velocity = this.calcVelocity(prev, current);
            derivative.PressureChange = this.calcPressureChange(prev, current);
            derivative.AccX = this.calcAccX(derivative, prevDerivative, current, prev);
            derivative.AccY = this.calcAccY(derivative, prevDerivative, current, prev);
            derivative.Acc = this.calcAcc(derivative, prevDerivative, current, prev);
            return derivative;
        }

    }
}
