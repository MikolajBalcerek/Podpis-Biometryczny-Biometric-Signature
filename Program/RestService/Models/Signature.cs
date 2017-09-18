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
        }
        public int Id { get; set; }
        public Boolean isOriginal { get; set; }
        public virtual ICollection<Stroke> Strokes { get; set; }
        public int AuthorId { get; set; }
    }
}