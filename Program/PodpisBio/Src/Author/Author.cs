using Newtonsoft.Json;
using PodpisBio.Src.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class Author
    {
        //PRAMETRY AKTUALIZOWANE Z BAZĄ DANYCH//
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Signature> Signatures { get; set; }
        //KONIEC//

        public Author()
        {
            Signatures = new List<Signature>();
        }
        public Author(String name) : this() { this.Name = name; }
        public Author(int id, String name) : this()
        {
            this.Id = id;
            this.Name = name;
        }

        public void addSignature(Signature signature)
        {
            this.Signatures.Add(signature);
        }

        public int getId() { return Id; }

        public String getName() { return Name; }

        public List<Signature> getAllSignatures()
        {
            return Signatures;
        }

        public List<Signature> getOriginalSignatures()
        {
            List<Signature> original = new List<Signature>();
            foreach(var sign in Signatures)
            {
                if (sign.isOriginal)
                {
                    original.Add(sign);
                }
            }
            return original;
        }

        public List<Signature> getFakeSignatures()
        {
            List<Signature> fake = new List<Signature>();
            foreach (var sign in Signatures)
            {
                if (!sign.isOriginal)
                {
                    fake.Add(sign);
                }
            }
            return fake;
        }

        public Signature getSignature(int index)
        {
            if (this.Signatures.Count <= index)
            {
                Debug.WriteLine("indeks większy od liczby podpisów.");
                //getSignature();
            }
            return this.Signatures[index];
        }

        public int getSignaturesNumber()
        {
            return this.Signatures.Count();
        }

        public bool EmptySignatures()
        {
            return Signatures.Count == 0;
        }
    }
}
