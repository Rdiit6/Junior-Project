using ClimaPulse.Enums;

namespace ClimaPulse.Models;

/// <summary>DTO produced by the anomaly detection engine and passed between services.</summary>
public class AnomalyResult
{
    public double ObservedValue { get; set; }
    public double BaselineValue { get; set; }
    public double DeviationSigma { get; set; }
    public string AnomalyType { get; set; } = string.Empty;
    public SeverityTier SeverityTier { get; set; }
}
