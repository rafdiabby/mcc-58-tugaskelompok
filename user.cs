using System;

/// <summary>
/// Summary description for Class1
/// </summary>
class User
{
    public string lastName;
    protected string password;
    public string firstName;
    public string username;
    public bool isAdmin;

    public string Password
    {
        get { return password; }
        set { password = value;  }
    }

    public User(string firstName, string lastName, string username, string password)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.username = username;
        this.password = BCrypt.Net.BCrypt.HashPassword(password);
    }
}

class Employee : User
{
    public Employee(string firstName, string lastName, string username, string password) : base(firstName, lastName, username, password) 
    { this.isAdmin = false; }

}

class Admin : User

{
    public Admin(string firstName, string lastName, string username, string password) : base(firstName, lastName, username, password)
    { this.isAdmin = true; }
}

