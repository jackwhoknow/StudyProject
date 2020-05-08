using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    abstract class PersonBuilder
    {
        protected Graphics g;
        protected Pen p;
        public PersonBuilder(Graphics g,Pen pen)
        {
            this.g = g;
            this.p = pen;
        }
        public abstract void BuildHead();
        public abstract void BuildBody();
        public abstract void BuildLeftHand();
        public abstract void BuildRightHand();
        public abstract void BuildLeftLeg();
        public abstract void BuildRightLeg();
    }
}
