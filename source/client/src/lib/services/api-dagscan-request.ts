import { Metagraph, Reward, SnapshotMetric, ValidatorNode, WalletRichlistInfo } from "@/lib/shared/types";

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
        ? `${apiUrl}/hypergraph/${networkUrl}/validators?metagraphAddress=${metagraphAddress}`
        : `${apiUrl}/hypergraph/${networkUrl}/validators`;

    const res = await fetch(url);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const validatorNodes = data as ValidatorNode[];
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
