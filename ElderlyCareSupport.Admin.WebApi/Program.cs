using ElderlyCareSupport.Admin.Application;
using ElderlyCareSupport.Admin.Infrastructure;
using ElderlyCareSupport.Admin.Logging;
using ElderlyCareSupport.Admin.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddLoggingFactory(builder.Configuration)
    .AddInfraServiceCollection()
    .AddVersioning();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseNetworkConfig();
app.UseAuthenticationConfiguration();
app.UseInfra();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors(o => o.WithOrigins("http://localhost:4200","https://localhost:44374/")
    .WithHeaders("Content-Type","Authorization","MAC-Address")
    .AllowAnyMethod());
app.UseHealthChecks(@"/health");
app.MapControllers();

await app.RunAsync();