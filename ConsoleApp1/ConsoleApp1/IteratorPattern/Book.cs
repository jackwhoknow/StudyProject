using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Book
    {
        private string name;
        public Book(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
