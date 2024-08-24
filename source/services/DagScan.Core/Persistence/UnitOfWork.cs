using Microsoft.EntityFrameworkCore;

namespace DagScan.Core.Persistence;

public sealed class EfUnitOfWork<TContext> : IUnitOfWork, IEfUnitOfWork
    where TContext : DbContext
{
    public EfUnitOfWork(TContext dbContext)
    {
        DbContext = dbContext;
    }

    public DbContext DbContext { get; private set; }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
