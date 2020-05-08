using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Invoker
    {
        private BaseCommand command;
        public void SetCommand(BaseCommand cmd)
        {
            this.command = cmd;
        }
        public void ExecuteCommand()
        {
            command.Execute();
        }
    }
}
