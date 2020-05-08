using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class StringDisplay : AbstractDisplay
    {
        public string content = "";
        public StringDisplay(string content)
        {
            this.content = content;
        }
        public override void Close()
        {
            PrintLine();
        }

        public override void Open()
        {
            PrintLine();
        }

        public override void Print()
        {
            Console.WriteLine("|" + content + "|");
        }
        private void PrintLine()
        {
            Console.Write("+");
            for(int i=0;i<content.Length;i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("+");
        }
    }
}
