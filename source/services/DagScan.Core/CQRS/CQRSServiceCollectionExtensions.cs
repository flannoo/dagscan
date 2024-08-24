using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DagScan.Core.CQRS;

public static class CQRSServiceCollectionExtensions
{
    public static IServiceCollection AddCQRS(
        this IServiceCollection services,
        Assembly[] assemblies,
        params Type[]? pipelines
    )
    {
        services.AddMediatR(cfg =>
        {
            foreach (var assembly in assemblies)
            {
                cfg.RegisterServicesFromAssembly(assembly);
            }

            if (pipelines == null)
            {
                return;
            }

            foreach (var pipeline in pipelines)
            {
                cfg.AddOpenBehavior(pipeline);
            }
        });

        return services;
    }
}
