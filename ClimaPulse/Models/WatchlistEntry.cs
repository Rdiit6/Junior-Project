namespace ClimaPulse.Models;

/// <summary>Row of the Watchlist table: one city watched by one profile.</summary>
public class WatchlistEntry
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public int CityId { get; set; }

    /// <summary>Per-city sensitivity, typically 1.5 to 3.5. Must be above 0.</summary>
    public double AlertThresholdZScore { get; set; } = 2.0;

    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
