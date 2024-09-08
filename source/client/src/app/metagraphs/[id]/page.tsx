"use client";

import React from "react"
import { LatestTransactions } from "@/components/latest-transactions"
import { LatestSnapshots } from "@/components/latest-snapshots"
import { useQuery } from "@tanstack/react-query";
import GridWallets from "@/components/grid-wallets";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { getMetagraphs, getSnapshotMetrics } from "@/lib/services/api-dagscan-request";
import { SnapshotList } from "@/components/snapshot-list";
import { TransactionList } from "@/components/transaction-list";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { AlertCircle } from "lucide-react";
import { ChartMetagraphSnapshotCount } from "@/components/chart-metagraph-snapshotcount";
import { ChartMetagraphSnapshotFees } from "@/components/chart-metagraph-snapshotfees";
import { ChartMetagraphTransactionCount } from "@/components/chart-metagraph-transaction-count";
import { ChartMetagraphTransactionVolume } from "@/components/chart-metagraph-transaction-volume";
import NodeExplorerMetagraph from "@/components/node-explorer-metagraph";

export default function MetagraphPage({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    const { data: metagraphs } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const { data, isLoading, isError } = useQuery({
        queryKey: ['snapshotmetrics', id],
        queryFn: async () => getSnapshotMetrics(),
        refetchOnWindowFocus: true,
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
                    <TabsTrigger value="nodes">Node Explorer</TabsTrigger>
                </TabsList>
                <TabsContent value="overview">
                    <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0 mb-4">
                        <div className="lg:w-1/2">
                            {isLoading ? (
                                <SkeletonCard />
                            ) : isError || !data ? (
                                <div className="flex justify-center items-center text-red-500">
                                    <AlertCircle className="h-8 w-8 mr-2" />
                                    <span>Failed to fetch data</span>
                                </div>
                            ) : (
                                <ChartMetagraphSnapshotCount snapshotMetrics={data} metagraphAddress={id} />
                            )}
                        </div>
                        <div className="lg:w-1/2">
                            {isLoading ? (
                                <SkeletonCard />
                            ) : isError || !data ? (
                                <div className="flex justify-center items-center text-red-500">
                                    <AlertCircle className="h-8 w-8 mr-2" />
                                    <span>Failed to fetch data</span>
                                </div>
                            ) : (
                                <ChartMetagraphSnapshotFees snapshotMetrics={data} metagraphAddress={id} />
                            )}
                        </div>
                    </div>
                    <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0 mb-4">
                        <div className="lg:w-1/2">
                            {isLoading ? (
                                <SkeletonCard />
                            ) : isError || !data ? (
                                <div className="flex justify-center items-center text-red-500">
                                    <AlertCircle className="h-8 w-8 mr-2" />
                                    <span>Failed to fetch data</span>
                                </div>
                            ) : (
                                <ChartMetagraphTransactionCount snapshotMetrics={data} metagraphAddress={id} />
                            )}
                        </div>
                        <div className="lg:w-1/2">
                            {isLoading ? (
                                <SkeletonCard />
                            ) : isError || !data ? (
                                <div className="flex justify-center items-center text-red-500">
                                    <AlertCircle className="h-8 w-8 mr-2" />
                                    <span>Failed to fetch data</span>
                                </div>
                            ) : (
                                <ChartMetagraphTransactionVolume snapshotMetrics={data} metagraphAddress={id} />
                            )}
                        </div>
                    </div>
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
                        <GridWallets metagraphAddress={id} />
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
                <TabsContent value="nodes">
                    <div className="mb-4">
                        <NodeExplorerMetagraph metagraphAddress={id} />
                    </div>
                </TabsContent>
            </Tabs>
        </div>
    )
}
