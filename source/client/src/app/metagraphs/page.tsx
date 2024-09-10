"use client";

import { ChartMetagraphSnapshotCount } from "@/components/chart-metagraph-snapshotcount";
import { ChartMetagraphSnapshotFees } from "@/components/chart-metagraph-snapshotfees";
import { LatestTransactions } from "@/components/latest-transactions";
import { MetagraphList } from "@/components/metagraph-list"
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { getMetagraphs, getSnapshotMetrics } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle } from "lucide-react";
import React from "react"

export default function MetagraphsPage() {
    const { data, isLoading, isError } = useQuery({
        queryKey: ['metagraphList'],
        queryFn: async () => getMetagraphs(),
        staleTime: 24 * 60 * 60 * 1000, // Cache data for 24 hours
    });

    const { data: snapshotMetricData } = useQuery({
        queryKey: ['snapshotmetrics'],
        queryFn: async () => getSnapshotMetrics(),
        refetchOnWindowFocus: true,
    });

    const filterMetagraphs = (data?.filter(metagraph => metagraph.metagraphAddress) || []).sort((a, b) => {
        if (a.name < b.name) return -1;
        if (a.name > b.name) return 1;
        return 0;
    });

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Metagraphs</h1>

            {isLoading ? (
                <SkeletonCard />
            ) : isError ? (
                <div className="flex justify-center items-center text-red-500">
                    <AlertCircle className="h-8 w-8 mr-2" />
                    <span>Failed to fetch data</span>
                </div>
            ) : (
                <>
                    <div className="mb-12">
                        <MetagraphList metagraphs={filterMetagraphs} />
                    </div>

                    {filterMetagraphs.map(metagraph => (
                        <>
                            <h2 className="text-2xl font-bold mb-4">{metagraph.symbol} Statistics</h2>
                            <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0 mb-4">
                                <div className="lg:w-1/2">
                                    <ChartMetagraphSnapshotCount snapshotMetrics={snapshotMetricData || []} metagraphAddress={metagraph.metagraphAddress} />
                                </div>
                                <div className="lg:w-1/2">
                                    <ChartMetagraphSnapshotFees snapshotMetrics={snapshotMetricData || []} metagraphAddress={metagraph.metagraphAddress} />
                                </div>
                            </div>
                        </>
                    ))}
                </>
            )}
        </div>
    )
}
