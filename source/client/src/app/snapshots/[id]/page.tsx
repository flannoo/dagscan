import SnapshotDetail from "@/components/snapshot-detail";
import SnapshotRewards from "@/components/snapshot-rewards";
import SnapshotTransactions from "@/components/snapshot-transactions";

export default function SnapshotDetailPage({ params }: { params: { id: string } }) {
    const id = Array.isArray(params.id) ? params.id[0] : params.id;

    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <div className="mb-4">
                <SnapshotDetail snapshotId={id} />
            </div>
            <div className="mb-4">
                <SnapshotTransactions snapshotId={id} />
            </div>
            <div className="mb-4">
                <SnapshotRewards snapshotId={id} />
            </div>
        </div>
    )
}
