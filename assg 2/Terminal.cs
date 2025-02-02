using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Terminal
{
    private string terminalName;
    public string TerminalName
    {
        get { return terminalName; }
        private set { terminalName = value; }
    }

    private Dictionary<string, Airline> airlines;
    public Dictionary<string, Airline> Airlines
    {
        get { return airlines; }
        private set { airlines = value; }
    }

    private Dictionary<string, Flight> flights;
    public Dictionary<string, Flight> Flights
    {
        get { return flights; }
        private set { flights = value; }
    }

    private Dictionary<string, BoardingGate> boardingGates;
    public Dictionary<string, BoardingGate> BoardingGates
    {
        get { return boardingGates; }
        private set { boardingGates = value; }
    }

    private Dictionary<string, double> gateFees;
    public Dictionary<string, double> GateFees
    {
        get { return gateFees; }
        private set { gateFees = value; }
    }

    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
        Airlines = new Dictionary<string, Airline>();
        Flights = new Dictionary<string, Flight>();
        BoardingGates = new Dictionary<string, BoardingGate>();
        GateFees = new Dictionary<string, double>();
    }

    public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, double> gateFees)
    {
        TerminalName = terminalName;
        Airlines = airlines ?? new Dictionary<string, Airline>();
        Flights = flights ?? new Dictionary<string, Flight>();
        BoardingGates = boardingGates ?? new Dictionary<string, BoardingGate>();
        GateFees = gateFees ?? new Dictionary<string, double>();
    }

    public bool AddAirline(Airline airline)
    {
        if (airline == null || Airlines.ContainsKey(airline.Name))
        {
            return false;
        }
        Airlines.Add(airline.Name, airline);
        return true;
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        if (boardingGate == null || BoardingGates.ContainsKey(boardingGate.GateName))
        {
            return false;
        }
        BoardingGates.Add(boardingGate.GateName, boardingGate);
        return true;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        if (flight == null)
        {
            return null;
        }

        foreach (var airline in airlines.Values)
        {
            if (airline.Flights.ContainsKey(flight.FlightNumber))
            {
                return airline;
            }
        }
        return null; // No matching airline found
    }

    public void PrintAirlineFees()
    {
        foreach (var kvp in GateFees)
        {
            Console.WriteLine($"Airline: {kvp.Key}, Fee: ${kvp.Value}");
        }
    }

    public override string ToString()
    {
        return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Flights: {Flights.Count}, Boarding Gates: {BoardingGates.Count}";
    }
}


