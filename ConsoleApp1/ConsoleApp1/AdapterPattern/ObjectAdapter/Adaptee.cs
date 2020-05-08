using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Adaptee
    {
        /// <summary> 
        /// 在被适配器类中的接口并不是客户端需要的接口 
        /// 比如这里是使用的中文，而我在客户端却必须要使用英文 
        /// 所以在这里我必须使用适配器来适配 
        /// </summary> 
        public void 得到温度()
        {
            Console.WriteLine("您得到了今日的温度");
        }
        public void 得到气压()
        {
            Console.WriteLine("您得到了今日的气压");
        }
        public void 得到湿度()
        {
            Console.WriteLine("您得到了今日的湿度");
        }
        public void 得到紫外线强度()
        {
            Console.WriteLine("您得到了今日的紫外线强度");
        }
    }
}
