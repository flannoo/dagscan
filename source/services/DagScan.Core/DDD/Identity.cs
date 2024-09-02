namespace DagScan.Core.DDD;

public abstract record Identity<TId>
{
    public TId Value { get; init; } = default!;

    public static implicit operator TId(Identity<TId> identityId) => identityId.Value;

    public override string ToString()
    {
        return IdAsString();
    }

    private string IdAsString()
    {
        return $"{GetType().Name} [InternalCommandId={Value}]";
    }
}
