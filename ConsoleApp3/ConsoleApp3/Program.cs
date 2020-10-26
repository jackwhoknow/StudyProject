using System;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What's your name?");
            var name = Console.ReadLine();
            var date = DateTime.Now;
            Console.WriteLine("\nHello,{0},on{1} at {2}!",name,date.Day,date.TimeOfDay);
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
