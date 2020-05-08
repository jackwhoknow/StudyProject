using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    public class Work
    {
        private State current;
        public Work()
        {
            this.current = new ForenoonState();
        }
        private double hour;

        public double Hour { get => hour; set => hour = value; }
        public bool TaskFinished { get => finished; set => finished = value; }

        private bool finished = false;

        public void SetState(State s)
        {
            current = s;
        }
        public void WriteProgram()
        {
            current.WriteProgram(this);
        }
    }
}
