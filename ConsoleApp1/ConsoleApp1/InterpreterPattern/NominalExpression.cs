using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class NominalExpression : AbstractExpression
    {
        public override void Interpret(ExpressionContext context)
        {
            Console.WriteLine("非终端解释器");
        }
    }
}
