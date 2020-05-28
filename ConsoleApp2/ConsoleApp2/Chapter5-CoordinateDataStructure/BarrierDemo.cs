using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class BarrierDemo
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
                if(barrier.CurrentPhaseNumber==10)
                {
                    throw new InvalidOperationException("No more phases allowed.");
                }
            });
            for(int i=0;i<_particapants;i++)
            {
                _tasks[i] = Task.Factory.StartNew((num) =>
                  {
                      var paiticipantNumber = (int)num;
                      for (int j = 0; j < 20; j++)
                      {
                          CreatePlanets(paiticipantNumber);
                          if(!_barrier.SignalAndWait(TimeOut))
                          {
                              Console.WriteLine("Participants are requiring more than {0}" +
                                  " seconds to reach the barrier.",TimeOut);
                              throw new OperationCanceledException(string.Format("Participants are requiring more than {0} seconds" +
                                  "to reach the barrier at the phase # {1}", TimeOut, _barrier.CurrentPhaseNumber), ct);
                          }
                          CreateStars(paiticipantNumber);
                          _barrier.SignalAndWait();
                          CheckCollisionBetweenPlanets(paiticipantNumber);
                          _barrier.SignalAndWait();
                          CheckCollisionBetweenStars(paiticipantNumber);
                          _barrier.SignalAndWait();
                          RenderCollisions(paiticipantNumber);
                          _barrier.SignalAndWait();
                      }
                  },i,ct);
            }
            //很多任务完成其工作之后
            var finalTask = Task.Factory.ContinueWhenAll(_tasks, (tasks) =>
               {
                   Task.WaitAll(_tasks);
                   Console.WriteLine("All the phased were executed.");
                   
               },ct);
            try
            {
                if(!finalTask.Wait(TimeOut*2))
                {
                    bool faulted = false;
                    for(int t=0;t<_particapants;t++)
                    {
                        if (_tasks[t].Status != TaskStatus.RanToCompletion)
                        {
                            faulted = true;
                            if(_tasks[t].Status != TaskStatus.Faulted)
                            {
                                if(_tasks[t].Exception!=null)
                                {
                                    foreach (var innerEx in _tasks[t].Exception.InnerExceptions)
                                    {
                                        Console.WriteLine(innerEx.Message);
                                    }
                                }
                            }
                            if(faulted)
                            {
                                Console.WriteLine("The phases faild their execution");
                            }
                            else
                            {
                                Console.WriteLine("All the phases were executed");
                            }
                        }
                    }
                }
            }
            catch(AggregateException ex)
            {
                foreach(var innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine(innerEx.Message);
                }
                Console.WriteLine("The phases faild their execution");
            }
            finally
            {
                _barrier.Dispose();
            }
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
