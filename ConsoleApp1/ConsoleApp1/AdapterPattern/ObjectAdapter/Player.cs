using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class Player
    {
        protected string name;
        public Player(string name)
        {
            this.name = name;
        }
        public abstract void Attack();
        public abstract void Defense();
    }
    class Fowards : Player
    {
        public Fowards(string name):base(name)
        {
        }
        public override void Attack()
        {
            Console.WriteLine("前锋 {0} 进攻",name);
        }

        public override void Defense()
        {
            Console.WriteLine("前锋 {0} 防守", name);
        }
    }
    class Center : Player
    {
        public Center(string name) : base(name)
        {
        }
        public override void Attack()
        {
            Console.WriteLine("中锋 {0} 进攻", name);
        }

        public override void Defense()
        {
            Console.WriteLine("中锋 {0} 防守", name);
        }
    }
    class Guards : Player
    {
        public Guards(string name) : base(name)
        {
        }
        public override void Attack()
        {
            Console.WriteLine("后卫 {0} 进攻", name);
        }

        public override void Defense()
        {
            Console.WriteLine("后卫 {0} 防守", name);
        }
    }
    class ForeignCenter
    {
        private string name = "";

        public string Name { get => name; set => name = value; }
        public void JingGong()
        {
            Console.WriteLine("外籍中锋 {0} 进攻", name);
        }
        public void FangShou()
        {
            Console.WriteLine("外籍中锋 {0} 防守", name);
        }
    }
}
