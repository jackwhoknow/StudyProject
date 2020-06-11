using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class PlnqDemo2
    {
        public static void Run()
        {
            string[] words =
                { "Day","Car","Land","Road","Mountain","River","Sea","Shore","Mouse"};
            var query = (from word in words.AsParallel()
                        where (word.Contains("a"))
                        select CountLetters(word)).Sum();
            Console.WriteLine("The total number of letters for the words that contains an 'a' is {0}",query);
            Console.ReadLine();
        }
        static int CountLetters(String key)
        {
            int letters = 0;
            for(int i=0;i<key.Length;i++)
            {
                if(Char.IsLetter(key,i))
                {
                    letters++;
                }
            }
            return letters;
        }
    }
}
