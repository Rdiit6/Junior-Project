using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClimaPulse.Enums;
using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the AnomalyEvent table, the Historical Anomaly Ledger. Replaces
/// AnomalyLogRepository. An active event must have no ended_at, and a closed event
/// must end at or after it started; the schema rejects anything else.
/// </summary>
public class AnomalyEventRepository : SqliteRepositoryBase, IRepository<AnomalyEvent>
{
    private const string Columns =
        "event_id, city_id, baseline_id, advisory_id, metric_type, severity_tier, started_at, ended_at, " +
        "peak_at, peak_observed_value, peak_zscore, baseline_mean_snapshot, baseline_stddev_snapshot, status, created_at";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public AnomalyEventRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(AnomalyEvent item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO AnomalyEvent (city_id, baseline_id, advisory_id, metric_type, severity_tier, started_at,
                                        ended_at, peak_at, peak_observed_value, peak_zscore,
                                        baseline_mean_snapshot, baseline_stddev_snapshot, status, created_at)
              VALUES ($city, $baseline, $advisory, $metric, $tier, $started,
                      $ended, $peakAt, $peakValue, $peakZ,
                      $mean, $std, $status, $created)",
            cmd =>
            {
                AddParam(cmd, "$city", item.CityId);
                AddParam(cmd, "$baseline", item.BaselineId);
                AddParam(cmd, "$advisory", item.AdvisoryId);
                AddParam(cmd, "$metric", item.MetricType.ToDb());
                AddParam(cmd, "$tier", item.SeverityTier.ToDb());
                AddParam(cmd, "$started", item.StartedAt.ToDb());
                AddParam(cmd, "$ended", item.EndedAt?.ToDb());
                AddParam(cmd, "$peakAt", item.PeakAt.ToDb());
                AddParam(cmd, "$peakValue", item.PeakObservedValue);
                AddParam(cmd, "$peakZ", item.PeakZScore);
                AddParam(cmd, "$mean", item.BaselineMeanSnapshot);
                AddParam(cmd, "$std", item.BaselineStdDevSnapshot);
                AddParam(cmd, "$status", item.Status.ToDb());
                AddParam(cmd, "$created", item.CreatedAt.ToDb());
            });
    }

    public void Remove(AnomalyEvent item) =>
        ExecuteNonQuery("DELETE FROM AnomalyEvent WHERE event_id = $id", cmd => AddParam(cmd, "$id", item.Id));

    public List<AnomalyEvent> GetAll() =>
        ReadList($"SELECT {Columns} FROM AnomalyEvent ORDER BY started_at DESC", Map);

    public List<AnomalyEvent> Query(Func<AnomalyEvent, bool> filter) =>
        GetAll().Where(filter).ToList();

    /// <summary>
    /// The open episode for a city and metric, or null. Used to decide whether a new
    /// reading continues an existing event or starts a new one.
    /// </summary>
    public AnomalyEvent? GetOpenEvent(int cityId, MetricType metric) =>
        ReadList(
            $@"SELECT {Columns} FROM AnomalyEvent
               WHERE city_id = $city AND metric_type = $metric AND status = 'active'
               ORDER BY started_at DESC
               LIMIT 1",
            Map,
            cmd =>
            {
                AddParam(cmd, "$city", cityId);
                AddParam(cmd, "$metric", metric.ToDb());
            }).FirstOrDefault();

    /// <summary>
    /// Closes an active event. Returns false if the event does not exist or is already
    /// closed. Replaces the old Acknowledge method.
    /// </summary>
    public bool Close(int eventId, DateTime endedAtUtc) =>
        ExecuteNonQuery(
            "UPDATE AnomalyEvent SET status = 'closed', ended_at = $ended WHERE event_id = $id AND status = 'active'",
            cmd =>
            {
                AddParam(cmd, "$id", eventId);
                AddParam(cmd, "$ended", endedAtUtc.ToDb());
            }) > 0;

    /// <summary>Writes every event to a CSV file, newest first.</summary>
    public void ExportCsv(string filePath)
    {
        var lines = new List<string>
        {
            "event_id,city_id,baseline_id,advisory_id,metric_type,severity_tier,started_at,ended_at," +
            "peak_at,peak_observed_value,peak_zscore,baseline_mean_snapshot,baseline_stddev_snapshot,status,created_at"
        };

        foreach (var e in GetAll())
        {
            lines.Add(string.Join(",",
                e.Id.ToString(CultureInfo.InvariantCulture),
                e.CityId.ToString(CultureInfo.InvariantCulture),
                e.BaselineId.ToString(CultureInfo.InvariantCulture),
                e.AdvisoryId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                e.MetricType.ToDb(),
                e.SeverityTier.ToDb(),
                e.StartedAt.ToDb(),
                e.EndedAt?.ToDb() ?? string.Empty,
                e.PeakAt.ToDb(),
                e.PeakObservedValue.ToString(CultureInfo.InvariantCulture),
                e.PeakZScore.ToString(CultureInfo.InvariantCulture),
                e.BaselineMeanSnapshot.ToString(CultureInfo.InvariantCulture),
                e.BaselineStdDevSnapshot.ToString(CultureInfo.InvariantCulture),
                e.Status.ToDb(),
                e.CreatedAt.ToDb()));
        }

        File.WriteAllLines(filePath, lines);
    }

    /// <summary>Writes every event to a JSON file, newest first.</summary>
    public void ExportJson(string filePath) =>
        File.WriteAllText(filePath, JsonSerializer.Serialize(GetAll(), JsonOptions));

    private static AnomalyEvent Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        CityId = r.GetInt32(1),
        BaselineId = r.GetInt32(2),
        AdvisoryId = NullableInt(r, 3),
        MetricType = r.GetString(4).ToMetricType(),
        SeverityTier = r.GetString(5).ToSeverityTier(),
        StartedAt = Timestamp(r, 6),
        EndedAt = NullableTimestamp(r, 7),
        PeakAt = Timestamp(r, 8),
        PeakObservedValue = r.GetDouble(9),
        PeakZScore = r.GetDouble(10),
        BaselineMeanSnapshot = r.GetDouble(11),
        BaselineStdDevSnapshot = r.GetDouble(12),
        Status = r.GetString(13).ToEventStatus(),
        CreatedAt = Timestamp(r, 14)
    };
}
