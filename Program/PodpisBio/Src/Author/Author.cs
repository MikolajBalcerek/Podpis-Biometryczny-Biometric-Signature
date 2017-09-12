using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class Author
    {
        private int id;
        private String name;
        private List<Signature> signatures = new List<Signature>();

        public Author(int id, String name)
        {
            this.id = id;
            this.name = name;
        }

        public void addSignature(Signature sign)
        {
            this.signatures.Add(sign);
        }

        public String getName() { return name; }

        public Signature getSignature()
        {
            return signatures[0];
        }
    }
}
