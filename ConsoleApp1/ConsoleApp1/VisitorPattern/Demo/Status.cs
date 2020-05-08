using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class Status
    {
        public abstract void GetManConclusion(Man man);
        public abstract void GetWomanConclusion(Woman woman);
    }
}
