using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;

namespace ElderlyCareSupport.Admin.Application.Service;

public class AdminAuthentication : IKeycloakAdminService
{
    private readonly IKeycloakAdminRepository _authenticationRepository;

    public AdminAuthentication(IKeycloakAdminRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<Tuple<TokenResponse?, bool>> AuthenticateAdminAsync(Contracts.Request.Admin adminRequest)
    {
        var result = await _authenticationRepository.AuthenticateAdminAsync(adminRequest);
        return result;
    }
}