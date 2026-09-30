using ClimaPulse.Enums;
using ClimaPulse.Models;
using ClimaPulse.Repositories;
using ClimaPulse.Strategies;

namespace ClimaPulse.Services;

/// <summary>
/// Orchestrates weather data fetching, strategy-based evaluation,
/// severity tier assignment, and anomaly log persistence.
/// </summary>
public class AnomalyDetectionService
{
    private readonly IAnomalyStrategy _strategy;
    private readonly WeatherService _weatherService;
    private readonly BaselineRepository _baselineRepo;
    private readonly AnomalyEventRepository _eventRepo;

    public AnomalyDetectionService(
        IAnomalyStrategy strategy,
        WeatherService weatherService,
        BaselineRepository baselineRepo,
        AnomalyEventRepository eventRepo)
    {
        _strategy = strategy;
        _weatherService = weatherService;
        _baselineRepo = baselineRepo;
        _eventRepo = eventRepo;
    }

    public async Task<AnomalyResult> AnalyzeCityAsync(City city)
    {
        throw new NotImplementedException();
    }

    public SeverityTier AssignTier(double zScore)
    {
        throw new NotImplementedException();
    }
}
