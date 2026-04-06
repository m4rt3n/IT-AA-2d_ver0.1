using SQLite;

public class UserData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public string Username { get; set; }

    public string Password { get; set; }
}