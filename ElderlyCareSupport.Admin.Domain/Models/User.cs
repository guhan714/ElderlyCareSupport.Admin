namespace ElderlyCareSupport.Admin.Domain.Models;

public class User
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public long PhoneNumber { get; set; } = long.MinValue;
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public long PostalCode { get; set; } = long.MinValue;
    public string Region { get; set; } = string.Empty;
    public UserType UserType { get; set; } 
    public bool IsActive { get; set; } = true;
};