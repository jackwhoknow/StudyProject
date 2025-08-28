using System;
using System.Diagnostics;

namespace ThreadStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            ParallelService.RunCountinousTask();            
            sw.Stop();
            Console.WriteLine($"ellapsed time:{sw.ElapsedMilliseconds/1000.0}s");
            Console.ReadLine();
        }
    }
}
