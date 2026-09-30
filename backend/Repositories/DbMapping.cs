using System.Globalization;
using ClimaPulse.Enums;

namespace ClimaPulse.Repositories;

/// <summary>
/// Converts between C# values and the text forms stored in SQLite.
/// Timestamps are UTC ISO 8601 text such as 2026-09-20T15:37:00Z, which sorts
/// correctly as a string.
/// </summary>
internal static class DbMapping
{
    private const string IsoFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'";

    // ---- timestamps ------------------------------------------------------

    public static string ToDb(this DateTime value)
    {
        var utc = value.Kind == DateTimeKind.Local
            ? value.ToUniversalTime()
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return utc.ToString(IsoFormat, CultureInfo.InvariantCulture);
    }

    public static DateTime ToDateTime(this string text) =>
        DateTime.ParseExact(text, IsoFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

    // ---- MetricType ------------------------------------------------------

    public static string ToDb(this MetricType value) => value switch
    {
        MetricType.Temperature => "temperature_2m",
        MetricType.Precipitation => "precipitation",
        MetricType.WindSpeed => "wind_speed_10m",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown metric type.")
    };

    public static MetricType ToMetricType(this string text) => text switch
    {
        "temperature_2m" => MetricType.Temperature,
        "precipitation" => MetricType.Precipitation,
        "wind_speed_10m" => MetricType.WindSpeed,
        _ => throw new FormatException($"Unknown metric_type '{text}' in the database.")
    };

    // ---- SeverityTier ----------------------------------------------------

    public static string ToDb(this SeverityTier value) => value switch
    {
        SeverityTier.Watch => "watch",
        SeverityTier.Warning => "warning",
        SeverityTier.Extreme => "extreme",
        SeverityTier.Normal => throw new InvalidOperationException(
            "SeverityTier.Normal means no anomaly and is never stored."),
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown severity tier.")
    };

    public static SeverityTier ToSeverityTier(this string text) => text switch
    {
        "watch" => SeverityTier.Watch,
        "warning" => SeverityTier.Warning,
        "extreme" => SeverityTier.Extreme,
        _ => throw new FormatException($"Unknown severity_tier '{text}' in the database.")
    };

    // ---- EventStatus -----------------------------------------------------

    public static string ToDb(this EventStatus value) => value switch
    {
        EventStatus.Active => "active",
        EventStatus.Closed => "closed",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown event status.")
    };

    public static EventStatus ToEventStatus(this string text) => text switch
    {
        "active" => EventStatus.Active,
        "closed" => EventStatus.Closed,
        _ => throw new FormatException($"Unknown status '{text}' in the database.")
    };

    // ---- AdvisoryCategory ------------------------------------------------

    public static string ToDb(this AdvisoryCategory value) => value switch
    {
        AdvisoryCategory.Health => "health",
        AdvisoryCategory.Climate => "climate",
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown advisory category.")
    };

    public static AdvisoryCategory ToAdvisoryCategory(this string text) => text switch
    {
        "health" => AdvisoryCategory.Health,
        "climate" => AdvisoryCategory.Climate,
        _ => throw new FormatException($"Unknown category '{text}' in the database.")
    };
}
