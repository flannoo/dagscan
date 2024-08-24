using System.Security.Cryptography;
using System.Text;
using DagScan.Application;
using DagScan.Application.Features.UpdateValidatorNodeLocations;
using DagScan.Core.Scheduling;
using Hangfire;
using NGuid;

namespace DagScan.Worker.Extensions;

public static class HangfireExtensions
{
    public static HostApplicationBuilder AddHangfire(
        this HostApplicationBuilder builder,
        string databaseConnectionString)
    {
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(databaseConnectionString));

        builder.Services.AddHangfireServer((options) =>
            {
                options.WorkerCount = 4;
                options.Queues = ["default", "ip-lookup"];
            }
        );

        builder.Services.Scan(scanner => scanner
            .FromAssembliesOf(typeof(IApplicationMarker))
            .AddClasses(classes => classes.AssignableTo<IJob>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        builder.Services.AddTransient<UpdateHypergraphValidatorNodeLocationJob>();

        return builder;
    }

    public static void InitRecurringJobs(this IServiceProvider serviceProvider)
    {
        var namespaceGuid = Guid.Parse("CF79A377-E302-49BF-9162-9B712029799C");
        using var serviceScope = serviceProvider.CreateScope();
        var jobs = serviceScope.ServiceProvider.GetServices<IJob>();
        var recurringJobManager = serviceScope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        foreach (var job in jobs)
        {
            var jobIdGuid = GuidHelpers.CreateFromName(namespaceGuid, job.GetType().FullName!);
            var jobName = job.GetType().Name;
            var jobId = $"{jobName}-{jobIdGuid}";
            recurringJobManager.AddOrUpdate(jobId, () => job.Execute(), job.Schedule);
        }
    }
}
