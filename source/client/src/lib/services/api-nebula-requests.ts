export type Reward = {
    RewardType: string;
    Address: string;
    TransactionHash: string;
    Currency: string;
    TimeStamp: string;
    Amount: number;
};

export type SnapshotFee = {
    Timestamp: string;
    FeeAmount: number;
}

export type SnapshotCount = {
    Timestamp: string;
    Count: number;
}

export type Wallet = {
    Address: string;
    Balance: number;
    BalanceUSD: number;
    Rank: number;
    Tag: string;
    SupplyPercentage: number;
    LastSnapshotReward: string;
}

export async function getRewards(walletAddresses: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_NEBULA_URL;
    const res = await fetch(`${apiUrl}/dag/v1/rewards?address=${walletAddresses}&startDate=&endDate=`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const rewards = data as Reward[];
    return rewards;
}

export async function getSnapshotFees() {
    const apiUrl = process.env.NEXT_PUBLIC_API_NEBULA_URL;
    const res = await fetch(`${apiUrl}/dag/v1/snapshotfees`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const snapshotFees = data as SnapshotFee[];
    return snapshotFees;
}

export async function getSnapshotCount() {
    const apiUrl = process.env.NEXT_PUBLIC_API_NEBULA_URL;
    const res = await fetch(`${apiUrl}/dag/v1/snapshotcount`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const snapshotCount = data as SnapshotCount[];
    return snapshotCount;
}


export async function getMetagraphWallets(metagraphSymbol: string) {
    if (!metagraphSymbol) {
        return [];
    }

    const apiUrl = process.env.NEXT_PUBLIC_API_NEBULA_URL;

    let res;

    if (metagraphSymbol === 'DAG') {
        res = await fetch(`${apiUrl}/dag/v1/dag/wallets`);
    } else {

        res = await fetch(`${apiUrl}/dag/v1/metagraph-wallets?metagraph=${metagraphSymbol}`);
    }

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const wallets = data as Wallet[];
    return wallets;
}
