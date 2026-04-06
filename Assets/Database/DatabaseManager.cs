using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private List<UserEntity> users = new List<UserEntity>();
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
    }

    public bool RegisterUser(string username, string password)
    {
        if (users.Any(u => u.Username == username))
            return false;

        users.Add(new UserEntity
        {
            Id = currentId++,
            Username = username,
            Password = password
        });

        return true;
    }

    public UserEntity LoginUser(string username, string password)
    {
        return users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);
    }
}