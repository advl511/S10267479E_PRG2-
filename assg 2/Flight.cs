﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class Flight:Airline
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }


    public Flight(string fn, string o, string des, DateTime et, string s)
    {
        FlightNumber = fn;
        Origin = o;
        Destination = des;
        ExpectedTime = et;
        Status = s;
    }

    public abstract double CalculateFees();

    public override string ToString()
    {
        return $"Flight: {base.ToString()}";
    }
    
   
}



