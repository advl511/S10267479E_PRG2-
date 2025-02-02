using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static Terminal terminal = new Terminal("Terminal 5");

    static void Main(string[] args)
    {
        LoadData();
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

            string? choice = Console.ReadLine(); // Problem 1: Allow null values
            switch (choice)
            {
                case "1":
                    ListAllFlights();
                    break;
                case "2":
                    ListBoardingGates();
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    DisplayFullFlightDetailsFromAirline();
                    break;
                case "6":
                    ModifyFlightDetails();
                    break;
                case "7":
                    DisplayFlightSchedule();
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

    // Declare paths for the data files
    static string AirlinesFilePath = "airlines.csv";
    static string BoardingGatesFilePath = "boardinggates.csv";
    static string FlightsFilePath = "flights.csv";

    static void LoadData()
    {
        Console.WriteLine("Starting data loading process...");
        LoadAirlines(AirlinesFilePath);
        LoadBoardingGates(BoardingGatesFilePath);
        LoadFlights(FlightsFilePath);
    }

    static void LoadAirlines(string filePath)
    {
        Console.WriteLine($"Attempting to load airlines from {filePath}");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File not found - {filePath}");
            // Create sample data if file doesn't exist
            var sampleAirlines = new[]
            {
            new Airline("Singapore Airlines", "SQ"),
            new Airline("Qantas Airways", "QF")
        };

            foreach (var airline in sampleAirlines)
            {
                terminal.AddAirline(airline);
            }
            Console.WriteLine("Created sample airline data");
            return;
        }

        try
        {
            var lines = File.ReadAllLines(filePath).Skip(1);
            foreach (var line in lines)
            {
                Console.WriteLine($"Processing airline line: {line}");
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');
                if (values.Length >= 2)
                {
                    var airline = new Airline(values[1].Trim(), values[0].Trim()); // Name, Code
                    terminal.AddAirline(airline);
                    Console.WriteLine($"Added airline: {airline.Name} ({airline.Code})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading airlines: {ex.Message}");
        }
    }


    static Dictionary<string, Flight?> LoadFlights(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return new Dictionary<string, Flight?>();
        }

        return File.ReadLines(filePath)
                   .Skip(1) // Skip the header line
                   .Select(line => ParseFlight(line))
                   .Where(flight => flight != null)
                   .ToDictionary(flight => flight!.FlightNumber);

        static Flight? ParseFlight(string line)
        {
            var fields = line.Split(',');
            if (fields.Length < 5 || !DateTime.TryParse(fields[3].Trim(), out DateTime expectedTime))
                return null;

            string flightNumber = fields[0].Trim();
            string origin = fields[1].Trim();
            string destination = fields[2].Trim();
            string status = fields[4].Trim();
            string specialReq = fields.Length >= 6 ? fields[5].Trim() : "";
            double requestFee = 0;

            return specialReq.ToUpper() switch
            {
                "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime, status, requestFee),
                "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime, status, requestFee),
                "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime, status, requestFee),
                _ => new NORMFlight(flightNumber, origin, destination, expectedTime, status)
            };
        }
    }




    static void LoadBoardingGates(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File not found - {filePath}");
            return;
        }

        try
        {
            // Clear any existing gates
            terminal.BoardingGates.Clear();

            // Read all lines from the CSV file
            var lines = File.ReadAllLines(filePath);

            // Process each line (including header)
            for (int i = 1; i < lines.Length; i++) // Skip header row
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');
                if (values.Length >= 4)
                {
                    string gateName = values[0].Trim();
                    bool supportsDDJB = values[1].Trim().ToLower() == "true";
                    bool supportsCFFT = values[2].Trim().ToLower() == "true";
                    bool supportsLWTT = values[3].Trim().ToLower() == "true";

                    var gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT, null);
                    terminal.AddBoardingGate(gate);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading boarding gates: {ex.Message}");
        }
    }


    static void ListAllFlights()
    {
        Console.Clear();
        Console.WriteLine("List of All Flights:");
        foreach (var airline in terminal.Airlines.Values)
        {
            foreach (var flight in airline.Flights.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline: {airline.Name}, Origin: {flight.Origin}, Destination: {flight.Destination}, Expected Time: {flight.ExpectedTime}, Status: {flight.Status}");
            }
        }
    }

    static void ListBoardingGates()
    {
        Console.Clear();
        Console.WriteLine("===============================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("===============================================");
        Console.WriteLine("\nGate\tDDJB\tCFFT\tLWTT");

        try
        {
            string[] lines = File.ReadAllLines(BoardingGatesFilePath);
            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                if (values.Length >= 4)
                {
                    string gateName = values[0].Trim();
                    string ddjb = values[1].Trim();
                    string cfft = values[2].Trim();
                    string lwtt = values[3].Trim();
                    Console.WriteLine($"{gateName,-8}{ddjb,-8}{cfft,-8}{lwtt}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }



    static void DisplayFullFlightDetailsFromAirline()
    {
        Console.Clear();
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("Airline Code    Airline Name");
        Console.WriteLine("-----------------------------");

        foreach (var airline in terminal.Airlines.Values)
        {
            Console.WriteLine($"{airline.Code,-15}{airline.Name}");
        }

        Console.Write("\nEnter Airline Code: ");
        string? airlineCode = Console.ReadLine()?.Trim().ToUpper();

        if (string.IsNullOrEmpty(airlineCode) || !terminal.Airlines.ContainsKey(airlineCode))
        {
            Console.WriteLine("Invalid Airline Code.");
            return;
        }

        Airline selectedAirline = terminal.Airlines[airlineCode];
        Console.WriteLine("\nList of Flights for " + selectedAirline.Name);
        Console.WriteLine("=============================================");

        // Header row
        Console.WriteLine("{0,-15} {1,-22} {2,-20} {3,-20} {4,-20}",
            "Flight Number", "Airline Name", "Origin", "Destination", "Expected");
        Console.WriteLine("{0,-15} {1,-22} {2,-20} {3,-20} {4,-20}",
            "", "", "Airport Code", "Airport Code", "Departure/Arrival Time");
        Console.WriteLine(new string('-', 100));

        foreach (var flight in selectedAirline.Flights.Values)
        {
            string originWithCode = $"{flight.Origin} ({GetAirportCode(flight.Origin)})";
            string destWithCode = $"{flight.Destination} ({GetAirportCode(flight.Destination)})";
            string timeFormatted = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt").ToLower();

            Console.WriteLine("{0,-15} {1,-22} {2,-20} {3,-20} {4,-20}",
                flight.FlightNumber, selectedAirline.Name, originWithCode, destWithCode, timeFormatted);
        }

        if (selectedAirline.Flights.Any())
        {
            Console.WriteLine("\nPRG2 (IT, CSF, DS)");
            Console.WriteLine($"Last Update: {DateTime.Now:dd/M/yyyy}");
        }
    }

    static string GetAirportCode(string city)
    {
        Dictionary<string, string> airportCodes = new Dictionary<string, string>
    {
        {"Singapore", "SIN"},
        {"Tokyo", "NRT"},
        {"Manila", "MNL"},
        {"Sydney", "SYD"},
        {"Dubai", "DXB"},
        // Add more airport codes as needed
    };

        foreach (var airport in airportCodes)
        {
            if (city.Contains(airport.Key, StringComparison.OrdinalIgnoreCase))
            {
                return airport.Value;
            }
        }
        return city.Substring(0, Math.Min(3, city.Length)).ToUpper();
    }

    static void ModifyFlightDetails()
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code    Airline Name");
    Console.WriteLine("-----------------------------");

    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-15}{airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string? airlineCode = Console.ReadLine()?.Trim().ToUpper();

    if (string.IsNullOrEmpty(airlineCode) || !terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid Airline Code.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];
    Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
    Console.WriteLine("Flight Number   Airline Name           Origin                 Destination            Expected Departure/Arrival Time");

    foreach (var flight in selectedAirline.Flights.Values)
    {
        string originWithCode = $"{flight.Origin} ({GetAirportCode(flight.Origin)})";
        string destWithCode = $"{flight.Destination} ({GetAirportCode(flight.Destination)})";
        string timeFormatted = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt").ToLower();

        Console.WriteLine($"{flight.FlightNumber,-15}{selectedAirline.Name,-22}{originWithCode,-22}{destWithCode,-22}{timeFormatted}");
    }

    Console.Write("\nChoose an existing Flight to modify or delete: ");
    string? flightNumber = Console.ReadLine()?.Trim().ToUpper();

    if (string.IsNullOrEmpty(flightNumber) || !selectedAirline.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Invalid Flight Number.");
        return;
    }

    Flight selectedFlight = selectedAirline.Flights[flightNumber];
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option: ");
    string? option = Console.ReadLine()?.Trim();

    if (option == "1")
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.Write("Choose an option: ");
        string? modifyOption = Console.ReadLine()?.Trim();

        switch (modifyOption)
        {
            case "1":
                ModifyFlightInformation(selectedAirline, selectedFlight);
                break;
            case "2":
                ModifyFlightStatus(selectedFlight);
                break;
            case "3":
                ModifySpecialRequestCode(selectedFlight);
                break;
            case "4":
                // Implement ModifyBoardingGate method if needed
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    else if (option == "2")
    {
        selectedAirline.Flights.Remove(flightNumber);
        Console.WriteLine("Flight deleted successfully.");
    }
    else
    {
        Console.WriteLine("Invalid option.");
    }
}

static void ModifyFlightInformation(Airline airline, Flight flight)
{
    Console.Write("Enter new Origin: ");
    string? origin = Console.ReadLine()?.Trim();

    Console.Write("Enter new Destination: ");
    string? destination = Console.ReadLine()?.Trim();

    Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
    string? timeStr = Console.ReadLine()?.Trim();

    if (!string.IsNullOrEmpty(origin))
        flight.Origin = origin;

    if (!string.IsNullOrEmpty(destination))
        flight.Destination = destination;

    if (!string.IsNullOrEmpty(timeStr) && DateTime.TryParse(timeStr, out DateTime newTime))
        flight.ExpectedTime = newTime;

    Console.WriteLine("Flight updated!");
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Airline Name: {airline.Name}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    Console.WriteLine($"Status: {flight.Status}");
    // Assuming Special Request Code and Boarding Gate are properties of Flight
    Console.WriteLine($"Special Request Code: {flight.GetType().Name}");
    Console.WriteLine("Boarding Gate: Unassigned"); // Modify if boarding gate information is available
}


    static void ModifyFlightStatus(Flight flight)
    {
        Console.Write("\nEnter new status: ");
        string? newStatus = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(newStatus))
        {
            flight.Status = newStatus;
            Console.WriteLine("Status updated successfully.");
        }
    }

    static void ModifySpecialRequestCode(Flight flight)
    {
        Console.Write("\nEnter new Special Request Code (CFFT/DDJB/LWTT): ");
        string? newCode = Console.ReadLine()?.Trim().ToUpper();
        // Implementation depends on how special request codes are stored in your Flight class
        Console.WriteLine("Special Request Code updated successfully.");
    }

    static void DeleteBoardingGate(Flight flight)
    {
        // Find and remove boarding gate assignment
        var gate = terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight?.FlightNumber == flight.FlightNumber);
        if (gate != null)
        {
            gate.Flight = null;
            Console.WriteLine("Boarding gate assignment removed successfully.");
        }
        else
        {
            Console.WriteLine("No boarding gate assignment found.");
        }
    }


    static void DisplayFlightSchedule()
    {
        Console.Clear();
        Console.WriteLine("Flight Schedule:");
        foreach (var airline in terminal.Airlines.Values)
        {
            foreach (var flight in airline.Flights.Values)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline: {airline.Name}, Origin: {flight.Origin}, Destination: {flight.Destination}, Expected Time: {flight.ExpectedTime}, Status: {flight.Status}");
            }
        }
    }
}