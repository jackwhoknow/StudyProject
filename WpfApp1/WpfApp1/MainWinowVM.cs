using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class MainWinowVM
    {
        private Person myPerson;
        private Animal myAnimal;

        public Person MyPerson
        {
            get
            {
                return myPerson;
            }
            set
            {
                myPerson = value;
            }
        }
        public Animal MyAnimal
        {
            get
            {
                return myAnimal;
            }
            set
            {
                myAnimal = value;
            }
        }
    }
}
