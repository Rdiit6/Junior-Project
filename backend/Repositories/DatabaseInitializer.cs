using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>Creates the database tables from Database/schema.sql on first run.</summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Runs schema.sql when the database has no City table yet. Safe to call on every
    /// start: an existing database is left untouched. SQLite creates the file itself
    /// when the connection opens, but the folder in the path must already exist.
    /// </summary>
    public static void EnsureCreated(string connectionString, string schemaSqlPath)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using (var check = connection.CreateCommand())
        {
            check.CommandText =
                "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'City';";
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                return;
        }

        using var create = connection.CreateCommand();
        create.CommandText = File.ReadAllText(schemaSqlPath);
        create.ExecuteNonQuery();
    }
}
