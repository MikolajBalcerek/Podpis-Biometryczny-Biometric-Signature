using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src
{
    class Signature
    {
        private List<Stroke> strokes = new List<Stroke>();

        public Signature() { }
        public Signature(List<Stroke> strokes)
        {
            this.strokes = strokes;
        }

        public void addStroke(Stroke stroke) { strokes.Add(stroke); }
        public void addStrokes(List<Stroke> strokes) { this.strokes = strokes; }
    }
}
