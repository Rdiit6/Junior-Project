namespace ClimaPulse.Models;

/// <summary>Row of the City table.</summary>
public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? AdminRegion { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public double? ElevationM { get; set; }
    public string? OpenMeteoRef { get; set; }
}
