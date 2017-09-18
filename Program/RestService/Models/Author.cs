using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class Author
    {
        public Author()
        {
            Signatures = new HashSet<Signature>();
        }
        public int Id { get; set; }
        public String Name { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
    }
}