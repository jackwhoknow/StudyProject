using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ObjectStructure
    {
        private IList<VPerson> elements = new List<VPerson>();
        public void Attach(VPerson element)
        {
            elements.Add(element);
        }
        public void Remove(VPerson element)
        {
            elements.Remove(element);
        }
        public void Display(Status visitor)
        {
            foreach(VPerson e in elements)
            {
                e.Accept(visitor);
            }
        }
    }
}
