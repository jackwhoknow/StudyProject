using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class MultiplyFactory : IFactory
    {
        public MathOperation CreateOperation()
        {
            return new MathMultiply();
        }
    }
}
