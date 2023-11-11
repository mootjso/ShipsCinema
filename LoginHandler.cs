using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LoginHandler
{
    private static List<User> users;
    private static int lastUserId;
    public static User loggedInUser { get; private set; }

    public static bool LogIn()
    {
        Console.CursorVisible = true;
        LoadUsers();
        bool login = true;

        while (login)
        {
            Console.CursorVisible = true;
            Console.Clear();
            // DisplayAsciiArt.Header();
            // AdHandler.DisplaySnacks();

            Console.WriteLine("Login to your account\n");

            Console.Write("E-mailadres: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = GetMaskedPassword();

            bool userLogIn = false;
            bool accountExists = false;

            foreach (var user in users)
            {
                if (user.Email == username)
                {
                    accountExists = true;

                    if (user.Password == password)
                    {
                        userLogIn = true;
                        loggedInUser = user;
                        login = false;
                    }
                }
            }

            if (accountExists)
            {
                if (userLogIn)
                {
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Login successful, press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect password, press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
            else
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("E-mailadres does not exist, press any key to continue");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        return false;
    }

    public static bool Register()
    {
        LoadUsers();
        bool makeAccount = true;

        // AdHandler.DisplaySnacks();
        while (makeAccount)
        {
            Console.Clear();
            // DisplayAsciiArt.Header();
            // AdHandler.DisplaySnacks();

            Console.WriteLine("Register new account\n");

            Console.CursorVisible = true;
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            string email;
            bool emailExists;
            do
            {
                Console.Clear();
                // DisplayAsciiArt.Header();
                // AdHandler.DisplaySnacks();

                Console.WriteLine("Register new account\n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Phone Number: {phoneNumber}");

                Console.Write("Email: ");
                email = Console.ReadLine();
                emailExists = users.Any(user => user.Email == email);

                if (emailExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Email is already registered, please choose a different email, press any key to continue");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }

            } while (emailExists);

            string password;
            bool validPassword;
            PasswordValidator validator = new PasswordValidator();

            do
            {
                Console.Clear();
                // DisplayAsciiArt.Header();
                // AdHandler.DisplaySnacks();

                Console.WriteLine("Register new account\n");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Phone Number: {phoneNumber}");
                Console.WriteLine($"Email: {email}");

                Console.WriteLine("\nPassword requirements: \n-Between 6 and 13 characters\n-1 Uppercase letter\n-1 Lowercase letter\n-1 Digit");
                Console.Write("\nPassword: ");
                password = GetMaskedPassword();
                validPassword = validator.IsValid(password);
                if (!validPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password does not meet the requirements, press any key to try again");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }
            } while (!validPassword);

            string confirmPassword;
            do
            {
                Console.Write("Confirm Password: ");
                confirmPassword = GetMaskedPassword();

                if (confirmPassword != password)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Passwords do not match, press any key to try again");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.CursorVisible = true;
                }

            } while (confirmPassword != password);

            var newUser = new User(++lastUserId, firstName, lastName, email, password, phoneNumber);
            users.Add(newUser);
            loggedInUser = newUser;
            SaveUsers();

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Account {email}, has been created, press any key to continue");
            Console.ResetColor();
            Console.ReadKey(true);
            makeAccount = false;

            return true; // Registration successful, return true
        }

        return false; // Registration failed, return false
    }

    public static void LoadUsers()
    {
        string filename = "UserAccounts.json";

        if (File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            users = JsonConvert.DeserializeObject<List<User>>(json);
            lastUserId = users.Max(u => u.Id);
        }
        else
        {
            users = new List<User>();
            lastUserId = 0;
        }
    }

    public static void SaveUsers()
    {
        string filename = "UserAccounts.json";
        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    private static string GetMaskedPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignore any key that is not a printable ASCII character or Enter
            if (char.IsLetterOrDigit(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsPunctuation(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after Enter is pressed
        return password;
    }
}
