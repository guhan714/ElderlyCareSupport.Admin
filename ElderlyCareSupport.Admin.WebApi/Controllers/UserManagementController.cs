using System.Net;
using Asp.Versioning;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Request;
using ElderlyCareSupport.Admin.Contracts.Response;
using ElderlyCareSupport.Admin.Domain.Models;
using ElderlyCareSupport.Admin.Shared.Messages;
using ElderlyCareSupport.Admin.WebApi.Abstractions;
using ElderlyCareSupport.Admin.WebApi.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[ApiVersion(1)]
[ApiController]
[Produces("application/json")]
[Route("api/v{v:apiVersion}/[controller]/users")]
[Authorize(Roles = "Admin")]
[MacFilter]
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

    [HttpGet("")]
    [ApiVersion(1)]
    public async Task<IActionResult> GetAllUsers([FromQuery] PageQueryParameters userQueryParameters)
    {
        var users = await _userService.GetAllUsersAsync(userQueryParameters);
        var response = ApiResponse(
            true,
            HttpStatusCode.OK,
            users);
        return response;
    }


    [HttpGet("{userId}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var isValidUserId = await _emailValidator.ValidateAsync(userId);
        if (!isValidUserId.IsValid)
            return ErrorResponse(isValidUserId);

        var user = await _userService.GetUserByIdAsync(userId);
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.FirstName))
        {
            return ApiResponse(
                false,
                HttpStatusCode.NotFound,
                user,
                [new Error(Messages.UserNotFound)]);
        }


        var response = ApiResponse(
            true,
            statusCode: HttpStatusCode.OK,
            data: user);
        return response;
    }


    [HttpPost("create")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        var validUser = await _userValidator.ValidateAsync(user);
        if (!validUser.IsValid)
            return ErrorResponse(validUser);

        var (data, success) = await _userService.AddUserAsync(user);
        if (!success)
            return ApiResponse(success, HttpStatusCode.InternalServerError, data, [new Error(Messages.InternalServerError)]);

        return ApiResponse(success, HttpStatusCode.Created, data);
    }


    [HttpPut("update")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateUserByEmail([FromBody] User user)
    {
        var validationResult = await _userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return ErrorResponse(validationResult);
        }

        var (data, success) = await _userService.UpdateUserAsync(user);
        if (!success)
        {
            return ApiResponse(success, HttpStatusCode.InternalServerError,
                data, [new Error(Messages.InternalServerError)]);
        }

        return ApiResponse(
            success,
            HttpStatusCode.NoContent,
            data);
    }


    [HttpDelete("{userId}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var validationResult = await _emailValidator.ValidateAsync(userId);
        if (!validationResult.IsValid)
            return ErrorResponse(validationResult);

        var (data, success) = await _userService.DeleteUserAsync(userId);
        if (!success)
        {
            return ApiResponse(false, HttpStatusCode.Conflict, data, [new Error(Messages.InternalServerError)]);
        }

        return ApiResponse(success, HttpStatusCode.OK, data);
    }
}