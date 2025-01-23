using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Infrastructure.Services;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.Users;

public class AuthenticationRepository : IKeycloakAdminRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly TokenProvider _tokenProvider;

    public AuthenticationRepository(IDbConnectionFactory dbConnectionFactory, TokenProvider tokenProvider)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _tokenProvider = tokenProvider;
    }

    public async Task<Tuple<TokenResponse, bool>> AuthenticateAdminAsync(Contracts.Request.Admin admin)
    {
        var authenticatedResponse = _tokenProvider.CreateToken(admin);
        return Tuple.Create(authenticatedResponse.Item1, authenticatedResponse.Item2);
    }
}