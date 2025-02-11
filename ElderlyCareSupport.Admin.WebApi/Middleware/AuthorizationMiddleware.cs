using System.Net;
using ElderlyCareSupport.Admin.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace ElderlyCareSupport.Admin.WebApi.Middleware;

public class AuthorizationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            await DenyAccess(context, $"User is not authenticated.", HttpStatusCode.Unauthorized);
        }
    }

    private async Task DenyAccess(HttpContext context, string message, HttpStatusCode statusCode)
    {
        var result = new
        {
            Success = false,
            StatusCode = statusCode,
            Data = Enumerable.Empty<string>(),
            Errors = new List<Error> { new(message) }
        };
        var objectResult = new ObjectResult(result) { StatusCode = (int)statusCode };
        await context.Response.WriteAsJsonAsync(objectResult.Value);
    }
}