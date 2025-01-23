using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TokenResponse = ElderlyCareSupport.Admin.Contracts.Response.TokenResponse;

namespace ElderlyCareSupport.Admin.Infrastructure.Services;

public sealed class TokenProvider(IConfiguration configuration)
{
    public Tuple<TokenResponse,bool> CreateToken(Contracts.Request.Admin admin)
    {
        string secretKey = configuration["JWT:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var expiryTime = int.Parse(configuration["JWT:TokenLifetime"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, admin.Username),
                new Claim(JwtRegisteredClaimNames.Email, admin.Username),
                new Claim(ClaimTypes.Role, admin.Role),
            ]),
            Expires = DateTime.Now.AddMinutes(expiryTime),
            SigningCredentials = credentials,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        var tokenResponse = new TokenResponse(AccessToken: token,
            ExpiresIn: tokenDescriptor.Expires,
            RefreshToken: refreshToken);
        var success = string.IsNullOrEmpty(token);
        return Tuple.Create(tokenResponse, !success);
    }


    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64]; 
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateAccessTokenFromRefreshToken(string refreshToken, string securityKey)
    {
        var handler = new JsonWebTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        var expires = int.Parse(configuration["JWT:TokenLifetime"]!);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(expires),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}