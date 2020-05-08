using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 扮演适配器(Adapter)的角色
    /// </summary>
    class PrintBanner : IPrint
    {
        private Banner banner;
        public PrintBanner(string info)
        {
            banner = new Banner(info);
        }
        public void printStrong()
        {
            banner.ShowWithAster();
        }

        public void printWeak()
        {
            banner.ShowWithParen(); 
        }
    }
}
