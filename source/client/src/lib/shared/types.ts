export type ValidatorNode = {
    walletAddress: string;
    walletId: string;
    ipAddress: string;
    nodeStatus: string;
    isInConsensus: boolean;
    serviceProvider: string;
    country: string;
    city: string;
    latitude: number;
    longitude: number;
};

export type Metagraph = {
    metagraphAddress: string;
    name: string;
    symbol: string;
    feeAddress: string;
    companyName: string;
    website: string;
    description: string;
}