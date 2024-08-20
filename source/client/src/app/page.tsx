import { ChartSnapshotCount } from "@/components/chart-snapshotcount";
import { ChartSnapshotFees } from "@/components/chart-snapshotfees";
import { LatestSnapshots } from "@/components/latest-snapshots";
import { LatestTransactions } from "@/components/latest-transactions";

export default function Home() {
  return (
    <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
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
    </div>
  );
}
