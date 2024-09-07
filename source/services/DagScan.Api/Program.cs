using System.Reflection;
using Carter;
using DagScan.Application;
using DagScan.Application.Data;
using DagScan.Core.CQRS;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new [] { new HangfireAuthFilter() }
    });
}

app.UseHttpsRedirection();
app.UseCors(defaultCorsPolicyName);
app.UseResponseCompression();

app.MapCarter();

app.Run();

internal sealed class HangfireAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
