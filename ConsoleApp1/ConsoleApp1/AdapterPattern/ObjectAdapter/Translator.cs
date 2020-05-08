using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Translator:Player
    {
        private ForeignCenter wjzf = new ForeignCenter();
        public Translator(string name):base(name)
        {
            wjzf.Name = name;
        }

        public override void Attack()
        {
            wjzf.JingGong();
        }

        public override void Defense()
        {
            wjzf.FangShou(); 
        }
    }
}
