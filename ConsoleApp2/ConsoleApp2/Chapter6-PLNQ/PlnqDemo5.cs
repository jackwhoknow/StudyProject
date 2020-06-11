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
    public static class PlnqDemo5
    {
        private static ParallelQuery<int> inputIntegers = ParallelEnumerable.Range(1, 100000000);
        private static double CalculateMean(System.Threading.CancellationToken ct)
        {
            return inputIntegers.AsParallel().WithCancellation(ct).Average();
        }
        /// <summary>
        /// 标准偏差
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="mean"></param>
        /// <returns></returns>
        private static double CalculateStandardDeviation(System.Threading.CancellationToken ct,double mean)
        {
            return inputIntegers.AsParallel().WithCancellation(ct).Aggregate
                (
                //Seed
                0d,
                // Update accumulator function
                (subTotal, thisNumber) => subTotal + Math.Pow((thisNumber - mean), 2),
                //Combine accumlators function
                (total, thisTask) => total + thisTask,
                //Result selector
                ((finalSum) => Math.Sqrt((finalSum / inputIntegers.Count() - 1)))
                );
        }
        /// <summary>
        /// 偏度
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <returns></returns>
        private static double CalculateSkewness(
            System.Threading.CancellationToken ct, double mean,double standardDeviation)
        {
            return inputIntegers.AsParallel().WithCancellation(ct).Aggregate
                (
                //Seed
                0d,
                // Update accumulator function
                (subTotal, thisNumber) => subTotal + Math.Pow(((thisNumber - mean) / standardDeviation), 3),
                //Combine accumlators function
                (total, thisTask) => total + thisTask,
                //Result selector
                (finalSum) => (finalSum * inputIntegers.Count() / ((inputIntegers.Count() - 1) * (inputIntegers.Count() - 2)))
                );
        }
        /// <summary>
        /// 峰度
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <returns></returns>
        private static double CalculateKurtosis(
            System.Threading.CancellationToken ct, double mean, double standardDeviation)
        {
            return inputIntegers.AsParallel().WithCancellation(ct).Aggregate
                (
                //Seed
                0d,
                // Update accumulator function
                (subTotal, thisNumber) => subTotal + Math.Pow((thisNumber - mean) / standardDeviation, 4),
                //Combine accumlators function
                (total, thisTask) => total + thisTask,
                //Result selector
                (finalSum) => ((finalSum * inputIntegers.Count() * (inputIntegers.Count() + 1)) /
                ((inputIntegers.Count() - 1) * (inputIntegers.Count() - 2) * (inputIntegers.Count() - 3)) -
                (3 * Math.Pow(inputIntegers.Count() - 1, 2)) / ((inputIntegers.Count() - 2) * (inputIntegers.Count() - 3)))
                );
        }
        public static void Run()
        {
            Console.ReadLine();
            Console.WriteLine("Started.");
            var cts = new System.Threading.CancellationTokenSource();
            var ct = cts.Token;
            var sw = Stopwatch.StartNew();

            var taskMean = new Task<double>(() => CalculateMean(ct),ct);

            var taskSTDev = taskMean.ContinueWith<double>((t) =>
              {
                  return CalculateStandardDeviation(ct, t.Result);
              }, TaskContinuationOptions.OnlyOnRanToCompletion);

            var taskSkewness = taskSTDev.ContinueWith<double>((t) =>
             {
                 return CalculateSkewness(ct, taskMean.Result, t.Result);
             }, TaskContinuationOptions.OnlyOnRanToCompletion);

            var taskKurtosis= taskSTDev.ContinueWith<double>((t) =>
            {
                return CalculateKurtosis(ct, taskMean.Result, t.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            var deffedCancelTask = Task.Factory.StartNew(() =>
              {
                  System.Threading.Thread.Sleep(5000);
                  cts.Cancel();
              }
            );

            try
            {
                taskMean.Start();
                Task.WaitAll(taskSkewness, taskKurtosis);
                Console.WriteLine("Mean: {0}",taskMean.Result);
                Console.WriteLine("Standard deviation: {0}", taskSTDev.Result);
                Console.WriteLine("Skewness: {0}", taskSkewness.Result);
                Console.WriteLine("Kurtosis: {0}", taskKurtosis.Result);
                Console.ReadLine();
            }
            catch (AggregateException ex)
            {
                foreach(Exception innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine(innerEx.ToString());
                    if(ex.InnerException is OperationCanceledException)
                    {
                        Console.WriteLine("Mean task: {0}",taskMean.Status);
                        Console.WriteLine("Standard deviation task: {0}", taskSTDev.Status);
                        Console.WriteLine("Skewness task: {0}", taskSkewness.Status);
                        Console.WriteLine("Kurtosis task: {0}", taskKurtosis.Status);
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
