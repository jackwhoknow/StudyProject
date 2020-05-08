using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class SqlserverDepartment : IDepartment
    {
        public Department GetDepart(int id)
        {
            Console.WriteLine("在 Sql Server 中根据Id得到Department表中的一条记录");
            return null;
        }

        public void Insert(Department depart)
        {
            Console.WriteLine("在 Sql Server 中给Department表增加一条记录,名称：{0},ID：{1}", depart.Name, depart.Id);
        }
    }
}
