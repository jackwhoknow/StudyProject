using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// 归约操作
    /// </summary>
    public static class PlnqDemo3
    {
        static int NUM_INTS = 500000000;
        static ParallelQuery<int> GenerateInputData()
        {
            return ParallelEnumerable.Range(1, NUM_INTS);
        }
        public static void Run()
        {
            var inputIntegers = GenerateInputData();
            var seqReductionQuery = (from intNum in inputIntegers.AsParallel()
                                     where ((intNum % 5) == 0)
                                     select (intNum / Math.PI)).Average();
            Console.WriteLine("Average {0}",seqReductionQuery);
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
