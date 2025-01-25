namespace ElderlyCareSupport.Admin.WebApi.Middleware;

public sealed class MacMiddleware : IMiddleware
{
    private readonly string _allowedMac = Environment.GetEnvironmentVariable("MAC_ADDRESS")!;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue("MAC-Address", out var macAddress) && context.User.Identity!.IsAuthenticated && macAddress.ToString() != _allowedMac)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied. Please try again. Mac Address is required.");
            return;
        }
        await next(context);
    }
}