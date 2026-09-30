using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the Watchlist table. Add throws SqliteException when the profile
/// already watches that city (UNIQUE on profile_id, city_id).
/// </summary>
public class WatchlistRepository : SqliteRepositoryBase, IRepository<WatchlistEntry>
{
    private const string Columns =
        "watchlist_id, profile_id, city_id, alert_threshold_zscore, notes, added_at";

    public WatchlistRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(WatchlistEntry item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO Watchlist (profile_id, city_id, alert_threshold_zscore, notes, added_at)
              VALUES ($profile, $city, $threshold, $notes, $added)",
            cmd =>
            {
                AddParam(cmd, "$profile", item.ProfileId);
                AddParam(cmd, "$city", item.CityId);
                AddParam(cmd, "$threshold", item.AlertThresholdZScore);
                AddParam(cmd, "$notes", item.Notes);
                AddParam(cmd, "$added", item.AddedAt.ToDb());
            });
    }

    public void Remove(WatchlistEntry item) =>
        ExecuteNonQuery("DELETE FROM Watchlist WHERE watchlist_id = $id", cmd => AddParam(cmd, "$id", item.Id));

    public List<WatchlistEntry> GetAll() =>
        ReadList($"SELECT {Columns} FROM Watchlist ORDER BY profile_id, added_at", Map);

    public List<WatchlistEntry> Query(Func<WatchlistEntry, bool> filter) =>
        GetAll().Where(filter).ToList();

    public List<WatchlistEntry> GetByProfile(int profileId) =>
        ReadList($"SELECT {Columns} FROM Watchlist WHERE profile_id = $profile ORDER BY added_at", Map,
            cmd => AddParam(cmd, "$profile", profileId));

    /// <summary>Changes the alert threshold and notes of an existing entry.</summary>
    public void Update(WatchlistEntry item) =>
        ExecuteNonQuery(
            @"UPDATE Watchlist SET alert_threshold_zscore = $threshold, notes = $notes
              WHERE watchlist_id = $id",
            cmd =>
            {
                AddParam(cmd, "$id", item.Id);
                AddParam(cmd, "$threshold", item.AlertThresholdZScore);
                AddParam(cmd, "$notes", item.Notes);
            });

    private static WatchlistEntry Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        ProfileId = r.GetInt32(1),
        CityId = r.GetInt32(2),
        AlertThresholdZScore = r.GetDouble(3),
        Notes = NullableString(r, 4),
        AddedAt = Timestamp(r, 5)
    };
}
