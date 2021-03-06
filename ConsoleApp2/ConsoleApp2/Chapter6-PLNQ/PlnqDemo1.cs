﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class PlnqDemo1
    {
        private static ConcurrentQueue<String> Keys;
        public static void Run()
        {
            //Keys = new ConcurrentQueue<string>();

            //Console.ReadLine();
            //Console.WriteLine("Started.");
            //var cts = new System.Threading.CancellationTokenSource();
            //var ct = cts.Token;
            //var sw = Stopwatch.StartNew();

            string[] words =
                { "Day","Car","Land","Road","Mountain","River","Sea","Shore","Mouse"};
            var query = from word in words.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                        where (word.Contains("a"))
                        orderby word descending
                        select word;
            foreach(string result in query)
            {
                Console.WriteLine(result);
            }
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
