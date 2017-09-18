using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PodpisBio.Src.Author;

namespace PodpisBio.Src.Service
{
    class SignatureService : RestService
    {
        public SignatureService() { }

        public Signature getSignature(int id)
        {
            return getObjectAsync<Signature>("Signatures/" + id);
        }

        public List<Signature> getSignatures()
        {
            return getObjectAsync<List<Signature>>("Signatures/");
        }

        public Signature postSignature(Signature signature)
        {
            return postObjectAsync<Signature>("Signatures/", signature);
        }
    }
}
