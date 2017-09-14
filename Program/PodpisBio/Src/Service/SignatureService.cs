using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Service
{
    class SignatureService
    {
        public SignatureService()
        {
            FetchAsync("http://localhost:61817/Api/Authors");
        }

        public async void FetchAsync(string url)
        {
            string jsonString;

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(url);
                

                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
                Debug.WriteLine(jsonString);
            }
        }
    }
}
