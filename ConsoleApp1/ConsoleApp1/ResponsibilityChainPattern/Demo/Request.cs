using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Request
    {
        private string requestType = "";
        private string requestContent = "";
        private int number;

        public string RequestType { get => requestType; set => requestType = value; }
        public string RequestContent { get => requestContent; set => requestContent = value; }
        public int Number { get => number; set => number = value; }
    }
}
