namespace ElderlyCareSupport.Admin.Domain;

public static class AuthRoles
{
    public static readonly string Admin = "Admin";
    public static readonly List<string> Admins =  [ Admin ];
    
    public static bool IsAdmin(this string role) => role == Admin;
}