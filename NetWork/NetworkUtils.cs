using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetWork
{
    internal class NetworkUtils
    {
        public static async void GetData()
        {
            HttpClient client = new HttpClient(new MessageHandler("error"));
            HttpResponseMessage response = null;
            response = await client.GetAsync("https://www.baidu.com/?tn=68018901_16_pg");
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response Status Code: {response.StatusCode} {response.ReasonPhrase}");
                string responseBodyContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"Received payload of {responseBodyContent.Length} characters");
            }
        }

        public static void ShowPage()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            Stream stream = client.OpenRead("https://www.baidu.com/?tn=68018901_16_pg");
            StreamReader sr = new StreamReader(stream);
            string line;
            while((line=sr.ReadLine())!=null)
            {
                Console.WriteLine(line);
            }
            stream.Close();
        }
    }
}
