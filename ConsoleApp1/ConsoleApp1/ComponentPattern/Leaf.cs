using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Leaf : Component
    {
        public Leaf(string name):base(name)
        {
        }
        public override void Add(Component c)
        {
            Console.WriteLine("Can't add to a leaf");
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-',depth)+name);
        }

        public override void Remove(Component c)
        {
            Console.WriteLine("Can't remove to a leaf");
        }
    }
}
