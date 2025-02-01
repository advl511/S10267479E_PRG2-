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

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.Write("\nPlease select your option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ListAllFlights(flights);
                    break;
                case "2":
                    ListBoardingGates(gates);
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    DisplayFlightDetails(airlines);
                    break;
                case "6":
                    ModifyOrDeleteFlight(airlines);
                    break;
                case "7":
                    break;
                case "0":
                    Console.WriteLine("Exiting program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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

    static void ListAllFlights(List<Flight> flights)
    {
        Console.WriteLine("\nAll Flights:");
        foreach (var flight in flights)
        {
            Console.WriteLine(flight);
        }
    }

    static void ListBoardingGates(List<Gate> gates)
    {
        Console.WriteLine("\nBoarding Gates:");
        foreach (var gate in gates)
        {
            Console.WriteLine(gate);
        }
    }


    static void DisplayBoardingGates(List<Gate> gates)
    {
        Console.WriteLine("\n==============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("==============================================");
        Console.WriteLine("Gate Name   DDJB   CFFT   LWTT");

        foreach (var gate in gates)
        {
            Console.WriteLine($"{gate.GateName,-10} {gate.DDJB,-5} {gate.CFFT,-5} {gate.LWTT,-5}");
        }
    }

    static void DisplayFlightDetails(List<Airline> airlines)
    {
        Console.WriteLine("\nAvailable Airlines:");
        foreach (var airline in airlines)
        {
            Console.WriteLine($"{airline.Code} - {airline.Name}");
        }
        Console.Write("Enter Airline Code: ");
        string airlineCode = Console.ReadLine().ToUpper();
        var selectedAirline = airlines.FirstOrDefault(a => a.Code == airlineCode);

        if (selectedAirline == null)
        {
            Console.WriteLine("Invalid airline code.");
            return;
        }

        Console.WriteLine("\nFlights for " + selectedAirline.Name + ":");
        foreach (var flight in selectedAirline.Flights)
        {
            Console.WriteLine(value: $"Flight {flight.FlightNumber}: {flight.Origin} -> {flight.Destination}");
        }

        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine();
        var selectedFlight = selectedAirline.Flights.FirstOrDefault(f => f.FlightNumber == flightNumber);

        if (selectedFlight == null)
        {
            Console.WriteLine("Invalid flight number.");
            return;
        }

        Console.WriteLine("\nFlight Details:");
        Console.WriteLine(selectedFlight);
    }

    static void ModifyOrDeleteFlight(List<Airline> airlines)
    {
        Console.Write("Enter Airline Code: ");
        string airlineCode = Console.ReadLine().ToUpper();
        var airline = airlines.FirstOrDefault(a => a.Code == airlineCode);
        if (airline == null)
        {
            Console.WriteLine("Invalid airline code.");
            return;
        }

        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine();
        var flight = airline.Flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
        if (flight == null)
        {
            Console.WriteLine("Invalid flight number.");
            return;
        }

        Console.WriteLine("[1] Modify Flight [2] Delete Flight");
        string choice = Console.ReadLine();
        if (choice == "1")
        {
            Console.Write("Enter new Origin: ");
            flight.Origin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            flight.Destination = Console.ReadLine();
            Console.WriteLine("Flight details updated.");
        }
        else if (choice == "2")
        {
            Console.Write("Confirm delete (Y/N): ");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                airline.Flights.Remove(flight);
                Console.WriteLine("Flight deleted.");
            }
        }
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

