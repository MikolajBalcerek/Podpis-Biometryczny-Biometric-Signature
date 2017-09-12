using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace PodpisBio.Src.Author
{
    class SignatureController
    {
        public List<Signature> signatures = new List<Signature>();

        public SignatureController() { }

        public void addSignature(Signature signature) { signatures.Add(signature); }

        //Add signature
        public Signature addSignature(IReadOnlyList<InkStroke> strokes)
        {
            Signature signature = new Signature();
            foreach (var strokeTemp in strokes)
            {
                Stroke stroke = new Stroke();
                signature.increaseStrokesCount();
                foreach (var pointTemp in strokeTemp.GetInkPoints())
                {
                    Src.Point point = new Src.Point((float)pointTemp.Position.X, (float)pointTemp.Position.Y, pointTemp.Pressure);
                    stroke.addPoint(point);
                }
                signature.addStroke(stroke);
                Debug.Write(signature.getStrokesCount());
            }
            signatures.Add(signature);
            signature.init();

            return signature;
        }

    }
}
