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
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Signature> Signatures { get; set; }

        private SignatureService signatureService;

        public Author()
        {
            Signatures = new List<Signature>();
            signatureService = new SignatureService();
        }
        public Author(String name) : this() { this.Name = name; }
        public Author(int id, String name) : this()
        {
            this.Id = id;
            this.Name = name;
        }
        public Author(int id, String name, List<Signature> list) : this()
        {
            this.Id = id;
            this.Name = name;

        }
        public void addSignature(Signature sign)
        {
            this.Signatures.Add(sign);

            sign.AuthorId = this.Id;
            
            sign = signatureService.postSignature(sign);
            if (sign != null) { Signatures.Add(sign); }
        }

        public int getId() { return Id; }

        public String getName() { return Name; }

        public List<Signature> getSignatures()
        {
            return Signatures;
        }
    }
}
