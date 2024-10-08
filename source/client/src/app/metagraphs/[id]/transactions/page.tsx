"use client";

import { TransactionList } from "@/components/transaction-list";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { getMetagraphs } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";

export default function TransactionsPage({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const metagraphSymbol = metagraphs?.find(metagraph => metagraph.metagraphAddress === id)?.symbol || '';

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Transactions</h1>
            <div className="mb-4">
                {metagraphSymbol ? (<TransactionList metagraphId={id} metagraphSymbol={metagraphSymbol} />) : (<SkeletonCard />)}
            </div>
        </div>
    )
}
