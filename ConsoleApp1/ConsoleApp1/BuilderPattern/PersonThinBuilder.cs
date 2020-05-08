using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class PersonThinBuilder : PersonBuilder
    {
        public PersonThinBuilder(Graphics g,Pen p):base(g,p)
        {
        }
        public override void BuildBody()
        {
            g.DrawRectangle(p, 60, 50, 10, 50);
        }

        public override void BuildHead()
        {
            g.DrawEllipse(p, 50, 20, 30, 30); 
        }

        public override void BuildLeftHand()
        {
            g.DrawLine(p,60,50,40,100);
        }

        public override void BuildLeftLeg()
        {
            g.DrawLine(p, 60, 100, 45, 150);
        }

        public override void BuildRightHand()
        {
            g.DrawLine(p, 60, 50, 40, 100);
        }

        public override void BuildRightLeg()
        {
            g.DrawLine(p, 70, 100, 85, 150);
        }
    }
}
