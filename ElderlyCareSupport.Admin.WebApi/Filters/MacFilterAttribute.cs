using System.Net;
using System.Text.Json;
using ElderlyCareSupport.Admin.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderlyCareSupport.Admin.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MacFilterAttribute : ActionFilterAttribute
{
    private readonly string _allowedMac;

    public MacFilterAttribute()
    {
        _allowedMac = Environment.GetEnvironmentVariable("MAC_ADDRESS")!;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("MAC-Address", out var macAddress))
        {
            DenyAccess(context, "Mac address is missing.", HttpStatusCode.ExpectationFailed);
            return;
        }
        
        if (context.HttpContext.User.Identity!.IsAuthenticated && macAddress.ToString() != _allowedMac)
        {
            DenyAccess(context, $"{macAddress} is not allowed.", HttpStatusCode.Forbidden);
            return;
        }
        
        if (macAddress.ToString() != _allowedMac)
        {
            DenyAccess(context, $"{macAddress} is not allowed.", HttpStatusCode.ExpectationFailed);
            return;
        }
        
        base.OnActionExecuting(context);
    }

    private static void DenyAccess(ActionExecutingContext context, string message, HttpStatusCode statusCode)
    {
        var result = new
        {
            Success = false,
            StatusCode = statusCode,
            Data = Enumerable.Empty<string>(),
            Errors = new List<Error> { new(message) }
        } ;
        
        context.Result = new ObjectResult(result) { StatusCode = (int)statusCode };
    }
    
}