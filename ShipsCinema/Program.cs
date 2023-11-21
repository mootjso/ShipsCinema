public class Program
{
    private static void Main()
    {
        User? loggedInUser = null;
        while (true)
        {
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };

            while (loggedInUser == null)
            {
                DisplayAsciiArt.Standby();

                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:
                        loggedInUser = LoginHandler.LogIn();

                        if (loggedInUser.IsAdmin)
                        {
                            AdminHandler.StartMenu(loggedInUser);
                            loggedInUser = null;
                        }
                        break;
                    case 1:
                        loggedInUser = LoginHandler.Register();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\nThank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\nWe hope to see you soon!");
                        Thread.Sleep(1500);
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }

            List<string> menuOptionsLoggedIn = new() { "Current Movies", "Show Schedule", "My Reservations", "Log Out" };
            while (loggedInUser != null)
            {
                menuText = $"Hello, {loggedInUser.FirstName} {loggedInUser.LastName}\n";
                int selection = Menu.Start(menuText, menuOptionsLoggedIn);
                switch (selection)
                {
                    case 0:
                        Console.Clear();
                        MovieHandler.ViewCurrentMovies();
                        break;
                    case 1:
                        Console.Clear();
                        Show? selectedShow = ShowHandler.SelectShowFromSchedule();
                        if (selectedShow is null)
                            continue;

                        var theater = TheaterHandler.CreateTheater(selectedShow);

                        string ReservationId = ReservationHandler.GetReservationID();

                        List<Ticket>? tickets = TheaterHandler.SelectSeats(loggedInUser, theater, ReservationId);
                        if (tickets is null)
                            continue;
                        Console.WriteLine("Your Reservation code is: " + ReservationId);
                        Console.WriteLine("Press any button to continue");
                        Console.ReadLine();
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\nCHECKOUT FUNCTIONALITY NOT IMPLEMENTED\n\nPRESS ANY KEY TO GO BACK");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        ReservationHandler.AddTicketsToReservations();
                        ReservationHandler.GetReservationsByUser(loggedInUser);
                        Console.WriteLine("PRESS ANY KEY TO GO BACK");
                        Console.ReadLine();
                        break;
                    case 3:
                        loggedInUser = null;
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\nThank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\nWe hope to sea you soon!");
                        Thread.Sleep(1500);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}