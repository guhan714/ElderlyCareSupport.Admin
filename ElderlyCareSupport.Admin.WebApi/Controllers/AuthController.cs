using System.Net;
using Asp.Versioning;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Infrastructure.Services;
using ElderlyCareSupport.Admin.Shared.Messages;
using ElderlyCareSupport.Admin.WebApi.Abstractions;
using ElderlyCareSupport.Admin.WebApi.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[ApiVersion(1)]
[Route("v{v:apiVersion}/auth")]
[ApiController]
[Produces("application/json")]
[AllowAnonymous]
[MacFilter]
public class AuthController : BaseController
{
    private readonly IKeycloakAdminService _adminService;
    private readonly TokenProvider _tokenProvider;
    private readonly IConfiguration _configuration;
    private readonly IValidator<Contracts.Request.Admin> _adminValidator;

    public AuthController(IKeycloakAdminService adminService,
        TokenProvider tokenProvider, IConfiguration configuration,
        IValidator<Contracts.Request.Admin> adminValidator) : base()
    {
        _adminService = adminService;
        _tokenProvider = tokenProvider;
        _configuration = configuration;
        _adminValidator = adminValidator;
    }

    [MapToApiVersion(1)]
    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateAdmin([FromBody] Contracts.Request.Admin adminRequest)
    {
        var validateResult = await _adminValidator.ValidateAsync(adminRequest);
        if (!validateResult.IsValid)
        {
            return ErrorResponse(validateResult);
        }

        var (authenticatedResponse, isAuthenticated) = await _adminService.AuthenticateAdminAsync(adminRequest);
        if (!isAuthenticated)
            return ApiResponse(isAuthenticated, HttpStatusCode.Unauthorized, authenticatedResponse,
                [new Error(Messages.InvalidCredentials)]);

        return ApiResponse(
            isAuthenticated,
            HttpStatusCode.OK,
            authenticatedResponse);
    }

    [MapToApiVersion(1)]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAction()
    {
        return ApiResponse(
            true,
            HttpStatusCode.OK,
            Enumerable.Empty<string>()
        );
    }

    [MapToApiVersion(1)]
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
            var error = new Error("NOT FOUND");
            return ApiResponse(
                false,
                HttpStatusCode.InternalServerError,
                Enumerable.Empty<string>(),
                [error]);
        }

        return ApiResponse(
            true,
            HttpStatusCode.OK,
            response);
    }
}