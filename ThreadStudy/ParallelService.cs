using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStudy
{
    internal class ParallelService
    {
        public static void PrintCpuInfo()
        {
            Console.WriteLine($"处理器核心数: {Environment.ProcessorCount}");
            ThreadPool.GetAvailableThreads(out int workerThreads, out _);
            Console.WriteLine($"可用工作线程: {workerThreads}");
        }

        public static void Demo1()
        {
            var count = 5000;
            var datas = BuildTestDatas(count);
            var loopResult = Parallel.For(0, count, i =>
            {
                var data = datas[i];
                Console.WriteLine($"data id:{data.Id},data value:{data.Value}, task:{Task.CurrentId},thread:{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(10);
            });
            Console.WriteLine($"Is complete:{loopResult.IsCompleted}");
        }

        public static void Demo2()
        {
            var count = 5000;
            var datas = BuildTestDatas(count);
            var loopResult = Parallel.For(0, count, async i =>
            {
                var data = datas[i];
                Console.WriteLine($"data id:{data.Id},data value:{data.Value}, task:{Task.CurrentId},thread:{Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(10);
            });
        }

        public static void Demo3()
        {
            var count = 5000;
            var datas = BuildTestDatas(count);
            var loopResult = Parallel.For(0, count, async (int i,ParallelLoopState pls) =>
            {
                var data = datas[i];
                Console.WriteLine($"data id:{data.Id},data value:{data.Value}, task:{Task.CurrentId},thread:{Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(10);
                if(i>100)
                {
                    pls.Break();
                }
            });
        }

        public static void Demo4()
        {
            Parallel.For<string>(0, 20, () =>
            {
                return Guid.NewGuid().ToString();
            }
            , (i, pls, str1) =>
            {
                Console.WriteLine($"body i： {i}, str1： {str1}, thread： {Thread.CurrentThread.ManagedThreadId},task：{Task.CurrentId}");
                Thread.Sleep(10);
                return $"i： {i}";
            },
            (str1) =>
            {
                Console.WriteLine($"finlly {str1}");
            });
        }

        private static List<DataInfo> BuildTestDatas(int count)
        {
            var datas = new List<DataInfo>();
            for (int i = 0; i < count; i++)
            {
                datas.Add(new DataInfo((i + 1).ToString(), i));
            }
            return datas;
        }
    }

    class DataInfo
    {
        public int Value { get; private set; }
        public string Id { get; private set; }
        public DataInfo(string id,int value)
        {
            Id = id;
            Value = value;
        }
    }
}
