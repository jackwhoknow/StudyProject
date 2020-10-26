using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LearnCsharp_01
{
    class Program
    {
        //static Task Main() => DoSomethingAsync();
        //static async Task DoSomethingAsync()
        //{
        //    Task<int> delayTask = DelayAsync();
        //    int result = await delayTask;
        //    Console.WriteLine($"Result:{result}");
        //    Console.ReadLine();
        //}
        //static async Task<int> DelayAsync()
        //{
        //    await Task.Delay(5000);
        //    return 5;
        //}  
        
        static void Main()
        {
            var numbers = GetSingleDigitNumbers();
        }
        static IEnumerable<int> GetSingleDigitNumbers()
        {
            int index = 0;
            while (index < 10)
                yield return index++;

            yield return 50;

            var items = new int[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
            foreach(var item in items)
            {
                yield return item;
            }
        }
    }
}
