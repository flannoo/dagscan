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
    l0ApiUrl: string;
    l1DataApiUrl: string;
    l1CurrencyApiUrl: string;
}

export type OnChainSnapshot = {
    ordinal: number;
    dataApplication: DataApplication;
    messages: any[];
};

export type DataApplication = {
    onChainState: string[];
}

export type WalletRichlistInfo = {
    rank: number;
    address: string;
    tag: string;
    balance: number;
    usdValue: number;
    supplyPercentage: number;
}

export type SnapshotMetric = {
    snapshotDate: string;
    metagraphAddress: string;
    isTimeTriggered: boolean;
    totalSnapshotCount: number;
    totalSnapshotFeeAmount: number;
    totalTransactionCount: number;    
    totalTransactionAmount: number;
    totalTransactionFeeAmount: number;
}

export type Reward = {
    walletAddress: string;
    transactionDate: string;
    metagraphAddress: string;
    amount: number;
    rewardCategory: string;
    transactionHash: string;
    ordinal: number;
    currencySymbol: string;
};

export type MetagraphBalance = {
    metagraphAddress: string;
    tokenSymbol: string;
    balance: number;
};

export type Balance = {
    walletAddress: string;
    balance: number;
    metagraphBalances: MetagraphBalance[];
};
