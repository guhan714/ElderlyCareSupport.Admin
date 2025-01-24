namespace ElderlyCareSupport.Admin.Application.IService;

public interface IHashingService
{
    bool VerifyPassword(string plainText, string passwordHash);
}