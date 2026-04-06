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
            return;

        string json = File.ReadAllText(path);
        database = JsonUtility.FromJson<UserDatabase>(json);

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
        return database.users
            .FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}