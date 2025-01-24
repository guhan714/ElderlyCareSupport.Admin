using ElderlyCareSupport.Admin.Application.IService;

namespace ElderlyCareSupport.Admin.Application.Service;

public class HashingService : IHashingService
{
    public bool VerifyPassword(string plainText, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(plainText, passwordHash);
    }
}