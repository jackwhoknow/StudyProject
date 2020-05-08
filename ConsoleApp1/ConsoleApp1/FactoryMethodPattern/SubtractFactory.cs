using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class SubtractFactory : IFactory
    {
        public MathOperation CreateOperation()
        {
            return new MathSubtract();
        }
    }
}
