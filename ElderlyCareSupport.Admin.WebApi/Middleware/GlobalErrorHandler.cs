using System.Net;
using System.Text.Json;
using ElderlyCareSupport.Admin.Contracts.Response;
using ILoggerFactory = ElderlyCareSupport.Admin.Logging.ILogger.ILoggerFactory;

namespace ElderlyCareSupport.Admin.WebApi.Middleware;

public class GlobalErrorHandler : IMiddleware
{
    private readonly ILoggerFactory _loggerFactory;

    public GlobalErrorHandler(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _loggerFactory.LogError(e.Message, e);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var error = new Error(exception.Message);

        var apiResponse = new ApiResponse<IEnumerable<string>>
        (
            Success: false,
            StatusCode: (HttpStatusCode)context.Response.StatusCode,
            Data: Enumerable.Empty<string>(),
            Errors: new List<Error>() { error }
        );

        await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse));
    }
}