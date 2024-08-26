export type Transaction = {
    hash: string;
    timestamp: string;
    amount: number;
    fee: number; 
    source: string;
    destination: string;
    blockHash: string;
    snapshotHash: string;
    snapshotOrdinal: number;
};

export type Block = string;

export type Reward = {
    destination: string;
    amount: number;
};

export type Snapshot = {
    hash: string;
    ordinal: number;
    lastSnapshotHash: string;
    timestamp: string;
    blocks: Block[];
    rewards: Reward[];
    fee: number;
    ownerAddress: string;
    sizeInKb: number;
};

export type Snapshots = {
    data: Snapshot[];
    meta: {
        next: string;
    };
};

export async function getLatestTransactions() {
    const res = await fetch('https://be-mainnet.constellationnetwork.io/transactions?limit=5');

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const transactions = data.data as Transaction[];
    return transactions;
}

export async function getSnapshotDetail(snapshotId: string, metagraphId: string) {
    let res;
    
    if (metagraphId === '' || metagraphId === 'DAG') {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/global-snapshots/${snapshotId}`);
    } else {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/snapshots/${snapshotId}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const snapshot = data.data as Snapshot;
    return snapshot;
}

export async function getSnapshotDetailRewards(snapshotId: string, metagraphId: string) {
    let res;
    
    if (metagraphId === '' || metagraphId === 'DAG') {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/global-snapshots/${snapshotId}/rewards`);
    } else {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/snapshots/${snapshotId}/rewards`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const rewards = data.data as Reward[];
    return rewards;
}

export async function getSnapshotDetailTransactions(snapshotId: string, metagraphId: string) {
    let res;
    
    if (metagraphId === '' || metagraphId === 'DAG') {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/global-snapshots/${snapshotId}/transactions`);
    } else {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/snapshots/${snapshotId}/transactions`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const transactions = data.data as Transaction[];
    return transactions;
}

export async function getLatestSnapshots() {
    const res = await fetch('https://be-mainnet.constellationnetwork.io/global-snapshots?limit=5');

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const snapshots = data.data as Snapshot[];
    return snapshots;
}

export async function getSnapshots(metagraphId: string, next: string) {
    let res;
    
    if (metagraphId === '' || metagraphId === 'DAG') {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/global-snapshots/${snapshotId}`);
    } else {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/snapshots/${snapshotId}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const snapshot = data.data as Snapshot;
    return snapshot;
}

export async function getLatestTransactionsMetagraph(metagraphId : string) {
    const res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/transactions?limit=5`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const transactions = data.data as Transaction[];
    return transactions;
}

export async function getLatestSnapshotsMetagraph(metagraphId : string) {
    const res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/snapshots?limit=5`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const snapshots = data.data as Snapshot[];
    return snapshots;
}

export async function getTransactionDetail(transactionId: string, metagraphId: string) {
    let res;
    
    if (metagraphId === '' || metagraphId === 'DAG') {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/transactions/${transactionId}`);
    } else {
        res = await fetch(`https://be-mainnet.constellationnetwork.io/currency/${metagraphId}/transactions/${transactionId}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const transaction = data.data as Transaction;
    return transaction;
}
