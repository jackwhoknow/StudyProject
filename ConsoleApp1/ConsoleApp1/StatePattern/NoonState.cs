using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class NoonState : State
    {
        public override void WriteProgram(Work work)
        {
            if(work.Hour<13)
            {
                Console.WriteLine("当前时间：{0}点，饿了，午饭；困了，午休。",work.Hour);
            }
            else
            {
                work.SetState(new AfternoonState());
                work.WriteProgram();
            }
        }
    }
}
