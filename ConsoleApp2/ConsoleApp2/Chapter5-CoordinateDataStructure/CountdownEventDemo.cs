using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class CountdownEventDemo
    {
        private static CountdownEvent _countdown;
        private static int MIN_PATHS = Environment.ProcessorCount;
        private static int MAX_PATHS = Environment.ProcessorCount * 3;
        
        public static void Run()
        {
            _countdown = new CountdownEvent(MIN_PATHS);
            var t1 = Task.Factory.StartNew(() =>
              {
              for (int i = MIN_PATHS; i<MAX_PATHS ;i++)
                  {
                      Console.WriteLine(">>>> {0} Concurrent paths start.",i);

                      //Reset the count to i
                      //需要解除阻塞的调用
                      _countdown.Reset(i);
                      SimulatePaths(i);
                      //Join
                      _countdown.Wait();
                      Console.WriteLine("<<<< {0} Concurrent paths end.",i);
                  }
              });
            try
            {
                t1.Wait();
                Console.WriteLine("The simulation was executes");
            }
            finally
            {
                _countdown.Dispose();
            }
            Console.ReadLine();
        }

        private static void SimulatePaths(int pathCount)
        {
            for(int i=0;i<pathCount;i++)
            {
                Task.Factory.StartNew((num) =>
                {
                    try
                    {
                        var pathNumber = (int)num;
                        var sw = Stopwatch.StartNew();
                        var rand = new Random();
                        Thread.Sleep(rand.Next(2000, 5000));
                        Console.WriteLine("Path {0} simulated.",pathNumber);
                    }
                    finally
                    {
                        //Signal the CountdownEvent
                        //to decrement the count
                        _countdown.Signal();
                    }
                }, i);
            }
        }
    }
}
