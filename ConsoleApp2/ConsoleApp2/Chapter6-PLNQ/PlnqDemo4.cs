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
    public static class PlnqDemo4
    {
        public static void Run()
        {
            int[] inputIntegers = { 0, 3, 4, 8, 15, 22, 34, 57, 68, 32, 21, 30 };
            var mean = inputIntegers.AsParallel().Average();

            //标准偏差
            var standardDeviation=inputIntegers.AsParallel().Aggregate(
                //Seed
                0d,
                // Update accumulator function
                (subTotal,thisNumber)=>subTotal+Math.Pow((thisNumber-mean),2),
                //Combine accumlators function
                (total,thisTask)=>total+thisTask,
                //Result selector
                ((finalSum)=>Math.Sqrt((finalSum/inputIntegers.Count()-1))));

            //偏度
            var skewness = inputIntegers.AsParallel().Aggregate(
                //Seed
                0d,
                // Update accumulator function
                (subTotal, thisNumber) => subTotal + Math.Pow(((thisNumber - mean)/standardDeviation), 3),
                //Combine accumlators function
                (total, thisTask) => total + thisTask,
                //Result selector
                (finalSum) => (finalSum * inputIntegers.Count()/((inputIntegers.Count()-1) * (inputIntegers.Count() - 2))));

            //峰度
            var kurtosis = inputIntegers.AsParallel().Aggregate(
                //Seed
                0d,
                // Update accumulator function
                (subTotal, thisNumber) => subTotal + Math.Pow((thisNumber - mean) / standardDeviation, 4),
                //Combine accumlators function
                (total, thisTask) => total + thisTask,
                //Result selector
                (finalSum) => ((finalSum * inputIntegers.Count()*(inputIntegers.Count() + 1)) /
                ((inputIntegers.Count() - 1) * (inputIntegers.Count() - 2) * (inputIntegers.Count() - 3)) - 
                (3*Math.Pow(inputIntegers.Count()-1,2))/((inputIntegers.Count() - 2)* (inputIntegers.Count() - 3))));
            Console.ReadLine();
        }
    }
}
