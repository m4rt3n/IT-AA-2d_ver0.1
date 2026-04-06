using System.IO;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private UserDatabase database = new UserDatabase();
    private string path;
    private int currentId = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Path.Combine(Application.persistentDataPath, "users.json");

        Load();
        SeedDummy();
        Save();
    }

    private void Load()
    {
        if (!File.Exists(path))
        {
            database = new UserDatabase();
            return;
        }

        string json = File.ReadAllText(path);
        database = JsonUtility.FromJson<UserDatabase>(json);

        if (database == null)
            database = new UserDatabase();

        if (database.users == null)
            database.users = new System.Collections.Generic.List<UserEntity>();

        if (database.users.Count > 0)
            currentId = database.users.Max(u => u.Id) + 1;
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(path, json);
    }

    private void SeedDummy()
    {
        if (database.users.Any(u => u.Username == "MartinDummy"))
            return;

        database.users.Add(new UserEntity
        {
            Id = currentId++,
            Username = "MartinDummy",
            Password = "1234"
        });
    }

    public bool RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        if (database.users.Any(u => u.Username == username))
            return false;

        database.users.Add(new UserEntity
        {
            Id = currentId++,
            Username = username,
            Password = password
        });

        Save();
        return true;
    }

    public UserEntity LoginUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        return database.users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);
    }

    public UserEntity GetUserById(int id)
    {
        return database.users.FirstOrDefault(u => u.Id == id);
    }

    public UserEntity GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        return database.users.FirstOrDefault(u => u.Username == username);
    }
}