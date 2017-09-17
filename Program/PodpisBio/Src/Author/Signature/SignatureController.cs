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
        public Signature addSignature(IReadOnlyList<InkStroke> strokes, bool isOriginal)
        {
            Signature signature = new Signature(isOriginal);
            foreach (var strokeTemp in strokes)
            {
                Stroke stroke = new Stroke();
                signature.increaseStrokesCount();
                foreach (var pointTemp in strokeTemp.GetInkPoints())
                {
                    Src.Point point = new Src.Point((float)pointTemp.Position.X, (float)pointTemp.Position.Y, pointTemp.Pressure, pointTemp.Timestamp, pointTemp.TiltX, pointTemp.TiltY);
                    stroke.addPoint(point);
                }
                signature.addStroke(stroke);
            }
            signatures.Add(signature);
            signature.init();

            return signature;
        }

    }
}
