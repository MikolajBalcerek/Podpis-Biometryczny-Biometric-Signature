using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace PodpisBio.Src.Author
{
    class TimeSize_Probe
    {
        //ZMIENNE
        private double heightY; //wysokość podpisu
        private double lengthX; //długość podpisu
        private double totalDrawingTime; //totalDrawingTime całego podpisu tylko czas aktualnych pociągnięć
        private List<double> TimeStampsForEachStroke;
        Signature testedSignature; //dana sygnatura dla TimeSize_Probe do badania
        private double totalRatioAreaToTime; //całkowity stosunek pola do czasu
        private List<Tuple<double, double, double>> StrokesDimensionsForEachStroke; //szerokość, długość, pole dla każdego ze Stroków
        private List<double> ratioAreaToTimeForEachStroke; //lista z ratio dla każdego pociągnięcia

        //KONSTRUKTOR
        public TimeSize_Probe(Signature givenSignature)
        {
            //Ustawianie początkowych wartości klasy TimeSize_Probe
            totalDrawingTime = 0;
            TimeStampsForEachStroke = new List<double>();
            ratioAreaToTimeForEachStroke = new List<double>();
            this.StrokesDimensionsForEachStroke = new List<Tuple<double, double, double>>();

            this.testedSignature = givenSignature;
            //get aktualne rozmiary podpisu
            //testedSignature.calcLength(true);
            lengthX = testedSignature.getLengthO();
            //testedSignature.calcHeight();
            heightY = testedSignature.getHeight();
            
            //Operacje Badania czasu/rozmiar
            calculateTimeForStrokes(); //policz czas sumaryczny i osobny pisania pociągnięć
            calculateTotalAreaToTimeRatio(); //policz stosunek przestrzeń podpisu do całkowitego czasu
            calculateSizeForEachStroke();
            calculateTotalAreaToTimeRatioForEachStroke();


        }

        //FUNKCJE POMOCNICZE


        private void calculateTimeForStrokes()
        //funkcja licząca sumaryczną i osobny czas pisania dla pociągnięć
        // funkcja wewnętrzna dla konstruktora
        {
            //dla każdego pociągnięcia
            foreach (Stroke stroke in testedSignature.getStrokesOriginal())
            {
                //weź początek pisania w pociągnięciu i koniec pisania
                /*
                ulong starting_time = stroke.GetInkPoints().First().Timestamp;
                ulong ending_time = stroke.GetInkPoints().Last().Timestamp;

                //delta to czas pisania pociągnięcia 
                ulong delta = ending_time - starting_time;

               */
                //TimeStampsForEachStroke.Add(stroke.StrokeDuration.Value); //Stary strokeDuration z richStrokes();

                TimeStampsForEachStroke.Add(stroke.DurationInMilis);
                //totalDrawingTime całego podpisu tylko czas aktualnych pociągnięć
                // totalDrawingTime = totalDrawingTime + delta;
                totalDrawingTime = stroke.DurationInMilis + totalDrawingTime;
            }
            //Debug.WriteLine("Total Drawing Time dla tego podpisu to: " + totalDrawingTime);
        }

        private void calculateSizeForEachStroke()
        //funkcja licząca rozmiar każdego osobnego pociągnięcia
        // funkcja wewnętrzna dla konstruktora
        {
            //dla każdego pociągnięcia
            foreach (Stroke stroke in testedSignature.getStrokesOriginal())
            {
                double strokeHeight = stroke.Height;
                double strokeWidth = stroke.Width;
                this.StrokesDimensionsForEachStroke.Add(new Tuple<double, double, double>(strokeHeight, strokeWidth, strokeHeight * strokeWidth));
                //Debug.WriteLine("Wymiary pociągnięcia to: " + strokeHeight + " " + strokeWidth);
            }
        }

        private void calculateTotalAreaToTimeRatio()
        //funkcja wewnętrzna licząca stosunek pola podpisu/czas całkowity podpisu
        {

            //testedSignature.calcHeight();
            //testedSignature.calcLength(true);
            //Debug.WriteLine("TUTAJ" + testedSignature.getHeight() + " " + testedSignature.getLentghO());
            //Debug.WriteLine("Liczba:     " + testedSignature.getHeight() * testedSignature.getLentghO());
            totalRatioAreaToTime = (testedSignature.getHeight() * testedSignature.getLengthO()) / this.totalDrawingTime;
            //Debug.WriteLine("Total Area to Time Ratio: " + TotalRatioAreaToTime);
          
        }

        private void calculateTotalAreaToTimeRatioForEachStroke()
        //funkcja wewnętrzna licząca stosunek pola podpisu/czas dla każdego pociagnięcia
        {
            int __counter__ = 0;
            foreach (Tuple<double, double, double> stroke in StrokesDimensionsForEachStroke)
            {
                //liczy ratio pole do czasu z listy czasów i dodaje do listy ratio
                double __ratio__ = stroke.Item3 / TimeStampsForEachStroke[__counter__];
                this.ratioAreaToTimeForEachStroke.Add(__ratio__);
                //Debug.WriteLine("Stroke numer " + __counter__ + "ma ratio rozmiar/czas" + __ratio__);
                __counter__++;
             }

        }

        public double getTotalDrawingTime()
           {
                return this.totalDrawingTime;
           }

        public double getTotalRatioAreaToTime()
            //zwraca stosunek pole podpisu do całkowitego czasu
        {
            return this.TotalRatioAreaToTime;
        }

        public List<double> getRatioAreaToTimeForEachStroke()
        {
            return this.ratioAreaToTimeForEachStroke;
        }

        public List<double> getTimeStampsForEachStroke()
        {
            return this.TimeStampsForEachStroke;
        }

        public List<double> RatioAreaToTimeForEachStroke { get => ratioAreaToTimeForEachStroke; }
        public List<Tuple<double, double, double>> StrokesDimensionsForEachStroke1 { get => StrokesDimensionsForEachStroke; }
        public double TotalRatioAreaToTime { get => totalRatioAreaToTime; }
    }
}
