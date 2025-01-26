//==========================================================
// Student Number	: S10267479E
// Student Name	: Tan Si Ming Scott
// Partner Name	: Lim Hong Sian
//==========================================================


class Program
{
    static void Main(string[] args)
    {
        var airlines = LoadAirlines("airlines.csv");
        var gates = LoadGates("gates.csv");
        var flights = LoadFlights("flights.csv", airlines, gates);

        Console.WriteLine("Airlines Loaded:");
        foreach (var airline in airlines)
        {
            Console.WriteLine(airline);
            foreach (var flight in airline.Flights)
            {
                Console.WriteLine($"   - {flight}");
            }
        }

        Console.WriteLine("\nBoarding Gates Loaded:");
        foreach (var gate in gates)
        {
            Console.WriteLine(gate);
        }
    }

    static List<Airline> LoadAirlines(string filePath)
    {
        var airlines = new List<Airline>();
        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string code = parts[0].Trim();
                    string name = parts[1].Trim();
                    airlines.Add(new Airline(name, code));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading airlines: {ex.Message}");
        }
        return airlines;
    }

    static List<Gate> LoadGates(string filePath)
    {
        var gates = new List<Gate>();
        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string gateNumber = parts[0].Trim();
                    string terminal = parts[1].Trim();
                    gates.Add(new Gate(gateNumber, terminal));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading gates: {ex.Message}");
        }
        return gates;
    }

    static List<Flight> LoadFlights(string filePath, List<Airline> airlines, List<Gate> gates)
    {
        var flights = new List<Flight>();
        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    string flightNumber = parts[0].Trim();
                    string origin = parts[1].Trim();
                    string destination = parts[2].Trim();
                    DateTime expectedTime;
                    if (!DateTime.TryParseExact(parts[3].Trim(), "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                        continue;

                    string specialRequestCode = parts[4].Trim();
                    var gate = GetGateForFlight(flightNumber, gates);
                    var airline = GetAirlineForFlight(flightNumber, airlines);

                    if (gate != null && airline != null)
                    {
                        Flight flight = new NORMFlight(flightNumber, origin, destination, expectedTime, specialRequestCode, gate);
                        airline.AddFlight(flight);
                        flights.Add(flight);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights: {ex.Message}");
        }
        return flights;
    }

    static Gate GetGateForFlight(string flightNumber, List<Gate> gates)
    {
        return gates.FirstOrDefault();
    }

    static Airline GetAirlineForFlight(string flightNumber, List<Airline> airlines)
    {
        return airlines.FirstOrDefault();
    }
}

