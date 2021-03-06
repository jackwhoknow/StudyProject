﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// 原子操作
    /// </summary>
    public static class AtomicOperationDemo
    {
        private static int _particapants = Environment.ProcessorCount;
        private static Task[] _tasks;
        private static Barrier _barrier;
        private const int TimeOut = 2000;

        public static void Run()
        {
            var cts = new System.Threading.CancellationTokenSource();
            var ct = cts.Token;
            _tasks = new Task[_particapants];
            _barrier = new Barrier(_particapants, (barrier) =>
            {
                Console.WriteLine("Current phase: {0}",barrier.CurrentPhaseNumber);
            });
            var s1 = new SpinLock(false);
            int totalRenders = 0;
            for(int i=0;i<_particapants;i++)
            {
                _tasks[i] = Task.Factory.StartNew((num) =>
                  {
                      var paiticipantNumber = (int)num;
                      for (int j = 0; j < 10; j++)
                      {
                          CreatePlanets(paiticipantNumber);
                          _barrier.SignalAndWait();
                          CreateStars(paiticipantNumber);
                          _barrier.SignalAndWait();
                          CheckCollisionBetweenPlanets(paiticipantNumber);
                          _barrier.SignalAndWait();
                          CheckCollisionBetweenStars(paiticipantNumber);
                          _barrier.SignalAndWait();
                          RenderCollisions(paiticipantNumber);
                          _barrier.SignalAndWait();
                          Interlocked.Increment(ref totalRenders);
                      }
                  },i,ct);
            }
            //很多任务完成其工作之后
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
               {
                   Task.WaitAll(_tasks);
                   Console.WriteLine("{0} renders were executed.",totalRenders);
                   _barrier.Dispose();
               });
            finalTask.Wait();
            Console.ReadLine();
        }

        private static void CreatePlanets(int participantNum)
        {
            if(participantNum==0)
            {
                SpinWait.SpinUntil(() => ( _barrier.ParticipantsRemaining == 0),TimeOut*3);
            }
            Console.WriteLine("Creating planets. Participant.# {0}", participantNum);
        }
        private static void CreateStars(int participantNum)
        {
            Console.WriteLine("Creating stars. Participant.# {0}", participantNum);
        }
        private static void CheckCollisionBetweenPlanets(int participantNum)
        {
            Console.WriteLine("Checking collisions between planets. Participant.# {0}", participantNum);
        }
        private static void CheckCollisionBetweenStars(int participantNum)
        {
            Console.WriteLine("Checking collisions between stars. Participant.# {0}", participantNum);
        }
        private static void RenderCollisions(int participantNum)
        {
            Console.WriteLine("Rendering collisions. Participant.# {0}", participantNum);
        }
    }
}
