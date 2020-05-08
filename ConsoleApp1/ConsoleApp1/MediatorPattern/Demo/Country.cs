using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public abstract class Country
    {
        protected UniteNations mediator;
        public Country(UniteNations mediator)
        {
            this.mediator = mediator;
        }
    }
}
