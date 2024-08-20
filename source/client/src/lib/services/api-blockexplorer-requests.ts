export type Transaction = {
    hash: string;
    timestamp: string;
    amount: number;
    fee: number;
};

export type Snapshot = {
    hash: string;
    ordinal: number;
    timestamp: string;
    amount: number;
    blocks: any[];
    rewards: any[];
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

export async function getLatestSnapshots() {
    const res = await fetch('https://be-mainnet.constellationnetwork.io/global-snapshots?limit=5');

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const snapshots = data.data as Snapshot[];
    return snapshots;
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
