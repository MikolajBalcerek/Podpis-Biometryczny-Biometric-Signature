using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Service
{
    class RestService
    {
        public static String connectionUrl = "http://localhost:61817/Api/";

        public RestService() { }

        public T getObjectAsync<T>(String objectUrl)
        {
            T result = default(T);
            HttpClient client = new HttpClient();

            // This allows for debugging possible JSON issues
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            };

            using (HttpResponseMessage response = client.GetAsync(connectionUrl+objectUrl).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result, settings);
                }
            }
            return result;
        }

        public T postObjectAsync<T>(String objectUrl, Object objectToPost)
        {
            T result = default(T);
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(objectToPost);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // This allows for debugging possible JSON issues
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            };

                using (HttpResponseMessage response = client.PostAsync(connectionUrl + objectUrl, httpContent).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result, settings);
                    }
                }

            return result;
        }

        public T deserializeJson<T>(String jsonString)
        {
            T result = default(T);
            HttpClient client = new HttpClient();

            // This allows for debugging possible JSON issues
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            };

            result = JsonConvert.DeserializeObject<T>(jsonString, settings);

            return result;
        }
    }
}
