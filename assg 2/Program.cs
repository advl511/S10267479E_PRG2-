//==========================================================
// Student Number	: S10267479E
// Student Name	: Tan Si Ming Scott
// Partner Name	: Lim Hong Sian
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    private static Terminal terminal;

    static void Main(string[] args)
    {
        // Initialize the terminal
        terminal = new Terminal("Changi Airport Terminal 5");

        // Load flights at the start
        LoadFlightsFromFile("flights.csv");

        // Check if the flights have been loaded into the terminal
        Console.WriteLine($"Number of flights in terminal: {terminal.Flights.Count}");
        foreach (var flight in terminal.Flights)
        {
            Console.WriteLine($"Flight {flight.Key}: {flight.Value.FlightNumber}");
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
                    ListAllFlights();
                    break;
                case "2":
                    // Add functionality for Boarding Gates
                    break;
                case "3":
                    AssignBoardingGateToFlight();
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
    static Dictionary<string, Airline> airlineDictionary = new Dictionary<string, Airline>
{
    { "SQ", new Airline("Singapore Airlines", "SQ") },
    { "MH", new Airline("Malaysia Airlines", "MH") },
    { "JL", new Airline("Japan Airlines", "JL") },
    { "CX", new Airline("Cathay Pacific", "CX") },
    { "QF", new Airline("Qantas Airways", "QF") },
    { "TR", new Airline("AirAsia", "TR") },
    { "EK", new Airline("Emirates", "EK") },
    { "BA", new Airline("British Airways", "BA") }
};

    static void LoadFlightsFromFile(string fileName)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        Console.WriteLine($"Looking for file at: {filePath}");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist!");
            return;
        }
        Console.WriteLine("File found. Attempting to read...");
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                int lineCount = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;
                    if (lineCount <= 5)
                        Console.WriteLine($"Line {lineCount}: {line}");
                    if (lineCount == 1) continue;

                    string[] values = line.Split(',');
                    if (values.Length < 4)
                    {
                        Console.WriteLine("Skipping invalid line: " + line);
                        continue;
                    }

                    string fullFlightNumber = values[0].Trim();
                    string airlineCode = fullFlightNumber.Split(' ')[0];
                    string origin = values[1].Trim();
                    string destination = values[2].Trim();
                    string expectedTimeStr = values[3].Trim();
                    string specialRequestCode = values.Length > 4 ? values[4].Trim() : "";

                    // Retrieve the airline using the code from the global dictionary
                    Airline airline = airlineDictionary.ContainsKey(airlineCode) ? airlineDictionary[airlineCode] : new Airline("Unknown Airline", airlineCode);

                    DateTime expectedTime;
                    if (!DateTime.TryParseExact(expectedTimeStr, "h:mm tt",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out expectedTime))
                    {
                        Console.WriteLine($"Invalid time format for flight {fullFlightNumber}: {expectedTimeStr}");
                        continue;
                    }

                    Flight flight = CreateFlight(fullFlightNumber, origin, destination, expectedTime, specialRequestCode);

                    if (flight != null)
                    {
                        // Add the flight to the terminal
                        terminal.Flights.Add(fullFlightNumber, flight);
                        Console.WriteLine($"Loaded flight: {fullFlightNumber} - {airline.Name}");
                    }
                }
            }
            Console.WriteLine("Flights loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights: {ex.Message}");
        }
        Console.WriteLine($"Number of flights loaded: {terminal.Flights.Count}");
    }




    static Flight CreateFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string specialRequestCode)
    {
        Flight flight;
        switch (specialRequestCode)
        {
            case "CFFT":
                flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 150);
                break;
            case "DDJB":
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 300);
                break;
            case "LWTT":
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "Scheduled", 500); // Default request fee
                break;
            default:
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                break;
        }

        return flight;
    }

    static void ListAllFlights()
    {
        // Check if there are any flights in the terminal
        if (terminal.Flights.Count == 0)
        {
            Console.WriteLine("No flights available.");
            return;
        }

        Console.WriteLine("\nList of Flights:");
        Console.WriteLine("------------------------------------------------------------");

        // Print table header with optimized column widths
        Console.WriteLine($"{"FlightNum",-7}{"Airline",-12}{"Origin",-15}{"Dest.",-15}{"Time",-10}{"Status",-10}");
        Console.WriteLine("------------------------------------------------------------");

        // Loop through all flights in the terminal
        foreach (var flightEntry in terminal.Flights)
        {
            Flight flight = flightEntry.Value;

            // Retrieve the airline code directly from the flight (as part of the flight number)
            string airlineCode = flight.FlightNumber.Split(' ')[0];

            // Retrieve the airline using the code from the global dictionary
            Airline airline = airlineDictionary.ContainsKey(airlineCode) ? airlineDictionary[airlineCode] : new Airline("Unknown Airline", airlineCode);

            // Display flight details with optimized spacing and truncated fields
            Console.WriteLine($"{flight.FlightNumber,-7}" +
                            $"{TruncateString(airline.Name, 12),-12}" +
                            $"{TruncateString(flight.Origin, 15),-15}" +
                            $"{TruncateString(flight.Destination, 15),-15}" +
                            $"{flight.ExpectedTime.ToString("HH:mm"),-10}" +
                            $"{flight.Status,-10}");
        }

        Console.WriteLine("------------------------------------------------------------");
    }

    // Helper method to truncate strings that are too long
    private static string TruncateString(string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Length <= maxLength ? str : str.Substring(0, maxLength - 3) + "...";
    }

    private static Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();

    // Method to assign boarding gate
    static void AssignBoardingGateToFlight()
    {
        // Request for flight number
        Console.Write("\nEnter the Flight Number: ");
        string flightNumber = Console.ReadLine().Trim();

        // Check if the flight exists in the terminal
        if (!terminal.Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine("Flight not found.");
            return;
        }

        // Retrieve the selected flight
        Flight selectedFlight = terminal.Flights[flightNumber];

        // Display flight details
        Console.WriteLine($"\nFlight Information:");
        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:HH:mm}");
        Console.WriteLine($"Status: {selectedFlight.Status}");

        // Prompt user to select a boarding gate
        string boardingGateName;
        BoardingGate selectedGate = null;

        while (true)
        {
            Console.Write("\nEnter the Boarding Gate: ");
            boardingGateName = Console.ReadLine().Trim();

            // Check if the gate already has an assigned flight
            if (boardingGates.ContainsKey(boardingGateName))
            {
                Console.WriteLine("This Boarding Gate is already assigned to another flight. Please try again.");
            }
            else
            {
                // If it's not assigned, ask if the gate supports the current flight type
                Console.WriteLine("\nSelect Flight Type:");
                Console.WriteLine("1. CFFT");
                Console.WriteLine("2. DDJB");
                Console.WriteLine("3. LWTT");
                string flightTypeChoice = Console.ReadLine().Trim();

                bool isGateSupported = false;

                // Check if the gate supports the selected flight type
                switch (flightTypeChoice)
                {
                    case "1": // CFFT
                        isGateSupported = selectedGate.SupportsCFFT;
                        break;
                    case "2": // DDJB
                        isGateSupported = selectedGate.SupportsDDJB;
                        break;
                    case "3": // LWTT
                        isGateSupported = selectedGate.SupportsLWTT;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }

                if (isGateSupported)
                {
                    // Assign the boarding gate to the flight
                    selectedGate.Flight = selectedFlight;
                    boardingGates[boardingGateName] = selectedGate;
                    Console.WriteLine($"Boarding Gate {boardingGateName} successfully assigned to Flight {selectedFlight.FlightNumber}.");
                    break;
                }
                else
                {
                    Console.WriteLine("This boarding gate does not support the selected flight type. Please try again.");
                }
            }
        }

        // Ask if the user wants to update the flight status
        Console.Write("\nWould you like to update the flight status? (Y/N): ");
        string statusChoice = Console.ReadLine().Trim().ToUpper();

        if (statusChoice == "Y")
        {
            Console.WriteLine("\nSelect the new status:");
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");

            string statusOption = Console.ReadLine().Trim();
            switch (statusOption)
            {
                case "1":
                    selectedFlight.Status = "Delayed";
                    break;
                case "2":
                    selectedFlight.Status = "Boarding";
                    break;
                case "3":
                    selectedFlight.Status = "On Time";
                    break;
                default:
                    Console.WriteLine("Invalid option. The status will remain unchanged.");
                    break;
            }
        }
        else
        {
            // Default status is "On Time"
            selectedFlight.Status = "On Time";
        }

        Console.WriteLine($"Flight {selectedFlight.FlightNumber} status updated to: {selectedFlight.Status}");
        Console.WriteLine("Boarding Gate assignment successful.");
    }

}

