namespace ClimaPulse.Models;

/// <summary>Row of the UserProfile table. A local profile, no password.</summary>
public class UserProfile
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>"C" or "F".</summary>
    public string PreferredTemperatureUnit { get; set; } = "C";

    /// <summary>BCP-47 tag such as id-ID or en-US.</summary>
    public string PreferredLocale { get; set; } = "id-ID";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
