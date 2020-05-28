using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class ManualResetEventDemo
    {
        private ManualResetEvent _mre;
        public ManualResetEventDemo()
        {
            this._mre = new ManualResetEvent(true);
        }
        public void CreateThreads()
        {
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(Run));
            t2.Start();
        }
        public void Set()
        {
            _mre.Set();
        }
        public void Reset()
        {
            _mre.Reset();
        }

        private void Run()
        {
            string strTheradID = string.Empty;
            try
            {
                while(true)
                {
                    //阻塞当前线程
                    this._mre.WaitOne();
                    strTheradID = Thread.CurrentThread.ManagedThreadId.ToString();
                    Console.WriteLine("Thread("+strTheradID+") is running...");
                    Thread.Sleep(5000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("线程（"+strTheradID+"）发生异常！错误描述："+ex.Message.ToString());
            }
        }
    }
}
