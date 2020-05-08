using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Finery :Person
    {
        protected Person component;
        public void Decorate(Person component)
        {
            this.component = component;
        }
        public override void Show()
        {
            if(component!=null)
            {
                component.Show();
            }
        }
    }
    class Tshirts: Finery
    {
        public override void Show()
        {
            Console.WriteLine("大T恤");
            base.Show();
        }
    }
    class BigTrouser : Finery
    {
        public override void Show()
        {
            Console.WriteLine("垮裤");
            base.Show();
        }
    }
}
