//==========================================================
// Student Number	: S10267479E
// Student Name	: Tan Si Ming Scott
// Partner Name	: Lim Hong Sian
//==========================================================

class Program
{
    static Terminal terminal = new Terminal("Terminal 5");
    static void Main(string[] args)
    {

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

                    break;
                case "2":

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
}

