using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Waiter
    {
        private IList<Command> orders=new List<Command>();
        public void SetOrder(Command command)
        {
            this.orders.Add(command);
            Console.WriteLine("增加订单：" + command.ToString() + "时间：" + DateTime.Now.ToString());
        }
        public void RemoveOrder(Command command)
        {
            this.orders.Remove(command);
            Console.WriteLine("取消订单："+command.ToString()+"时间："+DateTime.Now.ToString());
        }
        public void Notify()
        {
            foreach(Command cmd in this.orders)
            {
                cmd.ExecuteCommand();
            }
        }
    }
}
