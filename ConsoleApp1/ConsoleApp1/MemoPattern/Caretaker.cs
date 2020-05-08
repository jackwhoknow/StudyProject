using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Caretaker
    {
        private Memento memento;

        internal Memento Memento { get => memento; set => memento = value; }
    }
}
