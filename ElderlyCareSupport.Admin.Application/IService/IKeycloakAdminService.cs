using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.IService;

public interface IKeycloakAdminService
{
    Task<Tuple<TokenResponse?, bool>> AuthenticateAdminAsync(Contracts.Request.Admin adminRequest);
}