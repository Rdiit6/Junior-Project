using ClimaPulse.Enums;
using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the Baseline table. Remove throws SqliteException while anomaly
/// events still point at the baseline. To refresh a baseline, call Upsert instead of
/// deleting and re-adding, so existing events keep their reference.
/// </summary>
public class BaselineRepository : SqliteRepositoryBase, IRepository<Baseline>
{
    private const string Columns =
        "baseline_id, city_id, metric_type, month, mean_value, stddev_value, sample_size, baseline_start_year, baseline_end_year, computed_at";

    public BaselineRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(Baseline item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO Baseline (city_id, metric_type, month, mean_value, stddev_value, sample_size,
                                    baseline_start_year, baseline_end_year, computed_at)
              VALUES ($city, $metric, $month, $mean, $std, $n, $startYear, $endYear, $computed)",
            cmd => BindAll(cmd, item));
    }

    public void Remove(Baseline item) =>
        ExecuteNonQuery("DELETE FROM Baseline WHERE baseline_id = $id", cmd => AddParam(cmd, "$id", item.Id));

    public List<Baseline> GetAll() =>
        ReadList($"SELECT {Columns} FROM Baseline ORDER BY city_id, metric_type, month", Map);

    public List<Baseline> Query(Func<Baseline, bool> filter) =>
        GetAll().Where(filter).ToList();

    /// <summary>The baseline for one city, metric and calendar month, or null if none exists.</summary>
    public Baseline? GetByCityMetricMonth(int cityId, MetricType metric, int month) =>
        ReadList(
            $"SELECT {Columns} FROM Baseline WHERE city_id = $city AND metric_type = $metric AND month = $month",
            Map,
            cmd =>
            {
                AddParam(cmd, "$city", cityId);
                AddParam(cmd, "$metric", metric.ToDb());
                AddParam(cmd, "$month", month);
            }).FirstOrDefault();

    /// <summary>
    /// Inserts the baseline, or updates the existing row for the same city, metric and
    /// month in place. The row keeps its id, so AnomalyEvent rows stay linked.
    /// Sets item.Id to the row's id.
    /// </summary>
    public void Upsert(Baseline item)
    {
        using var connection = OpenConnection();

        using (var upsert = connection.CreateCommand())
        {
            upsert.CommandText =
                @"INSERT INTO Baseline (city_id, metric_type, month, mean_value, stddev_value, sample_size,
                                        baseline_start_year, baseline_end_year, computed_at)
                  VALUES ($city, $metric, $month, $mean, $std, $n, $startYear, $endYear, $computed)
                  ON CONFLICT (city_id, metric_type, month) DO UPDATE SET
                      mean_value          = excluded.mean_value,
                      stddev_value        = excluded.stddev_value,
                      sample_size         = excluded.sample_size,
                      baseline_start_year = excluded.baseline_start_year,
                      baseline_end_year   = excluded.baseline_end_year,
                      computed_at         = excluded.computed_at";
            BindAll(upsert, item);
            upsert.ExecuteNonQuery();
        }

        // last_insert_rowid() is not reliable after an update, so look the id up by key.
        using var lookup = connection.CreateCommand();
        lookup.CommandText =
            "SELECT baseline_id FROM Baseline WHERE city_id = $city AND metric_type = $metric AND month = $month";
        AddParam(lookup, "$city", item.CityId);
        AddParam(lookup, "$metric", item.MetricType.ToDb());
        AddParam(lookup, "$month", item.Month);
        item.Id = Convert.ToInt32(lookup.ExecuteScalar());
    }

    private static void BindAll(SqliteCommand cmd, Baseline item)
    {
        AddParam(cmd, "$city", item.CityId);
        AddParam(cmd, "$metric", item.MetricType.ToDb());
        AddParam(cmd, "$month", item.Month);
        AddParam(cmd, "$mean", item.MeanValue);
        AddParam(cmd, "$std", item.StdDevValue);
        AddParam(cmd, "$n", item.SampleSize);
        AddParam(cmd, "$startYear", item.BaselineStartYear);
        AddParam(cmd, "$endYear", item.BaselineEndYear);
        AddParam(cmd, "$computed", item.ComputedAt.ToDb());
    }

    private static Baseline Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        CityId = r.GetInt32(1),
        MetricType = r.GetString(2).ToMetricType(),
        Month = r.GetInt32(3),
        MeanValue = r.GetDouble(4),
        StdDevValue = r.GetDouble(5),
        SampleSize = r.GetInt32(6),
        BaselineStartYear = r.GetInt32(7),
        BaselineEndYear = r.GetInt32(8),
        ComputedAt = Timestamp(r, 9)
    };
}
