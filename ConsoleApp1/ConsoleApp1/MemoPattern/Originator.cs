using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 发起人
    /// </summary>
    class Originator
    {
        private string state;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        public void SetMemento(Memento memento)
        {
            state = memento.State;
        }
        public Memento CreateMemento()
        {
            return new Memento(state);
        }

        public void Show()
        {
            Console.WriteLine("State= "+state);
        }
    }
}
