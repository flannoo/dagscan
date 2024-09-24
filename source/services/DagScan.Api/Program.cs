using System.Reflection;
using Carter;
using DagScan.Application;
using DagScan.Application.Data;
using DagScan.Core.CQRS;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string defaultCorsPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(defaultCorsPolicyName, policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var databaseConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               throw new ArgumentException("DB_CONNECTION_STRING environment variable not found.");

builder.Services.AddDbContext<DagContext>(options => { options.UseSqlServer(databaseConnectionString); });
builder.Services.AddDbContext<ReadOnlyDagContext>(options => { options.UseSqlServer(databaseConnectionString); });

if (environment.IsDevelopment())
{
    builder.Services.AddAuthentication();
}
else
{
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("DAGSCAN_API:AzureAd"));
}

builder.Services.AddAuthorization();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(databaseConnectionString));

builder.Services
    .AddCqrs(
        new[] { Assembly.GetExecutingAssembly(), typeof(IApplicationMarker).Assembly }
    );

builder.Services.AddCarter();
builder.Services.AddResponseCompression();

builder.Services.AddHttpClient();

/*builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto |
                               ForwardedHeaders.XForwardedHost;
});*/

var app = builder.Build();

//app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseCors(defaultCorsPolicyName);
app.UseResponseCompression();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapHangfireDashboard("/hangfire", new DashboardOptions { Authorization = [new HangfireAnonymousFilter()] });
}
else
{
    app.MapHangfireDashboard("/hangfire", new DashboardOptions { Authorization = [new HangfireAuthFilter()] })
        .RequireAuthorization();
}

app.MapCarter();

app.Run();

internal sealed class HangfireAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin");
    }
}

internal sealed class HangfireAnonymousFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
