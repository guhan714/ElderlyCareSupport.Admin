using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderlyCareSupport.Admin.WebApi.Abstractions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate)]
public sealed class MacAddressFilterAttribute : ActionFilterAttribute
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
            context.Result = new ForbidResult();
        }

        base.OnActionExecuting(context);
    }
}