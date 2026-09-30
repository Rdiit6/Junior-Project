namespace ClimaPulse.Enums;

/// <summary>
/// Severity of a detected anomaly. Normal means no anomaly and is never stored.
/// Watch, Warning and Extreme are stored as text: watch, warning, extreme.
/// </summary>
public enum SeverityTier
{
    Normal,
    Watch,
    Warning,
    Extreme
}
