using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class SignatureController
    {
        public List<Signature> signatures = new List<Signature>();

        public SignatureController() { }

        public void addSignature(Signature signature) { signatures.Add(signature); }
    }
}
