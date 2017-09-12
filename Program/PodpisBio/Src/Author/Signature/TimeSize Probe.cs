using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class TimeSize_Probe
    {
        private double heightY; //wysokość podpisu
        private double lengthX; //długość podpisu

        public TimeSize_Probe(Signature givenSignature)
        {
            //get aktualne rozmiary podpisu
            givenSignature.calcLength();
            lengthX = givenSignature.getLentgh();
            givenSignature.calcHeight();
            heightY = givenSignature.getHeight();

            foreach (Stroke stroke in givenSignature.getStrokes())
            {
                foreach (var point in stroke.getPoints())
                { 
                    
                }
            }

        ~SizeProbe()  // destructor
        {
            exportData();

        }

        public void estimateSize(int currentPointX, int currentPointY)
        {
            if (currentPointX > farthestXs[1])
            {
                farthestXs[1] = currentPointX;
            }
            else if (currentPointX < farthestXs[0])
            {
                farthestXs[0] = currentPointX;
            }


            if (currentPointY > farthestYs[1])
            {
                farthestYs[1] = currentPointY;
            }
            else if (currentPointY < farthestYs[0])
            {
                farthestYs[0] = currentPointY;
            }
            calculateSize();



        }

        private void calculateSize()
        {
            sizeX = farthestXs[1] - farthestXs[0];
            sizeY = farthestYs[1] - farthestYs[0];

        }

        public int getArea()
        {
            return sizeX * sizeY;
        }

        public int getLengthX()
        {
            return lengthX;

        }

        public int getLengthY()
        {
            return lengthY;
        }

        public void exportData()
        { //TODO jak dostanę klasę podpis
          //Zwraca dane o podpisie do klasy podpis

        }



    }
}
}
