using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Operation
    {
        private double number1 = 0.0;
        private double number2 = 0.0;
        public double Number1
        {
            get
            {
                return number1;
            }
            set
            {
                number1 = value;
            }
        }
        public double Number2
        {
            get
            {
                return number2;
            }
            set
            {
                number2 = value;
            }
        }
        public virtual double GetResult()
        {
            double result = 0.0;
            return result;
        }
    }
}
