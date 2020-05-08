using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{ 
    class PersonDirector
    {
        private PersonBuilder pb;
        public PersonDirector(PersonBuilder pb)
        {
            this.pb = pb;
        }
        public void CreatePerson()
        {
            pb.BuildHead();
            pb.BuildBody();
            pb.BuildLeftHand();
            pb.BuildRightHand();
            pb.BuildLeftLeg();
            pb.BuildRightLeg();
        }
    }
}
