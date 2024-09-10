using DagScan.Application.Domain.ValueObjects;
using DagScan.Core.DDD;

namespace DagScan.Application.Domain;

public sealed class CurrencyPrice : Entity<int>
{
    public DateOnly Date { get; private init; }
    public MetagraphAddress? MetagraphAddress { get; private init; }
    public decimal Price { get; private set; }

    public static CurrencyPrice Create(MetagraphAddress? metagraphAddress, decimal price, DateOnly date)
    {
        return new CurrencyPrice() { Date = date, MetagraphAddress = metagraphAddress, Price = price };
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }
}
