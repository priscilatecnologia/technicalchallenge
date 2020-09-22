using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Challenge
{
    class SimpleJsonRequest
    {
        public static async Task<string> GetAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task<string> PostAsync(string url, Dictionary<string, string> data)
        {
            return await RequestAsync("POST", url, data);
        }

        public static async Task<string> PutAsync(string url, Dictionary<string, string> data)
        {
            return await RequestAsync("PUT", url, data);
        }

        public static async Task<string> PatchAsync(string url, Dictionary<string, string> data)
        {
            return await RequestAsync("PATCH", url, data);
        }

        public static async Task<string> DeleteAsync(string url, Dictionary<string, string> data)
        {
            return await RequestAsync("DELETE", url, data);
        }

        private static async Task<string> RequestAsync(string method, string url, Dictionary<string, string> data)
        {
            string dataString = JsonConvert.SerializeObject(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json";
            request.Method = method;

            using(Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using(HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}