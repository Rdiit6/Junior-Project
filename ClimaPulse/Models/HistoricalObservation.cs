using ClimaPulse.Enums;

namespace ClimaPulse.Models;

/// <summary>Row of the HistoricalObservation table: one raw reading pulled from Open-Meteo.</summary>
public class HistoricalObservation
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public MetricType MetricType { get; set; }

    /// <summary>UTC, hourly grain.</summary>
    public DateTime ObservedAt { get; set; }

    public double Value { get; set; }
    public string Source { get; set; } = "open-meteo";
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
}
