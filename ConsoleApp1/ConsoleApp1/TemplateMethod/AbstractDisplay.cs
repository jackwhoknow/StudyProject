using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public abstract class AbstractDisplay
    {
        public abstract void Open();
        public abstract void Close();
        public abstract void Print();
        public void Display()
        {
            Open();
            for(int i=0;i<5;i++)
            {
                Print();
            }
            Close();
        }
    }
}
