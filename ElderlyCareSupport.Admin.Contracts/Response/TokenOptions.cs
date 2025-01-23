using System.Text.Json.Serialization;

namespace ElderlyCareSupport.Admin.Contracts.Response;

public record TokenResponse(
    [property: JsonPropertyName("accessToken")]
    string AccessToken,
    [property: JsonPropertyName("expiresIn")]
    DateTime? ExpiresIn,
    [property: JsonPropertyName("refreshToken")]
    string RefreshToken
);