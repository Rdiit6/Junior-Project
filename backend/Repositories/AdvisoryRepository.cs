using ClimaPulse.Enums;
using ClimaPulse.Models;
using Microsoft.Data.Sqlite;

namespace ClimaPulse.Repositories;

/// <summary>
/// Reads and writes the Advisory table. To change advisory text, Add a new row with a
/// higher Version instead of editing, so old anomaly events keep the text they were shown.
/// Remove clears advisory_id on events that used it (ON DELETE SET NULL).
/// </summary>
public class AdvisoryRepository : SqliteRepositoryBase, IRepository<Advisory>
{
    private const string Columns =
        "advisory_id, metric_type, severity_tier, locale, category, title, body, version, effective_from";

    public AdvisoryRepository(string connectionString) : base(connectionString)
    {
    }

    public void Add(Advisory item)
    {
        item.Id = InsertAndGetId(
            @"INSERT INTO Advisory (metric_type, severity_tier, locale, category, title, body, version, effective_from)
              VALUES ($metric, $tier, $locale, $category, $title, $body, $version, $effective)",
            cmd =>
            {
                AddParam(cmd, "$metric", item.MetricType.ToDb());
                AddParam(cmd, "$tier", item.SeverityTier.ToDb());
                AddParam(cmd, "$locale", item.Locale);
                AddParam(cmd, "$category", item.Category.ToDb());
                AddParam(cmd, "$title", item.Title);
                AddParam(cmd, "$body", item.Body);
                AddParam(cmd, "$version", item.Version);
                AddParam(cmd, "$effective", item.EffectiveFrom.ToDb());
            });
    }

    public void Remove(Advisory item) =>
        ExecuteNonQuery("DELETE FROM Advisory WHERE advisory_id = $id", cmd => AddParam(cmd, "$id", item.Id));

    public List<Advisory> GetAll() =>
        ReadList($"SELECT {Columns} FROM Advisory ORDER BY metric_type, severity_tier, locale, version", Map);

    public List<Advisory> Query(Func<Advisory, bool> filter) =>
        GetAll().Where(filter).ToList();

    /// <summary>
    /// The current advisory for a metric, tier and locale: the highest version whose
    /// effective_from is not in the future. Returns null if there is none.
    /// </summary>
    public Advisory? GetLatest(MetricType metric, SeverityTier tier, string locale) =>
        ReadList(
            $@"SELECT {Columns} FROM Advisory
               WHERE metric_type = $metric AND severity_tier = $tier AND locale = $locale
                 AND effective_from <= $now
               ORDER BY version DESC
               LIMIT 1",
            Map,
            cmd =>
            {
                AddParam(cmd, "$metric", metric.ToDb());
                AddParam(cmd, "$tier", tier.ToDb());
                AddParam(cmd, "$locale", locale);
                AddParam(cmd, "$now", DateTime.UtcNow.ToDb());
            }).FirstOrDefault();

    private static Advisory Map(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        MetricType = r.GetString(1).ToMetricType(),
        SeverityTier = r.GetString(2).ToSeverityTier(),
        Locale = r.GetString(3),
        Category = r.GetString(4).ToAdvisoryCategory(),
        Title = r.GetString(5),
        Body = r.GetString(6),
        Version = r.GetInt32(7),
        EffectiveFrom = Timestamp(r, 8)
    };
}
