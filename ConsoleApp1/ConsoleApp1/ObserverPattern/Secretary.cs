using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Secretary:Subject
    {
        public event EventHandler Update;
        public string SubjectState
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }
        private string action;
        public void Notify()
        {
            Update();
        }
    }
}
