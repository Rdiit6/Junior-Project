using ClimaPulse.Models;

namespace ClimaPulse.Services;

public class ClimateTwinEngine
{
    private readonly List<City> _candidateCities;

    public ClimateTwinEngine(List<City> candidateCities)
    {
        _candidateCities = candidateCities;
    }

    public City FindTwin(AnomalyResult result)
    {
        throw new NotImplementedException();
    }
}
