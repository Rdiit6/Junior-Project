namespace ClimaPulse.Repositories;

/// <summary>
/// Where the SQLite database lives. Reads CLIMAPULSE_DB_PATH from the environment
/// and falls back to climapulse.db. Note that .NET does not load .env files by
/// itself, so set the variable in the launch profile or load .env with a library.
/// </summary>
public static class DatabaseConfig
{
    public const string DefaultDbPath = "climapulse.db";

    public static string DbPath =>
        Environment.GetEnvironmentVariable("CLIMAPULSE_DB_PATH") ?? DefaultDbPath;

    public static string ConnectionString => $"Data Source={DbPath}";
}
