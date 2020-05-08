using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Memento
    {
        public Memento(string state)
        {
            this.state=state;
        }
        private string state = "";

        public string State { get => state; private set => state = value; }
    }
}
