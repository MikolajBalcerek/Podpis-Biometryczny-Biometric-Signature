using PodpisBio.Src.Service;
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
        private List<Signature> signatures = new List<Signature>();
        private SignatureService signatureService = new SignatureService();

        public SignatureController() { }

        //Dodaje podpis (do bazy i lokalnie)
        public Signature addSignature(List<InkStroke> listStroke, Author author, bool isOriginal)
        {
            //Budujemy listę stroków na bazie informacji z inkCanvas
            var strokeList = buildStrokes(listStroke);
            //Budujemy KOMPLETNY obiekt klasy Signature (taki obiekt będzie dodawany i pobierany z bazy i będzie stanowił podstawę dla obliczeń)
            Signature signature = new Signature(strokeList, author.getId(), isOriginal);

            //Wysyła do bazy obiekt przez SignatureService. Jeśli baza zwróci ten sam obiekt -> jest zapisywany, jeśli zwróci obiekt==null wówczas nie został zapisany do bazy
            var responseSignature = signatureService.postSignature(signature);
            if (responseSignature != null)
            {
                signature.init();
                author.addSignature(signature);
                this.signatures.Add(signature);
            }

            return responseSignature;
        }

        public void addSignature(Signature signature)
        {
            this.signatures.Add(signature);
        }

        public List<Signature> getSignatures()
        {
            return this.signatures;
        }

        //Zwraca obiekt klasy Signature JEDYNIE z wypełnionymi strokami i punktami
        private List<Stroke> buildStrokes(List<InkStroke> strokes)
        {
            List<Stroke> strokeList = new List<Stroke>();
            foreach (var strokeTemp in strokes)
            {
                List<Point> pointList = new List<Point>();
                var height = strokeTemp.BoundingRect.Height;
                var width = strokeTemp.BoundingRect.Width;
                var duration = strokeTemp.StrokeDuration.Value.TotalMilliseconds;
                Stroke stroke = new Stroke(height, width, duration);
                foreach (var pointTemp in strokeTemp.GetInkPoints())
                {
                    Src.Point point = new Src.Point((float)pointTemp.Position.X, (float)pointTemp.Position.Y, pointTemp.Pressure, pointTemp.Timestamp, pointTemp.TiltX, pointTemp.TiltY);
                    pointList.Add(point);
                }
                stroke.init();
                strokeList.Add(stroke);
            }

            return strokeList;
        }

    }
}
