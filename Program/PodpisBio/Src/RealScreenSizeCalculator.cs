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
        double CPI;
        public RealScreenSizeCalculator(){ init(); }

        private void init()
        {
            double widthPx = DisplayInformation.GetForCurrentView().ScreenWidthInRawPixels;
            double heightPx = DisplayInformation.GetForCurrentView().ScreenHeightInRawPixels;
            double diagonalInch = DisplayInformation.GetForCurrentView().DiagonalSizeInInches.Value;

            double diagonalPx = Math.Sqrt(Math.Pow(widthPx, 2) + Math.Pow(heightPx, 2));

            this.CPI = diagonalPx / diagonalInch / 2.54;
        }

        public double getCPI() { return CPI; }
        public double toPixels(double milimeters)
        {
            return  (CPI * milimeters / 10.00);
        }
    }
}
