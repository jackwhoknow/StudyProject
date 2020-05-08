using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class WebUser
    {
        private string name = "";
        public WebUser(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
        }
    }
}
