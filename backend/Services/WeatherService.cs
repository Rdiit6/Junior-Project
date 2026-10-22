using ClimaPulse.Models;

namespace ClimaPulse.Services;

/// <summary>
/// The only class that talks to Open-Meteo. It uses two endpoints: the forecast API
/// for live readings and the archive API for historical data.
/// </summary>
public class WeatherService
{
    private readonly HttpClient _httpClient;

    /// <summary>Forecast API address, used by <see cref="FetchLiveAsync"/>.</summary>
    public Uri ForecastUri { get; }

    /// <summary>Archive API address, used by <see cref="FetchArchiveAsync"/>.</summary>
    public Uri ArchiveUri { get; }

    public WeatherService(HttpClient httpClient, OpenMeteoOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        _httpClient = httpClient;
        ForecastUri = ParseHttpUri(options.ForecastBaseUrl, nameof(options.ForecastBaseUrl));
        ArchiveUri = ParseHttpUri(options.ArchiveBaseUrl, nameof(options.ArchiveBaseUrl));
    }

    public async Task FetchLiveAsync(City city)
    {
        throw new NotImplementedException();
    }

    public async Task FetchArchiveAsync(City city)
    {
        throw new NotImplementedException();
    }

    private static Uri ParseHttpUri(string? value, string name)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException($"{name} must be an absolute http or https URL.", name);
        }

        return uri;
    }
}
