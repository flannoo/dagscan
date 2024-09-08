"use client";

import { ChartDagTransactionCount } from "@/components/chart-dag-transaction-count";
import { ChartDagTransactionVolume } from "@/components/chart-dag-transaction-volume";
import { ChartSnapshotCount } from "@/components/chart-snapshotcount";
import { ChartSnapshotFees } from "@/components/chart-snapshotfees";
import GridWallets from "@/components/grid-wallets";
import { LatestSnapshots } from "@/components/latest-snapshots";
import { LatestTransactions } from "@/components/latest-transactions";
import NodeExplorer from "@/components/node-explorer";
import { SnapshotList } from "@/components/snapshot-list";
import { TransactionList } from "@/components/transaction-list";
import { SkeletonCard } from "@/components/ui/skeleton-card";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { getSnapshotMetrics } from "@/lib/services/api-dagscan-request";
import { useQuery } from "@tanstack/react-query";
import { AlertCircle } from "lucide-react";

export default function HomePage() {

  const { data, isLoading, isError } = useQuery({
    queryKey: ['snapshotmetrics'],
    queryFn: async () => getSnapshotMetrics(),
    refetchOnWindowFocus: true,
  });

  return (
    <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
      <h1 className="text-2xl font-bold mb-4">DAG Hypergraph</h1>

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
                <ChartSnapshotCount snapshotMetrics={data} />
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
                <ChartSnapshotFees snapshotMetrics={data} />
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
                <ChartDagTransactionCount snapshotMetrics={data} />
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
                <ChartDagTransactionVolume snapshotMetrics={data} />
              )}
            </div>
          </div>
          <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0">
            <div className="lg:w-1/2">
              <LatestSnapshots />
            </div>
            <div className="lg:w-1/2">
              <LatestTransactions />
            </div>
          </div>
        </TabsContent>
        <TabsContent value="wallets">
          <div className="mb-4">
            <GridWallets />
          </div>
        </TabsContent>
        <TabsContent value="snapshots">
          <div className="mb-4">
            <SnapshotList />
          </div>
        </TabsContent>
        <TabsContent value="transactions">
          <div className="mb-4">
            <TransactionList />
          </div>
        </TabsContent>
        <TabsContent value="nodes">
          <div className="mb-4">
            <NodeExplorer />
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
