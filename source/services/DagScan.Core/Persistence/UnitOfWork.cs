using Microsoft.EntityFrameworkCore;

namespace DagScan.Core.Persistence;

public sealed class EfUnitOfWork<TContext>(TContext dbContext) : IUnitOfWork, IEfUnitOfWork
    where TContext : DbContext
{
    public DbContext DbContext { get; private set; } = dbContext;

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
