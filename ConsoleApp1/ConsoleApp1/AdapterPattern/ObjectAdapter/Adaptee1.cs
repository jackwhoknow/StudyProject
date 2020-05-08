using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 需要适配的类
    /// </summary>
    class Adaptee1
    {
        public void SpecialRequest()
        {
            Console.WriteLine("特殊请求！");
        }
    }
}
