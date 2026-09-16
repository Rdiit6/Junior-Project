namespace ClimaPulse.Models;

public class City
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsPinned { get; set; }
}
