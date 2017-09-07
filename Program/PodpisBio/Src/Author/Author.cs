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
        private List<Signature> signatures;

        public Author() {}

        public void addSignature(Signature sign)
        {
            this.signatures.Add(sign);
        }
    }
}
