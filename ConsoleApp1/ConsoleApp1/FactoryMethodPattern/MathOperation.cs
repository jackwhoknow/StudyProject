using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class MathOperation
    {
        private double number1;
        private double number2;
        public double Number1
        {
            get { return number1; }
            set { number1 = value; }
        }
        public double Number2
        {
            get { return number2; }
            set { number2 = value; }
        }
        public virtual double GetResult()
        {
            return 0.0;
        }
    }
}
