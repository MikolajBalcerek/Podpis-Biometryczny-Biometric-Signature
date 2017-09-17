using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Signature getSignature(int index)
        {
            if (this.signatures.Count <= index)
            {
                Debug.WriteLine("indeks większy od liczby podpisów.");
                getSignature();
            }
            return this.signatures[index];
        }

        public int getSignaturesNumber()
        {
            return this.signatures.Count();
        }

        public bool EmptySignatures()
        {
            return signatures.Count == 0;
        }
    }
}
