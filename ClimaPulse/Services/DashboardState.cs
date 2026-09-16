using ClimaPulse.Models;

namespace ClimaPulse.Services;

/// <summary>
/// Shared state service — DashboardComponent subscribes to OnChange
/// and re-renders whenever results are updated.
/// </summary>
public class DashboardState
{
    private List<City> _watchlist = new();
    private List<AnomalyResult> _lastResults = new();

    public event Action? OnChange;

    public void UpdateResults(List<City> watchlist, List<AnomalyResult> results)
    {
        _watchlist = watchlist;
        _lastResults = results;
        OnChange?.Invoke();
    }
}
