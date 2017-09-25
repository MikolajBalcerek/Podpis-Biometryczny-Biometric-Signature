using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class Signature
    {
        public Signature()
        {
            Strokes = new HashSet<Stroke>();
            lengthM = 0;
            lengthO = 0;
            height = 0;
        }
        public int Id { get; set; }
        public Boolean isOriginal { get; set; }
        public double lengthO { get; set; }
        public double lengthM { get; set; }
        public double height { get; set; }
        public virtual ICollection<Stroke> Strokes { get; set; }
        public int AuthorId { get; set; }
    }
}