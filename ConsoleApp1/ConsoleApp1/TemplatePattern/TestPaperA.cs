using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class TestPaperA: TestPaper
    {
        public override string Answer1()
        {
            return "B";
        }
        public override string Answer2()
        {
            return "C";
        }
        public override string Answer3()
        {
            return "D";
        }
    }
}
