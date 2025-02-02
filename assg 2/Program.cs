//==========================================================
// Student Number	: S10267479E
// Student Name	: Tan Si Ming Scott
// Partner Name	: Lim Hong Sian
//==========================================================

using System.Diagnostics.Metrics;

class Program
{
    static Terminal terminal = new Terminal("Terminal 5");

    static void Main(string[] args)
    {
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

    static void LoadFlightsFromFile(string fileName)
    {
        string filePath = Path.Combine(fileName);
        Console.WriteLine($"Looking for file at: {filePath}");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist!");
            return;
        }

        // File exists, let's try reading it.
        Console.WriteLine("File found. Attempting to read...");

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Check the first few lines of the file to ensure it's being read
                string line;
                int lineCount = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;
                    if (lineCount <= 5)  // Only print the first 5 lines for debugging
                        Console.WriteLine($"Line {lineCount}: {line}");

                    // Skip the header line
                    if (lineCount == 1) continue;

                    string[] values = line.Split(',');

                    // Ensure there are enough values in the line
                    if (values.Length < 4)
                    {
                        Console.WriteLine("Skipping invalid line: " + line);
                        continue;
                    }

                    // Extract flight details
                    string flightNumber = values[0].Trim();
                    string origin = values[1].Trim();
                    string destination = values[2].Trim();
                    string expectedTimeStr = values[3].Trim();
                    string specialRequestCode = values.Length > 4 ? values[4].Trim() : "";

                    // Parse expected time
                    DateTime expectedTime;
                    if (!DateTime.TryParseExact(expectedTimeStr, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out expectedTime))
                    {
                        Console.WriteLine($"Invalid time format for flight {flightNumber}: {expectedTimeStr}");
                        continue;
                    }

                    // Create flight object based on special request code
                    Flight flight = CreateFlight(flightNumber, origin, destination, expectedTime, specialRequestCode);

                    // Add flight to terminal's flights dictionary
                    if (flight != null)
                    {
                        terminal.Flights.Add(flightNumber, flight);
                        Console.WriteLine($"Loaded flight: {flightNumber}");
                    }
                }
            }

            Console.WriteLine("Flights loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights: {ex.Message}");
        }

        // After loading, check how many flights were loaded
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

        Console.WriteLine("List of Flights:");

        // Loop through all the flights in the terminal
        foreach (var flightEntry in terminal.Flights)
        {
            // Access the flight object
            Flight flight = flightEntry.Value;

            // Display basic information
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Airline Name: {flight.Name}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime.ToString("h:mm tt")}");
            Console.WriteLine(); // Empty line for separation between flights
        }
    }
}

    

