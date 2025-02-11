using System.Net;
using ElderlyCareSupport.Admin.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderlyCareSupport.Admin.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface |
                AttributeTargets.Delegate)]
internal sealed class MacAddressFilterAttribute : ActionFilterAttribute
{
    private readonly string _allowedMacAddress;

    public MacAddressFilterAttribute()
    {
        _allowedMacAddress = Environment.GetEnvironmentVariable("MAC_ADDRESS")!;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("MAC-Address", out var macAddress) ||
            macAddress.ToString() != _allowedMacAddress)
        {
            var result = new
            {
                Success = false,
                StatusCode = HttpStatusCode.Forbidden,
                Data = Enumerable.Empty<string>(),
                Errors = new List<Error> { new("Mac Address not found") }
            };

            context.Result = new ObjectResult(result) { StatusCode = (int)HttpStatusCode.Forbidden };
        }

        base.OnActionExecuting(context);
    }
}