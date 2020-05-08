using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ConcreteWebsite : Website
    {
        private string name = "";
        public ConcreteWebsite(string name)
        {
            this.name = name;
        }
        public override void Use(WebUser user)
        {
            Console.WriteLine("网站分类："+name+" 用户"+user.Name);
        }
    }
}
