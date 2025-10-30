using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
             NetworkUtils.GetData();
            //NetworkUtils.ShowPage();
            //NetworkUtils.ShowUri();
            //NetworkUtils.ShowUriBuilder();
            //NetworkUtils.DnsLookUp();
            Thread.Sleep(1000);
            Console.WriteLine("ABCDEFG");
            Console.ReadLine();
        }      
    }
}
