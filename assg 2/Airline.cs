//==========================================================
// Student Number	: S10267479E
// Student Name	: Tan Si Ming Scott
// Partner Name	: Lim Hong Sian
//==========================================================


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

    public Airline() { }
    public Airline(string name, string code)
    {
        Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        Flights = flights;
        Name = name;
        Code = code;
    }
    public bool AddFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            return false;
        }
        else
        {
            Flights.Add(flight.FlightNumber, flight);
            return true;
        }
    }
    public double CalculateFees()
    {
        double fees = 0;
        Console.WriteLine("placeholder");
        return fees;
    }
    public bool RemoveFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Remove(flight.FlightNumber);
            return true;
        }
        else
        {
            return false;
        }


    }
}
