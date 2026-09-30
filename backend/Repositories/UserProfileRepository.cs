using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the UserProfile table. Removing a profile also removes its
/// watchlist entries (ON DELETE CASCADE).
/// </summary>
public class UserProfileRepository : SqliteRepositoryBase, IRepository<UserProfile>
{
    private const string Columns =
        "profile_id, display_name, preferred_temperature_unit, preferred_locale, created_at, is_active";

    public UserProfileRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(UserProfile item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO UserProfile (display_name, preferred_temperature_unit, preferred_locale, created_at, is_active)
              VALUES ($name, $unit, $locale, $created, $active)",
            cmd =>
            {
                AddParam(cmd, "$name", item.DisplayName);
                AddParam(cmd, "$unit", item.PreferredTemperatureUnit);
                AddParam(cmd, "$locale", item.PreferredLocale);
                AddParam(cmd, "$created", item.CreatedAt.ToDb());
                AddParam(cmd, "$active", item.IsActive ? 1 : 0);
            });
    }

    public void Remove(UserProfile item) =>
        ExecuteNonQuery("DELETE FROM UserProfile WHERE profile_id = $id", cmd => AddParam(cmd, "$id", item.Id));

    public List<UserProfile> GetAll() =>
        ReadList($"SELECT {Columns} FROM UserProfile ORDER BY display_name", Map);

    public List<UserProfile> Query(Func<UserProfile, bool> filter) =>
        GetAll().Where(filter).ToList();

    public UserProfile? GetById(int id) =>
        ReadList($"SELECT {Columns} FROM UserProfile WHERE profile_id = $id", Map,
            cmd => AddParam(cmd, "$id", id)).FirstOrDefault();

    public void Update(UserProfile item) =>
        ExecuteNonQuery(
            @"UPDATE UserProfile
              SET display_name = $name, preferred_temperature_unit = $unit,
                  preferred_locale = $locale, is_active = $active
              WHERE profile_id = $id",
            cmd =>
            {
                AddParam(cmd, "$id", item.Id);
                AddParam(cmd, "$name", item.DisplayName);
                AddParam(cmd, "$unit", item.PreferredTemperatureUnit);
                AddParam(cmd, "$locale", item.PreferredLocale);
                AddParam(cmd, "$active", item.IsActive ? 1 : 0);
            });

    private static UserProfile Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        DisplayName = r.GetString(1),
        PreferredTemperatureUnit = r.GetString(2),
        PreferredLocale = r.GetString(3),
        CreatedAt = Timestamp(r, 4),
        IsActive = r.GetInt32(5) != 0
    };
}
