using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Adapter:Target
    {
        //在适配器中必须要维护一个被适配器类的对象 
        private Adaptee adaptee = new Adaptee();
        /// <summary> 
        /// 通过适配器来适配原来不能被客户端所认识的接口 
        /// </summary> 
        public override void GetTemperature()
        {
            adaptee.得到温度();
        }
        public override void GetPressure()
        {
            adaptee.得到气压();
        }
        public override void GetHumidity()
        {
            adaptee.得到湿度();
        }
        public override void GetUltraviolet()
        {
            adaptee.得到紫外线强度();
        }
    }
}
