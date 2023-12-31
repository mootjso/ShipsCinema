﻿public class User : HasID
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }

    public User(int id, string firstName, string lastName, string email, string password, string phoneNumber, bool isAdmin = false)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        IsAdmin = isAdmin;
    }

    public override string ToString()
    {
        string admin = IsAdmin ? "Admin" : "User";
        return $"{admin.PadRight(6)} | {FirstName.PadRight(15).Substring(0, 15)} | {LastName.PadRight(15).Substring(0, 15)} | {Email.PadRight(22).Substring(0, 22)} | {PhoneNumber}  ";
    }
}