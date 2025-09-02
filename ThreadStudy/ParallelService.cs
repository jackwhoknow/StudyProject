using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public static void Demo5()
        {
            var data = BuildTestDatas();
            ParallelLoopResult result = Parallel.ForEach<string>(data, s =>
            {
                Console.WriteLine(s);
            });
        }

        public static void Demo6()
        {
            var data = BuildTestDatas();
            ParallelLoopResult result = Parallel
                .ForEach<string>(data, (s,pls,l) =>
            {
                Console.WriteLine($"{s}  {l+1}");
                if(s=="one")
                {
                    pls.Break();
                }
            });
        }

        public static void RunSynchronousTask()
        {
            TaskMethod("just the main thread");
            var t1 = new Task(TaskMethod,"run sync");
            t1.RunSynchronously();
        }

        public static void LongRunningTask()
        {
            var t1 = new Task(TaskMethod,"long running",TaskCreationOptions.LongRunning);
            t1.Start();
        }

        public static void RunTaskWithResult()
        {
            var t1 = new Task<Tuple<int, int>>(TaskWithResult,Tuple.Create(8,3));
            t1.Start();
            Console.WriteLine(t1.Result);
            t1.Wait();
            Console.WriteLine($"result from task:{ t1.Result.Item1},{t1.Result.Item2}");
        }

        static Tuple<int,int> TaskWithResult(object division)
        {
            if(division is Tuple<int, int> tuple)
            {
                int result = tuple.Item1/tuple.Item2;
                int reminder = tuple.Item1 % tuple.Item2;
                Console.WriteLine("task creates a result...");
                return Tuple.Create(result, reminder);
            }           
            else
            {
                return Tuple.Create(0,0);
            }
        }

        public static void RunCountinousTask()
        {
            var t1 = new Task(DoOnFirst);

            var t2 = t1.ContinueWith(DoOnSecond,TaskContinuationOptions.OnlyOnFaulted);

            var t3 = t2.ContinueWith(DoOnThird);

            t1.Start();
        }

        static void DoOnFirst()
        {
            Console.WriteLine($"do some task {Task.CurrentId}");
            Thread.Sleep(3000);
        }

        static void DoOnSecond(Task t)
        {
            Console.WriteLine($"task {t.Id} finished");
            Console.WriteLine($"this task id {Task.CurrentId}");
            Console.WriteLine("second do some work");
            Thread.Sleep(1000);
        }

        static void DoOnThird(Task t)
        {
            Console.WriteLine($"task {t.Id} finished");
            Console.WriteLine($"this task id {Task.CurrentId}");
            Console.WriteLine("third do some work");
            Thread.Sleep(3000);
        }

        public static void ParentAndChild()
        {
            var parent = new Task(ParentTask);
            parent.Start();
            Thread.Sleep(2000);

            Console.WriteLine(parent.Status);
            Thread.Sleep(4000);
            Console.WriteLine(parent.Status);
        }

        static void ParentTask()
        {
            Console.WriteLine($"task id {Task.CurrentId}");
            var child = new Task(ChildTask);
            child.Start();
            //Thread.Sleep(10000);
            Console.WriteLine("parent start child ");
        }

        static void ChildTask()
        {
            Console.WriteLine("child");
            Thread.Sleep(50000);
            Console.WriteLine("child finished");
        }


        static object taskMethodLock = new object();
        static void TaskMethod(object title)
        {
            Console.WriteLine(title);
            Console.WriteLine("Task id: {0},thread: {1}",Task.CurrentId==null?"no task":Task.CurrentId.ToString(),
                Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine($"is pooled thread:{Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"is background thread:{Thread.CurrentThread.IsBackground}");
            Console.WriteLine();
        }

        public static void ParallelInvoke()
        {
            // 既可以用于任务，又可以用于数据并行性
            Parallel.Invoke(Foo, Bar, Schcool, Home, Factory, Company, Goverment);
        }
        static void Foo()
        {
            Console.WriteLine("foo");
        }
        static void Bar()
        {
            Console.WriteLine("bar");
        }
        static void Schcool()
        {
            Console.WriteLine("Schcool");
        }
        static void Home()
        {
            Console.WriteLine("Home");
        }
        static void Factory()
        {
            Console.WriteLine("Factory");
        }
        static void Company()
        {
            Console.WriteLine("Company");
        }
        static void Goverment()
        {
            Console.WriteLine("Goverment");
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
        private static string[] BuildTestDatas()
        {
            string[] data = { "one","two","three","four","five","six","seven","eight","nine","ten",
                "eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen","eighteen","nineteen","twenty"};
            return data;
        }

        public static void RunCancellationTokenSource()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => { Console.WriteLine("*** token canceled"); });
            cts.CancelAfter(500);

            try
            {
                ParallelLoopResult result = Parallel.For(0, 100, new ParallelOptions()
                {
                    CancellationToken = cts.Token,
                },
                x =>
                {
                    Console.WriteLine($"loop {x} started.");
                    int sum = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(2);
                        sum += i;
                    }
                    Console.WriteLine($"loop {x} finished.");
                });
            }
            catch(OperationCanceledException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        public static void RunCancelTask()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => { Console.WriteLine("*** token canceled"); });
            cts.CancelAfter(500);

            Task t1 = Task.Run(() =>
            {
                Console.WriteLine("in task");
                for(int i=0;i<100;i++)
                {
                    Thread.Sleep(100);
                    CancellationToken token= cts.Token;

                    if(token.IsCancellationRequested)
                    {
                        Console.WriteLine("cancelling was requested, "+"cancelling from within task");
                        token.ThrowIfCancellationRequested();
                        break;
                    }
                    Console.WriteLine("in loop");
                }
                Console.WriteLine("task finished without cancellation");
            },cts.Token);

            try
            {
                t1.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"exception: {ex.GetType().Name},{ex.Message}");
                foreach(var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine($"inner exception: {innerException.GetType().Name},{innerException.Message}");
                }
            }
        }

        public static void RunThreadPool()
        {
            int nWorkerThreads;
            int nCompletionPortThreads;
            ThreadPool.GetMaxThreads(out nWorkerThreads,out nCompletionPortThreads);

            Console.WriteLine($"worker thread count：{nWorkerThreads}，I/O thread count: {nCompletionPortThreads}");

            for(int i=0;i<50;i++)
            {
                ThreadPool.QueueUserWorkItem(JobForAThread);
            }

            Thread.Sleep(3000);
        }


        static void JobForAThread(object state)
        {
            for(int i=0;i<10;i++)
            {
                Console.WriteLine($"loop {i},running inside pooled thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(50);
            }
        }

        public static void RaceConditions()
        {
            var state = new StateObject();
            for(int i=0;i<3;i++)
            {
                Task.Run(() =>
                {
                    new SampleTask().RaceCondition(state);
                });
            }
        }
        public static void RunDeadLock()
        {
            var state1 = new StateObject();
            var state2 = new StateObject();
            new Task(new SampleTask2(state1, state2).DeadLock1).Start();
            new Task(new SampleTask2(state1, state2).DeadLock2).Start();
        }
        public static void RunLockSync()
        {
            int numTasks = 20;
            var state = new SharedState();
            var tasks = new Task[numTasks]; 
            for(int i=0;i<numTasks;i++)
            {
                tasks[i] = Task.Run(new Job(state).DoTheJob);
            }
            for (int i = 0; i < numTasks; i++)
            {
                tasks[i].Wait();
            }
            Console.WriteLine($"summarized {state.State}");
        }
        public static void RunWaitHandle()
        {
            bool createdNew;
            var mutex = new Mutex(false,"SingletonWinAppMutex",out createdNew);
            if(!createdNew)
            {
                Console.WriteLine("You can only start one instance of the application");
                return;
            }
        }

        public static void RunSemaphoreSlim()
        {
            int taskCount = 16;
            int semaphoreCount = 3;
            var semphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
            var tasks = new Task[taskCount];

            for(int i=0;i< taskCount;i++)
            {
                tasks[i]=Task.Run(()=> RunSemaphoreSlim(semphore));
            }

            Task.WaitAll(tasks);

            Console.WriteLine("All tasks finished");
        }
        private static void RunSemaphoreSlim(SemaphoreSlim semaphore)
        {
            bool isComplete = false;
            while(!isComplete)
            {
                if(semaphore.Wait(600))
                {
                    try
                    {
                        Console.WriteLine($"Task {Task.CurrentId} locks the semaphore");
                        Thread.Sleep(5000);
                    }
                    finally
                    {
                        Console.WriteLine($"Task {Task.CurrentId} releases the semaphore");
                        semaphore.Release();
                        isComplete = true;
                    }
                }
                else
                {
                    Console.WriteLine($"Timeout for task {Task.CurrentId}; wait again");
                }
            }
        }
    }

    class SampleTask
    {
        public void RaceCondition(object o)
        {
            Trace.Assert(o is StateObject, "o must be of type StateObject");
            StateObject state = o as StateObject;
            int i = 0;
            while(true)
            {
                state.ChangeState(i++);
            }
        }
    }

    class SampleTask2
    {
        private StateObject _s1;
        private StateObject _s2;
        public SampleTask2(StateObject s1, StateObject s2)
        {
            _s1 = s1;
            _s2 = s2;
        }

        public void DeadLock1()
        {
            Console.WriteLine($"DeadLock1-1");
            int i = 0;
            Console.WriteLine($"DeadLock1-2");
            while (true)
            {
                Console.WriteLine($"DeadLock1-3");
                lock (_s1)
                {
                    Console.WriteLine($"DeadLock1-4");
                    lock (_s2)
                    {
                        Console.WriteLine($"DeadLock1-5");
                        _s1.ChangeState(i);
                        Console.WriteLine($"DeadLock1-6");
                        _s2.ChangeState(i++);
                        Console.WriteLine($"DeadLock1-7");
                        Console.WriteLine($"still running,{i}");
                        Console.WriteLine($"DeadLock1-8");
                    }
                    Console.WriteLine($"DeadLock1-9");
                }
            }
        }

        public void DeadLock2()
        {
            Console.WriteLine($"DeadLock2-1");
            int i = 0;
            Console.WriteLine($"DeadLock2-2");
            while (true)
            {
                Console.WriteLine($"DeadLock2-3");
                lock (_s2)
                {
                    Console.WriteLine($"DeadLock2-4");
                    lock (_s1)
                    {
                        Console.WriteLine($"DeadLock2-5");
                        _s1.ChangeState(i);
                        Console.WriteLine($"DeadLock2-6");
                        _s2.ChangeState(i++);
                        Console.WriteLine($"DeadLock2-7");
                        Console.WriteLine($"still running,{i}");
                        Console.WriteLine($"DeadLock2-8");
                    }
                    Console.WriteLine($"DeadLock2-9");
                }
            }
        }
    }

    class StateObject
    {
        private int state = 5;
        //private object sync = new object();
        public void ChangeState(int loop)
        {
            //lock(sync)
            //{
            Console.WriteLine($"ChangeState-1");
            if (state == 5)
                {
                Console.WriteLine($"ChangeState-2");
                state++;
                Console.WriteLine($"ChangeState-3");
                Trace.Assert(state == 6, $"Race condition occurred after {loop} loops");
                Console.WriteLine($"ChangeState-4");
            }
            Console.WriteLine($"ChangeState-5");
            state = 5;
            Console.WriteLine($"ChangeState-6");
            //}            
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
    public class SharedState
    {
        private int _state;
        private object syncRoot = new object();
        public int State 
        { 
            get
            {
                lock(syncRoot)
                {
                    return _state;
                }
            }
            set
            {
                lock(syncRoot)
                {
                    _state = value;
                }
            }
        }
    }
    class Job
    {
        SharedState _sharedState;
        public Job(SharedState sharedState)
        {
            this._sharedState = sharedState;
        }
        public void DoTheJob()
        {
            for(int i=0;i<50000;i++)
            {
                lock(_sharedState)
                {
                    _sharedState.State += 1;
                }                
            }            
        }
    }
}
