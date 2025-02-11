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

        var securityKey = GetSecurityKey();
        var tokenDescriptor = GetSecurityTokenDescriptor(securityKey, admin.Username, admin.Role);
        var token = GenerateJwtToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        
        var tokenResponse = CreateTokenResponse(token,tokenDescriptor.Expires ,refreshToken);
        var success = string.IsNullOrEmpty(token);
        return Tuple.Create(tokenResponse, !success);
    }

    private SecurityTokenDescriptor GetSecurityTokenDescriptor(SymmetricSecurityKey key, string userName, string role)
    {
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var expiryTime = int.Parse(configuration["JWT:TokenLifetime"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, userName),
                new Claim(ClaimTypes.Role, role),
            ]),
            Expires = DateTime.Now.AddMinutes(expiryTime),
            SigningCredentials = credentials,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
        };
        
        return tokenDescriptor;
    }

    private static string GenerateRefreshToken()
    {
        Span<byte> randomNumber = stackalloc byte[64]; 
        RandomNumberGenerator.Fill(randomNumber);
        return Convert.ToBase64String(randomNumber.ToArray());
    }

    private SymmetricSecurityKey GetSecurityKey()
    {
        string secretKey = configuration["JWT:Secret"]!;
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    private string GenerateJwtToken(SecurityTokenDescriptor tokenDescriptor)
    {
        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
    
    private TokenResponse CreateTokenResponse(string accessToken, DateTime? expiresOn, string refreshToken)
    {
        return new TokenResponse(
            AccessToken: accessToken,
            ExpiresIn: expiresOn,
            RefreshToken: refreshToken);
    }
    
    public string GenerateAccessTokenFromRefreshToken(string refreshToken, string securityKey)
    {
        var handler = new JsonWebTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        var expires = int.Parse(configuration["JWT:TokenLifetime"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(expires),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}