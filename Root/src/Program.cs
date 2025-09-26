using System;

class Program
{
    static public void Main()
    {
        User QM = new User();
        Console.WriteLine(QM.Name);
        QM.Name = "Nina";
        Console.WriteLine(QM.Name);
    }
}

public class User
{
    private string _name = "";

    public User() { }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
