﻿using System.Net.Sockets;

public class CheckOutHandler
{
    public const string FileName = "Datasources/revenuePerShow.json";
    public static List<Revenue> Revenues;

    static CheckOutHandler()
    {
        Revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();
    }

    public static void AddShowToRevenue()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>(ShowHandler.FileName).ToList();
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

        foreach (var show in shows)
        {
            bool showExists = false;
            foreach (var revenue in revenues)
            {
                if (revenue.ShowId == show.Id)
                {
                    showExists = true;
                    break;
                }
            }

            if (!showExists)
            {
                double totalRevenueUpToNow = GetTotalRevenueUpToNow(show.Id);
                int month = show.DateAndTime.Month;

                Movie? movie = MovieHandler.GetMovieById(show.MovieId)!;
                Revenue? revenue = new Revenue(show.Id, movie.Title, totalRevenueUpToNow, month);

                revenues.Add(revenue);
                JSONMethods.WriteToJSON(revenues, FileName);
            }
        }
    }

    public static double GetTotalRevenueUpToNow(int showId)
    {
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>(TicketHandler.FileName).ToList();
        double revenue = 0;

        foreach (var ticket in tickets)
        {
            if (ticket.ShowId == showId)
                revenue += ticket.Price;
        }
        return revenue;
    }

    public static void AddToExistingRevenue(int showId, double moneyAdded)
    {
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

        foreach (var revenue in revenues)
        {
            if (revenue.ShowId == showId)
            {
                revenue.TotalRevenue += moneyAdded;
                break;
            }
        }
        JSONMethods.WriteToJSON(revenues, FileName);
    }

    public static void CheckOut()
    {
        bool checkOut = true;
        bool confirm = false;

        Console.CursorVisible = true;
        while (checkOut)
        {
            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please enter your credit card number:\nEXAMPLE: 4321-2432-2432-3424");
            Console.ForegroundColor = ConsoleColor.Blue;
            string creditCardInput = Console.ReadLine();
            Console.ResetColor();
            if (creditCardInput.Length != 19)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Credit card does NOT exist!\nPlease try again\nEnter the following format including the '-': XXXX-XXXX-XXXX-XXXX");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.CursorVisible = true;
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please input the expiration date:\nRequired format: MM/YY, Example: 02/25");
            Console.ForegroundColor = ConsoleColor.Blue;
            string experationCodeInput = Console.ReadLine();
            if (experationCodeInput.Length != 5)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect format, please try again.\nEnter the following format including the '/': XX/XX");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.CursorVisible = true;
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please input the CVC code (3 numbers on the back of the card):\nEXAMPLE: 454");
            Console.ForegroundColor = ConsoleColor.Blue;
            string cvc = Console.ReadLine();
            Console.ResetColor();
            if (cvc.Length != 3)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Card Verification Code, please use the correct format (e.g.: 454)");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.Clear();
                Console.CursorVisible = true;
                continue;
            }
            
            Console.CursorVisible = false;
            confirm = true;
            while (confirm == true)
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();
                Console.WriteLine($"Please confirm the following credit card details:\n\nCredit card number: {creditCardInput}\nExpiration date: {experationCodeInput}\nCVC: {cvc}\n\nIs this correct? (Y/N)\n");
                ConsoleKey pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.Y)
                {
                    Console.Clear();
                    Console.ResetColor();
                    DisplayAsciiArt.Header();
                    AdHandler.DisplaySnacks();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tickets successfully booked!\n");
                    Console.ResetColor();
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                else if (pressedKey == ConsoleKey.N)
                {
                    confirm = false;
                    break;
                }
            }
        }
    }
}