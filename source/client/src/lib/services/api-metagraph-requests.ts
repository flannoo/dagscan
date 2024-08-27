import { OnChainSnapshot } from "@/lib/shared/types";

export async function getOnChainDataSnapshot(snapshotId: string, apiUrl?: string) {
    if (!apiUrl) {
        return null;
    }

    var res = await fetch(`${apiUrl}/snapshots/${snapshotId}`);

    if (!res.ok) {
        throw new Error(`Failed to fetch: ${res.status} ${res.statusText}`);
    }
    const data = await res.json();
    const onchainSnapshot = data.value as OnChainSnapshot;
    return onchainSnapshot;
}
