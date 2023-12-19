﻿public static class FinancialMenu
{
    public static void Start()
    {
        while (true) // Year selection
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            List<string> menuOptions = FinancialHandler.GetYears();
            menuOptions.Add("Back");
            int index = Menu.Start("Financial Overview\n\nSelect a year:", menuOptions, true);
            if (index == menuOptions.Count || index == menuOptions.Count - 1)
            {
                break;
            }
            string year = menuOptions[index];

            while (true) // Quarter selection
            {
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                menuOptions = new() { "First Quarter", "Second Quarter", "Third Quarter", "Fourth Quarter" };
                menuOptions.Add("Back");
                index = Menu.Start($"Financial Overview\n\nYear: {year}\nSelect a Quarter:", menuOptions, true);
                if (index == menuOptions.Count || index == menuOptions.Count - 1)
                {
                    break;
                }
                string quarterLong = menuOptions[index];
                string quarterShort = index == 0 ? "q1" : index == 1 ? "q2" : index == 2 ? "q3" : "q4";

                while (true)
                {
                    menuOptions = new() { "By theater", "By movie" };
                    menuOptions.Add("Back");
                    index = Menu.Start($"Financial Overview\n\nYear: {year}, {quarterLong}\nCreate a csv file with the financial data:", menuOptions, true);
                    if (index == menuOptions.Count || index == menuOptions.Count - 1)
                    {
                        break;
                    }

                    string infoBy = index == 0 ? "byTheater" : "byMovie";
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("Financial Overview");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n\nA new csv file has been created '{year}-{quarterShort}-{infoBy}.csv' in the folder FOLDERNAME");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.ResetColor();
                    return;
                }
            }
        }
    }
}