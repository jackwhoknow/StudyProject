using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Iraq : Country
    {
        public Iraq(UniteNations uniteNations) : base(uniteNations)
        {
        }
        public void Declare(string message)
        {
            mediator.Declare(message, this);
        }
        public void GetMessage(string message)
        {
            Console.WriteLine("伊拉克获得对方信息：" + message);
        }
    }
}
