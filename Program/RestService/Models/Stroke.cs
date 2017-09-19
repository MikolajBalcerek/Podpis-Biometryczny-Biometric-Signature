using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class Stroke
    {    
        public Stroke()
        {
            Points = new HashSet<Point>();
        }
        public int Id { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double DurationInMilis { get; set; }
        public virtual ICollection<Point> Points { get; set; }
        public int SignatureId { get; set; }
    }
}