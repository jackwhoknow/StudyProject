using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static void ShowUri()
        {
            var msPage = new Uri("https://www.runoob.com/html/html-intro.html");
            ShowUri(msPage);
        }

        private static void ShowUri(Uri uri)
        {
            Console.WriteLine($"Query: {uri.Query}");
            Console.WriteLine($"AbsolutePath: {uri.AbsolutePath}");
            Console.WriteLine($"Scheme: {uri.Scheme}");
            Console.WriteLine($"Port: {uri.Port}");
            Console.WriteLine($"Host: {uri.Host}");
            Console.WriteLine($"IsDefaultPort: {uri.IsDefaultPort}");
        }

        public static void ShowUriBuilder()
        {
            var msPage = new UriBuilder("https", "www.runoob.com",443, "html/html-intro.html");
            ShowUri(msPage.Uri);


            var msPage1 = new UriBuilder()
            { 
                Scheme="https",
                Host= "www.runoob.com",
                Port=443,
                Path = "html/html-intro.html",
            };
            ShowUri(msPage1.Uri);
        }

        public static void DnsLookUp()
        {
            DnsLookUp("www.google.com");
            Console.WriteLine();
            DnsLookUp("www.baidu.com");
        }

        private static void DnsLookUp(string hostName)
        {
            try
            {
                IPHostEntry iphost = Dns.GetHostEntry(hostName);
                foreach(IPAddress ip in iphost.AddressList)
                {
                    Console.WriteLine($"AddressFamility:{ip.AddressFamily.ToString()}，IP: {ip.ToString()}");
                }
                Console.WriteLine($"HostName:{iphost.HostName}");
            }
            catch(System.Exception ex)
            {
                Console.WriteLine($"Unable t process the request because the following problem occurred: {ex.Message}");
            }
        }
    }
}
