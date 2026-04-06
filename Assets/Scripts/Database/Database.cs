using System;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance { get; private set; }

    private string connectionString;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        string dbPath = "URI=file:" + Application.streamingAssetsPath + "/game.db";
        connectionString = dbPath;

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
            }
        }
    }

    public bool RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO users (username, password)
                        VALUES (@username, @password);
                    ";

                    command.Parameters.Add(new SqliteParameter("@username", username.Trim()));
                    command.Parameters.Add(new SqliteParameter("@password", password));
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("RegisterUser fehlgeschlagen: " + ex.Message);
            return false;
        }
    }

    public UserData LoginUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT id, username
                        FROM users
                        WHERE username = @username AND password = @password
                        LIMIT 1;
                    ";

                    command.Parameters.Add(new SqliteParameter("@username", username.Trim()));
                    command.Parameters.Add(new SqliteParameter("@password", password));

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserData
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Username = reader["username"].ToString()
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("LoginUser fehlgeschlagen: " + ex.Message);
        }

        return null;
    }
}