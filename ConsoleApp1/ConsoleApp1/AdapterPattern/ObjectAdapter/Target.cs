using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public abstract class Target
    {
        /// <summary>
        /// 温度 
        /// </summary>
        public abstract void GetTemperature();
        /// <summary>
        /// 气压 
        /// </summary>
        public abstract void GetPressure();
        /// <summary>
        /// 湿度
        /// </summary>
        public abstract void GetHumidity();
        /// <summary>
        /// 紫外线强度 
        /// </summary>

        public abstract void GetUltraviolet();
    }
}
