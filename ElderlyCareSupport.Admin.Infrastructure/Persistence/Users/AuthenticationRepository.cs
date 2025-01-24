using ElderlyCareSupport.Admin.Application.IRepository;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Infrastructure.Services;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence.Users;

public class AuthenticationRepository : IKeycloakAdminRepository
{
    private readonly TokenProvider _tokenProvider;
    private readonly IHashingService _hashingService;

    public AuthenticationRepository(TokenProvider tokenProvider,
        IHashingService hashingService)
    {
        _tokenProvider = tokenProvider;
        _hashingService = hashingService;
    }

    public async Task<Tuple<TokenResponse, bool>> AuthenticateAdminAsync(Contracts.Request.Admin admin)
    {
        var userId = Environment.GetEnvironmentVariable("ADMIN_USERNAME", EnvironmentVariableTarget.Machine)!;
        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD", EnvironmentVariableTarget.Machine)!;

        var isValidPassword = _hashingService.VerifyPassword(admin.Password, password!);
        if (admin.Username != userId || !isValidPassword)
            return Tuple.Create(
                new TokenResponse(AccessToken: string.Empty, ExpiresIn: DateTime.MinValue, RefreshToken: string.Empty),
                false);

        var authenticatedResponse = _tokenProvider.CreateToken(admin);
        return Tuple.Create(authenticatedResponse.Item1, authenticatedResponse.Item2);
    }
}