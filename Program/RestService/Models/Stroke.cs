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
        public virtual ICollection<Point> Points { get; set; }
    }
}