using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{ 
    class WebsiteFactory
    {
        private Hashtable flyWeights = new Hashtable();
        public Website GetWebsiteCategory(string key)
        {
            if(!flyWeights.ContainsKey(key))
            {
                flyWeights.Add(key, new ConcreteWebsite(key));
            }
            return ((Website)flyWeights[key]);
        }
        public int GetWebsiteCount()
        {
            return flyWeights.Count;
        }
    }
}
