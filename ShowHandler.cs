﻿public static class ShowHandler
{
    public const string FileName = "shows.json";
    private static int LatestShowID;
    public static List<Show> Shows;

    static ShowHandler()
    {
        Shows = Initializer.GetShowObjects();
        if (Shows.Count > 0)
            LatestShowID = Shows.MaxBy(sm => sm.Id)!.Id;
        else
            LatestShowID = 0;
    }

    public static void EditShowSchedule()
    {
        List<string> menuOptions = new() { "View/Remove shows  ", "Add show", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Show Schedule\n\nSelect an option:", menuOptions);
            switch (index)
            {
                case 0:
                    RemoveShow();
                    break;
                case 1:
                    AddShow();
                    break;
                case 2:
                    inMenu = false;
                    break;
                default:
                    break;
            }
        }
    }

    public static Show? GetShowById(int id)
    {
        foreach (var show in Shows)
            if (show.Id == id)
                return show;
        return null;
    }

    public static List<Show> GetShowsByDate(DateTime date)
    {
        List<Show> shows = new();
        foreach (var show in Shows)
        {
            if (show.DateAndTime.Date == date.Date)
                shows.Add(show);
        }
        return shows;
    }

    public static void AddShow()
    {
        bool inMenu = true;
        while (inMenu)
        {
            // Movie Selection
            List<string> movieTitles = MovieHandler.GetMovieTitles();
            int index = Menu.Start("Show Schedule\n\nSelect a movie:", movieTitles);
            if (index == movieTitles.Count)  // If user presses left arrow key leave current while loop
                break;
            Movie movie = MovieHandler.Movies[index];

            // Date selection
            List<string> dates = CreateDatesList(14);
            index = Menu.Start("Show Schedule\n\nSelect a date:", dates);
            if (index == dates.Count)  // If user presses left arrow key leave current while loop
                break;

            // Time selection
            string dateString = dates[index];
            DateTime selectedTime = TimeSelection(movie, dateString);

            Show show = new(movie, selectedTime) { Id = LatestShowID += 1 };

            // Confirm or cancel selection
            string menuHeader = $"Show Schedule\n\nMovie: {movie.Title}\nDate: {show.DateString}\nTime: {show.StartTimeString} - {show.EndTimeString}\n\nAdd this show to the schedule:";
            int selection = ConfirmSelection(show, movie, menuHeader);
            if (!(selection == 0))
            {
                break;
            }
            
            Shows.Add(show);
            JSONMethods.WriteToJSON(Shows, FileName);
            
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe show has been added");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            
            Console.ReadKey();
            break;
        }
    }

    public static void RemoveShow()
    {
        List<string> dates = GetAllDates();
        dates.Add("Back");
        if (dates.Count <= 1)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nThere are currently no shows scheduled");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\nPress any key to go back");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Show Schedule\n\nSelect a date to see the shows for that day:", dates);
            if (index == dates.Count || index == dates.Count - 1)
            {
                return;
            }

            string dateString = dates[index];
            DateTime selectedDate = DateTime.Parse(dateString);
            List<Show> moviesForDate = GetShowsByDate(selectedDate);
            List<string> movieMenuStrings = CreateListMovieStrings(moviesForDate);

            index = Menu.Start($"Show Schedule\n\nShows on {dateString}:", movieMenuStrings);
            if (index == movieMenuStrings.Count || index == movieMenuStrings.Count - 1)
            {
                continue;
            }

            Show show = moviesForDate[index];
            Movie movie = MovieHandler.GetMovieById(show.MovieId)!;

            // Confirm or cancel selection
            string menuHeader = $"Show Schedule\n\nMovie: {movie.Title}\nDate: {show.DateString}\nTime: {show.StartTimeString} - {show.EndTimeString}\n\nRemove this show from the schedule:";
            int selection = ConfirmSelection(show, movie, menuHeader);
            if (!(selection == 0))
            {
                break;
            }

            Shows.Remove(show);
            JSONMethods.WriteToJSON(Shows, FileName);

            // Confirmation message
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe show has been removed");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            Console.ReadKey();

            return;
        }

    }

    private static List<string> CreateDatesList(int count)
    {
        List<string> dates = new();
        DateTime date = DateTime.Now;
        for (int i = 0; i < count; i++)
        {
            dates.Add(date.ToString("dd-MM-yyyy"));
            date = date.AddDays(1);
        }
        return dates;
    }

    private static int ConfirmSelection(Show show, Movie movie, string menuHeader)
    {
        List<string> menuOptions = new() { "Confirm Selection", "Cancel" };
        return Menu.Start(menuHeader, menuOptions);
    }

    private static DateTime TimeSelection(Movie movieToAdd, string dateString)
    {
        DateTime resultDateTime = DateTime.Now;
        bool correctInput = false;
        while (!(correctInput))
        {
            Console.Clear();
            Console.CursorVisible = true;
            DisplayAsciiArt.Header();
            Console.WriteLine($"Show Schedule\n\nMovie: {movieToAdd.Title}\nDate: {dateString}");
            Console.Write("\nStart time of the movie (HH:mm): ");
            string? timeString = Console.ReadLine();
            Console.CursorVisible = false;

            string combinedDateTime = dateString + " " + timeString;
            if (DateTime.TryParseExact(combinedDateTime, "dd-MM-yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out resultDateTime))
            {
                correctInput = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIncorrect input, press any key to try again.");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        return resultDateTime;
    }

    public static Show? SelectShowFromSchedule()
    {
        bool inMenu = true;
        while (inMenu)
        {
            List<string> dates = GetDatesOfFutureShows();
            if (dates.Count == 0)
            {
                List<string> menuOption = new() { "Back" };
                Menu.Start("Show Schedule\n\nThere are no movies scheduled at the moment, please come back later.\n", menuOption);

                return null;
            }

            // Date selection
            dates.Add("Back");
            int index = Menu.Start("Show Schedule\n\nSelect a date:", dates);
            if (index == dates.Count || index == dates.Count - 1)
                break;

            string dateString = dates[index];
            DateTime date = DateTime.Parse(dateString);

            List<Show> moviesForDate = GetShowsByDate(date);

            // Create list of formatted strings to display to the user
            List<string> movieMenuString = CreateListMovieStrings(moviesForDate);

            index = Menu.Start($"Show Schedule\n\nShows on {dateString}:", movieMenuString);
            if (index == movieMenuString.Count || index == movieMenuString.Count - 1)
                continue;

            // Confirm Selection
            Show show = moviesForDate[index];
            Movie movie = MovieHandler.GetMovieById(show.Id)!;
            string confirmationMessage = $"Show Schedule\n\nMake a reservation for '{movie.Title}' on {show.DateString}:";
            int selection = ConfirmSelection(show, movie, confirmationMessage);
            if (!(selection == 0))
            {
                break;
            }

            return moviesForDate[index];
        }
        return null;
    }

    public static List<string> GetDatesOfFutureShows()
    {
        List<DateTime> dates = new();
        foreach (var _show in ShowHandler.Shows)
        {
            DateTime date = _show.DateAndTime;
            if (date.Date >= DateTime.Today && !dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static List<string> CreateListMovieStrings(List<Show> shows)
    // Creates list of formatted strings: Start time - end time : Title
    {
        List<string> movieMenuStrings = new();
        foreach (var _show in shows)
        {
            Movie movie = MovieHandler.GetMovieById(_show.MovieId)!;
            movieMenuStrings.Add($"{_show.StartTimeString} - {_show.EndTimeString}: {movie.Title}  ");
        }
        movieMenuStrings.Add("Back");
        return movieMenuStrings;
    }

    public static List<string> GetAllDates()
    {
        List<DateTime> dates = new();
        foreach (var _show in Shows)
        {
            DateTime date = _show.DateAndTime;
            if (!dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }
}