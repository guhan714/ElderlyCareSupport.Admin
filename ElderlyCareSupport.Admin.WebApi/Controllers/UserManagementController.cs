using System.Net;
using Asp.Versioning;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.WebApi.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[ApiVersion(1)]
[ApiController]
[Produces("application/json")]
[Route("api/v{v:apiVersion}/[controller]/users")]

public class UserManagementController : BaseController
{
    private readonly IUserService _userService;
    private readonly IValidator<string> _emailValidator;
    private readonly IValidator<User> _userValidator;
    public UserManagementController(IUserService userService,
        IValidator<User> userValidator, IValidator<string> emailValidator)
    {
        _userService = userService;
        _userValidator = userValidator;
        _emailValidator = emailValidator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("")]
    [ApiVersion(1)]
    [MacAddressFilter]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParameters userQueryParameters)
    {
        var users = await _userService.GetAllUsersAsync(userQueryParameters);
        var response = ApiResponse(
            true,
            HttpStatusCode.OK,
            users);
        return response;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{userId}")]
    [MapToApiVersion(1)]
    [MacAddressFilter]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var isValidUserId = await _emailValidator.ValidateAsync(userId);
        if (!isValidUserId.IsValid)
            return HandleValidationErrors(isValidUserId);
        
        var user = await _userService.GetUserByIdAsync(userId);
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.FirstName))
            return ApiResponse(
                false,
                HttpStatusCode.NotFound,
                user,
                [new Error("User not found")]);

        var response = ApiResponse(
            true,
            statusCode:HttpStatusCode.OK,
            data:user);
        return response;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("update")]
    [MapToApiVersion(1)]
    [MacAddressFilter]
    public async Task<IActionResult> UpdateUserByEmail([FromBody] User user)
    {
        var validationResult = await _userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return HandleValidationErrors(validationResult);
        }

        var successfulUpdate = await _userService.UpdateUserAsync(user);
        if (!successfulUpdate.Item2)
        {
            return ApiResponse(successfulUpdate.Item2, HttpStatusCode.InternalServerError,
                successfulUpdate.Item1, [new Error("Internal Server Error")]);
        }

        return ApiResponse(
            successfulUpdate.Item2,
            HttpStatusCode.OK,
            successfulUpdate);
    }
}