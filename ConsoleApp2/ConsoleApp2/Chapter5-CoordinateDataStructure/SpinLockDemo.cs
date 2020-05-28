using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class SpinLockDemo
    {
        private static int _particapants = Environment.ProcessorCount;
        private static Task[] _tasks;
        private static Barrier _barrier;
        private const int TimeOut = 2000;

        public static void Run()
        {
            _tasks = new Task[_particapants];
            _barrier = new Barrier(_particapants, (barrier) =>
            {
                Console.WriteLine("Current phase: {0}",barrier.CurrentPhaseNumber);               
            });
            var sl = new SpinLock(false);
            var sb = new StringBuilder();
            for(int i=0;i<_particapants;i++)
            {
                _tasks[i] = Task.Factory.StartNew((num) =>
                  {
                      var localsb = new StringBuilder();
                      var paiticipantNumber = (int)num;
                      for (int j = 0; j < 20; j++)
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

                          var logline=String.Format("Time: {0},Phase: {1},Participant: {2},Phase completed OK \n",
                              DateTime.Now.TimeOfDay,_barrier.CurrentPhaseNumber,paiticipantNumber);

                          bool lockTaken = false;
                          try
                          {
                              sl.TryEnter(2000,ref lockTaken);
                              if(!lockTaken)
                              {
                                  Console.WriteLine("Lock timeout for participant: {0}",paiticipantNumber);
                                  throw new TimeoutException(string.Format("Participants are requiring more than {0} seconds " +
                                      "to acquire the lock at the Phase # {1}.",2000,_barrier.CurrentPhaseNumber));
                              }
                              sb.Append(logline);
                          }
                          finally
                          {
                              if (lockTaken)
                              {
                                  sl.Exit(false);
                              }
                          }
                      }
                  },i);
            }
            //很多任务完成其工作之后
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
               {
                   Task.WaitAll(_tasks);
                   Console.WriteLine("All the phased were executed.");
                   Console.WriteLine(sb);
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
