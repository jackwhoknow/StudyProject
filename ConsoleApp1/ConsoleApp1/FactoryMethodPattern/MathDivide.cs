﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class MathDivide:MathOperation
    {
        public override double GetResult()
        {
            return Number1/Number2;
        }
    }
}
