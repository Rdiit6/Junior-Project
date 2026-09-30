using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Shared SQLite plumbing for the repositories. Each call opens its own short-lived
/// connection, so repositories hold no open handles. Constraint violations from the
/// schema (UNIQUE, CHECK, FOREIGN KEY) surface as SqliteException.
/// </summary>
public abstract class SqliteRepositoryBase
{
    private readonly string _connectionString;

    protected SqliteRepositoryBase(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>Opens a connection with foreign key enforcement switched on.</summary>
    protected SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // SQLite enforces foreign keys per connection, and only when asked to.
        using var pragma = connection.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();

        return connection;
    }

    /// <summary>Binds a parameter, turning null into SQL NULL.</summary>
    protected static void AddParam(SqliteCommand command, string name, object? value) =>
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);

    /// <summary>Runs an INSERT, UPDATE or DELETE and returns the number of rows changed.</summary>
    protected int ExecuteNonQuery(string sql, Action<SqliteCommand> bind)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        bind(command);
        return command.ExecuteNonQuery();
    }

    /// <summary>Runs an INSERT and returns the id SQLite generated for the new row.</summary>
    protected int InsertAndGetId(string sql, Action<SqliteCommand> bind)
    {
        using var connection = OpenConnection();

        using (var insert = connection.CreateCommand())
        {
            insert.CommandText = sql;
            bind(insert);
            insert.ExecuteNonQuery();
        }

        // Same connection, so this returns the row inserted just above.
        using var lastId = connection.CreateCommand();
        lastId.CommandText = "SELECT last_insert_rowid();";
        return Convert.ToInt32(lastId.ExecuteScalar());
    }

    /// <summary>Runs a SELECT and maps every row with the given function.</summary>
    protected List<T> ReadList<T>(
        string sql, Func<SqliteDataReader, T> map, Action<SqliteCommand>? bind = null)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        bind?.Invoke(command);

        using var reader = command.ExecuteReader();
        var rows = new List<T>();
        while (reader.Read())
            rows.Add(map(reader));
        return rows;
    }

    // ---- reader helpers for nullable columns -----------------------------

    protected static string? NullableString(SqliteDataReader r, int i) =>
        r.IsDBNull(i) ? null : r.GetString(i);

    protected static double? NullableDouble(SqliteDataReader r, int i) =>
        r.IsDBNull(i) ? null : r.GetDouble(i);

    protected static int? NullableInt(SqliteDataReader r, int i) =>
        r.IsDBNull(i) ? null : r.GetInt32(i);

    protected static DateTime Timestamp(SqliteDataReader r, int i) =>
        r.GetString(i).ToDateTime();

    protected static DateTime? NullableTimestamp(SqliteDataReader r, int i) =>
        r.IsDBNull(i) ? null : r.GetString(i).ToDateTime();
}
