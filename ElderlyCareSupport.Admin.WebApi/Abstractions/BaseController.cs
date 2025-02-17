using System.Net;
using ElderlyCareSupport.Admin.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ElderlyCareSupport.Admin.WebApi.Abstractions;

public abstract class BaseController : ControllerBase
{
    protected static IActionResult SuccessResult<T>(
        HttpStatusCode statusCode,
        T data)
    {
        var response =  new
        {
            Success = true,
            StatusCode = statusCode,
            Data = data,
            Errors = Enumerable.Empty<Error>()
        };
        
        return new ObjectResult(response) {StatusCode = (int)statusCode};
    }

    protected static IActionResult FailureResult(HttpStatusCode statusCode, List<Error> errors)
    {
        var response =  new
        {
            Success = false,
            StatusCode = statusCode,
            Data = Enumerable.Empty<string>(),
            Errors = errors
        };
        return new ObjectResult(response) {StatusCode = (int)statusCode};
    }

    protected static IActionResult ValidationErrorResult(ValidationResult validationResult)
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