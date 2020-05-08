using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class TestPaper
    {
        public void TestQuestion1()
        {
            Console.WriteLine("第一个问题的答案：A:1 B:2 C:3 D:4");
            Console.WriteLine("答案："+Answer1());
        }
        public void TestQuestion2()
        {
            Console.WriteLine("第二个问题的答案：A:1 B:2 C:3 D:4");
            Console.WriteLine("答案：" + Answer2());
        }
        public void TestQuestion3()
        {
            Console.WriteLine("第三个问题的答案：A:1 B:2 C:3 D:4");
            Console.WriteLine("答案：" + Answer3());
        }
        public virtual string Answer1()
        {
            return "";
        }
        public virtual string Answer2()
        {
            return "";
        }
        public virtual string Answer3()
        {
            return "";
        }
    }
}
