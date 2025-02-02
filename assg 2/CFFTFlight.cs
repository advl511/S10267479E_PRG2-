using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CFFTFlight : Flight
{
    public double RequestFee { get; set; }

    public CFFTFlight(string fn, string o, string des, DateTime et, string s, double requestFee) : base(fn, o, des, et, s)
    {
        RequestFee = RequestFee;
    }

    public override double CalculateFees()
    {
        double fees = 0;
        if (Destination == "Singapore(SIN)")
        {
            fees += 500;
        }
        else if (Origin == "Singapore(SIN)")
        {
            fees += 800;
        }
        return fees + RequestFee + 300;
    }

    public override string ToString()
    {
        return $"CFFTFlight - {base.ToString()}, Request Fee: {RequestFee}";
    }
}
