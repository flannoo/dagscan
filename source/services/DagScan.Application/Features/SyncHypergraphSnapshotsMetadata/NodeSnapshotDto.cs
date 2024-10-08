﻿namespace DagScan.Application.Features.SyncHypergraphSnapshotsMetadata;

public sealed class NodeSnapshotDto
{
    public Value Value { get; set; } = default!;
    public List<Proof> Proofs { get; set; } = default!;
}

public sealed class Value
{
    public long Ordinal { get; set; }
    public long? Height { get; set; }
    public long? SubHeight { get; set; }
    public string? LastSnapshotHash { get; set; }
    public List<BlockContainer>? Blocks { get; set; }
    public Dictionary<string, List<StateChannelSnapshot>>? StateChannelSnapshots { get; set; }
    public List<object>? Rewards { get; set; }
    public long? EpochProgress { get; set; }
    public List<string>? NextFacilitators { get; set; }
    public object? Tips { get; set; } = default!;
    public StateProof? StateProof { get; set; }
    public string? Version { get; set; }
}

public sealed class StateChannelSnapshot
{
    public StateChannelValue? Value { get; set; }
    public List<Proof>? Proofs { get; set; }
}

public sealed class StateChannelValue
{
    public string? LastSnapshotHash { get; set; }
    public List<object>? Content { get; set; }
    public long Fee { get; set; }
}

public sealed class Proof
{
    public string Id { get; set; } = default!;
    public string Signature { get; set; } = default!;
}

public sealed class StateProof
{
    public string? LastStateChannelSnapshotHashesProof { get; set; }
    public string? LastTxRefsProof { get; set; }
    public string? BalancesProof { get; set; }
    public object? LastCurrencySnapshotsProof { get; set; }
}

public class TransactionValue
{
    public string? Source { get; set; }
    public string? Destination { get; set; }
    public long? Amount { get; set; }
    public long? Fee { get; set; }
    public object? Parent { get; set; }
    public object? Salt { get; set; }
}

public class Transaction
{
    public TransactionValue? Value { get; set; }
    public List<object>? Proofs { get; set; }
}

public class BlockValue
{
    public List<object>? Parent { get; set; }
    public List<Transaction>? Transactions { get; set; }
}

public class Block
{
    public BlockValue? Value { get; set; }
    public List<object>? Proofs { get; set; }
}

public class BlockContainer
{
    public Block? Block { get; set; }
    public long? UsageCount { get; set; }
}

