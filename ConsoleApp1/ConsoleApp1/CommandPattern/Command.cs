using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public abstract class Command
    {
        protected Barbecuer receiver;
        public Command(Barbecuer receiver)
        {
            this.receiver = receiver;
        }
        public abstract void ExecuteCommand();
    }

    public abstract class BaseCommand
    {
        protected Receiver receiver;
        public BaseCommand(Receiver receiver)
        {
            this.receiver = receiver;
        }
        public abstract void Execute();
    }
}
