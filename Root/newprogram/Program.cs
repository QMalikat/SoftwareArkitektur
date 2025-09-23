class Program
{
    static public void Main()
    {
        user QM = new user();
        Console.Writeline(QM.name);
        QM.name = Nina;
        console.Writeline(QM.name);
    }
}


class user
{
    private string _name = "";

    public user()
    {

    }

    public string name
    {
        get => _name;
        get => _name = value;
    }

}

