namespace ElderlyCareSupport.Admin.Domain.ValueObjects;

public class VolunteerDetails
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long PhoneNumber { get; set; } = long.MinValue;
    public string Gender { get; set; } = string.Empty;
}