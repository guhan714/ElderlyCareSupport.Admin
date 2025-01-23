using System.Net;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[Route("auth")]
[ApiController]
[AllowAnonymous]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IResponseCreator _responseCreator;
    private readonly IKeycloakAdminService _adminService;
    private readonly TokenProvider _tokenProvider;
    private readonly IConfiguration _configuration;

    public AuthController(IResponseCreator responseCreator, IKeycloakAdminService adminService,
        TokenProvider tokenProvider, IConfiguration configuration)
    {
        _responseCreator = responseCreator;
        _adminService = adminService;
        _tokenProvider = tokenProvider;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateAdmin([FromBody] Contracts.Request.Admin adminRequest)
    {
        var authenticatedResponse = await _adminService.AuthenticateAdminAsync(adminRequest);
        if (!authenticatedResponse.Item2)
            return Unauthorized(_responseCreator.CreateResponse(authenticatedResponse.Item2,
                HttpStatusCode.Unauthorized, authenticatedResponse.Item1,
                [new Error("NOT FOUND")]));

        return Ok(_responseCreator.CreateResponse(
            true,
            HttpStatusCode.OK,
            authenticatedResponse,
            Enumerable.Empty<Error>()));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAction()
    {
        return Ok(_responseCreator.CreateResponse(
            true,
            HttpStatusCode.OK,
            // ReSharper disable once UseCollectionExpression
            Enumerable.Empty<string>(),
            // ReSharper disable once UseCollectionExpression
            Enumerable.Empty<Error>()));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenResponse tokenResponse)
    {
        var newAccessToken =
            _tokenProvider.GenerateAccessTokenFromRefreshToken(tokenResponse.RefreshToken,
                _configuration["JWT:Secret"]!);

        var response = new TokenResponse(
            AccessToken: newAccessToken,
            RefreshToken: tokenResponse.RefreshToken,
            ExpiresIn: tokenResponse.ExpiresIn);

        if (string.IsNullOrEmpty(response.AccessToken))
        {
            return NotFound(_responseCreator.CreateResponse(
                false,
                HttpStatusCode.InternalServerError,
                Enumerable.Empty<string>(),
                [new Error("NOT FOUND")]));
        }

        return Ok(_responseCreator.CreateResponse(
            true,
            HttpStatusCode.OK,
            response,
            Enumerable.Empty<Error>()));
    }
}