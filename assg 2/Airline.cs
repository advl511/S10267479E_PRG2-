using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Airline
{
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string code;
    public string Code
    {
        get { return code; }
        set { code = value; }
    }

    private Dictionary<string, Flight> flights;
    public Dictionary<string, Flight> Flights
    {
        get { return flights; }
        set { flights = value; }
    }

    public Airline(string name, string code, Dictionary<string, Flight> flights)
    {
        Name = name;
        Code = code;
        Flights = flights;
    }
    public bool AddFlight(Flight)
    {
        Flights.Add(name, code);
        return true;
    }
    public double CalculateFees()
    {
        double fees = 0;
        Console.WriteLine("placeholder");
        return fees;
    }
    public bool RemoveFlight(Flight)
    {
        Flights.Remove(name);
        return true;
    }
    public override string ToString()
    {
        return "Airline; " + Name + "(" + Code + ")";
    }


}
