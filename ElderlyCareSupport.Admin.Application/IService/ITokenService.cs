using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface ITokenService
{
    Task<TokenResponse?> GenerateToken(Contracts.Request.Admin admin);
}