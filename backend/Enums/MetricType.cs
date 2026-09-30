namespace ClimaPulse.Enums;

/// <summary>
/// Weather metric a row refers to. Stored in SQLite as text:
/// temperature_2m, precipitation, wind_speed_10m.
/// </summary>
public enum MetricType
{
    Temperature,
    Precipitation,
    WindSpeed
}
