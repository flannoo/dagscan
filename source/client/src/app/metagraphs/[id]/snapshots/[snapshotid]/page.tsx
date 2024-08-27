"use client";

import SnapshotDetail from "@/components/snapshot-detail";
import SnapshotRewards from "@/components/snapshot-rewards";
import SnapshotTransactions from "@/components/snapshot-transactions";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { getMetagraphs } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";

export default function SnapshotDetailPage({ params }: { params: { id: string, snapshotid: string } }) {
    const snapid = Array.isArray(params.snapshotid) ? params.snapshotid[0] : params.snapshotid;
    const metagraphId = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });
    const metagraphSymbol = metagraphs?.find(metagraph => metagraph.metagraphAddress === metagraphId)?.symbol || '';
    const metagraphApiUrl = metagraphs?.find(metagraph => metagraph.metagraphAddress === metagraphId)?.l0ApiUrl || '';

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">

            <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
                <div className="mb-4">
                    {metagraphSymbol ? (
                        <SnapshotDetail snapshotId={snapid} metagraphId={metagraphId} metagraphSymbol={metagraphSymbol} onchainApiUrl={metagraphApiUrl} />
                    ) : (<SkeletonCard />)}
                </div>
                <div className="mb-4">
                    {metagraphSymbol ? (
                        <SnapshotTransactions snapshotId={snapid} metagraphId={metagraphId} metagraphSymbol={metagraphSymbol} />
                    ) : (<SkeletonCard />)}
                </div>
                <div className="mb-4">
                    {metagraphSymbol ? (
                        <SnapshotRewards snapshotId={snapid} metagraphId={metagraphId} metagraphSymbol={metagraphSymbol} />
                    ) : (<SkeletonCard />)}
                </div>
            </div>
        </div>
    )
}
