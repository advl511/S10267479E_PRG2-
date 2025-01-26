using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class LWTTFlight : Flight
{
    public double RequestFee { get; set; }

    public LWTTFlight(string fn, string o, string des, DateTime et, string s, double requestFee) : base(fn, o, des, et, s)
    {
        RequestFee = requestFee;
    }

    public override double CalculateFees()
    {
        return 500.0 + RequestFee + 500.0;
    }

    public override string ToString()
    {
        return $"LWTTFlight - {base.ToString()}, Request Fee: {RequestFee}";
    }

