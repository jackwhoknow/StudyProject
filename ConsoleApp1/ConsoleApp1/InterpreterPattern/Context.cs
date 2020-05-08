using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class ExpressionContext
    {
        private string input;
        private string output;

        public string Input { get => input; set => input = value; }
        public string Output { get => output; set => output = value; }
    }
}
