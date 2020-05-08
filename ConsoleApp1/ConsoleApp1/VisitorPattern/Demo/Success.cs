using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Success : Status
    {
        public override void GetManConclusion(Man concreteElementA)
        {
            Console.WriteLine("{0} {1}时，背后多半有一个伟大的女人", concreteElementA.GetType().Name,this.GetType().Name);
        }

        public override void GetWomanConclusion(Woman concreteElementB)
        {
            Console.WriteLine("{0} {1}时，背后大多有一个不成功的男人", concreteElementB.GetType().Name, this.GetType().Name);
        }
    }
}
