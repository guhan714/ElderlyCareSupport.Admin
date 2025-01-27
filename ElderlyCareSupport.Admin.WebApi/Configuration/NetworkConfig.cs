namespace ElderlyCareSupport.Admin.WebApi.Configuration;

public static class NetworkConfig
{
    public static IApplicationBuilder UseNetworkConfig(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");
            await next();
        });
        
        app.UseCsp(options =>
        {
            options.BlockAllMixedContent()
                .StyleSources(s => s.Self())
                .FontSources(s => s.Self())
                .ImageSources(s => s.Self())
                .ScriptSources(s => s.Self())
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ConnectSources(s => s.Self());
        });
        
        app.UseXfo(options => options.Deny());
        app.UseXXssProtection(options => options.EnabledWithBlockMode());
        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(options => options.StrictOriginWhenCrossOrigin());
        return app;
    }
}