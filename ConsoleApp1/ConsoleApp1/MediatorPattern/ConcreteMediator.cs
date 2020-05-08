using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ConcreteMediator : Mediator
    {
        private ConcreteColleague1 colleague1;
        private ConcreteColleague2 colleague2;

        public ConcreteColleague1 Colleague1 { get => colleague1; set => colleague1 = value; }
        public ConcreteColleague2 Colleague2 { get => colleague2; set => colleague2 = value; }

        public override void Send(string message, Colleague colleague)
        {
            if(colleague== colleague1)
            {
                colleague2.Notify(message);
            }
            else
            {
                colleague1.Notify(message);
            }
        }
    }
}
