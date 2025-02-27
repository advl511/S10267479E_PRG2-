﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class NORMFlight : Flight
{
    public NORMFlight(string fn, string o, string des, DateTime et, string s) : base(fn, o, des, et, s) { }

    public override double CalculateFees()
    {
        return 500.0; 
    }

    public override string ToString()
    {
        return $"NORMFlight: {base.ToString()}";
    }
}

