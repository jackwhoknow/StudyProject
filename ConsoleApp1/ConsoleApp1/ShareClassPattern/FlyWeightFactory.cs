using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class FlyWeightFactory
    {
        private Hashtable flyweights = new Hashtable();
        public FlyWeightFactory()
        {
            flyweights.Add("X", new ConcreteFlyWeight());
            flyweights.Add("Y", new ConcreteFlyWeight());
            flyweights.Add("Z", new ConcreteFlyWeight());
        }
        public FlyWeight GetFlyWeight(string key)
        {
           return ((FlyWeight)flyweights[key]);
        }

    }
}
