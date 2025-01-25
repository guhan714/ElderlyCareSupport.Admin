using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ElderlyCareSupport.Admin.WebApi.Abstractions;

public class ValidateMacAddressAttribute : ActionFilterAttribute
{
    private readonly string _allowedMacAddress;

    public ValidateMacAddressAttribute()
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