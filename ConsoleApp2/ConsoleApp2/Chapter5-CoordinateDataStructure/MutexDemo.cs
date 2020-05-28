using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class MutexDemo
    {
        private static int _particapants = Environment.ProcessorCount;
        private static Task<string>[] _tasks;
        private static Barrier _barrier;
        private const int TimeOut = 2000;

        public static void Run()
        {
            _tasks = new Task<string>[_particapants];
            _barrier = new Barrier(_particapants, (barrier) =>
            {
                Console.WriteLine("Current phase: {0}",barrier.CurrentPhaseNumber);               
            });            
            for(int i=0;i<_particapants;i++)
            {
                _tasks[i] = Task<string>.Factory.StartNew((num) =>
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

                          localsb.AppendFormat("Time: {0},Phase: {1},Participant: {2},Phase completed OK \n",
                              DateTime.Now.TimeOfDay,_barrier.CurrentPhaseNumber,paiticipantNumber);

                          //lock(sb)
                          //{
                          //    sb.Append(logline);
                          //}


                          //bool lockTaken = false;
                          //try
                          //{
                          //    Monitor.TryEnter(sb,2000, ref lockTaken);
                          //    if(!lockTaken)
                          //    {
                          //        Console.WriteLine("Lock timeout for participant: {0}", paiticipantNumber);
                          //        throw new TimeoutException(string.Format("Participants are requiring more than {0} seconds "+
                          //            "to acquire the lock at the Phase # {1}.",2000,_barrier.CurrentPhaseNumber));
                          //    }
                          //    //Monitor acquired a lock on sb
                          //    //Critical section
                          //    sb.Append(logline);
                          //    SpinWait.SpinUntil(() => (_barrier.ParticipantsRemaining == 0));
                          //    //End of critial section
                          //}
                          //finally
                          //{
                          //    if(lockTaken)
                          //    {
                          //        Monitor.Exit(sb);
                          //    }
                          //}
                      }
                      return localsb.ToString();
                  },i);
            }
            //很多任务完成其工作之后
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
               {
                   Task.WaitAll(_tasks);
                   Console.WriteLine("All the phased were executed.");
                   var finalSb = new StringBuilder();
                   for(int i=0;i<_particapants;i++)
                   {
                       if((!_tasks[i].IsFaulted) && (!_tasks[i].IsCanceled))
                       {
                           finalSb.Append(_tasks[i].Result);
                       }
                   }
                   Console.WriteLine(finalSb);
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
