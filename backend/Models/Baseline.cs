using ClimaPulse.Enums;

namespace ClimaPulse.Models;

/// <summary>
/// Row of the Baseline table: mean and standard deviation for one city,
/// one metric and one calendar month. Replaces the old ClimateBaseline class.
/// </summary>
public class Baseline
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public MetricType MetricType { get; set; }

    /// <summary>Calendar month, 1 through 12.</summary>
    public int Month { get; set; }

    public double MeanValue { get; set; }
    public double StdDevValue { get; set; }
    public int SampleSize { get; set; }
    public int BaselineStartYear { get; set; }
    public int BaselineEndYear { get; set; }
    public DateTime ComputedAt { get; set; } = DateTime.UtcNow;
}
