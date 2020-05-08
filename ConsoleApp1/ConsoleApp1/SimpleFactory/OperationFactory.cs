using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class OperationFactory
    {
        public static Operation CreateOperation(string operate)
        {
            Operation oper = null;
            switch(operate)
            {
                case "+":
                    oper = new Sum();
                    break;
                case "-":
                    oper = new Subtract();
                    break;
                case "*":
                    oper = new Multiply();
                    break;
                case "/":
                    oper = new Divide();
                    break;
            }
            return oper;
        }
    }
}
