using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace PodpisBio.Src
{
    class RealScreenSizeCalculator
    {
        //Pixels Per Centimeter
        private double PPC;
        public RealScreenSizeCalculator(){ init(); }

        private void init()
        {
            double widthPx = DisplayInformation.GetForCurrentView().ScreenWidthInRawPixels;
            double heightPx = DisplayInformation.GetForCurrentView().ScreenHeightInRawPixels;
            double diagonalInch = DisplayInformation.GetForCurrentView().DiagonalSizeInInches.Value;
            double scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            double diagonalPx = Math.Sqrt(Math.Pow(widthPx, 2) + Math.Pow(heightPx, 2));

            this.PPC = diagonalPx / diagonalInch / 2.54 / scale;
        }

        public double getPPC() { return PPC; }
        public double convertToPixels(double milimeters)
        {
            return  (PPC * milimeters / 10.00);
        }
    }
}
