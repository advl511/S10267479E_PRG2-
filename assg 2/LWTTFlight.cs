﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LWTTFlight : Flight
{
    public double RequestFee { get; set; }

    public LWTTFlight(string fn, string o, string des, DateTime et, string s, double requestFee) : base(fn, o, des, et, s)
    {
        requestFee = 500;
    }

    public override double CalculateFees()
    {
        double fees = 0;
        if (Destination == "Singapore(SIN)")
        {
            fees += 500;
        } else if (Origin == "Singapore(SIN)")
        {
            fees += 800;
        }
        return RequestFee;
    }

    public override string ToString()
    {
        return $"LWTTFlight: {base.ToString()}";
    }
}

