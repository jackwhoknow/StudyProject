using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class ConcreteFlyWeight : FlyWeight
    {
        public override void Operation(int extrinsicstate)
        {
            Console.WriteLine("具体Flyweight:"+ extrinsicstate);
        }
    }
}
