using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class AccessUser : IUser
    {
        public User GetUser(int id)
        {
            Console.WriteLine("在 Access 中根据Id得到User表中的一条记录");
            return null;
        }

        public void Insert(User user)
        {
            Console.WriteLine("在 Access 中给User表增加一条记录,姓名：{0},ID：{1}", user.Name, user.Id);
        }
    }
}
