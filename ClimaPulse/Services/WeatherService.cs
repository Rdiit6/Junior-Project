using ClimaPulse.Models;

namespace ClimaPulse.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public WeatherService(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task FetchLiveAsync(City city)
    {
        throw new NotImplementedException();
    }

    public async Task FetchArchiveAsync(City city)
    {
        throw new NotImplementedException();
    }
}
