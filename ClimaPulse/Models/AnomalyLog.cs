namespace ClimaPulse.Models;

public class AnomalyLog
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public DateTime Timestamp { get; set; }
    public double ObservedValue { get; set; }
    public double BaselineValue { get; set; }
    public double DeviationSigma { get; set; }
    public string AnomalyType { get; set; } = string.Empty;
    public string SeverityLevel { get; set; } = string.Empty;
    public bool IsAcknowledged { get; set; }
    public DateTime? AcknowledgedAt { get; set; }

    public void Acknowledge()
    {
        IsAcknowledged = true;
        AcknowledgedAt = DateTime.UtcNow;
    }
}
