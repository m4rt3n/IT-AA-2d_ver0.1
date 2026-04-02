using UnityEngine;
using System.IO;
using SQLite4Unity3d;
using System.Linq;

public class Database : MonoBehaviour
{
    private SQLiteConnection connection;

    void Start()
    {
        Init();
        CreateUser("Martin");

        User user = GetUser("Martin");

        if (user != null)
        {
            Debug.Log("Geladener User: " + user.Username + " Score: " + user.Score);
        }
    }

    void Init()
    {
        string dbPath = Path.Combine(Application.dataPath, "../Database/game.db");

        connection = new SQLiteConnection(dbPath);

        connection.CreateTable<User>();

        Debug.Log("Datenbank initialisiert");
    }

    public void CreateUser(string username)
    {
        var existingUser = connection.Table<User>().FirstOrDefault(x => x.Username == username);

        if (existingUser == null)
        {
            connection.Insert(new User { Username = username, Score = 0 });
            Debug.Log("User erstellt: " + username);
        }
        else
        {
            Debug.Log("User existiert bereits: " + username);
        }
    }

    public User GetUser(string username)
    {
        return connection.Table<User>().FirstOrDefault(x => x.Username == username);
    }
}

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Username { get; set; }

    public int Score { get; set; }
}