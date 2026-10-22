namespace ClimaPulse.Services;

/// <summary>
/// Open-Meteo endpoints used by <see cref="WeatherService"/>. Live readings and
/// historical data are served by two different hosts, so each has its own address.
/// Bind this from configuration section "OpenMeteo", for example the environment
/// variables OpenMeteo__ForecastBaseUrl and OpenMeteo__ArchiveBaseUrl.
/// </summary>
public class OpenMeteoOptions
{
    public const string SectionName = "OpenMeteo";

    /// <summary>Forecast API, used for current readings.</summary>
    public string ForecastBaseUrl { get; set; } = "https://api.open-meteo.com/v1/forecast";

    /// <summary>Historical (archive) API, used for the 30-year baseline data.</summary>
    public string ArchiveBaseUrl { get; set; } = "https://archive-api.open-meteo.com/v1/archive";
}
