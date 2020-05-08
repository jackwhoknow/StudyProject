using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Multiply:Operation
    {
        public override double GetResult()
        {
            return Number1*Number2;
        }
    }
}
