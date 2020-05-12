using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        private static int sleepTime = 50;

        private const int NUM_AES_KEYS = 800000;
        private const int NUM_MD5_HASHES = 100000;
        static void Main(string[] args)
        {
            //一、Parallel
            //1.Parallel.Invoke
            //Parallel.Invoke(AAA, BBB, CCC, DDD, EEE, FFF,GGG,HHH,III,JJJ,KKK);

            //2.Parallel.For
            //var sw = Stopwatch.StartNew();
            //2.1 串行执行
            //GenerateAESKeys();
            //GenerateMD5Hashes();
            //AES: 00:00:02.4825148
            //AES: 00:00:02.9151648
            //00:00:05.4039578

            //2.2 并行执行
            //Parallel.Invoke(() => GenerateAESKeys(),
            //      () => GenerateMD5Hashes());
            //AES: 00:00:02.5062344
            //AES: 00:00:03.0147940
            //00:00:03.0278609

            //加速比=串行执行时间/并行执行时间

            //2.3 循环并行化
            //ParallelGenerateAESKeys();
            //ParallelGenerateMD5Hashes();
            //AES: 00:00:00.6445913
            //AES: 00:00:00.9585550
            //00:00:01.6042362

            //2.3.1 循环并行分区
            //ParallelPartitionGenerateAESKeys();
            //ParallelPartitionGenerateMD5Hashes();
            //AES: 00:00:00.6294282
            //AES: 00:00:00.8030584
            //00:00:01.4338800

            //分区
            //AES: 00:00:00.6769209
            //AES: 00:00:00.7786603
            //00:00:01.4567862

            //Parallel.Invoke(
            //    () => ParallelGenerateAESKeysMaxDegree(Environment.ProcessorCount - 0),
            //    () => ParallelGenerateAESKeysMaxDegree(Environment.ProcessorCount - 0)
            //);
            //Console.WriteLine(sw.Elapsed.ToString());

            //AES: 00:00:01.2183191
            //AES: 00:00:01.3957050


            //00:00:01.4049319

            //二、Task
            //3.1.1 System.Threading.Tasks Task
            //var sw = Stopwatch.StartNew();
            //var t1 = new Task(() => GenerateAESKeys());
            //var t2 = new Task(() => GenerateMD5Hashes());

            ////var t1 = new Task(() => AAA());
            ////var t2 = new Task(() => BBB());
            ////S
            //t1.Start();
            //t2.Start();

            //3.1.4 等待任务完成
            //Waitting for all the tasks to finish
            //if(!Task.WaitAll(new Task[] { t1, t2 },5000))
            //{
            //    Console.WriteLine("Print number not finished!");
            //    Console.WriteLine(t1.Status.ToString());
            //    Console.WriteLine(t2.Status.ToString());
            //}
            //if(t2.Wait(5000))
            //{
            //    Console.WriteLine("T1 task not finished!");
            //    Console.WriteLine(t1.Status.ToString());
            //}
            //00:00:00.0090862
            //AES: 00:00:02.5941785
            //AES: 00:00:02.9803477

            //3.1.6 通过取消标记取消任务
            //Console.WriteLine("Started");
            //var cts = new System.Threading.CancellationTokenSource();
            //var ct = cts.Token;
            //var sw = Stopwatch.StartNew();
            ////var t1 = Task.Factory.StartNew(() => GenerateAESKeysCanel(ct), ct);
            ////var t2 = Task.Factory.StartNew(() => GenerateMD5HashesCancel(ct), ct);

            //var t1 = Task.Factory.StartNew(() => AAACancel(ct), ct);
            //var t2 = Task.Factory.StartNew(() => BBBCancel(ct), ct);

            //System.Threading.Thread.Sleep(10000);
            ////cts.Cancel();
            //try
            //{
            //    if(!Task.WaitAll(new Task[] { t1,t2},1000))
            //    {
            //        Console.WriteLine("time too long");
            //        Console.WriteLine(t1.Status);
            //        Console.WriteLine(t2.Status);
            //    }
            //}
            //catch(AggregateException ex)
            //{
            //    if (t1.IsFaulted)
            //    {
            //        foreach (Exception innerex in t1.Exception.InnerExceptions)
            //        {
            //            Console.WriteLine(innerex.ToString());
            //        }
            //    }
            //    if (t2.IsFaulted)
            //    {
            //        foreach (Exception innerex in t2.Exception.InnerExceptions)
            //        {
            //            Console.WriteLine(innerex.ToString());
            //        }
            //    }
            //    if(t1.IsCanceled)
            //    {
            //        Console.WriteLine("T1 was cancelled");
            //    }
            //    if (t2.IsCanceled)
            //    {
            //        Console.WriteLine("T2 was cancelled");
            //    }
            //    Console.WriteLine(sw.Elapsed.ToString());
            //    Console.WriteLine("Finished");
            //    Console.ReadLine();
            //}

            //3.1.7 从任务值返回值 (失败)
            //var cts = new System.Threading.CancellationTokenSource();
            //var ct = cts.Token;
            //var sw = Stopwatch.StartNew();
            //var t1 = Task.Factory.StartNew(() => AAAWithReturnValue(ct), ct);
            //try
            //{
            //    t1.Wait(3000, ct);
            //}
            //catch(AggregateException ex)
            //{
            //    foreach(Exception innerEx in ex.InnerExceptions)
            //    {
            //        Console.WriteLine(innerEx.ToString());
            //    } 
            //    if(t1.IsCanceled)
            //    {
            //        Console.WriteLine("Task 1 was cancelled");
            //    }
            //    if(t1.IsFaulted)
            //    {
            //        foreach (Exception innerex in t1.Exception.InnerExceptions)
            //        {
            //            Console.WriteLine(innerex.ToString());
            //        }
            //    }
            //    var t2 = Task.Factory.StartNew(() =>
            //      {
            //          Console.WriteLine("-------------------------------------");
            //          for (int i=0;i<t1.Result.Count;i++)
            //          {
            //              Console.WriteLine(t1.Result[i]);
            //          }
            //      },TaskCreationOptions.LongRunning);
            //    Console.WriteLine(sw.Elapsed.ToString());
            //    Console.WriteLine("Finished");
            //}

            //3.1.9 通过延续才串联多个任务
            //var cts = new System.Threading.CancellationTokenSource();
            //var ct = cts.Token;
            //var sw = Stopwatch.StartNew();
            //var t1 = Task.Factory.StartNew(() => AAAWithReturnValue(ct), ct);

            //var t2 = t1.ContinueWith((t) =>
            //  {
            //      Console.WriteLine("-------------------------------------");
            //      for (int i=0;i<t.Result.Count;i++)
            //      {
            //          Console.WriteLine(t.Result[i]);
            //      }
            //  });
            //try
            //{
            //    t2.Wait();
            //    Console.WriteLine(sw.Elapsed.ToString());
            //    Console.WriteLine("Finished");
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (Exception innerEx in ex.InnerExceptions)
            //    {
            //        Console.WriteLine(innerEx.ToString());
            //    }
            //    Console.WriteLine(sw.Elapsed.ToString());
            //}

            #region---------------第四章  并发集合---------------

            //4.1.1 理解并发集合提供的功能
            // lock
            //4.1.2 ConcurrentQueue
            //var sw = Stopwatch.StartNew();
            //keyList = new ConcurrentQueue<string>();
            //var tAsync = Task.Factory.StartNew(() => ParallelPartitionGenerateAESKeys());
            //string lastKey;
            //while((tAsync.Status==TaskStatus.Running || 
            //    tAsync.Status == TaskStatus.WaitingToRun))
            //{
            //    var countResult = keyList.Count(key => key.Contains("F"));
            //    Console.WriteLine("So far,the number of keys that cotains an F is: {0}", countResult);
            //    if(keyList.TryPeek(out lastKey))
            //    {
            //        Console.WriteLine("The first key in the queue is: {0}", lastKey);
            //    }
            //    else
            //    {
            //        Console.WriteLine("No keys yet");
            //    }
            //    System.Threading.Thread.Sleep(500);
            //}
            //tAsync.Wait();
            //Console.WriteLine("Number of keys in the List: {0}", keyList.Count);
            //Console.WriteLine(sw.Elapsed.ToString());

            //4.1.3 理解并行的生产者-消费者模式
            var sw = Stopwatch.StartNew();
            _keysQueue = new ConcurrentQueue<string>();
            _byteArrayQueue = new ConcurrentQueue<byte[]>();
            var taskAESKeys = Task.Factory.StartNew(() => ParallelPartitionGenerateAESKeys(Environment.ProcessorCount-1));
            var taskHexStrings = Task.Factory.StartNew(() => ConvertAESKeyToHex(taskAESKeys));
            string lastKey;
            while ((taskHexStrings.Status == TaskStatus.Running ||
                taskHexStrings.Status == TaskStatus.WaitingToRun))
            {
                var countResult = _keysQueue.Count(key => key.Contains("F"));
                Console.WriteLine("So far,the number of keys that cotains an F is: {0}", countResult);
                if (_keysQueue.TryPeek(out lastKey))
                {
                    Console.WriteLine("The first key in the queue is: {0}", lastKey);
                }
                else
                {
                    Console.WriteLine("No keys yet");
                }
                System.Threading.Thread.Sleep(500);
            }
            Task.WaitAll(taskAESKeys, taskHexStrings);
            Console.WriteLine("Number of keys in the List: {0}", _keysQueue.Count);
            Console.WriteLine(sw.Elapsed.ToString());
            #endregion
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        #region---------- Parallel.Invoke(AAA, BBB, CCC......)----------
        private static void AAA()
        {
            for(int i=1;i<=100;i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void BBB()
        {
            for (int i = 101; i <= 20000; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void AAACancel(System.Threading.CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            for (int i = 1; i <= 1000; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
                ct.ThrowIfCancellationRequested();
            }
        }
        private static List<string> AAAWithReturnValue(System.Threading.CancellationToken ct)
        {
            List<string> res = new List<string>();
            var sw = Stopwatch.StartNew();
            for (int i = 1; i <= 100; i++)
            {
                Console.WriteLine(i);
                res.Add(i.ToString());
                Thread.Sleep(sleepTime);
                if(ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }
            }
            return res;
        }
        private static void BBBCancel(System.Threading.CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var sw = Stopwatch.StartNew();
            for (int i = 101; i <= 20000; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
                if(sw.Elapsed.TotalSeconds>10)
                {
                    throw new TimeoutException("BBBCancel is taken more than 10 seconds to complete");
                }
                ct.ThrowIfCancellationRequested();
            }
        }
        private static void CCC()
        {
            for (int i = 201; i <= 300; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void DDD()
        {
            for (int i = 301; i <= 400; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void EEE()
        {
            for (int i = 401; i <= 500; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void FFF()
        {
            for (int i = 501; i <= 600; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void GGG()
        {
            for (int i = 601; i <= 700; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void HHH()
        {
            for (int i = 701; i <= 800; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void III()
        {
            for (int i = 801; i <= 900; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void JJJ()
        {
            for (int i = 901; i <= 1000; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        private static void KKK()
        {
            for (int i = 1001; i <= 1100; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(sleepTime);
            }
        }
        #endregion
        #region----------Parallel.For,Parallel.Foreach ----------
        private static string ConvertToTextString(Byte[] byteArray)
        {
            var sb = new StringBuilder(byteArray.Length);
            for(int i=0;i< byteArray.Length;i++)
            {
                sb.Append(byteArray[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private static void GenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            var aesM = new AesManaged();
            for(int i=0;i<= NUM_AES_KEYS;i++)
            {
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexString=ConvertToTextString(result);
                //Console.WriteLine("AES KEY: {0}", hexString);
            }
            Console.WriteLine("AES: "+sw.Elapsed.ToString());
        }
        private static void GenerateAESKeysCanel(System.Threading.CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var sw = Stopwatch.StartNew();
            var aesM = new AesManaged();
            for (int i = 0; i <= NUM_AES_KEYS; i++)
            {
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexString = ConvertToTextString(result);
                //Console.WriteLine("AES KEY: {0}", hexString);
                ct.ThrowIfCancellationRequested();
            }
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ParallelGenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            Parallel.For(1, NUM_AES_KEYS, (int i) =>
             {
                 var aesM = new AesManaged();
                 aesM.GenerateKey();
                 byte[] result = aesM.Key;
                 string hexString = ConvertToTextString(result);
             });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static ConcurrentQueue<string> keyList;
        private static void ParallelPartitionGenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            Parallel.ForEach(Partitioner.Create(1, NUM_AES_KEYS ,((int)(NUM_AES_KEYS/Environment.ProcessorCount)+1)), range =>
               {
                   var aesM = new AesManaged();
                   for(int i=range.Item1;i<range.Item2;i++)
                   {
                       aesM.GenerateKey();
                       byte[] result = aesM.Key;
                       string hexString = ConvertToTextString(result);
                       keyList.Enqueue(hexString);
                   }
               });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }

        private static ConcurrentQueue<Byte[]> _byteArrayQueue;
        private static ConcurrentQueue<string> _keysQueue;
        private static void ParallelPartitionGenerateAESKeys(int maxDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegree;
            var sw = Stopwatch.StartNew();
            Parallel.ForEach(Partitioner.Create(1, NUM_AES_KEYS+1), parallelOptions, range =>
            {
                var aesM = new AesManaged();
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    aesM.GenerateKey();
                    byte[] result = aesM.Key;
                    _byteArrayQueue.Enqueue(result);
                }
            });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ConvertAESKeyToHex(Task taskProducer)
        {
            var sw = Stopwatch.StartNew();
            while(taskProducer.Status==TaskStatus.Running || taskProducer.Status==TaskStatus.WaitingToRun ||
                (_byteArrayQueue.Count>0))
            {
                Byte[] result;
                if(_byteArrayQueue.TryDequeue(out result))
                {
                    string hexString = ConvertToTextString(result);
                    _keysQueue.Enqueue(hexString);
                }
            }
            Debug.WriteLine("HEX: "+ sw.Elapsed.ToString());
        }
        private static void GenerateMD5Hashes()
        {
            var sw = Stopwatch.StartNew();
            var md5M = MD5.Create();
            for (int i = 0; i <= NUM_MD5_HASHES; i++)
            {
                byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + i.ToString());
                byte[] result = md5M.ComputeHash(data);
                string hexString = ConvertToTextString(result);
                //Console.WriteLine("MD5 HASH: {0}", hexString);
            }
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void GenerateMD5HashesCancel(System.Threading.CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            var sw = Stopwatch.StartNew();
            var md5M = MD5.Create();
            for (int i = 0; i <= NUM_MD5_HASHES; i++)
            {
                byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + i.ToString());
                byte[] result = md5M.ComputeHash(data);
                string hexString = ConvertToTextString(result);
                //Console.WriteLine("MD5 HASH: {0}", hexString);
                ct.ThrowIfCancellationRequested();
            }
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ParallelGenerateMD5Hashes()
        {
            var sw = Stopwatch.StartNew();
            Parallel.For(1, NUM_MD5_HASHES, (int i) =>
             {
                 var md5M = MD5.Create();
                 byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + i.ToString());
                 byte[] result = md5M.ComputeHash(data);
                 string hexString = ConvertToTextString(result);
             });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ParallelPartitionGenerateMD5Hashes()
        {
            var sw = Stopwatch.StartNew();
            Parallel.ForEach(Partitioner.Create(1, NUM_MD5_HASHES ,((int)(NUM_MD5_HASHES/Environment.ProcessorCount)+1)), range =>
            {
                var md5M = MD5.Create();
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + i.ToString());
                    byte[] result = md5M.ComputeHash(data);
                    string hexString = ConvertToTextString(result);
                }
            });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static IEnumerable<int> GenerateMD5InputData()
        {
            return Enumerable.Range(1, NUM_AES_KEYS);
        }
        private static void ParallelForeachGenerateMD5Hashes()
        {
            var sw = Stopwatch.StartNew();
            var intPutData = GenerateMD5InputData();
            Parallel.ForEach(intPutData, (int number) =>
             {
                 var md5 = MD5.Create();
                 byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number.ToString());
                 byte[] result = md5.ComputeHash(data);
                 string hexString = ConvertToTextString(result);
             });
            Console.WriteLine("MD5: " + sw.Elapsed.ToString());
        }
        private static void DisplayParrellelLoopResult(ParallelLoopResult loopResult)
        {
            string text = "";
            if(loopResult.IsCompleted)
            {
                text = "The loop ran to completion";
            }
            else
            {
                if(loopResult.LowestBreakIteration.HasValue)
                {
                    text = "The loop ended by calling the Break statement";
                }
                else
                {
                    text = "The loop ended prematurely with a Stop statement";
                }
            }
            Console.WriteLine(text);
        }
        private static void ParallelForEachGenerateMD5HashesBreak()
        {
            var sw = Stopwatch.StartNew();
            var intPutData = GenerateMD5InputData();
            var loopResult= Parallel.ForEach(intPutData, (int number,ParallelLoopState loopState) =>
            {
                var md5 = MD5.Create();
                byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number.ToString());
                byte[] result = md5.ComputeHash(data);
                string hexString = ConvertToTextString(result);
                if(sw.Elapsed.Seconds>3)
                {
                    loopState.Break();
                    return;
                }
            });
            DisplayParrellelLoopResult(loopResult);
            Console.WriteLine("MD5: " + sw.Elapsed.ToString());
        }
        private static void ParallelForEachGenerateMD5HashesException()
        {
            var sw = Stopwatch.StartNew();
            var intPutData = GenerateMD5InputData();
            var loopResult = new ParallelLoopResult();
            try
            {
                loopResult = Parallel.ForEach(intPutData, (int number, ParallelLoopState loopState) =>
                {
                    var md5 = MD5.Create();
                    byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + number.ToString());
                    byte[] result = md5.ComputeHash(data);
                    string hexString = ConvertToTextString(result);
                    if (sw.Elapsed.Seconds > 3)
                    {
                        throw new TimeoutException("Parallel.Foreach is taking more than 3 seconds to complete.");                        
                    }
                });
            }
            catch(AggregateException ex)
            {
                foreach(Exception innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine(innerEx.ToString());
                }
            }
            DisplayParrellelLoopResult(loopResult);
            Console.WriteLine("MD5: " + sw.Elapsed.ToString());
        }
        private static void ParallelGenerateAESKeysMaxDegree(int maxDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegree;
            var sw = Stopwatch.StartNew();
            Parallel.For(1, NUM_AES_KEYS+1, parallelOptions ,(int i) =>
            {
                var aesM = new AesManaged();
                aesM.GenerateKey();
                byte[] result = aesM.Key;
                string hexString = ConvertToTextString(result);
            });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        private static void ParallelGenerateMD5HashesMaxDegree(int maxDegree)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegree;
            var sw = Stopwatch.StartNew();
            Parallel.For(1, NUM_MD5_HASHES+1, parallelOptions,(int i) =>
            {
                var md5M = MD5.Create();
                byte[] data = Encoding.Unicode.GetBytes(Environment.UserName + i.ToString());
                byte[] result = md5M.ComputeHash(data);
                string hexString = ConvertToTextString(result);
            });
            Console.WriteLine("AES: " + sw.Elapsed.ToString());
        }
        #endregion
    }
}
