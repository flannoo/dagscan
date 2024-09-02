namespace DagScan.Application.Features.SyncHypergraphValidatorNodes;

public sealed record HypergraphInConsensusPeers(
    string Id
);

public sealed record HypergraphInConsensusInfo(
    List<HypergraphInConsensusPeers> Peers
);
