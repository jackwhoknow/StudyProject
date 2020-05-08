using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Woman : VPerson
    {
        public override void Accept(Status visitor)
        {
            visitor.GetWomanConclusion(this);
        }
    }
}
