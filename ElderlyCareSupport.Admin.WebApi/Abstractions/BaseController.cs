using System.Net;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ElderlyCareSupport.Admin.WebApi.Abstractions;

public abstract class BaseController : ControllerBase
{
    protected IActionResult ApiResponse<T>(
        bool success,
        HttpStatusCode statusCode,
        T data,
        IEnumerable<Error>? errors = null)
    {
        var response =  new
        {
            Success = success,
            StatusCode = statusCode,
            Data = data,
            Errors = errors ?? Enumerable.Empty<Error>()
        };
        
        return new ObjectResult(response) {StatusCode = (int)statusCode};
    }


    protected IActionResult HandleValidationErrors(ValidationResult validationResult)
    {
        var response = new
        {
            Success = false,
            StatusCode = 400,
            Data = Enumerable.Empty<string>(),
            Errors = validationResult.Errors.Select(x => new Error(x.ErrorMessage))
        };
        
        return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
    }
}