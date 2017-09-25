using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    // TODO: MK dodaj obliczanie pochodnych tiltów
    class Derivatives
    {
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float Velocity { get; set; }
        public float AccX { get; set; }
        public float AccY { get; set; }
        public float Acc { get; set; }
        public float DtiltX { get; set; }
        public float DtiltY { get; set; }
        public float PressureChange { get; set; }
        /*
         * lista zmian sił nacisku
         * -1 -- nacisk maleje
         *  0 -- nacisk się nie zmienia
         * +1 -- nacisk rośnie
         */

        public Derivatives()
        {
            VelocityX = 0;
            VelocityY = 0;
            Velocity = 0;
            AccX = 0;
            AccY = 0;
            Acc = 0;
            DtiltX = 0;
            DtiltY = 0;
            PressureChange = 0;
        }

        public Derivatives(float v, float vx, float vy, float acc, float accx, float accy, float dtiltX, float dtiltY, float pc)
        {
            this.Velocity = v;
            this.VelocityX = vx;
            this.VelocityY = vy;
            this.Acc = acc;
            this.AccX = accx;
            this.AccY = accy;
            this.DtiltX = dtiltX;
            this.DtiltY = dtiltY;
            this.PressureChange = pc;
        }

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

        private float calcDtiltX(Point prevPoint, Point currentPoint)
        {
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            var deltaTilt = currentPoint.getTiltX() - prevPoint.getTiltX();
            return deltaTilt / deltaTime;
        }

        private float calcDtiltY(Point prevPoint, Point currentPoint)
        {
            var deltaTime = currentPoint.getTime() - prevPoint.getTime();
            var deltaTilt = currentPoint.getTiltY() - prevPoint.getTiltY();
            return deltaTilt / deltaTime;
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
            //Debug.WriteLine("Adam liczy pochodne.");
            Derivatives derivative = new Derivatives();
            derivative.VelocityX = this.calcVelocityX(prev, current);
            derivative.VelocityY = this.calcVelocityY(prev, current);
            derivative.Velocity = this.calcVelocity(prev, current);
            derivative.PressureChange = this.calcPressureChange(prev, current);
            derivative.AccX = this.calcAccX(derivative, prevDerivative, current, prev);
            derivative.AccY = this.calcAccY(derivative, prevDerivative, current, prev);
            derivative.Acc = this.calcAcc(derivative, prevDerivative, current, prev);
            derivative.DtiltX = this.calcDtiltX(prev, current);
            derivative.DtiltY = this.calcDtiltY(prev, current);
            //Debug.WriteLine("Adam policzył pochodne");
            return derivative;
        }

    }
}
