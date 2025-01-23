namespace ElderlyCareSupport.Admin.Contracts.Request;

public class Admin
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}