using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Department
    {
        private string name = "";
        private int id;

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}
