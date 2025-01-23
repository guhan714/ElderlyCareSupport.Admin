using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.IRepository;

public interface IKeycloakAdminRepository
{
    Task<Tuple<TokenResponse, bool>> AuthenticateAdminAsync(Contracts.Request.Admin admin);
}