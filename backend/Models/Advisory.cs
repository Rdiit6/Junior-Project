using ClimaPulse.Enums;

namespace ClimaPulse.Models;

/// <summary>
/// Row of the Advisory table: guidance text for one metric, tier and locale.
/// Each edit gets a new Version so old events keep the text they were shown.
/// </summary>
public class Advisory
{
    public int Id { get; set; }
    public MetricType MetricType { get; set; }
    public SeverityTier SeverityTier { get; set; }
    public string Locale { get; set; } = "id-ID";
    public AdvisoryCategory Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
}
