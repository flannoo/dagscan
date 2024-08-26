"use client";

import { ChartSnapshotCount } from "@/components/chart-snapshotcount";
import { ChartSnapshotFees } from "@/components/chart-snapshotfees";
import GridWallets from "@/components/grid-wallets";
import { LatestSnapshots } from "@/components/latest-snapshots";
import { LatestTransactions } from "@/components/latest-transactions";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";

export default function Home() {

  return (
    <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
      <h1 className="text-2xl font-bold mb-4">DAG Hypergraph</h1>

      <Tabs defaultValue="overview" className="w-full">
        <TabsList className="w-full justify-start">
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="wallets">Wallet Explorer</TabsTrigger>
        </TabsList>
        <TabsContent value="overview">
          <div className="flex flex-col lg:flex-row lg:space-x-4 space-y-4 lg:space-y-0 mb-4">
            <div className="lg:w-1/2">
              <ChartSnapshotCount />
            </div>
            <div className="lg:w-1/2">
              <ChartSnapshotFees />
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
            <GridWallets metagraphSymbol="DAG" />
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
}
