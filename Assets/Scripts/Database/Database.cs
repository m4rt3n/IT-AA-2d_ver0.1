using SQLite;
using UnityEngine;
using System.IO;
using System.Linq;

public class Database : MonoBehaviour
{
    public static Database Instance { get; private set; }

    private SQLiteConnection db;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        string dbPath = Path.Combine(Application.persistentDataPath, "game.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        db.CreateTable<UserData>();
    }

    public bool RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var existingUser = db.Table<UserData>()
            .FirstOrDefault(u => u.Username == username);

        if (existingUser != null)
            return false;

        var newUser = new UserData
        {
            Username = username.Trim(),
            Password = password
        };

        db.Insert(newUser);
        return true;
    }

    public UserData LoginUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        return db.Table<UserData>()
            .FirstOrDefault(u => u.Username == username.Trim() && u.Password == password);
    }

    private void OnDestroy()
    {
        db?.Close();
    }
}