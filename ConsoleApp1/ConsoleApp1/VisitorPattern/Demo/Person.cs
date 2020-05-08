using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class VPerson
    {
        public abstract void Accept(Status visitor);
    }
}
