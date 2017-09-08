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
        private int strokesCount;
        public Signature() {
            strokesCount = 0;
        }
        public Signature(List<Stroke> strokes)
        {
            strokesCount = 0;
            this.strokes = strokes;
        }

        public void addStroke(Stroke stroke) { strokes.Add(stroke); }
        public void addStrokes(List<Stroke> strokes) { this.strokes = strokes; }

        public void increaseStrokesCount() { this.strokesCount = this.strokesCount + 1; }
        public int getStrokesCount() { return this.strokesCount; }

    }
}
