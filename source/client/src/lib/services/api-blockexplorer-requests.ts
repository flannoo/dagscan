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

export type Transactions = {
    data: Transaction[];
    meta: {
        next: string;
    };
};

export async function getLatestTransactions(metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/transactions?limit=5`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/transactions?limit=5`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const transactions = data.data as Transaction[];
    return transactions;
}

export async function getTransactions(metagraphId?: string, next?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let url: string;
    if (!metagraphId) {
        url = `${apiUrl}/transactions?limit=14${next ? `&next=${next}` : ''}`;
    } else {
        url = `${apiUrl}/currency/${metagraphId}/transactions?limit=14${next ? `&next=${next}` : ''}`;
    }

    const res = await fetch(url);
    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const transactions = data as Transactions;
    return transactions;
}

export async function getLatestSnapshots(metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/global-snapshots?limit=5`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/snapshots?limit=5`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const snapshots = data.data as Snapshot[];
    return snapshots;
}

export async function getSnapshots(metagraphId?: string, next?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let url: string;
    if (!metagraphId) {
        url = `${apiUrl}/global-snapshots?limit=14${next ? `&next=${next}` : ''}`;
    } else {
        url = `${apiUrl}/currency/${metagraphId}/snapshots?limit=14${next ? `&next=${next}` : ''}`;
    }

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const snapshots = data as Snapshots;
    return snapshots;
}

export async function getSnapshotDetail(snapshotId: string, metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/global-snapshots/${snapshotId}`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/snapshots/${snapshotId}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const snapshot = data.data as Snapshot;
    return snapshot;
}

export async function getSnapshotDetailRewards(snapshotId: string, metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/global-snapshots/${snapshotId}/rewards`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/snapshots/${snapshotId}/rewards`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const rewards = data.data as Reward[];
    return rewards;
}

export async function getSnapshotDetailTransactions(snapshotId: string, metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;

    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/global-snapshots/${snapshotId}/transactions`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/snapshots/${snapshotId}/transactions`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const transactions = data.data as Transaction[];
    return transactions;
}

export async function getTransactionDetail(transactionId: string, metagraphId?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_BLOCKEXPLORER_URL;
    let res;
    if (!metagraphId) {
        res = await fetch(`${apiUrl}/transactions/${transactionId}`);
    } else {
        res = await fetch(`${apiUrl}/currency/${metagraphId}/transactions/${transactionId}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const transaction = data.data as Transaction;
    return transaction;
}
