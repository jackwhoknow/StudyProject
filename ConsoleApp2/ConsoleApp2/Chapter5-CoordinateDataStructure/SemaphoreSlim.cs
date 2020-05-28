using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class SemaphoreDemo
    {
        private static Semaphore _semaphore;
        private static Task[] _tasks;
        private const int MAX_MACHINES = 3;
        private static int _attackers = Environment.ProcessorCount;
        public static void Run()
        {
            _tasks = new Task[_attackers];
            _semaphore = new Semaphore(MAX_MACHINES,MAX_MACHINES);
            Console.WriteLine("{0} attackers are going to be able to enter the semaphore.", MAX_MACHINES);
            for(int i =0;i< _attackers;i++)
            {
                _tasks[i] = Task.Factory.StartNew((num) =>
                  {
                      var attackNumber = (int)num;
                      for(int j=0;j<10;j++)
                      {
                          SimulateAttacks(attackNumber);
                      }
                  }, i);
            }
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
               {
                   Task.WaitAll(_tasks);
                   Console.WriteLine("The simulation was executed.");
                   _semaphore.Dispose();
               });
            finalTask.Wait();
            Console.ReadLine();
        }
        private static void SimulateAttacks(int attackNumber)
        {
            var sw = Stopwatch.StartNew();
            var rand = new Random();

            Thread.Sleep(rand.Next(2000, 5000));

            Console.WriteLine("WAIT #### Attacker {0} requested to enter the semaphore.",attackNumber);
            _semaphore.WaitOne();
            try
            {
                Console.WriteLine("ENTER ---> Attacker {0} entered the semaphore.",attackNumber);
                sw.Restart();
                Thread.Sleep(rand.Next(2000, 5000));
            }
            finally
            {
                _semaphore.Release();
                Console.WriteLine("Release <--- Attacker {0} release the semaphore.", attackNumber);
            }
        }
    }
}
