using ElderlyCareSupport.Admin.Application;
using ElderlyCareSupport.Admin.Infrastructure;
using ElderlyCareSupport.Admin.Logging;
using ElderlyCareSupport.Admin.WebApi.Configuration;
using ElderlyCareSupport.Admin.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddLoggingFactory(builder.Configuration);

builder.Services.AddScoped<GlobalErrorHandler>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthenticationConfiguration();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandler>();

app.MapControllers();

await app.RunAsync();