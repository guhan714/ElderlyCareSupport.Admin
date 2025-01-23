using System.Net;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserManagementController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IResponseCreator _responseCreator;
    private readonly IValidator<string> _userValidator;

    public UserManagementController(IUserService userService, IResponseCreator responseCreator,
        IValidator<string> userValidator)
    {
        _userService = userService;
        _responseCreator = responseCreator;
        _userValidator = userValidator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParameters userQueryParameters)
    {
        var users = await _userService.GetAllUsersAsync(userQueryParameters);
        var response = _responseCreator.CreateResponse(
            true,
            HttpStatusCode.OK,
            users,
            Enumerable.Empty<Error>());
        return Ok(response);
    }

    [Authorize(Roles = "User")]
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var isValidUserId = await _userValidator.ValidateAsync(userId);
        var responseContent = _responseCreator.CreateResponse(false, HttpStatusCode.BadRequest,
            Enumerable.Empty<string>(), isValidUserId.Errors.Select(e => new Error(e.ErrorMessage)));
        if (!isValidUserId.IsValid)
            return BadRequest(responseContent);

        var user = await _userService.GetUserByIdAsync(userId);
        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.FirstName))
            return NotFound(_responseCreator.CreateResponse(
                false,
                HttpStatusCode.NotFound,
                Enumerable.Empty<string>(),
                Enumerable.Empty<Error>()));


        var response = _responseCreator.CreateResponse(
            true,
            HttpStatusCode.OK,
            user,
            Enumerable.Empty<Error>());
        return Ok(response);
    }
}