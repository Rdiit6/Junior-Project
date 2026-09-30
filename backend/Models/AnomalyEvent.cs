using ClimaPulse.Enums;

namespace ClimaPulse.Models;

/// <summary>
/// Row of the AnomalyEvent table (the Historical Anomaly Ledger).
/// One row is one sustained episode, not one polling tick.
/// Replaces the old AnomalyLog class.
/// </summary>
public class AnomalyEvent
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public int BaselineId { get; set; }
    public int? AdvisoryId { get; set; }
    public MetricType MetricType { get; set; }
    public SeverityTier SeverityTier { get; set; }
    public DateTime StartedAt { get; set; }

    /// <summary>Null while the episode is still open.</summary>
    public DateTime? EndedAt { get; set; }

    public DateTime PeakAt { get; set; }
    public double PeakObservedValue { get; set; }
    public double PeakZScore { get; set; }

    /// <summary>Copy of the baseline mean at detection time, so recomputing baselines never rewrites history.</summary>
    public double BaselineMeanSnapshot { get; set; }

    /// <summary>Copy of the baseline standard deviation at detection time.</summary>
    public double BaselineStdDevSnapshot { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Ends the episode. Replaces the old Acknowledge() method.</summary>
    public void Close(DateTime endedAtUtc)
    {
        EndedAt = endedAtUtc;
        Status = EventStatus.Closed;
    }
}
