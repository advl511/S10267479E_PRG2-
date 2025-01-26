using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardingGate
{
    private string gateName;
    public string GateName
    {
        get { return gateName; }
        set { gateName = value; }
    }

    private bool supportsCFFT;
    public bool SupportsCFFT
    {
        get { return supportsCFFT; }
        set { supportsCFFT = value; }
    }

    private bool supportsDDJB;
    public bool SupportsDDJB
    {
        get { return supportsDDJB; }
        set { supportsDDJB = value; }
    }

    private bool supportsLWTT;
    public bool SupportsLWTT
    {
        get { return supportsLWTT; }
        set { supportsLWTT = value; }
    }

    private Flight flight;
    public Flight Flight
    {
        get { return flight; }
        set { flight = value; }
    }

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
        Flight = flight;
    }
    public double CalculateFees()
    {
        double fees = 0;
        Console.WriteLine("placeholder");
        return fees;
    }
    public string ToString()
    {
        return string.Format(null);
    }

}