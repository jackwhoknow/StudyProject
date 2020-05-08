using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class UnitedNationSecurityCouncil : UniteNations
    {
        private USA colleague1;
        private Iraq colleague2;

        internal USA Colleague1 { set => colleague1 = value; }
        internal Iraq Colleague2 { set => colleague2 = value; }

        public override void Declare(string message, Country country)
        {
            if (country== colleague1)
            {
                colleague2.GetMessage(message);
            }
            else
            {
                colleague1.GetMessage(message);
            }
        }
    }
}
