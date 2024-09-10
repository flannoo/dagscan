import { Balance, Metagraph, MetagraphNode, Reward, SnapshotMetric, ValidatorNode, ValidatorNodeUptime, WalletRichlistInfo, WalletValidatorNodes } from "@/lib/shared/types";

export async function getMetagraphs(network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const res = await fetch(`${apiUrl}/metagraphs/${networkUrl}`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const metagraphs = data as Metagraph[];
    return metagraphs;
}

export async function getHypergraphValidatorNodes(network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const res = await fetch(`${apiUrl}/hypergraph/${networkUrl}/validators`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as ValidatorNode[];
    return validatorNodes;
}

export async function getMetagraphValidatorNodes(network?: string, metagraphAddress?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = metagraphAddress
        ? `${apiUrl}/metagraphs/${networkUrl}/validators?metagraphAddress=${metagraphAddress}`
        : `${apiUrl}/metagraphs/${networkUrl}/validators`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as MetagraphNode[];
    return validatorNodes;
}

export async function getMetagraphWallets(network?: string, metagraphAddress?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = metagraphAddress
        ? `${apiUrl}/wallets/${networkUrl}?metagraphAddress=${metagraphAddress}`
        : `${apiUrl}/wallets/${networkUrl}`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const wallets = data as WalletRichlistInfo[];
    return wallets;
}

export async function getSnapshotMetrics(network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/hypergraph/${networkUrl}/snapshots/metrics`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const wallets = data as SnapshotMetric[];
    return wallets;
}

export async function getRewards(walletAddresses: string, network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/wallets/${networkUrl}/${walletAddresses}/rewards?startDate=2021-01-01&endDate=2025-01-01`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const wallets = data as Reward[];
    return wallets;
}

export async function getBalance(walletAddress: string, network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/wallets/${networkUrl}/${walletAddress}`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const wallets = data as Balance;
    return wallets;
}

export async function getValidatorNodes(walletAddress: string, network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/hypergraph/${networkUrl}/validators/${walletAddress}`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const validators = data as WalletValidatorNodes;
    return validators;
}

export async function getValidatorNodeUptime(walletAddress: string, network?: string) {
    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/hypergraph/${networkUrl}/validators/${walletAddress}/uptime`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const validators = data as ValidatorNodeUptime[];
    return validators;
}


export async function getOnChainDataSnapshot(snapshotId: string, metagraphAddress?: string, network?: string) {
    if (!metagraphAddress) {
        return null;
    }

    const apiUrl = process.env.NEXT_PUBLIC_API_DAGSCAN_URL;
    const networkUrl = network || 'mainnet';

    const url = `${apiUrl}/metagraphs/${networkUrl}/${metagraphAddress}/snapshot/${snapshotId}`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }

    const data = await res.json();
    const onchainSnapshot = data as number[];
    return onchainSnapshot;
}
