using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    /// <summary>
    /// 包装一个Adapter1对象，把源接口转换成目标接口
    /// </summary>
    class Adapter1 :Target1
    {
        private Adaptee1 adaptee = new Adaptee1();
        public override void Request()
        {
            adaptee.SpecialRequest();
        }
    }
}
