namespace ClimaPulse.Models;

public class ClimateBaseline
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public int Month { get; set; }
    public double AvgTempMean { get; set; }
    public double StdDevTemp { get; set; }
    public double AvgPrecipitationMm { get; set; }
}
