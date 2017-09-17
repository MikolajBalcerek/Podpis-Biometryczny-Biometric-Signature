using Newtonsoft.Json;
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
        private int id;
        private String name;
        private List<Signature> signatures = new List<Signature>();

        public Author(int id, String name)
        {
            this.id = id;
            this.name = name;
        }
        public Author(int id, String name, List<Signature> list)
        {
            this.id = id;
            this.name = name;
        }
        public void addSignature(Signature sign)
        {
            Debug.WriteLine(getName() + " ma " + this.signatures.Count + " podpisów.");
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
        public async void DBSaveSignature()//Dodawanie podpisu do bazy danych, TODO: w ktorym miejscu to zaimplementowac
        {
            var author = new Author(this.id , this.name);
            var authorJson = JsonConvert.SerializeObject(author);
            var client = new HttpClient();

            var HttpContent = new StringContent(authorJson);
            HttpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            await client.PostAsync("http://localhost:61817/Api/Authors", HttpContent);
        }
    }
}
