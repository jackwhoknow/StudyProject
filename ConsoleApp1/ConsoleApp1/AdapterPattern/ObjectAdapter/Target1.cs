using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 客户期待的接口
    /// </summary>
    class Target1
    {
        public virtual void Request()
        {
            Console.WriteLine("普通请求！");
        }
    }
}
