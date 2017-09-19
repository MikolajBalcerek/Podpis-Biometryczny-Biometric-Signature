using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.FinalScore
{
    class SignVerification
    {
        public List<double> init(Signature sign, List<Signature> signList/*, Wagi*/)
        {
            List<double> ver = new List<double>();
            foreach(Signature first in signList)
            {
                ver.Add(check(first, sign));
            }

            return ver;
        }

        private double check(Signature first, Signature second/*, Wagi*/)
        {
            double temp = 0;
            double length = checkLength(first, second)/* * Waga.*/;
            /*
             * Reszta 
             */

            temp = length /*+ Reszta*/;

            return temp;
        }

        private double checkLength(Signature first, Signature second)
        {
            double temp = 1-(Math.Abs(first.getLentgh()-second.getLentgh()) / first.getLentgh());

            return temp;
        }
    }
}
