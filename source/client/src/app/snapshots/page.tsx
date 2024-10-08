import { SnapshotList } from "@/components/snapshot-list";

export default function SnapshotPage() {
    return (
        <div className="container mx-auto px-4 lg:px-8 mb-4 mt-4">
            <h1 className="text-2xl font-bold mb-4">Snapshots</h1>
            <SnapshotList />
        </div>
    )
}
