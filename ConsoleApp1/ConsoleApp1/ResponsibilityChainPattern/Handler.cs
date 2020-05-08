using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class Handler
    {
        protected Handler successor;
        /// <summary>
        /// 设置继任者
        /// </summary>
        /// <param name="successor"></param>
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }
        public abstract void HandleRequest(int request);
    }
}
