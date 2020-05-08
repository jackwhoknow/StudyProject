using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 现在的实际情况(被适配->Adaptee)
    /// </summary>
    public class Banner
    {
        private string info;
        public Banner(string info)
        {
            this.info = info;
        }
        public void ShowWithParen()
        {
            Console.WriteLine("("+this.info+")");
        }
        public void ShowWithAster()
        {
            Console.WriteLine("*" + this.info + "*");
        }
    }
}
