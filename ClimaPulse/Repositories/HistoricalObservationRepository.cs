using ClimaPulse.Enums;
using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the HistoricalObservation table. Add throws SqliteException for a
/// reading that already exists (UNIQUE on city_id, metric_type, observed_at). Use
/// AddRange for bulk loads from Open-Meteo, which skips readings already stored.
/// </summary>
public class HistoricalObservationRepository : SqliteRepositoryBase, IRepository<HistoricalObservation>
{
    private const string Columns =
        "observation_id, city_id, metric_type, observed_at, value, source, fetched_at";

    public HistoricalObservationRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(HistoricalObservation item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO HistoricalObservation (city_id, metric_type, observed_at, value, source, fetched_at)
              VALUES ($city, $metric, $observed, $value, $source, $fetched)",
            cmd =>
            {
                AddParam(cmd, "$city", item.CityId);
                AddParam(cmd, "$metric", item.MetricType.ToDb());
                AddParam(cmd, "$observed", item.ObservedAt.ToDb());
                AddParam(cmd, "$value", item.Value);
                AddParam(cmd, "$source", item.Source);
                AddParam(cmd, "$fetched", item.FetchedAt.ToDb());
            });
    }

    /// <summary>
    /// Inserts many readings in one transaction, ignoring any that already exist, so a
    /// re-fetch is safe. Returns how many new rows were stored. Ids are not set on items.
    /// </summary>
    public int AddRange(IEnumerable<HistoricalObservation> items)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            @"INSERT OR IGNORE INTO HistoricalObservation (city_id, metric_type, observed_at, value, source, fetched_at)
              VALUES ($city, $metric, $observed, $value, $source, $fetched)";

        var city = command.Parameters.AddWithValue("$city", 0);
        var metric = command.Parameters.AddWithValue("$metric", string.Empty);
        var observed = command.Parameters.AddWithValue("$observed", string.Empty);
        var value = command.Parameters.AddWithValue("$value", 0.0);
        var source = command.Parameters.AddWithValue("$source", string.Empty);
        var fetched = command.Parameters.AddWithValue("$fetched", string.Empty);

        var inserted = 0;
        foreach (var item in items)
        {
            city.Value = item.CityId;
            metric.Value = item.MetricType.ToDb();
            observed.Value = item.ObservedAt.ToDb();
            value.Value = item.Value;
            source.Value = item.Source;
            fetched.Value = item.FetchedAt.ToDb();
            inserted += command.ExecuteNonQuery();
        }

        transaction.Commit();
        return inserted;
    }

    public void Remove(HistoricalObservation item) =>
        ExecuteNonQuery("DELETE FROM HistoricalObservation WHERE observation_id = $id",
            cmd => AddParam(cmd, "$id", item.Id));

    public List<HistoricalObservation> GetAll() =>
        ReadList($"SELECT {Columns} FROM HistoricalObservation ORDER BY city_id, metric_type, observed_at", Map);

    public List<HistoricalObservation> Query(Func<HistoricalObservation, bool> filter) =>
        GetAll().Where(filter).ToList();

    /// <summary>Readings for one city and metric between two UTC instants, inclusive, oldest first.</summary>
    public List<HistoricalObservation> GetRange(int cityId, MetricType metric, DateTime fromUtc, DateTime toUtc) =>
        ReadList(
            $@"SELECT {Columns} FROM HistoricalObservation
               WHERE city_id = $city AND metric_type = $metric
                 AND observed_at >= $from AND observed_at <= $to
               ORDER BY observed_at",
            Map,
            cmd =>
            {
                AddParam(cmd, "$city", cityId);
                AddParam(cmd, "$metric", metric.ToDb());
                AddParam(cmd, "$from", fromUtc.ToDb());
                AddParam(cmd, "$to", toUtc.ToDb());
            });

    private static HistoricalObservation Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        CityId = r.GetInt32(1),
        MetricType = r.GetString(2).ToMetricType(),
        ObservedAt = Timestamp(r, 3),
        Value = r.GetDouble(4),
        Source = r.GetString(5),
        FetchedAt = Timestamp(r, 6)
    };
}
