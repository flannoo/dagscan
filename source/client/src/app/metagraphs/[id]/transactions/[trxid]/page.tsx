"use client";

import TransactionDetail from "@/components/transaction-detail";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { getMetagraphs } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";

export default function TransactionDetailPage({ params }: { params: { id: string, trxid: string } }) {
    const trxid = Array.isArray(params.trxid) ? params.trxid[0] : params.trxid;
    const metagraphId = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });
    const metagraphSymbol = metagraphs?.find(metagraph => metagraph.metagraphAddress === metagraphId)?.symbol || '';

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <div className="mb-4">
                {metagraphSymbol ? (
                    <TransactionDetail transactionId={trxid} metagraphId={metagraphId} metagraphSymbol={metagraphSymbol} />
                ) : (<SkeletonCard />)}
            </div>
        </div>
    )
}
