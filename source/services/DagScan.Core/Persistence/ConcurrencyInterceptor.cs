using DagScan.Core.DDD;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DagScan.Core.Persistence;

public sealed class ConcurrencyInterceptor : SaveChangesInterceptor
{
    public ConcurrencyInterceptor()
    {
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IHaveAggregateVersion>())
        {
            entry.CurrentValues[nameof(IHaveAggregateVersion.ConcurrencyVersion)] = entry.Entity.ConcurrencyVersion + 1;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
