"use client";

import React from "react"
import { LatestTransactions } from "@/components/latest-transactions"
import { LatestSnapshots } from "@/components/latest-snapshots"
import { useQuery } from "@tanstack/react-query";
import GridWallets from "@/components/grid-wallets";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { getMetagraphs } from "@/lib/services/api-dagscan-request";
import { SnapshotList } from "@/components/snapshot-list";
import { TransactionList } from "@/components/transaction-list";

export default function Metagraph({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const metagraphSymbol = metagraphs?.find(metagraph => metagraph.metagraphAddress === id)?.symbol || '';

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">{metagraphSymbol} Metagraph</h1>

            <Tabs defaultValue="overview" className="w-full">
                <TabsList className="w-full justify-start">
                    <TabsTrigger value="overview">Overview</TabsTrigger>
                    <TabsTrigger value="wallets">Wallets</TabsTrigger>
                    <TabsTrigger value="snapshots">Snapshots</TabsTrigger>
                    <TabsTrigger value="transactions">Transactions</TabsTrigger>
                </TabsList>
                <TabsContent value="overview">
                    <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0">
                        <div className="lg:w-1/2">
                            <LatestSnapshots metagraphId={id} metagraphSymbol={metagraphSymbol} />
                        </div>
                        <div className="lg:w-1/2">
                            <LatestTransactions metagraphId={id} metagraphSymbol={metagraphSymbol} />
                        </div>
                    </div>
                </TabsContent>
                <TabsContent value="wallets">
                    <div className="mb-4">
                        <GridWallets metagraphSymbol={metagraphSymbol} />
                    </div>
                </TabsContent>
                <TabsContent value="snapshots">
                    <div className="mb-4">
                        <SnapshotList metagraphId={id} metagraphSymbol={metagraphSymbol} />
                    </div>
                </TabsContent>
                <TabsContent value="transactions">
                    <div className="mb-4">
                        <TransactionList metagraphId={id} metagraphSymbol={metagraphSymbol} />
                    </div>
                </TabsContent>
            </Tabs>
        </div>
    )
}
