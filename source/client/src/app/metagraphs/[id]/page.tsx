"use client";

import React from "react"
import { LatestTransactionsMetagraph } from "@/components/latest-transactions-metagraph"
import { LatestSnapshotsMetagraph } from "@/components/latest-snapshots-metagraph"
import { getMetagraphs } from "@/lib/services/api-nebula-requests";
import { useQuery } from "@tanstack/react-query";
import GridWallets from "@/components/grid-wallets";

export default function Metagraph({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const metagraphSymbol = metagraphs?.find(metagraph => metagraph.MetagraphAddress === id)?.MetagraphName || '';

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">{metagraphSymbol} Metagraph</h1>
            <div className="mb-4">
                <GridWallets metagraphSymbol={metagraphSymbol} />
            </div>
            <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0">
                <div className="lg:w-1/2">
                    <LatestSnapshotsMetagraph metagraphId={id} metagraphSymbol={metagraphSymbol} />
                </div>
                <div className="lg:w-1/2">
                    <LatestTransactionsMetagraph metagraphId={id} metagraphSymbol={metagraphSymbol} />
                </div>
            </div>
        </div>
    )
}
